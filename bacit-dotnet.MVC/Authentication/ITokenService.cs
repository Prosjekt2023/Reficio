using bacit_dotnet.MVC.Entities;

namespace bacit_dotnet.MVC.Authentication
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, EmployeeEntity employee);
        bool ValidateToken(string key, string issuer, string audience, string token);
        public bool IsTokenValid(string key, string issuer, string token);
    }
}
