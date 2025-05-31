using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Facilities;

namespace HSS.System.V2.Domain.Models.Medical;

public class MedicalLabTest : Test
{
    public string SampleType { get; set; }

    public virtual ICollection<MedicalLabAppointment> MedicalLabAppointments { get; set; }
    public virtual ICollection<MedicalLab> MedicalLabs { get; set; }


    public virtual ICollection<MedicalLabTestResultField> Fields { get; set; }
}


public class MedicalLabTestResultField : BaseClass
{
    public string KeyName { get; set; }
    public ResultFieldType ResultFieldType { get; set; }
    public bool IsRequired { get; set; }


    public virtual ICollection<MedicalLabTest> Tests { get; set; }
}

public enum ResultFieldType
{
    Text,
    Number,
    Boolean
}