namespace WorkBalance.Domain
{
    public interface IDomainContextFactory
    {
        IDomainContext Create();
    }
}
