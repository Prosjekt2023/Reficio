using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bacit_dotnet.MVC.DataAccess;
using bacit_dotnet.MVC.Models;

namespace bacit_dotnet.MVC.Controllers
{
    [Authorize(Roles="Admin")]
    public class AppRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
		
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        //List All the Roles created by the Users
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
		
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            //Avoid duplicate roles
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }

            return RedirectToAction("Index");
        }
    }
}