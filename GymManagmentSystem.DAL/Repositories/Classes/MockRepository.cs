using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Repositories.Classes
{
    public class MockRepository : IPLanRepository
    {
        public Task<int> AddAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Plan plan)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            List<Plan> plans = new List<Plan>()
            {
                new() { Name = "Test" }
            };
            return await Task.FromResult(plans.AsEnumerable());
        }

        public Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Plan plan)
        {
            throw new NotImplementedException();
        }
    }
}
