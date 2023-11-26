using System.ComponentModel.DataAnnotations;

namespace bacit_dotnet.MVC.Models.Composite;

public class ServiceFormViewModel
{
    public int ServiceFormId { get; set; }

    public string Customer { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateReceived { get; set; }


    public string Address { get; set; }
    public string Email { get; set; }
    public int OrderNumber { get; set; }
    public int Phone { get; set; }
    public string ProductType { get; set; }
    public int Year { get; set; }
    public int SerialNumber { get; set; }

    public string Service { get; set; }
    public string Warranty { get; set; }
    public string Agreement { get; set; }
    public string RepairDescription { get; set; }
    public string UsedParts { get; set; }
    public string WorkHours { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CompletionDate { get; set; }

    public string ReplacedPartsReturned { get; set; }
    public string ShippingMethod { get; set; }
    public string CustomerSignature { get; set; }
    public string RepairerSignature { get; set; }
}