using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Medical;

namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

public class MedicineForClinicDto : IOutputDto<MedicineForClinicDto, Medicine>
{
    public string MedicineId { get; set; }
    public string MedicineName { get; set; }

    public MedicineForClinicDto MapFromModel(Medicine model)
    {
        MedicineId = model.Id;
        MedicineName = model.Name;

        return this;
    }
}