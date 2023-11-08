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
    
    /*public IActionResult Index()
    {
        var CheckList = _repository.GetAll();
        return View(CheckList);
    }*/
    
    public IActionResult Index(int id)
    {
        var CheckList = _repository.GetAll(id);
        if (CheckList == null)
        {
            return NotFound();
        }
        return View(CheckList);
    }
}