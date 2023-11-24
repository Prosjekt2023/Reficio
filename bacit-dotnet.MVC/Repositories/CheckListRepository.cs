using Dapper;
using MySqlConnector;
using System.Data;
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

      
        public CheckListViewModel GetOneRowById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM Checklist WHERE ChecklistId = @Id";
                return dbConnection.QuerySingleOrDefault<CheckListViewModel>(query, new { Id = id });
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
/* Insert method that returns the inserted int Id to  */
        public int Insert(CheckListViewModel checkListViewModel)
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

                int insertedId = dbConnection.ExecuteScalar<int>(sql, checkListViewModel);
                return insertedId;
            }
        }
    }
}
