using FluentResults;

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

    }
}
