using HSS.System.V2.Application.DTOs.Clinic;
using HSS.System.V2.Application.Interfaces;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

namespace HSS.System.V2.Application.Services
{
    public class ClinicService : IClinicServices
    {
        private readonly IClinicServices _clinicRepository;
        private readonly IMedicalHistoryRepository _medicalHistoryRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IRadiologyCenterRepository _radiologyRepository;

        public ClinicService(
            IClinicServices clinicRepository,
            IMedicalHistoryRepository medicalHistoryRepository,
            IAppointmentRepository appointmentRepository,
            IRadiologyRepository radiologyRepository)
        {
            _clinicRepository = clinicRepository;
            _medicalHistoryRepository = medicalHistoryRepository;
            _appointmentRepository = appointmentRepository;
            _radiologyRepository = radiologyRepository;
        }

        public async Task<ClinicDetailsDto> GetClinicDetailsAsync(int clinicId)
        {
            var clinic = await _clinicRepository.GetByIdAsync(clinicId);
            return new ClinicDetailsDto
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                Email = clinic.Email,
                OpeningTime = clinic.OpeningTime,
                ClosingTime = clinic.ClosingTime
            };
        }

        public async Task<CurrentClinicDetailsDto> GetCurrentClinicDetailsAsync(int clinicId)
        {
            var clinic = await _clinicRepository.GetByIdAsync(clinicId);
            var appointments = await _appointmentRepository.GetTodayByClinicAsync(clinicId);
            var radiology = await _radiologyRepository.GetTodayByClinicAsync(clinicId);

            return new CurrentClinicDetailsDto
            {
                Clinic = await GetClinicDetailsAsync(clinicId),
                TodayAppointments = appointments.ConvertAll(a => new AppointmentDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = (AppointmentStatusDTO)a.Status
                }),
                RadiologyOrders = radiology.ConvertAll(r => new RadiologyOrderDTO
                {
                    Id = r.Id,
                    PatientId = r.PatientId,
                    DoctorId = r.DoctorId,
                    Type = r.Type,
                    Status = (RadiologyStatusDTO)r.Status
                })
            };
        }

        public async Task<List<MedicalHistoryDto>> GetMedicalHistoriesAsync(int clinicId)
        {
            var histories = await _medicalHistoryRepository.GetByClinicAsync(clinicId);
            return histories.ConvertAll(h => new MedicalHistoryDTO
            {
                Id = h.Id,
                PatientId = h.PatientId,
                Diagnosis = h.Diagnosis,
                Treatment = h.Treatment,
                CreatedAt = h.CreatedAt
            });
        }

        public async Task<MedicalHistoryDto> GetMedicalHistoryByIdAsync(int clinicId, int medicalHistoryId)
        {
            var history = await _medicalHistoryRepository.GetByIdAsync(medicalHistoryId);
            return new MedicalHistoryDto
            {
                Id = history.Id,
                PatientId = history.PatientId,
                Diagnosis = history.Diagnosis,
                Treatment = history.Treatment,
                CreatedAt = history.CreatedAt
            };
        }

        public async Task SubmitClinicResultAsync(int clinicId, ClinicResultRequestDto request)
        {
            await _clinicRepository.SaveResultAsync(
                clinicId,
                request.AppointmentId,
                request.Diagnosis,
                request.Treatment,
                request.Prescription,
                request.Notes);
        }

        public async Task EndAppointmentAsync(int clinicId, int appointmentId)
        {
            await _appointmentRepository.UpdateAppointmentStatusAsync(clinicId, appointmentId);
        }

        Task<ClinicDto> IClinicServices.GetClinicDetailsAsync(int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(int clinicId)
        {
            throw new NotImplementedException();
        }
    }
    }
