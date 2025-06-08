using FluentResults;
using HSS.System.V2.Domain.Models.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface ITestResultRepository
    {
        Task<Result> AddTestResult(ICollection<MedicalLabTestResult> result);
        Task<Result> UpdateTestResult(ICollection<MedicalLabTestResult> result, string appointmentId);
        Task<Result> DeleteTestResult(string appointmentId);
        Task<Result<IEnumerable<MedicalLabTestResult>>> GetTestResult(string appointmentId);
        Task<Result<IEnumerable<MedicalLabTestResultField>>> GetMedicalLabTestResultFieldsAsync(string tistId);
    }
}
