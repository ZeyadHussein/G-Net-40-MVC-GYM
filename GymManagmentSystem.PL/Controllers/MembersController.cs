using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetALLMembersAsync(ct);
            return View(members);
        }
        #endregion
        #region Create
        [HttpGet]
        public IActionResult Create()=>View();
        [HttpPost]
        public async Task<IActionResult>CreateMember (CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            var Result = await _memberService.CreateMemberAsync(model, ct);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member created successfully";


            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create member";
            }
            return RedirectToAction(nameof(Index));

            



        }

        #endregion

        #region MemberDetails
        [HttpGet]
        public async Task<IActionResult>MemberDetails(int id,CancellationToken ct)
        {
            var Member=await _memberService.GetMemberDetailsAsync(id,ct);
            if(Member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);

        }


        #endregion

        #region HealthRecordDetails
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id,CancellationToken ct)
        {
            var record = await _memberService.GetmemberHealthRecordAsync(id, ct);
            if (record is null)
            {
                TempData["ErrorMessage"] = "Health record not found";
                return RedirectToAction(nameof(Index));
            }
            return View(record);

        }


        #endregion
        #region Edit
        [HttpGet]
        public async Task<IActionResult>EditMember(int id,CancellationToken ct)
        {
            var Member =await _memberService.GetMemberToUpdateAsync(id,ct);
            if (Member is null) 
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);



        }
        [HttpPost]
        public async Task<IActionResult>EditMember(int id,MemberToUpdateViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid)return View(model);
            var Result=await _memberService.UpdateMemberDetailsAsync(id,model,ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update member";

            }
            return View(model);
        }
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View("DeleteMember", Member);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await _memberService.RemoveMemberAsync(id, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete member. Ensure there are no future bookings for";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
