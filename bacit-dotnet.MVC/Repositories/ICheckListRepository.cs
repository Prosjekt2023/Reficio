using bacit_dotnet.MVC.Models.CheckList;

namespace bacit_dotnet.MVC.Repositories;

    public interface ICheckListRepository
    {
        public IEnumerable<CheckListViewModel> GetAll();
        
        public IEnumerable<CheckListViewModel> GetSomeOrderInfo();

        public CheckListViewModel GetOneRowById(int id);
        void Insert(CheckListViewModel checkListViewModel);
    }