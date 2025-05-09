using FluentResults;

using HSS.System.V2.Domain.People;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IPatientRepository
    {
        Task<Result<Patient>> GetPatientById(string id, params Expression<Func<Patient, object>>[] includes); 
        Task<Result<Patient>> GetPatientWithMedicalHistoryById(string id);
        Task<Result<Patient>> GetPatientWithMedicalHistoryByNationalId(string id);
        Task<Result<Patient>> GetPatientByNationalId(string nationalId, params Expression<Func<Patient, object>>[] includes);
        Task<Result> UpdatePatientDetails(Patient patient);
        Task<Result> CreateMedicalHistoryRecordFromEndedTicket(string ticketId);
    }
}
