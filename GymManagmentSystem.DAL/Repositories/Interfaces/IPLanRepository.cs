using GymManagmentSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Repositories.Interfaces
{
    public interface IPLanRepository
    {

        Task<IEnumerable<Plan>> GetAllAsync(bool tracking=false, CancellationToken ct=default);
        Task<Plan?> GetByIdAsync(int id, CancellationToken ct=default);
        public Task<int> AddAsync(Plan plan);
        public Task<int> UpdateAsync(Plan plan);
        public Task<int> DeleteAsync(Plan plan);
    }
}
