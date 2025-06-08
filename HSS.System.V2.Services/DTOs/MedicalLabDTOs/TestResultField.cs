using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.MedicalLabDTOs
{
    public class TestResultField
    {
        public string Name { get; set; }
        public string ResultFieldType { get; set; }
        public bool IsRequired { get; set; }

        public static TestResultField MapFromModel(MedicalLabTestResultField model)
        {
            return new TestResultField()
            {
                Name = model.KeyName,
                ResultFieldType = model.ResultFieldType.ToString(),
                IsRequired = model.IsRequired
            };
            
        }
    }
}

