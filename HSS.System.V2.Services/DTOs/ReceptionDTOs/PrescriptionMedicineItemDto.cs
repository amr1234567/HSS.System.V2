using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class PrescriptionMedicineItemDto : IOutputDto<PrescriptionMedicineItemDto, PrescriptionMedicineItem>
    {
        public string MedicineName { get; set; }
        public string? Instructions { get; set; }
        public int Quantity { get; set; }
        public int TimesPerDay { get; set; }
        public int DurationInDays { get; set; }
        public string MedicineId { get; set; }
        public string PrescriptionId { get; set; }

        public PrescriptionMedicineItemDto MapFromModel(PrescriptionMedicineItem model)
        {
            MedicineId = model.MedicineId;
            MedicineName = model.MedicineName;
            Quantity = model.Quantity;
            Instructions = model.Instructions;
            TimesPerDay = model.TimesPerDay;
            DurationInDays = model.DurationInDays;
            PrescriptionId = model.MedicineId;
            return this;
        }
    }
}
