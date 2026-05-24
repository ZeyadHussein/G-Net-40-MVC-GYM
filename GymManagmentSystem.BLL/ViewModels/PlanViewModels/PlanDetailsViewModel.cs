using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.BLL.ViewModels.PlanViewModels
{
    public class PlanDetailsViewModel
    {
        public int Id { get; set; }

        public string PlanName { get; set; } = null!;

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public string Description { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
