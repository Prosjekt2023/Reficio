using System.ComponentModel.DataAnnotations;

namespace bacit_dotnet.MVC.Models.Composite;

public class CheckListViewModel
{
    public int ChecklistId { get; set; } 
    public string ClutchCheck { get; set; }
    public string BrakeCheck { get; set; }
    public string DrumBearingCheck { get; set; }
    public string PTOCheck { get; set; }
    public string ChainTensionCheck { get; set; }
    public string WireCheck { get; set; }
    public string PinionBearingCheck { get; set; }
    public string ChainWheelKeyCheck { get; set; }
    public string HydraulicCylinderCheck { get; set; }
    public string HoseCheck { get; set; }
    public string HydraulicBlockTest { get; set; }
    public string TankOilChange { get; set; }
    public string GearboxOilChange { get; set; }
    public string RingCylinderSealsCheck { get; set; }
    public string BrakeCylinderSealsCheck { get; set; }
    public string WinchWiringCheck { get; set; }
    public string RadioCheck { get; set; }
    public string ButtonBoxCheck { get; set; }
    public string PressureSettings { get; set; }
    public string FunctionTest { get; set; }
    public string TractionForceKN { get; set; }
    public string BrakeForceKN { get; set; }
    
    public string Freeform { get; set; }
    
    public string Sign { get; set; }
    
    [Required(ErrorMessage = "CompletionDate is required.")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CompletionDate { get; set; }
    
    
}