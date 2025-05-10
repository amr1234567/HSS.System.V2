using FluentResults;
using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.DataAccess.Repositories
{
    public class PatientRepository(AppDbContext context) : IPatientRepository
    {
        public Task<Result> CreateMedicalHistoryRecordFromEndedTicket(string ticketId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Patient>> GetPatientById(string id, params Expression<Func<Patient, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Patient>> GetPatientByNationalId(string nationalId, params Expression<Func<Patient, object>>[] includes)
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

        public Task<Result> UpdatePatientDetails(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}
