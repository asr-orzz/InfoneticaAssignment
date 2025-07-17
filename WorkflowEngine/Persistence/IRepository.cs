namespace WorkflowEngine.Persistence;

public interface IRepository<T>
{
    T Save(T entity);
    T? Find(string id);
    IEnumerable<T> All();
}
