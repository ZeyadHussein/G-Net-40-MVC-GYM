using GymManagmentSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentSystem.DAL.Models
{
    public class Trainer:GymUser
    {
        public Specialites Speciality { get; set; }
        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
