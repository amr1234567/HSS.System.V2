using FluentResults;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface IMedicalLabTestResultServices
    {
        Task<Result> CreateTestResultAsync(string medicalLapAppointmentId, TestResultDto result);
        Task<Result<TestResultDto>> GetTestResultForMedicalLabAppointment(string medicalLapAppointmentId);
        Task<Result> UpdateTestResultAsync(string medicalLapAppointmentId, TestResultDto result);
        Task<Result> DeleteTestResultAsync(string medicalLapAppointmentId);
    }

    public record TestResultDto
    {
        [Required]
        public ICollection<TestResultRowDto> ResultRow { get; set; }
        [AllowNull]
        public string TestReport { get; set; }
    }
    public record TestResultRowDto
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public object Value { get; set; }
    }
}
