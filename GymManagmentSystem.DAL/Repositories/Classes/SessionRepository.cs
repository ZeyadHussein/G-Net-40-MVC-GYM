using GymManagmentSystem.DAL.dbcontext;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> query = _dbContext.Sessions.AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(ct);
        }

        public Task<int> GetCountOfBookedSlotsAsync(int SessionId, CancellationToken ct = default)
            => _dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == SessionId);


        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default)
       => _dbContext.Sessions.AsNoTracking()
            .Include(s => s.Trainer)
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == SessionId);
    }
}
