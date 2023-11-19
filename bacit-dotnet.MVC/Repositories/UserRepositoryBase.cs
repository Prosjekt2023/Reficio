using Microsoft.AspNetCore.Identity;

namespace bacit_dotnet.MVC.Repositories
{
    // Abstract class serving as a base for user repository implementations
    public abstract class UserRepositoryBase
    {
        // Field to hold the UserManager for IdentityUser
        UserManager<IdentityUser> userManager;

        // Constructor to initialize UserManager<IdentityUser>
        public UserRepositoryBase(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        // Method to check if a user has an "Admin" role based on email
        public bool IsAdmin(string email)
        {
            // Retrieve the IdentityUser based on the provided email
            var identity = userManager.Users.FirstOrDefault(x => x.Email == email);

            // Retrieve existing roles for the user
            var existingRoles = userManager.GetRolesAsync(identity).Result;

            // Check if the user has the "Admin" role
            return existingRoles.FirstOrDefault(x => x == "Admin") != null;
        }

        // Method to set roles for a user based on email and provided role list
        protected void SetRoles(string userEmail, List<string> roles)
        {
            // Retrieve the IdentityUser based on the provided email
            var identity = userManager.Users.FirstOrDefault(x => x.Email == userEmail);

            // Retrieve existing roles for the user
            var existingRoles = userManager.GetRolesAsync(identity).Result;

            // Remove existing roles before assigning new roles
            foreach (var existingRole in existingRoles)
            {
                var result = userManager.RemoveFromRoleAsync(identity, existingRole).Result;
            }

            // Add new roles to the user
            foreach (var role in roles)
            {
                if (!userManager.IsInRoleAsync(identity, role).Result)
                {
                    var result = userManager.AddToRoleAsync(identity, role).Result;
                }
            }
        }

        // Property to expose the UserManager<IdentityUser>
        public UserManager<IdentityUser> UserManager { get; }
    }
}
