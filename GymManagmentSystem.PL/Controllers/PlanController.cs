
using GymManagmentSystem.DAL.DbContexts;
using GymManagmentSystem.DAL.Repositories.Classes;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCV0001.Controllers
{
    public class PlanController : Controller

    {
         private readonly IPLanRepository _planRepository;
        public PlanController(IPLanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);
            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
    }

}
