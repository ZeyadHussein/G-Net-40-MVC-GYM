using GymManagmentSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<int> AddAsync(TEntity entity);
        public Task<int> UpdateAsync(TEntity entity);
        public Task<int> DeleteAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity , bool>> predicate,CancellationToken ct=default);
       Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,bool tracking = false,CancellationToken ct = default);


    }
}
