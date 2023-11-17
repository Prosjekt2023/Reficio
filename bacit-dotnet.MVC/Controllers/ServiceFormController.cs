using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Models.ServiceForm;  
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class ServiceFormController : Controller
    {
        private readonly ServiceFormRepository _repository;

        // Add the parameter to the constructor
        public ServiceFormController(ServiceFormRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ServiceFormViewModel serviceFormViewModel)
        {
            if (ModelState.IsValid)
            {
                // Use the injected repository
                _repository.Insert(serviceFormViewModel);
                return RedirectToAction("Index", "ServiceOrder");
            }

            return View(serviceFormViewModel);
        }

        // Other controller actions
    }
}