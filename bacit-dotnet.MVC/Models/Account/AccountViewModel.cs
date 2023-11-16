using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using bacit_dotnet.MVC.DataAccess;

namespace bacit_dotnet.MVC.Models.Account
{
    public class AccountViewModel
    {
        public List<ReficioApplicationUser> Users { get; set; }
        
    }
}