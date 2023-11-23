using bacit_dotnet.MVC.DataAccess;
using bacit_dotnet.MVC.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bacit_dotnet.MVC.Repositories
{
    public class EFUserRepository : UserRepositoryBase, IUserRepository
    {
        private readonly DataContext dataContext;

        public EFUserRepository(DataContext dataContext, UserManager<IdentityUser> userManager) : base (userManager)
        {
            this.dataContext = dataContext;
        }

        public void Delete(string email)
        {
            // Check if the user is an Admin
            var identityUser = userManager.FindByEmailAsync(email).Result;
            if (identityUser != null)
            {
                var roles = userManager.GetRolesAsync(identityUser).Result;
                if (roles.Contains("Admin"))
                {
                    throw new InvalidOperationException("Cannot delete an admin user.");
                }

                // Delete user from AspNetUsers table
                var result = userManager.DeleteAsync(identityUser).Result;
                if (!result.Succeeded)
                {
                    // Handle the error
                    throw new InvalidOperationException("Could not delete the user from AspNetUsers table.");
                }
            }

            // Delete user from the application's Users table
            UserEntity? user = GetUserByEmail(email);
            if (user != null)
            {
                dataContext.Users.Remove(user);
                dataContext.SaveChanges();
            }
        }

        private UserEntity? GetUserByEmail(string email)
        {
            return dataContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public List<UserEntity> GetUsers()
        {
            return dataContext.Users.ToList();
        }

        public void Add(UserEntity user)
        {
            var existingUser = GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists found");
            }
            dataContext.Users.Add(user);
            dataContext.SaveChanges();
        }
        public void Update(UserEntity user, List<string> roles)
        {
            var existingUser = GetUserByEmail(user.Email);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.Email = user.Email;
            existingUser.Name = user.Name;
            dataContext.SaveChanges();
            SetRoles(user.Email, roles);
        }

    }
}
