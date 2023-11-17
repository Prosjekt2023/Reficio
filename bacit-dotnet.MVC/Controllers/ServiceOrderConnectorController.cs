using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Controllers
{
    [Authorize]
    public class ServiceOrderConnectorController : Controller
    {
        private readonly ServiceFormRepository _serviceFormRepository;
        private readonly CheckListRepository _checkListRepository; // Assuming you have a CheckListRepository

        public ServiceOrderConnectorController(ServiceFormRepository serviceFormRepository, CheckListRepository checkListRepository)
        {
            _serviceFormRepository = serviceFormRepository;
            _checkListRepository = checkListRepository;
        }

        public IActionResult Index(int id) // 
        {
            var serviceFormEntry = _serviceFormRepository.GetRelevantData(id);
            var checkListEntry = _checkListRepository.GetRelevantData(id); // Assuming you have a method to get CheckList data

            if (serviceFormEntry == null || checkListEntry == null)
            {
                return NotFound();
            }

            var compositeViewModel = new CompositeViewModel
            {
                ServiceForm = serviceFormEntry,
                CheckList = checkListEntry,
            };

            return View(compositeViewModel);
        }
    }