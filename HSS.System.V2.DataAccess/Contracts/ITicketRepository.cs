using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Prescriptions;

namespace HSS.System.V2.DataAccess.Contracts
{
    public interface ITicketRepository
    {
        Task<Result> CreateTicket(Ticket model);
        Task<Result> UpdateTicket(Ticket model);
        Task<Result> DeleteTicket(Ticket model);
        Task<Result<Ticket>> GetTicketById(string ticketId);
        Task<Result<Ticket>> GetTicketByIdInDetails(string ticketId);
        Task<Result<PagedResult<Ticket>>> GetOpenTicketsForPatient(string patientId, int size = 10, int page = 1);
        Task<Result<IEnumerable<Ticket>>> GetOpenTicketsForPatient(string patientId);
        Task<Result<PagedResult<Ticket>>> GetAllTicketForPatient(string patientId, int size = 10, int page = 1);
        Task<Result<IEnumerable<Ticket>>> GetAllTicketForPatient(string patientId);
        Task<Result<PagedResult<Ticket>>> GetAllTicketForPatientByNationalId(string patientNationalId, int size = 10, int page = 1);
        Task<Result<PagedResult<Ticket>>> GetAllOpenedTicketInHospitalForPatient(string hospitalId, string patientId, int size = 10, int page = 1);
        Task<Result<IEnumerable<Ticket>>> GetAllOpenedTicketInHospitalForPatient(string hospitalId, string patientId);
        Task<Result> DeleteTicket(string ticketId);
        Task<Result<bool>> IsTicketHasReExaminationNow(string ticketId);
    }
}
