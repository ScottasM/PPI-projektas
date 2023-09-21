namespace PPI_projektas.objects.abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
