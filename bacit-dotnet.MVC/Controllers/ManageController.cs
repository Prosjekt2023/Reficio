using bacit_dotnet.MVC.Models.Account.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bacit_dotnet.MVC.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ManageController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Manage/Index
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new UserProfileViewModel
            {
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                // Set other properties as needed
            };

            // Note: The view path is explicitly provided if it's not in the default location
            return View("~/Views/Account/Manage/Index.cshtml", model);
        }

        // POST: /Manage/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Account/Manage/Index.cshtml", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Unexpected error when trying to set phone number.");
                    return View("~/Views/Account/Manage/Index.cshtml", model);
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            ViewData["StatusMessage"] = "Your profile has been updated";
            return View("~/Views/Account/Manage/Index.cshtml", model);
        }
    }
}
