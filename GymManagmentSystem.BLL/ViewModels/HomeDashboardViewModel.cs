using GymManagmentSystem.BLL.ViewModels.SessionViewModels;
using System.Collections.Generic;

namespace GymManagmentSystem.PL.ViewModels
{
    public class HomeDashboardViewModel
    {
        public AnalyticsViewModel Analytics { get; set; } = new();

        public IEnumerable<SessionViewModel>? LatestSessions { get; set; }
    }
}