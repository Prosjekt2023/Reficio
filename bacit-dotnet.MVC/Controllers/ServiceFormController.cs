using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using bacit_dotnet.MVC.Models.ServiceForm;  

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
                _repository.Insert(serviceFormViewModel);
                return RedirectToAction("Index", "ServiceOrder");
            }

            return View(serviceFormViewModel);
        }

        // Other controller actions
    }

    }
