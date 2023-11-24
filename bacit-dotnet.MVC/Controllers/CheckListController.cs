/*
 *Author Johannes aka Lord Of CheckLists
 * Patch 2.1 patch makes it possible to view created checklists in realtime
 * Irepository is updated from void to int
 */

using bacit_dotnet.MVC.Models.Composite;
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    /*
     *The controller's constructor accepts an ICheckListRepository object.
     *The _repository private readonly field stores the repository object.*/
    public class CheckListController : Controller
    {
        private readonly ICheckListRepository _repository;

        public CheckListController (ICheckListRepository repository)
        {
            _repository = repository;
        }
       /*
        * public IActionResult Index() is a method that handles HTTP GET requests
        * It returns the default view associated with this action,
        * which is typically a page where users can view or start filling out a checklist
        */
        public IActionResult Index()
        {
            return View();
        }
        /*
         * [HttpPost]from the Index method deals with POST requests
         * -from CheckList viewpage that uses the POST Method to submit the form data
         * 
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckListViewModel checkListViewModel)
        {
            if (ModelState.IsValid)
            {
                var id = _repository.Insert(checkListViewModel); // Returns the entry that was created using the defined method in the repository
                return RedirectToAction("Index", "FilledOutCheckList", new { id = id }); // Reddirects to FilledOutCheckListController
            }
            
            return View(checkListViewModel);
        }
        
    }
}