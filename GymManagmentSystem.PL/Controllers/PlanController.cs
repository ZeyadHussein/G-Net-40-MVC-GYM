using GymManagmentSystem.BLL.ViewModels.PlanViewModels;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MCV0001.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPLanRepository _planRepository;

        public PlanController(IPLanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);

            var result = plans.Select(plan => new PlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationInDays,
                Description = plan.Description,
                IsActive = plan.IsActive
            });

            return View(result);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";

                return RedirectToAction(nameof(Index));
            }

            var result = new PlanDetailsViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationInDays,
                Description = plan.Description,
                IsActive = plan.IsActive
            };

            return View(result);
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";

                return RedirectToAction(nameof(Index));
            }

            var result = new UpdatePlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationInDays,
                Description = plan.Description,
                IsActive = plan.IsActive
            };

            return View(result);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(
            int id,
            UpdatePlanViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";

                return RedirectToAction(nameof(Index));
            }

            plan.Price = model.Price;
            plan.DurationInDays = model.DurationDays;
            plan.Description = model.Description;

            await _planRepository.UpdateAsync(plan);

            TempData["SuccessMessage"] =
                "Plan updated successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Activate / Deactivate

        [HttpPost]

        public async Task<IActionResult> Activate(
            int id,
            CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null)
            {
                TempData["ErrorMessage"] =
                    "Plan not found";

                return RedirectToAction(nameof(Index));
            }

            plan.IsActive = !plan.IsActive;

            await _planRepository.UpdateAsync(plan);

            TempData["SuccessMessage"] =
                plan.IsActive
                ? "Plan activated successfully"
                : "Plan deactivated successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}