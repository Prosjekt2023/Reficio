using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    public class ServiceOrderConnectorController : Controller
    {
        private readonly IServiceFormRepository _serviceFormRepository;
        private readonly ICheckListRepository _checkListRepository; // Assuming you have a CheckListRepository

        public ServiceOrderConnectorController(IServiceFormRepository serviceFormRepository,
            ICheckListRepository checkListRepository)
        {
            _serviceFormRepository = serviceFormRepository;
            _checkListRepository = checkListRepository;
        }

        public IActionResult Index(int id) 
        {
            var serviceFormEntry = _serviceFormRepository.GetRelevantData(id);
            var checkListEntry = _checkListRepository.GetRelevantData(id);
/*
           if (serviceFormEntry == null || checkListEntry == null)
            {
                return NotFound(); // Return NotFound only if both are null
            }
*/
            var compositeViewModel = new CompositeViewModel
            {
                ServiceForm = serviceFormEntry,
                CheckList = checkListEntry,
            };

            return View(compositeViewModel);
        }

    }
}