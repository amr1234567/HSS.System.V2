using FluentResults;

using HSS.System.V2.Domain.DTOs;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs.PatientDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

namespace HSS.System.V2.Services.Contracts
{
    public interface IReceptionServices
    {
        #region Hospital Entities
        // --------------------------------------------------------------------
        // Methods related to retrieving hospital departments, rooms, specializations,
        // clinics, radiology centers, medical labs, and equipment.
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves all hospital departments in the specified hospital.
        /// </summary>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <returns>A <see cref="Result{T}"/> containing an enumerable collection of <see cref="HospitalDepartmentDto"/> objects.</returns>
        Task<Result<HospitalDepartments>> GetAllHospitalDepartmentsInHospital(string hospitalId);

        /// <summary>
        /// Retrieves all specializations in the specified hospital.
        /// </summary>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="SpecializationDto"/> objects.</returns>
        Task<Result<PagedResult<SpecializationDto>>> GetAllSpecializationsInHospital(string hospitalId, int page, int pageSize);

        /// <summary>
        /// Retrieves all clinics for a given specialization in the specified hospital.
        /// </summary>
        /// <param name="specializationId">The unique identifier of the specialization.</param>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="ClinicDto"/> objects.</returns>
        Task<Result<PagedResult<ClinicDto>>> GetAllClinics(string specializationId, string hospitalId, int page, int pageSize);

        /// <summary>
        /// Retrieves all radiology centers in the specified hospital.
        /// </summary>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="RadiologyCenterDto"/> objects.</returns>
        Task<Result<PagedResult<RadiologyCenterDto>>> GetAllRadiologyCenters(string hospitalId, int page, int pageSize);

        /// <summary>
        /// Retrieves all radiology centers that perform a specific test in the specified hospital.
        /// </summary>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <param name="radiologyTestId">The unique identifier of the radiology test.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="RadiologyCenterDto"/> objects.</returns>
        Task<Result<PagedResult<RadiologyCenterDto>>> GetAllRadiologyCentersDoTest(string hospitalId, string radiologyTestId, int page, int pageSize);

        /// <summary>
        /// Retrieves all medical labs in the specified hospital.
        /// </summary>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="MedicalLabDto"/> objects.</returns>
        Task<Result<PagedResult<MedicalLabDto>>> GetAllMedicalLabs(string hospitalId, int page, int pageSize);

        /// <summary>
        /// Retrieves all medical labs that perform a specific test in the specified hospital.
        /// </summary>
        /// <param name="hospitalId">The unique identifier of the hospital.</param>
        /// <param name="medicalTestId">The unique identifier of the medical test.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="MedicalLabDto"/> objects.</returns>
        Task<Result<PagedResult<MedicalLabDto>>> GetAllMedicalLabsDoTest(string hospitalId, string medicalTestId, int page, int pageSize);

        #endregion

        #region Appointments
        // --------------------------------------------------------------------
        // Methods related to appointment management including retrieving appointments
        // and their queues.
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves all appointments for the specified clinic.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="AppointmentDto"/> objects.</returns>
        Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForClinic(string clinicId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10);

