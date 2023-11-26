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
/*
 * We made an GetOneRowById to get a specific row
 * entry in the database, this is soo we can retrieve a specific filledoutChecklist
 */  
        public CheckListViewModel GetOneRowById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM Checklist WHERE ChecklistId = @Id";
                return dbConnection.QuerySingleOrDefault<CheckListViewModel>(query, new { Id = id });
            }
        }
/*
 * We made an GetRelevantData to get a specific parameters as
 * - and int, this is soo we can query the database for a specific ChecklistId that is type int
 */
        
        public CheckListViewModel GetRelevantData(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT ChecklistId FROM Checklist WHERE ChecklistId = @Id";
                return dbConnection.QuerySingleOrDefault<CheckListViewModel>(query, new { Id = id });
            }
        }
/* Insert method that returns the inserted int Id to the viewmodel
*The function makes the checklist visible after its filled out 
*/
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
