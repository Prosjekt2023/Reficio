using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers;

public class FilledOutCheckListController : Controller
{
    private readonly CheckListRepository _repository;

    public FilledOutCheckListController(CheckListRepository repository)
    {
        _repository = repository;
    }
    
    public IActionResult Index(int id)
    {
        var Checklist = _repository.GetOneRowById(id);
        if (Checklist == null)
        {
            return NotFound();
        }
        return View(Checklist);
    }
}