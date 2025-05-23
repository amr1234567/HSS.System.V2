using FluentResults;

using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.DataAccess.Contracts;

public interface IMedicalHistoryRepository
{
    Task<Result<IEnumerable<MedicalHistory>>> GetAllMedicalHistoryById(string patientId);
    Task<Result<MedicalHistory>> GetMedicalHistoryById(string medicalHistoryId);
    Task<Result<MedicalHistory>> GetMedicalHistoryByIdInDetails(string medicalHistoryId);
    Task<Result<IEnumerable<MedicalHistory>>> GetAllMedicalHistoryByNationalId(string patientNationlId);
    Task<Result> CreateMedicalHistory(Ticket ticket);
    Task<Result> CreateMedicalHistory(MedicalHistory model);
    Task<Result<IEnumerable<MedicalHistory>>> GetAllMedicalHistoryByIdInDetails(string patientId);
}
