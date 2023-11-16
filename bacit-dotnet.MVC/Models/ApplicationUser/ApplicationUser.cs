using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace bacit_dotnet.MVC.Models.ApplicationUser
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100")]
        public string Firstname { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Lastname { get; set; }
    }
}