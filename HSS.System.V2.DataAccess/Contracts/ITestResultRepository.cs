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
        Task<Result> AddTestResult(IEnumerable<MedicalLabTestResultFieldValue> result);
        Task<Result> UpdateTestResult(IEnumerable<MedicalLabTestResultFieldValue> result, string appointmentId);
        Task<Result> DeleteTestResult(string appointmentId);
        Task<Result<IEnumerable<MedicalLabTestResultFieldValue>>> GetTestResult(string appointmentId);
        Task<Result<IEnumerable<MedicalLabTestResultField>>> GetMedicalLabTestResultFieldsAsync(string tistId);
        Task<Result<MedicalLabTestResultField>> GetTestFieldById(string fieldId);
    }
}
