﻿using Dapper;
//using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using MySqlConnector;
//using System.Collections.Generic;
using System.Data;
//using System.Linq;
using bacit_dotnet.MVC.Models.ServiceForm;

namespace bacit_dotnet.MVC.Repositories
{
    public class ServiceFormRepository
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
        
        public IEnumerable<ServiceFormViewModel> GetSomeOrderInfo()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<ServiceFormViewModel>("SELECT Id, Customer, DateReceived, OrderNumber FROM ServiceFormEntry");
            }
        }

        public void ExistingInsertMethod(ServiceFormViewModel serviceFormViewModel)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO ServiceFormEntry (Customer, DateReceived, Address, Email, OrderNumber, Phone, ProductType, Year, Service, Warranty, SerialNumber, Agreement, RepairDescription, UsedParts, WorkHours, CompletionDate, ReplacedPartsReturned, ShippingMethod, CustomerSignature, RepairerSignature) VALUES (@Customer, @DateReceived, @Address, @Email, @OrderNumber, @Phone, @ProductType, @Year, @Service, @Warranty, @SerialNumber, @Agreement, @RepairDescription, @UsedParts, @WorkHours, @CompletionDate, @ReplacedPartsReturned, @ShippingMethod, @CustomerSignature, @RepairerSignature)", serviceFormViewModel);
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