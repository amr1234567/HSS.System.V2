using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class SpecializationDto : IOutputDto<SpecializationDto, Specialization>
    {
        public string Id { set; get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Icon { set; get; }

        public SpecializationDto MapFromModel(Specialization model)
        {
            Id = model.Id;
            Name = model.Name;
            Description = model.Description;
            Icon = model.Icon;
            return this;
        }
    }
}