using FluentResults;

using HSS.System.V2.Domain.Models.People;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IPatientRepository
    {
        Task<Result<Patient?>> GetPatientById(string id, params Expression<Func<Patient, object>>[] includes); 
        Task<Result<Patient>> GetPatientWithMedicalHistoryById(string id);
        Task<Result<Patient>> GetPatientWithMedicalHistoryByNationalId(string id);
        Task<Result<Patient?>> GetPatientByNationalId(string nationalId, params Expression<Func<Patient, object>>[] includes);
        Task<Result> UpdatePatientPicture(string userId, string imagePath);
        Task<Result> CreateMedicalHistoryRecordFromEndedTicket(string ticketId);
        Task<Result<Patient?>> GetPatientByNationalIdOrEmail(string nationalId, string email);
        Task<Result> RegisterPatient(Patient patient);
        Task<Result> UpdatePatient(Patient patient);
    }
}
