using System.Collections.Concurrent;

namespace InfomedicsPortal.Infrastructure.InMemory;

public class BaseRepository<TEntity, TKey> where TEntity : class
{
    private readonly ConcurrentDictionary<TKey, TEntity> _entities;
    private readonly Func<TEntity, TKey> _keySelector;

    public BaseRepository(Func<TEntity, TKey> keySelector)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        _entities = new ConcurrentDictionary<TKey, TEntity>();
    }
    
    public BaseRepository(Func<TEntity, TKey> keySelector, ConcurrentDictionary<TKey, TEntity> intialCollection)
    {
        _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        _entities = intialCollection ??  throw new ArgumentNullException(nameof(intialCollection));
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var key = _keySelector(entity);
        if (!_entities.TryAdd(key, entity))
            throw new InvalidOperationException($"Entity with key {key} already exists.");

        return await Task.FromResult(entity);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        _entities.TryGetValue(id, out var entity);
        return await Task.FromResult(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await Task.FromResult(_entities.Values.ToList());
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var key = _keySelector(entity);
        if (!_entities.ContainsKey(key))
            throw new KeyNotFoundException($"Entity with key {key} not found.");

        _entities[key] = entity;
        return await Task.FromResult(entity);
    }

    public async Task<bool> DeleteAsync(TKey id)
    {
        var removed = _entities.TryRemove(id, out _);
        return await Task.FromResult(removed);
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await Task.FromResult(_entities.ContainsKey(id));
    }
}