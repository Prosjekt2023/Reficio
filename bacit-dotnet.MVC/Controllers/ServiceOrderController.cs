using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    [Authorize]
    public class ServiceOrderController : Controller
    {
        private readonly IServiceFormRepository _repository;

        public ServiceOrderController(IServiceFormRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var serviceFormEntry = _repository.GetSomeOrderInfo();
            return View(serviceFormEntry);
        }

        
    }
}