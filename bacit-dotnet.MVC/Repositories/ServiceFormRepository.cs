using Dapper;
//using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using MySqlConnector;
//using System.Collections.Generic;
using System.Data;
//using System.Linq;
using bacit_dotnet.MVC.Models.Composite;


namespace bacit_dotnet.MVC.Repositories
{
    public class ServiceFormRepository : IServiceFormRepository
    {
        private readonly IConfiguration _config;

        // Constructor to inject IConfiguration (for connection string)
        public ServiceFormRepository(IConfiguration config)
        {
            _config = config;
        }
        
        // Property to get IDbConnection using the injected connection string
        public IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public IEnumerable<ServiceFormViewModel> GetAll()
        {
            // Establishes a connection using the Connection property, which returns an IDbConnection
            using (IDbConnection dbConnection = Connection)
            {
                // Opens the database connection
                dbConnection.Open();

                // Executes a SQL query to select all records from the ServiceFormEntry table
                // Maps the result to a collection of ServiceFormViewModel using Dapper's Query method
                return dbConnection.Query<ServiceFormViewModel>("SELECT * FROM ServiceFormEntry");
            }
        }

        public ServiceFormViewModel GetOneRowById(int id)
        {
            // Establishes a connection using the Connection property, which returns an IDbConnection
            using (IDbConnection dbConnection = Connection)
            {
                // Opens the database connection
                dbConnection.Open();

                // Defines the SQL query to select a record based on ServiceFormId parameterized with @Id
                var query = "SELECT * FROM ServiceFormEntry WHERE ServiceFormId = @Id";

                // Executes the SQL query using Dapper's QuerySingleOrDefault method
                // It maps the result to a single ServiceFormViewModel object or null if not found
                return dbConnection.QuerySingleOrDefault<ServiceFormViewModel>(query, new { Id = id });
            }
        }


        
        public IEnumerable<ServiceFormViewModel> GetSomeOrderInfo()
        {
            // Establishes a connection using the Connection property, which returns an IDbConnection
            using (IDbConnection dbConnection = Connection)
            {
                // Opens the database connection
                dbConnection.Open();

                // Executes a SQL query to select specific columns (ServiceFormId, Customer, DateReceived, OrderNumber)
                // Maps the result to a collection of ServiceFormViewModel using Dapper's Query method
                return dbConnection.Query<ServiceFormViewModel>("SELECT ServiceFormId, Customer, DateReceived, OrderNumber FROM ServiceFormEntry");
            }
        }

        
        public ServiceFormViewModel GetRelevantData(int id)
        {
            // Establishes a connection using the Connection property, which returns an IDbConnection
            using (IDbConnection dbConnection = Connection)
            {
                // Opens the database connection
                dbConnection.Open();

                // Defines the SQL query to select specific columns based on ServiceFormId parameterized with @Id
                var query = "SELECT ServiceFormId, OrderNumber, Customer, Email, Phone, Address, DateReceived FROM ServiceFormEntry WHERE ServiceFormId = @Id";

                // Executes the SQL query using Dapper's QuerySingleOrDefault method
                // It maps the result to a single ServiceFormViewModel object or null if not found
                return dbConnection.QuerySingleOrDefault<ServiceFormViewModel>(query, new { Id = id });
            }
        }


        public void Insert(ServiceFormViewModel serviceFormViewModel)
        {
            // Establishes a connection using the Connection property, which returns an IDbConnection
            using (IDbConnection dbConnection = Connection)
            {
                // Opens the database connection
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO ServiceFormEntry (ServiceFormId, Customer, DateReceived, Address, Email, OrderNumber, Phone, ProductType, Year, Service, Warranty, SerialNumber, Agreement, RepairDescription, UsedParts, WorkHours, CompletionDate,ReplacedPartsReturned, ShippingMethod, CustomerSignature, RepairerSignature) VALUES (@ServiceFormId, @Customer, @DateReceived, @Address, @Email, @OrderNumber, @Phone, @ProductType, @Year, @Service, @Warranty, @SerialNumber, @Agreement, @RepairDescription, @UsedParts, @WorkHours, @CompletionDate, @ReplacedPartsReturned, @ShippingMethod, @CustomerSignature, @RepairerSignature)", serviceFormViewModel);
            }
        }
    }
}