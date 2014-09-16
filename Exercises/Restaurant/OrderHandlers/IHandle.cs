namespace Restaurant.OrderHandlers
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}