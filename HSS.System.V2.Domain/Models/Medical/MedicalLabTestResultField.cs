using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Models.Common;

namespace HSS.System.V2.Domain.Models.Medical;

public class MedicalLabTestResultField : BaseClass
{
    public string KeyName { get; set; }
    public ResultFieldType ResultFieldType { get; set; }
    public bool IsRequired { get; set; }

    public virtual ICollection<MedicalLabTestResultFieldValue> TestResultFieldValues { get; set; }
    public virtual ICollection<MedicalLabTest> Tests { get; set; }
}
