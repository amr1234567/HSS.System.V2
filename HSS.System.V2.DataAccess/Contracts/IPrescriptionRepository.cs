using FluentResults;

using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IPrescriptionRepository
    {
        Task<Result> CreateMedicalPrescription(Prescription model);
        Task<Result> UpdateMedicalPrescription(Prescription model);
        Task<Result<Prescription>> GetMedicalPrescriptionById(string id);
        Task<Result<PrescriptionMedicineItem>> GetMedicalPrescriptionItemById(string id);
        Task<Result<IEnumerable<Prescription>>> GetAllMedicalPrescription(string userId);
    }
}
