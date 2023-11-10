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

                // Insert the associated checkpoints into the CheckpointsEntry table
                var checkpoint = new CheckListViewModel
                {
                    CheckpointId = checklistId,
                    ClutchCheck = checkListViewModel.ClutchCheck,
                    BrakeCheck = checkListViewModel.BrakeCheck,
                    DrumBearingCheck = checkListViewModel.DrumBearingCheck,
                    PTOCheck = checkListViewModel.PTOCheck,
                    ChainTensionCheck = checkListViewModel.ChainTensionCheck,
                    WireCheck = checkListViewModel.WireCheck,
                    PinionBearingCheck = checkListViewModel.PinionBearingCheck,
                    ChainWheelKeyCheck = checkListViewModel.ChainWheelKeyCheck,
                    HydraulicCylinderCheck = checkListViewModel.HydraulicCylinderCheck,
                    HoseCheck = checkListViewModel.HoseCheck,
                    HydraulicBlockTest = checkListViewModel.HydraulicBlockTest,
                    TankOilChange = checkListViewModel.TankOilChange,
                    GearboxOilChange = checkListViewModel.GearboxOilChange,
                    RingCylinderSealsCheck = checkListViewModel.RingCylinderSealsCheck,
                    BrakeCylinderSealsCheck = checkListViewModel.BrakeCylinderSealsCheck,
                    WinchWiringCheck = checkListViewModel.WinchWiringCheck,
                    RadioCheck = checkListViewModel.RadioCheck,
                    ButtonBoxCheck = checkListViewModel.ButtonBoxCheck,
                    PressureSettings = checkListViewModel.PressureSettings,
                    FunctionTest = checkListViewModel.FunctionTest,
                    TractionForceKN = checkListViewModel.TractionForceKN,
                    BrakeForceKN = checkListViewModel.BrakeForceKN
                };
                dbConnection.Execute(
                    "INSERT INTO CheckpointsEntry (ClutchCheck, BrakeCheck, DrumBearingCheck, PTOCheck, " +
                    "ChainTensionCheck, WireCheck, PinionBearingCheck, ChainWheelKeyCheck, " +
                    "HydraulicCylinderCheck, HoseCheck, HydraulicBlockTest, TankOilChange, " +
                    "GearboxOilChange, RingCylinderSealsCheck, BrakeCylinderSealsCheck, " +
                    "WinchWiringCheck, RadioCheck, ButtonBoxCheck, PressureSettings, " +
                    "FunctionTest, TractionForceKN, BrakeForceKN) " +
                    "VALUES (@ClutchCheck, @BrakeCheck, @DrumBearingCheck, @PTOCheck, " +
                    "@ChainTensionCheck, @WireCheck, @PinionBearingCheck, @ChainWheelKeyCheck, " +
                    "@HydraulicCylinderCheck, @HoseCheck, @HydraulicBlockTest, @TankOilChange, " +
                    "@GearboxOilChange, @RingCylinderSealsCheck, @BrakeCylinderSealsCheck, " +
                    "@WinchWiringCheck, @RadioCheck, @ButtonBoxCheck, @PressureSettings, " +
                    "@FunctionTest, @TractionForceKN, @BrakeForceKN)",
                    checkpoint
                );
            }
        }
    }
}