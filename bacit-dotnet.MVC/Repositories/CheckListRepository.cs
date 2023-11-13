using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using bacit_dotnet.MVC.Models.CheckList;

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
                var query = "SELECT * FROM CheckpointsEntry";
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

        public void Insert(CheckListViewModel checkListViewModel)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                // Insert the checklist data into the Checklist table
                var checklistId = dbConnection.ExecuteScalar<int>(
                    "INSERT INTO Checklist (Sign, Freeform, CompletionDate) " +
                    "VALUES (@Sign, @Freeform, @CompletionDate); SELECT LAST_INSERT_ID()",
                    new
                    {
                        checkListViewModel.Sign,
                        checkListViewModel.Freeform,
                        checkListViewModel.CompletionDate
                    }
                );

                // Update the ChecklistId in the model with the newly created ChecklistId
                checkListViewModel.ChecklistId = checklistId;

                // Insert the associated checkpoints into the CheckpointsEntry table
                dbConnection.Execute(
                    "INSERT INTO CheckpointsEntry (ChecklistId, ClutchCheck, BrakeCheck, DrumBearingCheck, PTOCheck, " +
                    "ChainTensionCheck, WireCheck, PinionBearingCheck, ChainWheelKeyCheck, " +
                    "HydraulicCylinderCheck, HoseCheck, HydraulicBlockTest, TankOilChange, " +
                    "GearboxOilChange, RingCylinderSealsCheck, BrakeCylinderSealsCheck, " +
                    "WinchWiringCheck, RadioCheck, ButtonBoxCheck, PressureSettings, " +
                    "FunctionTest, TractionForceKN, BrakeForceKN) " +
                    "VALUES (@ChecklistId, @ClutchCheck, @BrakeCheck, @DrumBearingCheck, @PTOCheck, " +
                    "@ChainTensionCheck, @WireCheck, @PinionBearingCheck, @ChainWheelKeyCheck, " +
                    "@HydraulicCylinderCheck, @HoseCheck, @HydraulicBlockTest, @TankOilChange, " +
                    "@GearboxOilChange, @RingCylinderSealsCheck, @BrakeCylinderSealsCheck, " +
                    "@WinchWiringCheck, @RadioCheck, @ButtonBoxCheck, @PressureSettings, " +
                    "@FunctionTest, @TractionForceKN, @BrakeForceKN)",
                    checkListViewModel
                );
            }
        }
    }
}