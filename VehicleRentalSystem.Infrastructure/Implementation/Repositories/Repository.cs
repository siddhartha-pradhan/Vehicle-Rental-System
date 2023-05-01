using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Infrastructure.Persistence;
using VehicleRentalSystem.Application.Interfaces.Repositories;

namespace VehicleRentalSystem.Infrastructure.Implementation.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;

    private DbSet<T> _dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual T Get(Guid id)
    {
        return _dbSet.Find(id);
    }

    public virtual T GetItem(Guid? id)
    {
        return _dbSet.Find(id);
    }

    public virtual T Retrieve(string id)
    {
        return _dbSet.Find(id);
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = _dbSet;

        query = query.Where(filter);

        return query.FirstOrDefault();
    }

    public virtual List<T> GetAll(bool includeDeleted = false)
    {
        if (!includeDeleted)
        {
            return FilterDeleted();
        }

        return _dbSet.ToList();
    }

    public virtual List<T> FilterDeleted()
    {
        IQueryable<T> query = _dbSet;

        return query.ToList();
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(List<T> entity)
    {
        _dbSet.AddRange(entity);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(List<T> entity)
    {
        _dbSet.RemoveRange(entity);
    }
}