        /// <summary>
        /// Retrieves the queue of appointments for the specified clinic.
        /// </summary>
        /// <param name="clinicId">The unique identifier of the clinic.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a paged result of <see cref="AppointmentDto"/> objects representing the queue.
        /// </returns>
        Task<Result<PagedResult<AppointmentDto>>> GetQueueForClinic(string clinicId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Retrieves all appointments for the specified radiology center.
        /// </summary>
        /// <param name="radiologyCenterId">The unique identifier of the radiology center.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="AppointmentDto"/> objects.</returns>
        Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForRadiologyCenter(string radiologyCenterId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10);

        /// <summary>
        /// Retrieves the queue of appointments for the specified radiology center.
        /// </summary>
        /// <param name="radiologyCenterId">The unique identifier of the radiology center.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a paged result of <see cref="AppointmentDto"/> objects representing the queue.
        /// </returns>
        Task<Result<PagedResult<AppointmentDto>>> GetQueueForRadiologyCenter(string radiologyCenterId, int page = 1, int pageSize = 10);

        /// <summary>
        /// Retrieves all appointments for the specified medical lab.
        /// </summary>
        /// <param name="medicalLabId">The unique identifier of the medical lab.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="AppointmentDto"/> objects.</returns>
        Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForMedicalLab(string medicalLabId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10);

        /// <summary>
        /// Retrieves the queue of appointments for the specified medical lab.
        /// </summary>
        /// <param name="medicalLabId">The unique identifier of the medical lab.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a paged result of <see cref="AppointmentDto"/> objects representing the queue.
        /// </returns>
        Task<Result<PagedResult<AppointmentDto>>> GetQueueForMedicalLab(string medicalLabId, int page = 1, int pageSize = 10);

        #endregion

        #region Tickets
        // --------------------------------------------------------------------
        // Methods related to retrieving open tickets and ticket details.
        // --------------------------------------------------------------------

        /// <summary>
        /// Retrieves all open tickets for a patient using their national ID.
        /// </summary>
        /// <param name="nationalId">The national identifier of the patient.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="TicketDto"/> objects.</returns>
        Task<Result<PatientDetailsWithTicketsDto>> GetOpenTicketsForPatientByNationalId(string nationalId, int page, int pageSize);

        /// <summary>
        /// Retrieves all open tickets for a patient using their unique identifier.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="Result{T}"/> containing a paged result of <see cref="TicketDto"/> objects.</returns>
        Task<Result<PagedResult<TicketDto>>> GetOpenTicketsForPatientById(string patientId, int page, int pageSize);

        #endregion

        #region Appointment Actions
        // --------------------------------------------------------------------
        // Methods for creating, modifying, and confirming appointments.
        // --------------------------------------------------------------------

        /// <summary>
        /// Creates a new clinic appointment.
        /// </summary>
        /// <param name="model">A <see cref="CreateClinicAppointmentModelForPatient"/> containing the appointment details.</param>
        /// <returns>A <see cref="Result"/> indicating whether the appointment creation was successful.</returns>
        Task<Result> CreateAppointment(CreateClinicAppointmentModelForReception model);

        /// <summary>
        /// Creates a new radiology appointment.
        /// </summary>
        /// <param name="model">A <see cref="CreateRadiologyAppointmentModelForPatient"/> containing the appointment details.</param>
        /// <returns>A <see cref="Result"/> indicating whether the appointment creation was successful.</returns>
        Task<Result> CreateAppointment(CreateRadiologyAppointmentModelForReception model);

        /// <summary>
        /// Creates a new medical lab appointment.
        /// </summary>
        /// <param name="model">A <see cref="CreateMedicalLabAppointmentModelForPatient"/> containing the appointment details.</param>
        /// <returns>A <see cref="Result"/> indicating whether the appointment creation was successful.</returns>
        Task<Result> CreateAppointment(CreateMedicalLabAppointmentModelForReception model);

        /// <summary>
        /// Terminates an existing appointment.
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment to be terminated.</param>
        /// <returns>A <see cref="Result"/> indicating whether the termination was successful.</returns>
        Task<Result> TerminateAppointment(string appointmentId);

        /// <summary>
        /// Reschedules an appointment to a new date and time.
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment.</param>
        /// <param name="newDateTime">The new date and time for the appointment.</param>
        /// <returns>A <see cref="Result"/> indicating whether the rescheduling was successful.</returns>
        Task<Result> RescheduleAppointment(string appointmentId, string departmentId, DateTime newDateTime);

        /// <summary>
        /// Confirms a patient's entry into a specified room.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <param name="nationalId">The national identifier of the patient.</param>
        /// <returns>A <see cref="Result"/> indicating whether the entry confirmation was successful.</returns>
        Task<Result> ConfirmPatientEntryIntoRoom(string roomId, string nationalId);

        /// <summary>
        /// Confirms that a patient has left a specified room.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room.</param>
        /// <param name="nationalId">The national identifier of the patient.</param>
        /// <returns>A <see cref="Result"/> indicating whether the exit confirmation was successful.</returns>
        Task<Result> ConfirmPatientLeaveTheRoom(string roomId, string nationalId);

        #endregion

        #region Queue Actions
        // --------------------------------------------------------------------
        // Methods for managing appointment queues such as adding, removing,
        // and swapping appointments within the queue.
        // --------------------------------------------------------------------

        /// <summary>
        /// Removes an appointment from the queue.
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment to be removed.</param>
        /// <returns>A <see cref="Result"/> indicating whether the removal was successful.</returns>
        Task<Result> RemoveAppointmentFromQueue(string appointmentId);

        /// <summary>
        /// Adds an appointment to the queue.
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment to be added.</param>
        /// <returns>A <see cref="Result"/> indicating whether the addition was successful.</returns>
        Task<Result> AddClinicAppointmentForQueue(string appointmentId, string departmentId);
        Task<Result> AddMedicalLabAppointmentForQueue(string appointmentId, string departmentId);
        Task<Result> AddRadiologyCenterAppointmentForQueue(string appointmentId, string departmentId);


        #endregion

        #region Ticket Actions
        // --------------------------------------------------------------------
        // Methods for ticket management including creating new tickets and processing payments.
        // --------------------------------------------------------------------

        /// <summary>
        /// Creates a new ticket based on the provided details.
        /// </summary>
        /// <param name="model">A <see cref="CreateTicketModel"/> containing the ticket details.</param>
        /// <returns>A <see cref="Result"/> indicating whether the ticket creation was successful.</returns>
        Task<Result> CreateNewTicket(CreateTicketModel model);

        Task<Result> CloseTicket(string ticketId);
        Task<Result<List<DateTime>>> GetAvailableTimeSlotsForClinic(string clinicId, DateTime? date);
        Task<Result<List<DateTime>>> GetAvailableTimeSlotsForMedicalLab(string medicalLabId, DateTime? date);
        Task<Result<List<DateTime>>> GetAvailableTimeSlotsForRadiologyCenter(string radiologyCenterId, DateTime? date);
        Task<Result> StartAppointment(string appointmentId);
        Task<Result> SwapClinicAppointments(string appointmentId1, string appointmentId2);
        Task<Result> SwapMedicalLabAppointments(string appointmentId1, string appointmentId2);
        Task<Result> SwapRadiologyCenterAppointments(string appointmentId1, string appointmentId2);

        #endregion
    }

}
