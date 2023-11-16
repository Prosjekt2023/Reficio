using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bacit_dotnet.MVC.DataAccess;
using bacit_dotnet.MVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using bacit_dotnet.MVC.Models.Account;
using Microsoft.AspNetCore.Authorization;

namespace bacit_dotnet.MVC.Controllers
{

    [Authorize(Roles="Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ReficioApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ReficioApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }



        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // User not found
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Optionally, you can perform additional actions after a successful delete
                // For example, refreshing the user list or displaying a success message.
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                // Handle errors, such as displaying an error message
                TempData["ErrorMessage"] = "Error deleting user.";
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Index()
        {
            // Get a list of users  
            var users = await _userManager.Users.ToListAsync();           

            // Pass the users to the view
            var viewModel = new AccountViewModel { Users = users, };
            return View(viewModel);
        }

        
    }



}
