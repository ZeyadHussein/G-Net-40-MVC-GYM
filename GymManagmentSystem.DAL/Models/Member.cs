using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }

        public HealthRecord HealthRecord { get; set; }=null!;

        public ICollection<Booking> MemberSessions { get; set; }=null!;

        public ICollection<Membership> MemberPlans { get; set; }=null!;
        public bool IsActive { get; set; }
    }
}
