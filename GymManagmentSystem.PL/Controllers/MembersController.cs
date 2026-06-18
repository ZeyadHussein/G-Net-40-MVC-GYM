using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.PL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]

    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members =
                await _memberService.GetALLMembersAsync(ct);

            return View(members);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateMember(
            CreateMemberViewModel model,
            CancellationToken ct)
        {
            // VERY IMPORTANT FOR DEBUGGING

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(
                            $"{item.Key} : {error.ErrorMessage}");
                    }
                }

                return View(nameof(Create), model);
            }

            var result =
                await _memberService
                    .CreateMemberAsync(model, ct);

            if (!result)
            {
                TempData["ErrorMessage"] =
                    "Failed to create member";

                // STAY ON THE PAGE

                return View(nameof(Create), model);
            }

            TempData["SuccessMessage"] =
                "Member created successfully";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region MemberDetails

        [HttpGet]

        public async Task<IActionResult>
            MemberDetails(
            int id,
            CancellationToken ct)
        {
            var member =
                await _memberService
                    .GetMemberDetailsAsync(
                    id,
                    ct);

            if (member == null)
            {
                TempData["ErrorMessage"] =
                    "Member not found";

                return RedirectToAction(
                    nameof(Index));
            }

            return View(member);
        }

        #endregion

        #region HealthRecordDetails

        [HttpGet]

        public async Task<IActionResult>
            HealthRecordDetails(
            int id,
            CancellationToken ct)
        {
            var record =
                await _memberService
                    .GetmemberHealthRecordAsync(
                    id,
                    ct);

            if (record == null)
            {
                TempData["ErrorMessage"] =
                    "Health record not found";

                return RedirectToAction(
                    nameof(Index));
            }

            return View(record);
        }

        #endregion

        #region Edit

        [HttpGet]

        public async Task<IActionResult>
            EditMember(
            int id,
            CancellationToken ct)
        {
            var member =
                await _memberService
                    .GetMemberToUpdateAsync(
                    id,
                    ct);

            if (member == null)
            {
                TempData["ErrorMessage"] =
                    "Member not found";

                return RedirectToAction(
                    nameof(Index));
            }

            return View(member);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult>
            EditMember(
            int id,
            MemberToUpdateViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await _memberService
                    .UpdateMemberDetailsAsync(
                    id,
                    model,
                    ct);

            if (!result)
            {
                TempData["ErrorMessage"] =
                    "Failed to update member";

                return View(model);
            }

            TempData["SuccessMessage"] =
                "Member updated successfully";

            return RedirectToAction(
                nameof(Index));
        }

        #endregion

        #region Delete

        [HttpGet]

        public async Task<IActionResult>
            Delete(
            int id,
            CancellationToken ct)
        {
            var member =
                await _memberService
                    .GetMemberDetailsAsync(
                    id,
                    ct);

            if (member == null)
            {
                TempData["ErrorMessage"] =
                    "Member not found";

                return RedirectToAction(
                    nameof(Index));
            }

            return View(
                "DeleteMember",
                member);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult>
            DeleteConfirmed(
            int id,
            CancellationToken ct)
        {
            var result =
                await _memberService
                    .RemoveMemberAsync(
                    id,
                    ct);

            if (!result)
            {
                TempData["ErrorMessage"] =
                    "Failed to delete member";

                return RedirectToAction(
                    nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Member deleted successfully";

            return RedirectToAction(
                nameof(Index));
        }

        #endregion
    }
}