using HSS.System.V2.Domain.Models.Common;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs
{
    public class PrescriptionDto : IOutputDto<PrescriptionDto, Prescription>
    {
        public string? Notes { get; set; }

        public string ClinicAppointmentId { get; set; }
        public virtual IEnumerable<PrescriptionMedicineItemDto> Items { get; set; }
        public PrescriptionDto MapFromModel(Prescription model)
        {
            Notes = model.Notes;
            ClinicAppointmentId = model.ClinicAppointmentId;
            Items = model.Items is null ? [] : model.Items.Select(i => new PrescriptionMedicineItemDto().MapFromModel(i));
            return this;
        }
    }
}
