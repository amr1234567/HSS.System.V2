using FluentResults;

using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.Domain.DTOs;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.People;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.Models.Queues;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.Contracts;
using HSS.System.V2.Services.DTOs.GeeneralDTOs;
using HSS.System.V2.Services.DTOs.ReceptionDTOs;
using HSS.System.V2.Services.Helpers;

using Microsoft.EntityFrameworkCore;

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
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly INotificationRepository _notificationRepository;
        private readonly AppDbContext _context;

        public ReceptionServices(IAppointmentRepository appointmentRepository, IQueueRepository queueRepository,
            IPatientRepository patientRepository, ITicketRepository ticketRepository,
            IMedicineRepository medicineRepository, IHospitalRepository hospitalRepository,
            ISpecializationReporitory specializationReporitory, ITestsRepository testsRepository,
            ITestRequiredRepository testRequiredRepository, IBackgroundTaskQueue taskQueue,
            INotificationRepository notificationRepository, AppDbContext context)
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
            _taskQueue = taskQueue;
            _notificationRepository = notificationRepository;
            _context = context;
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
            var clinics = await _hospitalRepository.GetAllClinicsBySpecilizationId(specializationId, hospitalId);
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
            var patient = await _patientRepository.GetPatientByNationalId(nationalId);
            if (patient.IsFailed || patient.Value == null)
                return new BadRequestError("no patient found with this national id");
            var tickets = await _context.Tickets
                    .AsNoTracking()
                    .Where(t => t.PatientNationalId == nationalId && t.State == TicketState.Active)
                    .Select(t => new TicketDto().MapFromModel(t))
                    .ToListAsync();
            return new PatientDetailsWithTicketsDto()
            {
                PatientId = patient.Value.Id,
                Tickets = tickets.GetPaged(page, pageSize),
                PatientName = patient.Value.Name,
                PatientNationalId = nationalId
            };
        }

        public async Task<Result<PagedResult<TicketDto>>> GetOpenTicketsForPatientById(string patientId, int page, int pageSize)
        {
            var patient = await _patientRepository.GetPatientById(patientId);
            if (patient.IsFailed || patient.Value == null)
                return new BadRequestError("no patient found with this national id");
            var tickets = await _ticketRepository.GetOpenTicketsForPatient(patientId);
            if (tickets.IsFailed)
                return Result.Fail(tickets.Errors);
            return tickets.Value.Select(t => new TicketDto().MapFromModel(t)).GetPaged(page, pageSize);
        }

        public async Task<Result> CreateAppointment(CreateClinicAppointmentModelForReception model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientByNationalId(model.NationalId)
                .EnsureNoneAsync((p => p is null, new EntityNotExistsError("هذا المريض غير موجود ")));
            if (patient.IsFailed)
                return Result.Fail(patient.Errors);

            var clinic = await _hospitalRepository.GetHospitalDepartmentItem<Clinic>(model.ClinicId)
                .EnsureNoneAsync((c => c is null, new EntityNotExistsError("هذه العيادة غير موجودة")));
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
            entity.PatientId = patient.Value.Id;
            entity.ExpectedDuration = clinic.Value.PeriodPerAppointment;
            entity.EmployeeName = string.Empty;

            var check = await _ticketRepository.IsTicketHasReExaminationNow(model.TicketId);
            if (string.IsNullOrEmpty(ticket.Value.FirstClinicAppointmentId))
            {
                ticket.Value.FirstClinicAppointment = entity;
                ticket.Value.Appointments.Add(entity);
            }
            else if (check.Value)
            {
                while (clinicAppointment.ReExamiationClinicAppointemnt is not null)
                {
                    clinicAppointment = clinicAppointment.ReExamiationClinicAppointemnt;
                }
                clinicAppointment.ReExamiationClinicAppointemntId = entity.Id;
                entity.PreExamiationClinicAppointemntId = clinicAppointment.Id;
                ticket.Value.Appointments.Add(entity);
            }
            else
            {
                return new BadRequestError("هذه التذكرة ممتلئة، من فضلك افتح تذكرة جديدة");
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
                    entity.TicketId = testRequired.Value.ClinicAppointment.TicketId;
                    await _testRequiredRepository.UpdateTestRequiredAsync(testRequired.Value);
                }
                else
                {
                    return new BadRequestError("you must provide ticket id or test required id");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ticket.Value.FirstClinicAppointmentId))
                {
                    entity.TicketId = ticket.Value.Id;
                    entity.ClinicAppointmentId = null;
                }
                else
                {
                    return new BadRequestError("this ticket already has clinic appointment");
                }
            }
            entity.DepartmentName = radiologyCenter.Value!.Name;
            entity.HospitalName = radiologyCenter.Value.Hospital.Name;
            entity.PatientName = patient.Value.Name;
            entity.PatientId = patient.Value.Id;
            entity.HospitalId = radiologyCenter.Value.Hospital.Id;
            entity.PatientNationalId = patient.Value.NationalId;
            entity.EmployeeName = string.Empty;
            entity.ExpectedDuration = radiologyCenter.Value.PeriodPerAppointment;

            return await _appointmentRepository.CreateAppointmentAsync(entity);
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
            else
            {
                if (string.IsNullOrEmpty(ticket.Value.FirstClinicAppointmentId))
                {
                    entity.TicketId = ticket.Value.Id;
                }
                else
                {
                    return new BadRequestError("this ticket already has clinic appointment");
                }
            }
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

        public async Task<Result> RescheduleClinicAppointment(string appointmentId, string departmentId, DateTime newDateTime)
        {
            var app = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (app.IsFailed)
                return Result.Fail(app.Errors);

            var inDateApp = await _appointmentRepository.GetAppointmentByDateTimeInDepartmentAsync<Clinic>(departmentId, newDateTime);
            if (inDateApp.IsSuccess)
                return new BadRequestError("Date is already resived");

            app.Value.SchaudleStartAt = newDateTime;
            var result = await _appointmentRepository.UpdateAppointmentAsync(app.Value);
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                _queueRepository.RecalculateQueueAppointmentTimes<ClinicQueue>(app.Value.QueueId);
            });
            return result;
        }
        
        public async Task<Result> RescheduleMedicalLabAppointment(string appointmentId, string departmentId, DateTime newDateTime)
        {
            var app = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (app.IsFailed)
                return Result.Fail(app.Errors);

            var inDateApp = await _appointmentRepository.GetAppointmentByDateTimeInDepartmentAsync<MedicalLab>(departmentId, newDateTime);
            if (inDateApp.IsSuccess)
                return new BadRequestError("Date is already resived");

            app.Value.SchaudleStartAt = newDateTime;
            var result = await _appointmentRepository.UpdateAppointmentAsync(app.Value);
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                _queueRepository.RecalculateQueueAppointmentTimes<MedicalLabQueue>(app.Value.QueueId);
            });
            return result;
        }
        
        public async Task<Result> RescheduleRadiologyAppointment(string appointmentId, string departmentId, DateTime newDateTime)
        {
            var app = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (app.IsFailed)
                return Result.Fail(app.Errors);

            var inDateApp = await _appointmentRepository.GetAppointmentByDateTimeInDepartmentAsync<RadiologyCenter>(departmentId, newDateTime);
            if (inDateApp.IsSuccess)
                return new BadRequestError("Date is already resived");

            app.Value.SchaudleStartAt = newDateTime;
            var result = await _appointmentRepository.UpdateAppointmentAsync(app.Value);
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                _queueRepository.RecalculateQueueAppointmentTimes<RadiologyCenterQueue>(app.Value.QueueId);
            });
            return result;
        }

        public async Task<Result> RemoveClinicAppointmentFromQueue(string appointmentId)
        {
            var app = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (app.IsFailed)
                return Result.Fail(app.Errors);
            if (string.IsNullOrEmpty(app.Value.QueueId))
                return new BadRequestError("The appointment not in any queue");
            return await _queueRepository.RemoveAppointmentFromQueue<ClinicAppointment, ClinicQueue>(appointmentId);
        }

        public async Task<Result> RemoveMedicalLabAppointmentFromQueue(string appointmentId)
        {
            var app = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (app.IsFailed)
                return Result.Fail(app.Errors);
            if (string.IsNullOrEmpty(app.Value.QueueId))
                return new BadRequestError("The appointment not in any queue");
            return await _queueRepository.RemoveAppointmentFromQueue<MedicalLabAppointment, MedicalLabQueue>(appointmentId);
        }

        public async Task<Result> RemoveRadiologyCenterAppointmentFromQueue(string appointmentId)
        {
            var app = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (app.IsFailed)
                return Result.Fail(app.Errors);
            if (string.IsNullOrEmpty(app.Value.QueueId))
                return new BadRequestError("The appointment not in any queue");
            return await _queueRepository.RemoveAppointmentFromQueue<RadiologyCeneterAppointment, RadiologyCenterQueue>(appointmentId);
        }

        public async Task<Result> AddClinicAppointmentForQueue(string appointmentId)
        {

            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<ClinicAppointment>(appointmentId)
              .OnSuccessAsync(a => a.State = AppointmentState.InQueue)
              .ThenWithFirstReturnAsync(_appointmentRepository.UpdateAppointmentAsync);

            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);

          
            var app = appResult.Value;

            var now = DateTime.UtcNow;
            var appointmentDate = app.SchaudleStartAt.Date;


            var departmentResult = await _hospitalRepository.GetHospitalDepartmentItem<Clinic>(app.ClinicId);
            if (departmentResult.IsFailed)
                return Result.Fail(departmentResult.Errors);

            // Get clinic's start datetime on the day of the appointment
            var clinicStartDateTime = appointmentDate.Add(departmentResult.Value.StartAt);

            var twelveHoursBeforeClinic = clinicStartDateTime.AddHours(-12);

            var check = now < twelveHoursBeforeClinic || now >= app.SchaudleStartAt;

            if (check)
                return new BadRequestError("لا يمكنك الحضور الآن. الحضور متاح قبل الموعد بحد أقصى ١٢ ساعة، وحتى وقت الموعد فقط.");

            var queueResult = await _queueRepository.GetQueueByDepartmentId<ClinicQueue>(app.ClinicId);
            if (queueResult.IsFailed)
            {
                var addQueueResult = await _queueRepository.CreateQueue(new ClinicQueue
                {
                    Id = Guid.NewGuid().ToString(),
                    ClinicId = app.ClinicId,
                });
                if (addQueueResult.IsFailed)
                    return Result.Fail(addQueueResult.Errors);
                queueResult = await _queueRepository.GetQueueByDepartmentId<ClinicQueue>(app.ClinicId);
                if (queueResult.IsFailed)
                    return Result.Fail(queueResult.Errors);
            }
            var result = await _queueRepository.AddAppointmentToQueue<ClinicAppointment ,ClinicQueue>(appointmentId, queueResult.Value.Id);
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                var ticket = await _ticketRepository.GetTicketById(appResult.Value.TicketId);
                if (ticket.IsFailed)
                    return;

                await _notificationRepository.CreateNotification(new()
                {
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString(),
                    PatientId = ticket.Value.PatientId,
                    Seen = false,
                    UpdatedAt = DateTime.UtcNow,
                    ///TO Implement
                    MessageData = null,
                    Data = null,
                });
            });

            return result;
        }

        public async Task<Result> AddMedicalLabAppointmentForQueue(string appointmentId)
        {
           
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<MedicalLabAppointment>(appointmentId)
              .OnSuccessAsync(a => a.State = AppointmentState.InQueue)
              .ThenWithFirstReturnAsync(_appointmentRepository.UpdateAppointmentAsync);
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);


            var app = appResult.Value;
            var now = DateTime.UtcNow;
            var appointmentDate = app.SchaudleStartAt.Date;


            var departmentResult = await _hospitalRepository.GetHospitalDepartmentItem<MedicalLab>(app.MedicalLabId);
            if (departmentResult.IsFailed)
                return Result.Fail(departmentResult.Errors);

            // Get clinic's start datetime on the day of the appointment
            var clinicStartDateTime = appointmentDate.Add(departmentResult.Value.StartAt);

            var twelveHoursBeforeClinic = clinicStartDateTime.AddHours(-12);

            var check = now < twelveHoursBeforeClinic || now >= app.SchaudleStartAt;

            if (check)
                return new BadRequestError("لا يمكنك الحضور الآن. الحضور متاح قبل الموعد بحد أقصى ١٢ ساعة، وحتى وقت الموعد فقط.");

            var queueResult = await _queueRepository.GetQueueByDepartmentId<MedicalLabQueue>(app.MedicalLabId);
            if (queueResult.IsFailed)
            {
                var addQueueResult = await _queueRepository.CreateQueue(new MedicalLabQueue
                {
                    Id = Guid.NewGuid().ToString(),
                    MedicalLabId = app.MedicalLabId,
                });
                if (addQueueResult.IsFailed)
                    return Result.Fail(addQueueResult.Errors);
                queueResult = await _queueRepository.GetQueueByDepartmentId<MedicalLabQueue>(app.MedicalLabId);
                if (queueResult.IsFailed)
                    return Result.Fail(queueResult.Errors);
            }
            return await _queueRepository.AddAppointmentToQueue<MedicalLabAppointment, MedicalLabQueue>(appointmentId, queueResult.Value.Id);
        }

        public async Task<Result> AddRadiologyCenterAppointmentForQueue(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId)
              .OnSuccessAsync(a => a.State = AppointmentState.InQueue)
              .ThenWithFirstReturnAsync(_appointmentRepository.UpdateAppointmentAsync);

            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);

            var app = appResult.Value;
            var now = DateTime.UtcNow;
            var appointmentDate = app.SchaudleStartAt.Date;


            var departmentResult = await _hospitalRepository.GetHospitalDepartmentItem<RadiologyCenter>(app.RadiologyCeneterId);
            if (departmentResult.IsFailed)
                return Result.Fail(departmentResult.Errors);
            if (departmentResult.Value is null)
                return new BadRequestError();

            // Get clinic's start datetime on the day of the appointment
            var clinicStartDateTime = appointmentDate.Add(departmentResult.Value.StartAt);

            var twelveHoursBeforeClinic = clinicStartDateTime.AddHours(-12);

            var check = now < twelveHoursBeforeClinic || now >= app.SchaudleStartAt;
            if (check)
                return new BadRequestError("لا يمكنك الحضور الآن. الحضور متاح قبل الموعد بحد أقصى ١٢ ساعة، وحتى وقت الموعد فقط.");
            var queueResult = await _queueRepository.GetQueueByDepartmentId<RadiologyCenterQueue>(app.RadiologyCeneterId);
            if (queueResult.IsFailed)
            {
                var addQueueResult = await _queueRepository.CreateQueue(new RadiologyCenterQueue
                {
                    Id = Guid.NewGuid().ToString(),
                    RadiologyCeneterId = app.RadiologyCeneterId,
                });
                if (addQueueResult.IsFailed)
                    return Result.Fail(addQueueResult.Errors);
                queueResult = await _queueRepository.GetQueueByDepartmentId<RadiologyCenterQueue>(app.RadiologyCeneterId);
                if (queueResult.IsFailed)
                    return Result.Fail(queueResult.Errors);
            }
            return await _queueRepository.AddAppointmentToQueue<RadiologyCeneterAppointment, RadiologyCenterQueue>(appointmentId, queueResult.Value.Id);
        }

        public async Task<Result> CreateNewTicket(string patientIdentifier, PatientIdentifierType identifierType, string hospitalId)
        {
            var patient = new Patient();
            if (identifierType == PatientIdentifierType.NationalId)
            {
                var patientResult = await _patientRepository.GetPatientByNationalId(patientIdentifier);
                if (patientResult.IsFailed || patientResult.Value is null)
                    return new BadRequestError("لا يوجد مريض بهذا الرقم القومي");
                patient = patientResult.Value;
            }
            else
            {
                var patientResult = await _patientRepository.GetPatientById(patientIdentifier);
                if (patientResult.IsFailed || patientResult.Value is null)
                    return new BadRequestError("لا يوجد مريض بهذا الرقم القومي");
                patient = patientResult.Value;
            }

            var ticket = new Ticket
            {
                Id = Guid.NewGuid().ToString(),
                PatientId = patient.Id,
                HospitalCreatedInId = hospitalId,
                State = TicketState.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PatientName = patient.Name,
                PatientNationalId = patient.NationalId,
            };

            await _ticketRepository.CreateTicket(ticket);
            return Result.Ok();
        }

        public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsForClinic(string clinicId, DateTime? date)
        {
            return await _hospitalRepository.GetAvailableTimeSlotsAsync<Clinic>(clinicId, date ?? DateTime.UtcNow);
        }

        public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsForMedicalLab(string medicalLabId, DateTime? date)
        {
            return await _hospitalRepository.GetAvailableTimeSlotsAsync<MedicalLab>(medicalLabId, date ?? DateTime.UtcNow);
        }

        public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsForRadiologyCenter(string radiologyCenterId, DateTime? date)
        {
            return await _hospitalRepository.GetAvailableTimeSlotsAsync<RadiologyCenter>(radiologyCenterId, date ?? DateTime.UtcNow);
        }

        public async Task<Result> StartClinicAppointment(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<ClinicAppointment>(appointmentId);
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);
            var a = appResult.Value;
            if (a.State == AppointmentState.InProgress)
                return new BadRequestError("الحجز قد بدأ بالفعل");
            if (a.State == AppointmentState.Completed)
                return new BadRequestError("الحجز قد انتهي، لا يمكن بدأه من جديد");
            if (a.State == AppointmentState.Terminated)
                return new BadRequestError("الحجز قد ألغي، من فضلك أحجز موعد جديد");

            return await ChangeAppointmentState(appointmentId, AppointmentState.InProgress);
        }

        public async Task<Result> StartMedicalLabAppointment(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<MedicalLabAppointment>(appointmentId);
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);
            var a = appResult.Value;
            if (a.State == AppointmentState.InProgress)
                return new BadRequestError("الحجز قد بدأ بالفعل");
            if (a.State == AppointmentState.Completed)
                return new BadRequestError("الحجز قد انتهي، لا يمكن بدأه من جديد");
            if (a.State == AppointmentState.Terminated)
                return new BadRequestError("الحجز قد ألغي، من فضلك أحجز موعد جديد");

            return await ChangeAppointmentState(appointmentId, AppointmentState.InProgress);
        }

        public async Task<Result> StartRadiologyAppointment(string appointmentId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync<RadiologyCeneterAppointment>(appointmentId);
            if (appResult.IsFailed)
                return Result.Fail(appResult.Errors);
            var a = appResult.Value;
            if (a.State == AppointmentState.InProgress)
                return new BadRequestError("الحجز قد بدأ بالفعل");
            if (a.State == AppointmentState.Completed)
                return new BadRequestError("الحجز قد انتهي، لا يمكن بدأه من جديد");
            if (a.State == AppointmentState.Terminated)
                return new BadRequestError("الحجز قد ألغي، من فضلك أحجز موعد جديد");

            return await ChangeAppointmentState(appointmentId, AppointmentState.InProgress);
        }

        private async Task<Result> ChangeAppointmentState(string appointmentId, AppointmentState newState)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId)
                .OnSuccessAsync(a => a.State = newState);
            if (appointment.IsFailed)
                return Result.Fail(appointment.Errors);
            return await _appointmentRepository.UpdateAppointmentAsync(appointment.Value);
        }

        public async Task<Result<PagedResult<TestDto<RadiologyTest>>>> GetAllRadiologyTestsInHospital(string hospitalId, int page, int size)
        {
            var tests = await _testsRepository.GetAllTestsInHospital<RadiologyTest>(hospitalId, page, size);
            if (tests.IsFailed)
                return Result.Fail(tests.Errors);
            return new PagedResult<TestDto<RadiologyTest>>
            {
                Items = tests.Value.Items.Select(t => new TestDto<RadiologyTest>().MapFromModel(t)),
                CurrentPage = page,
                PageSize = size,
                TotalCount = tests.Value.TotalCount,
            };
        }

        public async Task<Result<PagedResult<TestDto<MedicalLabTest>>>> GetAllMedicalLabTestsTestsInHospital(string hospitalId, int page, int size)
        {
            var tests = await _testsRepository.GetAllTestsInHospital<MedicalLabTest>(hospitalId, page, size);
            if (tests.IsFailed)
                return Result.Fail(tests.Errors);
            return new PagedResult<TestDto<MedicalLabTest>>
            {
                Items = tests.Value.Items.Select(t => new TestDto<MedicalLabTest>().MapFromModel(t)),
                CurrentPage = page,
                PageSize = size,
                TotalCount = tests.Value.TotalCount,
            };
        }
    }
}
