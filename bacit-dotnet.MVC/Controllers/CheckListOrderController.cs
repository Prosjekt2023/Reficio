using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class CheckListOrderController : Controller
    {
        private readonly ICheckListRepository _repository;

        public CheckListOrderController(ICheckListRepository repository)
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