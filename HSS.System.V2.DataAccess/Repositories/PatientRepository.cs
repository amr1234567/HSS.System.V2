using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.People;

using System.Linq.Expressions;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        public Task<Result<Patient>> GetPatientById(string id, params Expression<Func<Patient, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Patient>> GetPatientWithMedicalHistoryById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Patient>> GetPatientWithMedicalHistoryByNationalId(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Patient>> GetPatientByNationalId(string nationalId, params Expression<Func<Patient, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdatePatientDetails(Patient patient)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateMedicalHistoryRecordFromEndedTicket(string ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
