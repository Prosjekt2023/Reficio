/*
 *Forfatter Johannes aka Lord Of CheckLists
 * Patch 2.1 Redirigerer etter opprettelse av instanse til spesifike int verdi
 * Irepository er også oppdatert fra void tul int
 */

using bacit_dotnet.MVC.Models.Composite;
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class CheckListController : Controller
    {
        private readonly ICheckListRepository _repository;

        public CheckListController (ICheckListRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckListViewModel checkListViewModel)
        {
            if (ModelState.IsValid)
            {
                var id = _repository.Insert(checkListViewModel); // Når instansen insertes i databsen returneres int verdien
                return RedirectToAction("Index", "FilledOutCheckList", new { id = id }); // Redirigerer til FilledOutCheckListController
            }
            
            return View(checkListViewModel);
        }
        
    }
}