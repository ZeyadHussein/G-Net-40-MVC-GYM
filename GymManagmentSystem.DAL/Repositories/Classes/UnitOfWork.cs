using GymManagmentSystem.DAL.dbcontext;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly ISessionRepository _sessionRepository;

        private readonly Dictionary<string, object> _repositories = new();

        public UnitOfWork(
            GymDbContext dbContext,
            ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            _sessionRepository = sessionRepository;
        }

        // ✅ FIX: real implementation (NO null, NO NotImplementedException)
        public ISessionRepository _SessionRepository => _sessionRepository;

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(typeName, out var repo))
                return (IGenericRepository<TEntity>)repo;

            var newRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[typeName] = newRepo;

            return newRepo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _dbContext.SaveChangesAsync(ct);
    }
}