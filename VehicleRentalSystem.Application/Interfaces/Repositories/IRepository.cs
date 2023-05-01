using System.Linq.Expressions;

namespace VehicleRentalSystem.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    T Get(Guid id);

    T GetItem(Guid? id);

    T Retrieve(string id);

    T GetFirstOrDefault(Expression<Func<T, bool>> filter);

    List<T> FilterDeleted();

    List<T> GetAll(bool filterDeleted = false);

    void Add(T entity);

    void AddRange(List<T> entity);

    void Remove(T entity);

    void RemoveRange(List<T> entity);
}

