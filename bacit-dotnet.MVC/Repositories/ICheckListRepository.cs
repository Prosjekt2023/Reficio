using bacit_dotnet.MVC.Models.Composite;

namespace bacit_dotnet.MVC.Repositories;

    public interface ICheckListRepository
    {


        public CheckListViewModel GetOneRowById(int id);

        public CheckListViewModel GetRelevantData(int id);
        int Insert(CheckListViewModel checkListViewModel);
    }