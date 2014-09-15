namespace StopLossF

module StopLossManager = 
    
    let stopLoss price = price - 0.1M

    type Message = 
        | PositionAcquired of price : decimal
        | PriceUpdated of price : decimal
        | Event of Event
    and Event = 
        | TargetUpdated of price : decimal
        | StopLossTriggered of price : decimal
        | FutureMessage of seconds : int * Message

    type Position = 
        { OpeningPrice : decimal
          StopLoss : decimal 
          Prices : decimal list }

    let handleMessages msgs existingPosition = 
        msgs
        |> List.fold (fun (position,events) msg ->
            match position with
            | None ->
                match msg with
                | PositionAcquired price -> (Some { OpeningPrice = price; StopLoss = price |> stopLoss; Prices = [] }),events
                | _ -> None,events
            | Some pos ->
                match msg with
                | PositionAcquired _ -> failwithf "Unexpected PositionAcquired message, Position is already open"
                | PriceUpdated price -> 
                    let updatedPosition = Some { pos with Prices = price::pos.Prices }
                    let futurePriceUpdated seconds = FutureMessage (seconds,(PriceUpdated price))
                    updatedPosition,(futurePriceUpdated 10)::(futurePriceUpdated 7)::events
                | Event (FutureMessage (_,futureMessage)) ->
                    match futureMessage with
                    | PriceUpdated price ->
                        let minPrice = pos.Prices |> List.min
                        let maxPrice = pos.Prices |> List.max
                        let newPrices = pos.Prices |> List.filter (fun p -> p <> price) // todo: what if duplicate prices?
                        let newPos = Some { pos with Prices = newPrices }
                        if minPrice > pos.StopLoss then
                            newPos,(TargetUpdated (minPrice |> stopLoss))::events
                        elif maxPrice < pos.StopLoss then
                            newPos,(StopLossTriggered price)::events
                        else newPos,events
                    | m -> failwithf "Only support PriceUpdated future messages. Unexpected future message %A" m
                | Event e -> failwithf "Unexpected FutureMessage Event. Expecting PriceUpdated only"
                 
        ) existingPosition,[]

    
    
