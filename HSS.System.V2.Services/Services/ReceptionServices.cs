using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.DataAccess.Repositories;
using HSS.System.V2.Domain.DTOs;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.PatientDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HSS.System.V2.Services.Services
{
    public class ReceptionServices : IReceptionServices
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IQueueRepository _queueRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly ISpecializationReporitory _specializationReporitory;
        private readonly ITestsRepository _testsRepository;
        private readonly ITestRequiredRepository _testRequiredRepository;

        public ReceptionServices(IAppointmentRepository appointmentRepository, IQueueRepository queueRepository,
            IPatientRepository patientRepository, ITicketRepository ticketRepository,
            IMedicineRepository medicineRepository, IHospitalRepository hospitalRepository,
            ISpecializationReporitory specializationReporitory, ITestsRepository testsRepository,
            ITestRequiredRepository testRequiredRepository)
        {
            _appointmentRepository = appointmentRepository;
            _queueRepository = queueRepository;
            _patientRepository = patientRepository;
            _ticketRepository = ticketRepository;
            _medicineRepository = medicineRepository;
            _hospitalRepository = hospitalRepository;
            _specializationReporitory = specializationReporitory;
            _testsRepository = testsRepository;
            _testRequiredRepository = testRequiredRepository;
        }

        public async Task<Result<HospitalDepartments>> GetAllHospitalDepartmentsInHospital(string hospitalId)
        {
            var depts = await _hospitalRepository.GetAllHospitalDepartments(hospitalId);
            return depts;
        }

        public async Task<Result<PagedResult<SpecializationDto>>> GetAllSpecializationsInHospital(string hospitalId, int page, int pageSize)
        {
            var specilizations = await _specializationReporitory.GetAllInHospitalAsync(hospitalId);
            if (specilizations.IsFailed)
                return Result.Fail(specilizations.Errors);
            return specilizations.Value
                .Select(s => new SpecializationDto().MapFromModel(s)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<ClinicDto>>> GetAllClinics(string specializationId, string hospitalId, int page, int pageSize)
        {
            var clinics = await _hospitalRepository.GetHospitalDepartmentItems<Clinic>(hospitalId);
            if (clinics.IsFailed)
                return Result.Fail(clinics.Errors);
            return clinics.Value.Select(c => new ClinicDto().MapFromModel(c)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<RadiologyCenterDto>>> GetAllRadiologyCenters(string hospitalId, int page, int pageSize)
        {
            var centers = await _hospitalRepository.GetHospitalDepartmentItems<RadiologyCenter>(hospitalId);
            if (centers.IsFailed)
                return Result.Fail(centers.Errors);
            return centers.Value.Select(c => new RadiologyCenterDto().MapFromModel(c)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<RadiologyCenterDto>>> GetAllRadiologyCentersDoTest(string hospitalId, string radiologyTestId, int page, int pageSize)
        {
            var centers = await _hospitalRepository.GetHospitalDepartmentItems<RadiologyCenter>(hospitalId);
            if (centers.IsFailed)
                return Result.Fail(centers.Errors);
            return centers.Value.Where(c => _testsRepository.IsRadiologyCenterDoTest(c.Id, radiologyTestId).Value)
                .Select(c => new RadiologyCenterDto().MapFromModel(c)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<MedicalLabDto>>> GetAllMedicalLabs(string hospitalId, int page, int pageSize)
        {
            var centers = await _hospitalRepository.GetHospitalDepartmentItems<MedicalLab>(hospitalId);
            if (centers.IsFailed)
                return Result.Fail(centers.Errors);
            return centers.Value.Select(c => new MedicalLabDto().MapFromModel(c)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<MedicalLabDto>>> GetAllMedicalLabsDoTest(string hospitalId, string medicalTestId, int page, int pageSize)
        {
            var centers = await _hospitalRepository.GetHospitalDepartmentItems<MedicalLab>(hospitalId);
            if (centers.IsFailed)
                return Result.Fail(centers.Errors);
            return centers.Value.Where(c => _testsRepository.IsMedicalLabDoTest(c.Id, medicalTestId).Value)
                .Select(c => new MedicalLabDto().MapFromModel(c)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForClinic(string clinicId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10)
        {
            var apps = await _appointmentRepository.GetAllForClinicAsync(clinicId, new(dateFrom, dateTo));
            return apps.Value.Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForClinic(string clinicId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<ClinicQueue>(clinicId);
            return apps.Value.ClinicAppointments
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForRadiologyCenter(string radiologyCenterId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10)
        {
            var apps = await _appointmentRepository.GetAllForRadiologyCenterAsync(radiologyCenterId, new(dateFrom, dateTo));
            return apps.Value.Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForRadiologyCenter(string radiologyCenterId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<RadiologyCenterQueue>(radiologyCenterId);
            return apps.Value.RadiologyCeneterAppointments
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForMedicalLab(string medicalLabId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10)
        {
            var apps = await _appointmentRepository.GetAllForMedicalLabAsync(medicalLabId, new(dateFrom, dateTo));
            return apps.Value.Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForMedicalLab(string medicalLabId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<MedicalLabQueue>(medicalLabId);
            return apps.Value.MedicalLabAppointments
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PatientDetailsWithTicketsDto>> GetOpenTicketsForPatientByNationalId(string nationalId, int page, int pageSize)
        {
            var patient =await _patientRepository.GetPatientByNationalId(nationalId);
            if (patient.IsFailed || patient.Value == null)
                return new BadRequestError("no patient found with this national id");
            var tickets = await _ticketRepository.GetOpenTicketsForPatient(patient.Value.Id, page: page, size: pageSize);
            if (tickets.IsFailed)
                return Result.Fail(tickets.Errors);
            return new PatientDetailsWithTicketsDto()
            {
                PatientId = patient.Value.Id,
                Tickets = tickets.Value,
                PatientName = patient.Value.Name,
                PatientNationalId = nationalId
            };
        }

        public async Task<Result<PagedResult<TicketDto>>> GetOpenTicketsForPatientById(string patientId, int page, int pageSize)
        {
            var patient = await _patientRepository.GetPatientById(patientId);
            if (patient.IsFailed || patient.Value == null)
                return new BadRequestError("no patient found with this national id");
            var tickets = await _ticketRepository.GetOpenTicketsForPatient(patientId, page: page, size: pageSize);
            if (tickets.IsFailed)
                return Result.Fail(tickets.Errors);
            return new PatientDetailsWithTicketsDto()
            {
                PatientId = patient.Value.Id,
                Tickets = tickets.Value,
                PatientName = patient.Value.Name,
                PatientNationalId = patient.Value.NationalId
            };
        }

        public async Task<Result> CreateAppointment(CreateClinicAppointmentModelForReception model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientByNationalId(model.NationalId);
            if (patient.IsFailed)
                return Result.Fail(patient.Errors);
            var clinic = await _hospitalRepository.GetHospitalDepartmentItem<Clinic>(model.ClinicId);
            if (clinic.IsFailed)
                return Result.Fail(clinic.Errors);
            var ticket = await _ticketRepository.GetTicketById(model.TicketId);
            if (ticket.IsFailed)
                return Result.Fail(ticket.Errors);
            var clinicAppointment = ticket.Value.FirstClinicAppointment;
            var entity = model.ToModel();
            entity.ClinicId = clinic.Value!.Id;
            entity.DepartmentName = clinic.Value.Name;
            entity.HospitalName = clinic.Value.Hospital.Name;
            entity.HospitalId = clinic.Value.HospitalId;
            entity.PatientNationalId = patient.Value.NationalId;
            entity.PatientName = patient.Value.Name;
            entity.ExpectedDuration = clinic.Value.PeriodPerAppointment;

            var check = await _ticketRepository.IsTicketHasReExaminationNow(model.TicketId);
            if (check.Value)
            {
                while (clinicAppointment.ReExamiationClinicAppointemnt is not null)
                {
                    clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
                }
                clinicAppointment.ReExamiationClinicAppointemntId = entity.Id;
                entity.PreExamiationClinicAppointemntId = clinicAppointment.Id;
            }
            else
            {
                ticket.Value.FirstClinicAppointment = entity;
            }


            return await _ticketRepository.UpdateTicket(ticket.Value);
        }

        public async Task<Result> CreateAppointment(CreateRadiologyAppointmentModelForReception model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientByNationalId(model.NationalId);
            if (patient.IsFailed)
                return Result.Fail(patient.Errors);
            var radiologyCenter = await _hospitalRepository.GetHospitalDepartmentItem<RadiologyCenter>(model.RadiologyCenterId);
            if (radiologyCenter.IsFailed)
                return Result.Fail(radiologyCenter.Errors);
            var entity = model.ToModel();
            var ticket = await _ticketRepository.GetTicketById(model.TicketId);
            if (ticket.IsFailed)
            {
                var testRequired = await _testRequiredRepository.GetTestRequiredByIdAsync(model.TextRequiredId);
                if (!testRequired.IsFailed && testRequired.Value is not null)
                {
                    entity.ClinicAppointmentId = testRequired.Value.ClinicAppointmentId;
                    testRequired.Value.Used = true;
                    await _testRequiredRepository.UpdateTestRequiredAsync(testRequired.Value);
                }
                else
                {
                    return new BadRequestError("you must provide ticket id or test required id");
                }
            }
            entity.TicketId = ticket.Value.Id;
            entity.ClinicAppointmentId = null;
            entity.RadiologyCeneterId = radiologyCenter.Value!.Id;
            entity.DepartmentName = radiologyCenter.Value!.Name;
            entity.HospitalName = radiologyCenter.Value.Hospital.Name;
            entity.PatientName = patient.Value.Name;
            entity.HospitalId = radiologyCenter.Value.Hospital.Id;
            entity.PatientNationalId = patient.Value.NationalId;
            entity.ExpectedDuration = radiologyCenter.Value.PeriodPerAppointment;

            return await  _ticketRepository.UpdateTicket(ticket.Value);
        }

        public async Task<Result> CreateAppointment(CreateMedicalLabAppointmentModelForReception model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientById(model.NationalId);
            if (patient.IsFailed)
                return Result.Fail(patient.Errors);
            var medicalLab = await _hospitalRepository.GetHospitalDepartmentItem<MedicalLab>(model.MedicalLabId);
            if (medicalLab.IsFailed)
                return Result.Fail(medicalLab.Errors);
            var entity = model.ToModel();
            var ticket = await _ticketRepository.GetTicketById(model.TicketId);
            if (ticket.IsFailed)
            {
                var testRequired = await _testRequiredRepository.GetTestRequiredByIdAsync(model.TextRequiredId);
                if (!testRequired.IsFailed && testRequired.Value is not null)
                {
                    entity.ClinicAppointmentId = testRequired.Value.ClinicAppointmentId;
                    testRequired.Value.Used = true;
                    await _testRequiredRepository.UpdateTestRequiredAsync(testRequired.Value);
                }
                else
                {
                    return new BadRequestError("you must provide ticket id or test required id");
                }
            }
            entity.TicketId = ticket.Value.Id;
            entity.ClinicAppointmentId = null;
            entity.MedicalLabId = medicalLab.Value!.Id;
            entity.DepartmentName = medicalLab.Value.Name;
            entity.HospitalName = medicalLab.Value.Hospital.Name;
            entity.PatientName = patient.Value.Name;
            entity.HospitalId = medicalLab.Value.HospitalId;
            entity.PatientNationalId = patient.Value.NationalId;
            entity.ExpectedDuration = medicalLab.Value.PeriodPerAppointment;

            return await  _ticketRepository.UpdateTicket(ticket.Value);
        }

        public async Task<Result> TerminateAppointment(string appointmentId)
        {
            return await ChangeAppointmentState(appointmentId, AppointmentState.Terminated);
        }

        public async Task<Result> SwapClinicAppointments(string appointmentId1, string appointmentId2)
        {
            var queueId = "";
            return await _appointmentRepository.GetAppointmentByIdAsync(appointmentId1)
                .OnSuccessAsync(a => queueId = a.QueueId)
                .ThenAsync(a1 => _appointmentRepository.GetAppointmentByIdAsync(appointmentId2)
                .EnsureNoneAsync((a2) => a2.QueueId == queueId, new BadArgumentsError("Appointments must be in the same queue."))
                .ThenAsync(a2 => _appointmentRepository.SwapAppointmentsAsync<ClinicAppointment>(appointmentId1, appointmentId2)));
        }
        public async Task<Result> SwapMedicalLabAppointments(string appointmentId1, string appointmentId2)
        {
            var queueId = "";
            return await _appointmentRepository.GetAppointmentByIdAsync(appointmentId1)
                .OnSuccessAsync(a => queueId = a.QueueId)
                .ThenAsync(a1 => _appointmentRepository.GetAppointmentByIdAsync(appointmentId2)
                .EnsureNoneAsync((a2) => a2.QueueId == queueId, new BadArgumentsError("Appointments must be in the same queue."))
                .ThenAsync(a2 => _appointmentRepository.SwapAppointmentsAsync<MedicalLabAppointment>(appointmentId1, appointmentId2)));
        }
        public async Task<Result> SwapRadiologyCenterAppointments(string appointmentId1, string appointmentId2)
        {
            var queueId = "";
            return await _appointmentRepository.GetAppointmentByIdAsync(appointmentId1)
                .OnSuccessAsync(a => queueId = a.QueueId)
                .ThenAsync(a1 => _appointmentRepository.GetAppointmentByIdAsync(appointmentId2)
                .EnsureNoneAsync((a2) => a2.QueueId == queueId, new BadArgumentsError("Appointments must be in the same queue."))
                .ThenAsync(a2 => _appointmentRepository.SwapAppointmentsAsync<RadiologyCeneterAppointment>(appointmentId1, appointmentId2)));
        }

        public Task<Result> RescheduleAppointment(string appointmentId, string departmentId, DateTime newDateTime)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ConfirmPatientEntryIntoRoom(string roomId, string nationalId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> ConfirmPatientLeaveTheRoom(string roomId, string nationalId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RemoveAppointmentFromQueue(string appointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> AddClinicAppointmentForQueue(string appointmentId, string departmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> AddMedicalLabAppointmentForQueue(string appointmentId, string departmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> AddRadiologyCenterAppointmentForQueue(string appointmentId, string departmentId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CreateNewTicket(CreateTicketModel model)
        {
            throw new NotImplementedException();
        }

        public Task<Result> CloseTicket(string ticketId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<DateTime>>> GetAvailableTimeSlotsForClinic(string clinicId, DateTime? date)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<DateTime>>> GetAvailableTimeSlotsForMedicalLab(string medicalLabId, DateTime? date)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<DateTime>>> GetAvailableTimeSlotsForRadiologyCenter(string radiologyCenterId, DateTime? date)
        {
            throw new NotImplementedException();
        }

        public Task<Result> StartAppointment(string appointmentId)
        {
            throw new NotImplementedException();
        }

        private async Task<Result> ChangeAppointmentState(string appointmentId, AppointmentState newState)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId)
                .OnSuccessAsync(a => a.State = newState);
            if (appointment.IsFailed)
                return Result.Fail(appointment.Errors);
            return await _appointmentRepository.UpdateAppointmentAsync(appointment.Value);
        }
    }
}
