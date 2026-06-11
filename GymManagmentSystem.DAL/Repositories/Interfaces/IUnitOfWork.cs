using GymManagmentSystem.DAL.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // Session repository (your custom repository)
        ISessionRepository _SessionRepository { get; }

        // Generic repository factory
        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity, new();

        // Save changes
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}