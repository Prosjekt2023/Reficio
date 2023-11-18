using bacit_dotnet.MVC.Models.CheckList;
using bacit_dotnet.MVC.Models.ServiceForm;

namespace bacit_dotnet.MVC.Models.Composite
{
    public class CompositeViewModel
    {
        public CheckListViewModel CheckList { get; set; }
        public ServiceFormViewModel ServiceForm { get; set; }
    }
}