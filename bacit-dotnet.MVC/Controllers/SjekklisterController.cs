using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Models.CheckList;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class SjekklisterController : Controller
    {
        private readonly CheckListRepository _repository;

        public SjekklisterController(CheckListRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var Checklist = _repository.GetAll();
            return View(Checklist);
        }
    }
}