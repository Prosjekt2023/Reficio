using Dapper;
//using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using MySqlConnector;
//using System.Collections.Generic;
using System.Data;
//using System.Linq;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Models.ServiceForm;

namespace bacit_dotnet.MVC.Repositories
{
    public class ServiceFormRepository : IServiceFormRepository
    {
        private readonly IConfiguration _config;
        public ServiceFormRepository()
        {
            // Legg til eventuell initialisering her
        }

        public ServiceFormRepository(IConfiguration config)
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

        public IEnumerable<ServiceFormViewModel> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<ServiceFormViewModel>("SELECT * FROM ServiceFormEntry");
            }
        }

        public ServiceFormViewModel GetOneRowById(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT * FROM ServiceFormEntry WHERE ServiceFormId = @Id";
                return dbConnection.QuerySingleOrDefault<ServiceFormViewModel>(query, new { Id = id });
            }
        }

        
        public IEnumerable<ServiceFormViewModel> GetSomeOrderInfo()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<ServiceFormViewModel>("SELECT ServiceFormId, Customer, DateReceived, OrderNumber FROM ServiceFormEntry");
            }
        }
        
        public ServiceFormViewModel GetRelevantData(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                var query = "SELECT ServiceFormId, OrderNumber, Customer, Email, Phone, Address, DateReceived FROM ServiceFormEntry WHERE ServiceFormId = @Id";
                return dbConnection.QuerySingleOrDefault<ServiceFormViewModel>(query, new { Id = id });
            }
        }

        public void ExistingInsertMethod(ServiceFormViewModel serviceFormViewModel)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                dbConnection.Execute("INSERT INTO ServiceFormEntry (Customer, DateReceived, Address, Email, OrderNumber, Phone, ProductType, Year, Service, Warranty, SerialNumber, Agreement, RepairDescription, UsedParts, WorkHours, CompletionDate, ReplacedPartsReturned, ShippingMethod, CustomerSignature, RepairerSignature) VALUES (@Customer, @DateReceived, @Address, @Email, @OrderNumber, @Phone, @ProductType, @Year, @Service, @Warranty, @SerialNumber, @Agreement, @RepairDescription, @UsedParts, @WorkHours, @CompletionDate, @ReplacedPartsReturned, @ShippingMethod, @CustomerSignature, @RepairerSignature)", serviceFormViewModel);

                dbConnection.Execute("INSERT INTO ServiceFormEntry (ServiceFormId, Customer, DateReceived, Address, Email, OrderNumber, Phone, ProductType, Year, Service, Warranty, SerialNumber, Agreement, RepairDescription, UsedParts, WorkHours, CompletionDate,ReplacedPartsReturned, ShippingMethod, CustomerSignature, RepairerSignature) VALUES (@ServiceFormId, @Customer, @DateReceived, @Address, @Email, @OrderNumber, @Phone, @ProductType, @Year, @Service, @Warranty, @SerialNumber, @Agreement, @RepairDescription, @UsedParts, @WorkHours, @CompletionDate, @ReplacedPartsReturned, @ShippingMethod, @CustomerSignature, @RepairerSignature)", serviceFormViewModel);

            }
        }
        public int NumberOfTimesInsertCalled { get; private set; }

        public virtual void Insert(ServiceFormViewModel serviceFormViewModel)
        {
            // Implementasjon av Insert-metoden
            NumberOfTimesInsertCalled++;
        }

    }
}