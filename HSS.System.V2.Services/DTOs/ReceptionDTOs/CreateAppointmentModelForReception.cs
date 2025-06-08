using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Common;

using System.ComponentModel.DataAnnotations;

namespace HSS.System.V2.Services.DTOs.ReceptionDTOs;

/// <summary>
/// Model that recive the data that responsible for create a new appointment
/// </summary>
public record CreateAppointmentModelForReception
{
    [Required]
    public DateTime ExpectedTimeForStart { get; set; }
    [Required]
    public string NationalId { get; set; }
}

/// <inheritdoc/>
public record CreateClinicAppointmentModelForReception : CreateAppointmentModelForReception, IInputModel<ClinicAppointment>
{
    [Required]
    public string ClinicId { set; get; }
    [Required]
    public string TicketId { get; set; }

    /// <inheritdoc/>
    public ClinicAppointment ToModel()
    {
        return new()
        {
            ClinicId = ClinicId,
            Id = Guid.NewGuid().ToString(),
            TicketId = TicketId,
            State = Domain.Enums.AppointmentState.NotStarted,
            CreatedAt = HelperDate.GetCurrentDate(),
            UpdatedAt = HelperDate.GetCurrentDate(),
            SchaudleStartAt = ExpectedTimeForStart
        };
    }
}

/// <inheritdoc/>
public record CreateRadiologyAppointmentModelForReception : CreateAppointmentModelForReception, IInputModel<RadiologyCeneterAppointment>
{
    [Required]
    public string RadiologyCenterId { get; set; }
    public string? TicketId { get; set; }
    public string? TextRequiredId { get; set; }
    public string? TestId { set; get; }


    /// <inheritdoc/>
    public RadiologyCeneterAppointment ToModel()
    {
        return new()
        {
            SchaudleStartAt = ExpectedTimeForStart,
            CreatedAt = HelperDate.GetCurrentDate(),
            Id = Guid.NewGuid().ToString(),
            UpdatedAt = HelperDate.GetCurrentDate(),
            State = Domain.Enums.AppointmentState.NotStarted,
            RadiologyCeneterId = RadiologyCenterId,
        };
    }
}

/// <inheritdoc/>
public record CreateMedicalLabAppointmentModelForReception : CreateAppointmentModelForReception, IInputModel<MedicalLabAppointment>
{
    [Required]
    public string MedicalLabId { get; set; }
    public string? TicketId { get; set; }
    public string? TextRequiredId { get; set; }


    /// <inheritdoc/>
    public MedicalLabAppointment ToModel()
    {
        return new()
        {
            SchaudleStartAt = ExpectedTimeForStart,
            CreatedAt = HelperDate.GetCurrentDate(),
            Id = Guid.NewGuid().ToString(),
            UpdatedAt = HelperDate.GetCurrentDate(),
            State = Domain.Enums.AppointmentState.NotStarted,
            MedicalLabId = MedicalLabId
        };
    }
}
