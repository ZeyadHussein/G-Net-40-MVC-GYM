using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.BLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public int Id { get; set; }

        [Required]
        public string PlanName { get; set; } = null!;

        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 3650)]
        public int DurationDays { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
