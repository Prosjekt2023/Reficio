using System.ComponentModel.DataAnnotations;
using bacit_dotnet.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using bacit_dotnet.MVC.DataAccess;

namespace bacit_dotnet.MVC.Models.Account.Manage
{
    public class UserProfileViewModel : PageModel
    {
        public string Username { get; set; }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public ManageController Input { get; set; }

        [Display(Name = "Phone number")] public string PhoneNumber { get; set; }
    }
}