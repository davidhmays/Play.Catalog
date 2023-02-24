using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    //Making more generic. For reuse in other services.
    // public interface IItemsRepository
    // ALso a constraint: Has to be a class implementing the IEntity
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task UpdateAsync(T entity);
        Task RemoveAsync(Guid id);
    }

}
