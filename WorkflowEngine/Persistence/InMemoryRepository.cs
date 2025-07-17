using System.Collections.Concurrent;

namespace WorkflowEngine.Persistence;

public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly ConcurrentDictionary<string, T> _store = new();
    private readonly Func<T, string> _keySelector;

    public InMemoryRepository(Func<T, string> keySelector)
    {
        _keySelector = keySelector;
    }

    public T Save(T entity)
    {
        _store[_keySelector(entity)] = entity;
        return entity;
    }

    public T? Find(string id)
    {
        return _store.TryGetValue(id, out var entity) ? entity : null;
    }

    public IEnumerable<T> All() => _store.Values;
}
