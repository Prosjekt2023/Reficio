
 // We import the composite folder soo the controller can utilize both Checklist
 // and ServiceFormviewmodel through the Compositeviewmodel.
 //Import the Repository folder for necessary repositories
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class ServiceOrderConnectorController : Controller
    {
        private readonly IServiceFormRepository _serviceFormRepository;
        private readonly ICheckListRepository _checkListRepository; // Assuming you have a CheckListRepository

 // Since the connector manages both Checklist and Serviceform we have to have
 // two dependencies
 
        public ServiceOrderConnectorController(IServiceFormRepository serviceFormRepository,
            ICheckListRepository checkListRepository)
        {
            _serviceFormRepository = serviceFormRepository;
            _checkListRepository = checkListRepository;
        }

 // Use an action method to get and IActionResult that
 // -return a parameter, The parameter is int since our
 // -entries are stored with Id's as ints in out database
 // Creating new instance of CompositeViewModel that ServiceOrderConnector can use
 //  We use the method GetRelevantData to get the int for checklistId.
 //  With the Id we can reach the tables of our Repository instance "IRepositories"
 
        public IActionResult Index(int id) 
        {
            var serviceFormEntry = _serviceFormRepository.GetRelevantData(id);
            var checkListEntry = _checkListRepository.GetRelevantData(id);
            var compositeViewModel = new CompositeViewModel
            {
                ServiceForm = serviceFormEntry,
                CheckList = checkListEntry,
            };

            return View(compositeViewModel);
        }

    }
}


