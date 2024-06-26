using bacit_dotnet.MVC.Models.Composite;

namespace bacit_dotnet.MVC.Repositories;

public interface IServiceFormRepository
{
    public IEnumerable<ServiceFormViewModel> GetSomeOrderInfo();
    public ServiceFormViewModel GetRelevantData(int id);

    public ServiceFormViewModel GetOneRowById(int id);

    void Insert(ServiceFormViewModel serviceFormViewModel);
}