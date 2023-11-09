using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers;

public class FilledOutServiceFormController : Controller
{
    private readonly ServiceFormRepository _repository;

    public FilledOutServiceFormController(ServiceFormRepository repository)
    {
        _repository = repository;
    }
    
    public IActionResult Index(int id)
    {
        var serviceFormEntry = _repository.GetOneRowById(id);
        if (serviceFormEntry == null)
        {
            return NotFound();
        }
        return View(serviceFormEntry);
    }
}