using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; // Required for IEnumerable
using bacit_dotnet.MVC.Models.CheckList; // Required for CheckListViewModel


namespace bacit_dotnet.MVC.Controllers
{
    public class FilledOutCheckListController : Controller
    {
        private readonly CheckListRepository _repository;

        public FilledOutCheckListController(CheckListRepository repository)
        {
            _repository = repository;
        }

        // This action method retrieves all checklist entries and displays them
        public IActionResult Index()
        {
            IEnumerable<CheckListViewModel> checkLists = _repository.GetAll();
            return View(checkLists);
        }

        // Additional action methods can be added here if needed
    }
}