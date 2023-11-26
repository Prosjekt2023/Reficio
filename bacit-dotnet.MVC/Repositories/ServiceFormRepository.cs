using System.Data;
using bacit_dotnet.MVC.Models.Composite;
using Dapper;
using MySqlConnector;
//using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
//using System.Collections.Generic;
//using System.Linq;


namespace bacit_dotnet.MVC.Repositories;

public class ServiceFormRepository : IServiceFormRepository
{
    private readonly IConfiguration _config;

    public ServiceFormRepository(IConfiguration config)
    {
        _config = config;
    }

    public IDbConnection Connection => new MySqlConnection(_config.GetConnectionString("DefaultConnection"));

    public ServiceFormViewModel GetOneRowById(int id)
    {
        using (var dbConnection = Connection)
        {
            dbConnection.Open();
            var query = "SELECT * FROM ServiceFormEntry WHERE ServiceFormId = @Id";
            return dbConnection.QuerySingleOrDefault<ServiceFormViewModel>(query, new { Id = id });
        }
    }


    public IEnumerable<ServiceFormViewModel> GetSomeOrderInfo()
    {
        using (var dbConnection = Connection)
        {
            dbConnection.Open();
            return dbConnection.Query<ServiceFormViewModel>(
                "SELECT ServiceFormId, Customer, DateReceived, OrderNumber FROM ServiceFormEntry");
        }
    }

    public ServiceFormViewModel GetRelevantData(int id)
    {
        using (var dbConnection = Connection)
        {
            dbConnection.Open();
            var query =
                "SELECT ServiceFormId, OrderNumber, Customer, Email, Phone, Address, DateReceived FROM" +
                " ServiceFormEntry WHERE ServiceFormId = @Id";
            return dbConnection.QuerySingleOrDefault<ServiceFormViewModel>(query, new { Id = id });
        }
    }

    public void Insert(ServiceFormViewModel serviceFormViewModel)
    {
        using (var dbConnection = Connection)
        {
            dbConnection.Open();
            dbConnection.Execute(
                "INSERT INTO ServiceFormEntry (ServiceFormId, Customer, DateReceived, Address, Email, OrderNumber, Phone, ProductType, Year, Service, Warranty, SerialNumber, Agreement, RepairDescription, UsedParts, WorkHours, CompletionDate,ReplacedPartsReturned, ShippingMethod, CustomerSignature, RepairerSignature)" +
                " VALUES (@ServiceFormId, @Customer, @DateReceived, @Address, @Email, @OrderNumber, @Phone, @ProductType, @Year, @Service, @Warranty, @SerialNumber, @Agreement, @RepairDescription, @UsedParts, @WorkHours, @CompletionDate, @ReplacedPartsReturned, @ShippingMethod, @CustomerSignature, @RepairerSignature)",
                serviceFormViewModel);
        }
    }
}