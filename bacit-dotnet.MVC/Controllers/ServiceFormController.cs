using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Models.ServiceForm;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    [Authorize] // Authorize attribute to enforce authentication for this controller
    public class ServiceFormController : Controller
    {
        private readonly IServiceFormRepository _repository;

        public ServiceFormController(IServiceFormRepository repository)
        {
            _repository = repository;
        }

        // GET: ServiceForm/Index
        public IActionResult Index()
        {
            return View(); // Returns the default view for the Index action
        }

        // POST: ServiceForm/Index
        [HttpPost] // Handles HTTP POST requests
        [ValidateAntiForgeryToken] // Helps prevent cross-site request forgery (CSRF) attacks
        public IActionResult Index(ServiceFormViewModel serviceFormViewModel)
        {
            if (ModelState.IsValid) // Checks if the model passed validation
            {
                _repository.Insert(serviceFormViewModel); // Inserts valid data into the repository
                return RedirectToAction("Index", "ServiceOrder"); // Redirects to ServiceOrder/Index action
            }
            
            return View(serviceFormViewModel); // If model state is invalid, returns the view with validation errors
        }
    }
}