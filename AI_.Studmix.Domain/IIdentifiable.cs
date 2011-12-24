namespace AI_.Studmix.Domain
{
    public interface IIdentifiable<out T>
    {
        T ID { get; }
    }
}
