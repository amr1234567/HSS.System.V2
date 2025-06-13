using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

using System.Text.Json.Serialization;

namespace HSS.System.V2.Services.DTOs.GeeneralDTOs
{
    public record TestDto<T> : IOutputDto<TestDto<T>, T> where T : Test
    {
        public string TestId { set; get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double TestPrice { get; set; }
        public double EstimatedDurationInMinutes { get; set; }
        public string? Icon { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? SampleType { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BodyPart { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RequiresContrast { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PreparationInstructions { get; set; }


        public TestDto<T> MapFromModel(T model)
        {
            TestId = model.Id;
            Name = model.Name;
            Description = model.Description;
            TestPrice = model.TestPrice;
            EstimatedDurationInMinutes = model.EstimatedDurationInMinutes;
            Icon = model.Icon;
            if (model is MedicalLabTest labTest)
            {
                SampleType = labTest.SampleType;
            }
            else if (model is RadiologyTest radiologyTest)
            {
                BodyPart = radiologyTest.BodyPart;
                RequiresContrast = radiologyTest.RequiresContrast;
                PreparationInstructions = radiologyTest.PreparationInstructions;
            }
            return this;
        }
    }
}
