using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

public record DiseaseForClinicDto : IOutputDto<DiseaseForClinicDto, Disease>
{
    public string DiseaseId { get; set; }
    public string DiseaseName { get; set; }

    public DiseaseForClinicDto MapFromModel(Disease model)
    {
        DiseaseId = model.Id;
        DiseaseName = model.Name;

        return this;
    }
}
