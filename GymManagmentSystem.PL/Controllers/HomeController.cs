using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagmentSystem.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;

        public HomeController(
            ILogger<HomeController> logger,
            ISessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var analytics = await _sessionService.GetAnalyticsAsync(ct);
            var sessions = await _sessionService.GetAllSessionsAsync(ct);

            var model = new HomeDashboardViewModel
            {
                Analytics = analytics,
                LatestSessions = sessions?.Take(6)
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}