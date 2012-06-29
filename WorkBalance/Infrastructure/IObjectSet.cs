namespace WorkBalance.Infrastructure
{
    public interface IObjectSet<T> : System.Data.Objects.IObjectSet<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
    }
}