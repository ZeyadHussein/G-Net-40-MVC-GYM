using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GymManagmentSystem.DAL.Models;
using GymManagmentSystem.BLL.ViewModels.AcountViewodel;

namespace GymManagmentSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager,ILogger<AccountController> logger, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct=default)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var User = await _userManager.FindByEmailAsync(model.Email);
            if(User is null || string.IsNullOrEmpty(User.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password attempt.");
                return View(model);
            }
            var result =await _signInManager.PasswordSignInAsync(User.UserName, model.Password, model.RememberMe, lockoutOnFailure:true);
            if(result.Succeeded)
            {
                _logger.LogInformation("User {UserId} SignIn.", User.Id);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if(result.IsLockedOut)
            {
                _logger.LogWarning("User {UserId} locked out.", User.Id);
                ModelState.AddModelError(string.Empty, "User account is temporarily locked.");
            }
            else if(result.IsNotAllowed)
            {
               
                ModelState.AddModelError(string.Empty, "User account is not allowed to sign in.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                
            }
            return View(model);
        }
        #endregion
    }
}
