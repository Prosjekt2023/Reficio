using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class CheckListOrderController : Controller
    {
        private readonly CheckListRepository _repository;

        public CheckListOrderController(CheckListRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var Checklist = _repository.GetSomeOrderInfo();
            return View(Checklist);
        }
    }
}