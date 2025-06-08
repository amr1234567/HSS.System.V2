using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ClinicDTOs
{
    public record TestRequiredNeeded : IInputModel<TestRequired>
    {
        public string TestId { get; set; }
        public TestRequired ToModel()
        {
            return new()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = HelperDate.GetCurrentDate(),
                UpdatedAt = HelperDate.GetCurrentDate(),
                TestId = TestId,
                Used = false,
            };
        }
    }
}