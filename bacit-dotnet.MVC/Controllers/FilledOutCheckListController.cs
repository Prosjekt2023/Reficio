using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    [Authorize]
    public class FilledOutCheckListController : Controller
    {
        private readonly ICheckListRepository _repository;
/*
 * Use a dependency injection soo the controller
 * -can leave the implementation to the repository instance
 * - "Irepository"
 */
        public FilledOutCheckListController(ICheckListRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index(int id)
        {
            var checklist = _repository.GetOneRowById(id);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }
    }
}