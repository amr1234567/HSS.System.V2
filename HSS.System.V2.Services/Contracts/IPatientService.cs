﻿using FluentResults;

using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Services.DTOs.PatientDTOs;

using Microsoft.AspNetCore.Http;

namespace HSS.System.V2.Services.Contracts
{
    public interface IPatientService
    {
        #region Not Used
        //Task<Result<List<HospitalDto>>> GetHospitalByCity(int cityId);
        //Task<Result<List<HospitalDto>>> GetHospitalsById(string? SpecificationId, SpecificationType? type,  string? hospitalName, bool? nearest = false);
        //Task<Result<List<HospitalDto>>> GetHospitalsBySpecialization(string specializationId);
        #endregion

        #region Notification
        Task<Result<int>> NotificationCount();
        Task<Result<List<NotificationDto>>> GetNotifications();
        Task<Result<NotificationData>> GetNotificationData(string notificationId);
        Task<Result> SeenNotification(string notificationId);
        #endregion

        #region Survey
        Task<Result<DiabetesResponse?>> SugerTest(DiabetesTestModel model);
        #endregion

        #region Home
        Task<Result<PagedResult<AppointmentView>>> GetCurrentAppontments(PaginationRequest pagination);
        Task<Result<object>> GetAppointmentDetails(string appointmentId);
        #endregion

        #region Appointment
        Task<Result<PagedResult<SpecialzationDto>>> GetAllSpecifications(PaginationRequest pagination);
        Task<Result<PagedResult<RadiologyTestDto>>> GetAllRadiologyTest(PaginationRequest pagination);
        Task<Result<PagedResult<MedicalLabTestDto>>> GetAllMedicalLabTest(PaginationRequest pagination);
        Task<Result<PagedResult<HospitalDto>>> GetHospitalsBySpecificationId( string specializationId, PaginationRequest pagination);
        Task<Result<PagedResult<HospitalDto>>> GetHospitalsByRadiologyTestId( string radiologyTestId, PaginationRequest pagination);
        Task<Result<PagedResult<HospitalDto>>> GetHospitalsByMedicalLabTestId( string medicalLabTestId, PaginationRequest pagination);
        Task<Result<List<DebartmentDto>>> GetClinics(string hospitalId, string SpecificationId);
        Task<Result<List<DebartmentDto>>> GetRadiologyCenter(string hospitalId, string TestId);
        Task<Result<List<DebartmentDto>>> GetMedicalLabs(string hospitalId, string TestId);
        Task<Result> CreateTicket(string hospitalId);
        Task<Result> CreateClinicAppointment(CreateClinicAppointmentModelForPatient model);
        Task<Result> CreateRadiologyAppointMent(CreateRadiologyAppointmentModelForPatient model);
        Task<Result> CreateMedicalLabAppointment(CreateMedicalLabAppointmentModelForPatient model);
        #endregion

        #region Required
        Task<Result<List<TestRquiredForPatientDto>>> GetMedicalLabTestsRequired();
        Task<Result<List<TestRquiredForPatientDto>>> GetRadiologyTestsRequired();
        Task<Result<PagedResult<PrescriptionDto>>> GetAllPrescriptionsRequired(PaginationRequest pagination);
        Task<Result<List<MedicineDto>>> GetMedicineByPrescriptionId(string prescriptionId);
        #endregion

        #region Current Appointments
        Task<Result<PagedResult<AppointmentView>>> GetCurrentClinicAppointments(PaginationRequest pagination);
        Task<Result<PagedResult<AppointmentView>>> GetCurrentMedicalLabAppointments(PaginationRequest pagination);
        Task<Result<PagedResult<AppointmentView>>> GetCurrentRadiologyCenterAppointments(PaginationRequest pagination);
        #endregion

        #region Profile
        Task<Result> UploadProfilePicture(IFormFile file);
        Task<Result<ProfileInformationDto>> GetPatientProfileInformation();
        Task<Result<List<TicketInformationDto>>> GetAllActiveTicketsOfPatient();
        Task<Result<List<TicketInformationDto>>> GetAllInactiviTicketsOfPatient();
        Task<Result<PagedResult<AppointmentView>>> GetTicketContent(string ticketId, PaginationRequest pagination);
        Task<Result<object>> GetAppointmentContent(string appointmentId);
        Task<Result> CancelAppointment(string appointmentId);
        Task<Result<List<SpecialzationDto>>> GetAllSpecialzations();
        Task<Result<IEnumerable<TicketViewDto>>> GetActiveTicketInHospital(string hospitalId);
        Task<Result<PagedResult<TicketViewDto>>> GetActiveTicketForPatient(PaginationRequest pagination);
        Task<Result<FinalStepBookingAppointmentDetails>> GetFinalStepBookingAppointmentDetailsAsync(string appointmentId);
        Task<Result<TestRquiredForPatientDto>> GetTestRequiredById(string testRequiredId);
        #endregion
    }
} 
