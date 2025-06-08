using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.ResultHelpers.Errors;

using Microsoft.EntityFrameworkCore;

namespace HSS.System.V2.DataAccess.Repositories;

public class MedicalHistoryRepository : IMedicalHistoryRepository
{
    private readonly AppDbContext _context;

    public MedicalHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<MedicalHistory>>> GetAllMedicalHistoryById(string patientId)
    {
        var list = await _context.MedicalHistories
            .Where(p => p.PatientId == patientId)
            .Include(m => m.Patient)
            .ToListAsync();
        return list;
    }

    public async Task<Result<IEnumerable<MedicalHistory>>> GetAllMedicalHistoryByIdInDetails(string patientId)
    {
        var list = await _context.MedicalHistories
            .Where(p => p.PatientId == patientId)
            .Include(m => m.Patient)
            .Include(p => p.Appointments)
                .ThenInclude(a => ((ClinicAppointment)a).MedicalLabAppointments)
            .Include(p => p.Appointments)
                .ThenInclude(a => ((ClinicAppointment)a).RadiologyCeneterAppointments)
            .ToListAsync();
        return list;
    }

    public async Task<Result<IEnumerable<MedicalHistory>>> GetAllMedicalHistoryByNationalId(string patientNationlId)
    {
        var patient = await _context.Patients
        .Include(p => p.MedicalHistories)
          .FirstOrDefaultAsync(p => p.NationalId == patientNationlId);
        if (patient == null)
            return new EntityNotExistsError("patient not found");
        return patient.MedicalHistories.ToList();
    }

    public async Task<Result> CreateMedicalHistory(Ticket ticket)
    {
        var model = new MedicalHistory()
        {
            Appointments = ticket.Appointments,
            CreatedAt = ticket.CreatedAt,
            FirstClinicAppointmentId = ticket.FirstClinicAppointmentId,
            Id = ticket.Id,
            PatientId = ticket.PatientId,
            PatientName = ticket.PatientName,
            PatientNationalId = ticket.PatientNationalId,
            UpdatedAt = HelperDate.GetCurrentDate(),
            TicketId = ticket.Id,
        };
        await _context.MedicalHistories.AddAsync(model);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> CreateMedicalHistory(MedicalHistory model)
    {
        await _context.MedicalHistories.AddAsync(model);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<MedicalHistory>> GetMedicalHistoryById(string medicalHistoryId)
    {
        return await _context.MedicalHistories
          .Where(p => p.Id == medicalHistoryId)
          .Include(m => m.Patient)
          .FirstOrDefaultAsync();
    }

    public async Task<Result<MedicalHistory>> GetMedicalHistoryByIdInDetails(string medicalHistoryId)
    {
        return await _context.MedicalHistories
          .Where(p => p.Id == medicalHistoryId)
          .Include(m => m.Patient)
          .Include(p => p.Appointments)
              .ThenInclude(a => ((ClinicAppointment)a).MedicalLabAppointments)
          .Include(p => p.Appointments)
              .ThenInclude(a => ((ClinicAppointment)a).RadiologyCeneterAppointments)
          .FirstOrDefaultAsync();
    }
}
