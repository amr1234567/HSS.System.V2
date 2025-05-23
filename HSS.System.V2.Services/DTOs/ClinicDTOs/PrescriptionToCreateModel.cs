using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Services.DTOs.ClinicDTOs;

public record PrescriptionToCreateModel : IInputModel<Prescription>
{
    public string? Notes { get; set; }
    public List<PrescriptionMedicineItemToCreate> Items { get; set; }

    public Prescription ToModel()
    {
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Notes = Notes,
            Items = Items.Select(item => item.ToModel()).ToList()
        };
    }
}

public class PrescriptionMedicineItemToCreate : IInputModel<PrescriptionMedicineItem>
{
    public string? Instructions { get; set; }
    public int Quantity { get; set; }
    public int TimesPerDay { get; set; }
    public int DurationInDays { get; set; }
    public string MedicineId { get; set; }
    public string PrescriptionId { get; set; }

    public PrescriptionMedicineItem ToModel()
    {
        return new()
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Instructions = Instructions,
            Quantity = Quantity,
            TimesPerDay = TimesPerDay,
            DurationInDays = DurationInDays,
            MedicineId = MedicineId,
            PrescriptionId = PrescriptionId
        };
    }
}