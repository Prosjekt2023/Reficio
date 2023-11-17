using bacit_dotnet.MVC.Models.Composite;

namespace bacit_dotnet.MVC.Repositories;

    public interface IServiceFormRepository
    {
        public IEnumerable<ServiceFormViewModel> GetAll();

        public IEnumerable<ServiceFormViewModel> GetSomeOrderInfo();
        
        
    }