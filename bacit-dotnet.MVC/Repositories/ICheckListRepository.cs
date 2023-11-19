using bacit_dotnet.MVC.Models.Composite;

namespace bacit_dotnet.MVC.Repositories;

    public interface ICheckListRepository
    {
        public IEnumerable<CheckListViewModel> GetAll();
        
        public IEnumerable<CheckListViewModel> GetSomeOrderInfo();

        public CheckListViewModel GetOneRowById(int id);

        public CheckListViewModel GetRelevantData(int id);
        void Insert(CheckListViewModel checkListViewModel);
    }