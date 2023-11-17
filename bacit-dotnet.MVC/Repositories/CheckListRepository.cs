using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using bacit_dotnet.MVC.Models.CheckList;
using bacit_dotnet.MVC.Models.Composite;

namespace bacit_dotnet.MVC.Repositories
{
    public class CheckListRepository : ICheckListRepository
    {
        private readonly IConfiguration _config;

        public CheckListRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public IEnumerable<CheckListViewModel> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM Checklist";
                var results = dbConnection.Query<CheckListViewModel>(query);

                return results;
            }
        }
        
        public CheckListViewModel GetOneRowById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM Checklist WHERE ChecklistId = @Id";
                return dbConnection.QuerySingleOrDefault<CheckListViewModel>(query, new { Id = id });
            }
        }
        
        public IEnumerable<CheckListViewModel> GetSomeOrderInfo()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<CheckListViewModel>("SELECT ChecklistId, Sign, Freeform, CompletionDate FROM Checklist");
            }
        }
        
        public CheckListViewModel GetRelevantData(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT ChecklistId FROM Checklist WHERE ChecklistId = @Id";
                return dbConnection.QuerySingleOrDefault<CheckListViewModel>(query, new { Id = id });
            }
        }

        public void Insert(CheckListViewModel checkListViewModel)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                var sql = "INSERT INTO Checklist (ClutchCheck, BrakeCheck, DrumBearingCheck, PTOCheck, " +
                          "ChainTensionCheck, WireCheck, PinionBearingCheck, ChainWheelKeyCheck, " +
                          "HydraulicCylinderCheck, HoseCheck, HydraulicBlockTest, TankOilChange, " +
                          "GearboxOilChange, RingCylinderSealsCheck, BrakeCylinderSealsCheck, " +
                          "WinchWiringCheck, RadioCheck, ButtonBoxCheck, PressureSettings, " +
                          "FunctionTest, TractionForceKN, BrakeForceKN, Sign, Freeform, CompletionDate) " +
                          "VALUES (@ClutchCheck, @BrakeCheck, @DrumBearingCheck, @PTOCheck, " +
                          "@ChainTensionCheck, @WireCheck, @PinionBearingCheck, @ChainWheelKeyCheck, " +
                          "@HydraulicCylinderCheck, @HoseCheck, @HydraulicBlockTest, @TankOilChange, " +
                          "@GearboxOilChange, @RingCylinderSealsCheck, @BrakeCylinderSealsCheck, " +
                          "@WinchWiringCheck, @RadioCheck, @ButtonBoxCheck, @PressureSettings, " +
                          "@FunctionTest, @TractionForceKN, @BrakeForceKN, @Sign, @Freeform, @CompletionDate); " +
                          "SELECT LAST_INSERT_ID()";

                var checklistId = dbConnection.ExecuteScalar<int>(sql, checkListViewModel);

                checkListViewModel.ChecklistId = checklistId;
            }
        }
    }
}
