namespace HSS.System.V2.Domain.DTOs;

public record HospitalDepartments
{
    public int NumberOfClinics { get; set; }
    public int NumberOfRadiologyCenters { get; set; }
    public int NumberOfMedicalLabs { get; set; }
    public int NumberOfPharmacies { get; set; }
    public int NumberOfReceptions { get; set; }

}
