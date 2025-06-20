﻿using FluentResults;

using Hangfire;

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
        private readonly IUnitOfWork _unitOfWork;

        public ReceptionServices(IAppointmentRepository appointmentRepository, IQueueRepository queueRepository,
            IPatientRepository patientRepository, ITicketRepository ticketRepository,
            IMedicineRepository medicineRepository, IHospitalRepository hospitalRepository,
            ISpecializationReporitory specializationReporitory, ITestsRepository testsRepository,
            ITestRequiredRepository testRequiredRepository, IBackgroundTaskQueue taskQueue,
            INotificationRepository notificationRepository, AppDbContext context, IUnitOfWork unitOfWork)
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
            _unitOfWork = unitOfWork;
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
            return apps.Value
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForClinic(string clinicId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<ClinicQueue>(clinicId);
            return apps.Value.ClinicAppointments
                .Where(a => a.State is AppointmentState.InQueue or AppointmentState.InProgress)
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForRadiologyCenter(string radiologyCenterId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10)
        {
            var apps = await _appointmentRepository.GetAllForRadiologyCenterAsync(radiologyCenterId, new(dateFrom, dateTo));
            return apps.Value
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForRadiologyCenter(string radiologyCenterId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<RadiologyCenterQueue>(radiologyCenterId);
            return apps.Value.RadiologyCeneterAppointments
                .Where(a => a.State is AppointmentState.InQueue or AppointmentState.InProgress)
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetAllAppointmentsForMedicalLab(string medicalLabId, DateTime? dateFrom = null, DateTime? dateTo = null, int page = 1, int pageSize = 10)
        {
            var apps = await _appointmentRepository.GetAllForMedicalLabAsync(medicalLabId, new(dateFrom, dateTo));
            return apps.Value
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        public async Task<Result<PagedResult<AppointmentDto>>> GetQueueForMedicalLab(string medicalLabId, int page = 1, int pageSize = 10)
        {
            var apps = await _queueRepository.GetQueueByDepartmentId<MedicalLabQueue>(medicalLabId);
            return apps.Value.MedicalLabAppointments
                .Where(a => a.State is AppointmentState.InQueue or AppointmentState.InProgress)
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(x => x.State)
                .Select(a => new AppointmentDto().MapFromModel(a)).GetPaged(page, pageSize);
        }

        /// <inheritdoc/>
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
                Tickets = tickets.OrderByDescending(t => t.AbleToBook).GetPaged(page, pageSize),
                PatientName = patient.Value.Name,
                PatientNationalId = nationalId
            };
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<Result> CreateAppointment(CreateClinicAppointmentModelForReception model)
        {
            if (model.ExpectedTimeForStart < HelperDate.GetCurrentDate())
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


            var update = await _ticketRepository.UpdateTicket(ticket.Value);
            if (update.IsSuccess)
                return await _unitOfWork.SaveAllChanges() > 0 ? update : new BadRequestError("no rows effected");
            return update;
        }

        //public async Task<Result> CreateAppointment(CreateRadiologyAppointmentModelForReception model)
        //{
        //    //check if the user booking in the past, if that true return bad request
        //    //if the user provide ticket id and test id
        //    //  get the ticket and the test to check if it is really there in DB
        //    //  then check if the ticket is empty from appointments
        //    //  if anything wrong return bad request, if it correct add the model to DB
        //    //else if the user provide testRequiredId
        //    //  then get it and check for nullablity, then get the ticket id from the clinic appointment inside the testReuired and then save the needed data
        //    //else return bad request
        //    if (model.ExpectedTimeForStart < HelperDate.GetCurrentDate())
        //        return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
        //    if(!((!string.IsNullOrEmpty(model.TestId) && !string.IsNullOrEmpty(model.TicketId)) || string.IsNullOrEmpty(model.TextRequiredId)))
        //        return new BadArgumentsError("البيانات المطلوبة غير متوفرة");
        //    var patient = await _patientRepository.GetPatientByNationalId(model.NationalId);
        //    if (patient.IsFailed)
        //        return Result.Fail(patient.Errors);
        //    var radiologyCenter = await _hospitalRepository.GetHospitalDepartmentItem<RadiologyCenter>(model.RadiologyCenterId);
        //    if (radiologyCenter.IsFailed)
        //        return Result.Fail(radiologyCenter.Errors);
        //    var entity = model.ToModel();
        //    var ticket = await _ticketRepository.GetTicketById(model.TicketId);
        //    if (ticket.IsFailed)
        //    {
        //        var testRequired = await _testRequiredRepository.GetTestRequiredByIdAsync(model.TextRequiredId);
        //        if (!testRequired.IsFailed && testRequired.Value is not null)
        //        {
        //            entity.ClinicAppointmentId = testRequired.Value.ClinicAppointmentId;
        //            testRequired.Value.Used = true;
        //            entity.TicketId = testRequired.Value.ClinicAppointment.TicketId;
        //            await _testRequiredRepository.UpdateTestRequiredAsync(testRequired.Value);
        //        }
        //        else
        //        {
        //            return new BadRequestError("you must provide ticket id or test required id");
        //        }
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(ticket.Value.FirstClinicAppointmentId))
        //        {
        //            entity.TicketId = ticket.Value.Id;
        //            entity.ClinicAppointmentId = null;
        //        }
        //        else
        //        {
        //            return new BadRequestError("this ticket already has clinic appointment");
        //        }
        //    }
        //    entity.DepartmentName = radiologyCenter.Value!.Name;
        //    entity.HospitalName = radiologyCenter.Value.Hospital.Name;
        //    entity.PatientName = patient.Value.Name;
        //    entity.PatientId = patient.Value.Id;
        //    entity.HospitalId = radiologyCenter.Value.Hospital.Id;
        //    entity.PatientNationalId = patient.Value.NationalId;
        //    entity.EmployeeName = string.Empty;
        //    entity.ExpectedDuration = radiologyCenter.Value.PeriodPerAppointment;

        //    return await _appointmentRepository.CreateAppointmentAsync(entity);
        //}


        /// <inheritdoc/>
        public async Task<Result> CreateAppointment(CreateRadiologyAppointmentModelForReception model)
        {
            // 1) تأكد أن وقت الحجز ليس في الماضي
            if (model.ExpectedTimeForStart < HelperDate.GetCurrentDate())
                return new BadArgumentsError("لا يمكن بدء الحجز في الماضي");

            // 2) حدد ما إذا كان المستخدم أعطى (TicketId + TestId) أو (TestRequiredId)
            bool hasTicketAndTest =
                !string.IsNullOrEmpty(model.TicketId) &&
                !string.IsNullOrEmpty(model.TestId);

            bool hasTestRequired =
                !string.IsNullOrEmpty(model.TextRequiredId);

            // إذا لم يتم تلبية أيٍّ من الشروط، ارجع خطأ
            if (!hasTicketAndTest && !hasTestRequired)
                return new BadArgumentsError("البيانات المطلوبة غير متوفرة");

            // 3) جلب بيانات الـ Patient
            var patientResult = await _patientRepository.GetPatientByNationalId(model.NationalId);
            if (patientResult.IsFailed)
                return Result.Fail(patientResult.Errors);

            // 4) جلب بيانات الـ RadiologyCenter
            var radiologyCenterResult =
                await _hospitalRepository.GetHospitalDepartmentItem<RadiologyCenter>(model.RadiologyCenterId);
            if (radiologyCenterResult.IsFailed)
                return Result.Fail(radiologyCenterResult.Errors);

            // 5) إنشاء كيان الـ Appointment من الـ DTO
            var entity = model.ToModel();

            if (hasTestRequired)
            {
                // ======== Branch B: المستخدم أعطى TestRequiredId فقط ========
                var testRequiredResult =
                    await _testRequiredRepository.GetTestRequiredByIdAsync(model.TextRequiredId);

                if (testRequiredResult.IsFailed || testRequiredResult.Value == null)
                    return new BadArgumentsError("الاختبار الموصوف غير موجود في النظام");

                // تأكد أن الـ testRequired مرتبط بحجز في العيادة
                if (testRequiredResult.Value.ClinicAppointment == null)
                    return new BadArgumentsError("لا يوجد حجز عيادة مرتبط بهذا الاختبار");

                // انسخ الـ ClinicAppointmentId و TicketId من الـ testRequired
                entity.ClinicAppointmentId = testRequiredResult.Value.ClinicAppointmentId;
                entity.TicketId = testRequiredResult.Value.ClinicAppointment.TicketId;
                entity.TestId = testRequiredResult.Value.TestId;

                // وضع حالة Used = true على الـ testRequired
                testRequiredResult.Value.Used = true;
                var updateResult = await _testRequiredRepository.UpdateTestRequiredAsync(testRequiredResult.Value);
                if (updateResult.IsFailed)
                    return Result.Fail(updateResult.Errors);
            }

            else if (hasTicketAndTest)
            {
                // ======== Branch A: المستخدم أعطى TicketId + TestId ========
                var ticketResult = await _ticketRepository.GetTicketById(model.TicketId);
                if (ticketResult.IsFailed || ticketResult.Value == null)
                    return new BadArgumentsError("التذكرة غير موجودة في النظام");

                // تأكد أن الليستClinicAppointmentId فارغ (أي لم يتم حجز في العيادة بعد)
                if (!string.IsNullOrEmpty(ticketResult.Value.FirstClinicAppointmentId))
                    return new BadRequestError("هذه التذكرة تحتوي بالفعل على حجز");

                // (اختياري) إذا كان لديك مستودع خاص بالـ RadiologyTest:
                // var testResult = await _radiologyTestRepository.GetById(model.TestId);
                // if (testResult.IsFailed || testResult.Value == null)
                //     return new BadArgumentsError("الاختبار المطلوب غير موجود في النظام");

                entity.TicketId = ticketResult.Value.Id;
                entity.TestId = model.TestId!;
                entity.ClinicAppointmentId = null;
            }

            // 6) املأ باقي الحقول المشتركة
            var center = radiologyCenterResult.Value!;
            var patient = patientResult.Value;

            entity.DepartmentName = center.Name;
            entity.HospitalName = center.Hospital.Name;
            entity.PatientName = patient.Name;
            entity.PatientId = patient.Id;
            entity.HospitalId = center.Hospital.Id;
            entity.PatientNationalId = patient.NationalId;
            entity.EmployeeName = string.Empty;                     // فعليًا سيتم حجز الاسم لاحقًا أو يبقى فارغًا
            entity.ExpectedDuration = center.PeriodPerAppointment;

            // 7) أخيرًا، اضف الـ entity إلى قاعدة البيانات
            var updateAppResult = await _appointmentRepository.CreateAppointmentAsync(entity);
            if (updateAppResult.IsFailed)
                return Result.Fail(updateAppResult.Errors);

            return await _unitOfWork.SaveAllChanges() > 0 ? Result.Ok() : new BadRequestError();
        }


        public async Task<Result> CreateAppointment(CreateMedicalLabAppointmentModelForReception model)
        {
            if (model.ExpectedTimeForStart < HelperDate.GetCurrentDate())
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

            var update = await _ticketRepository.UpdateTicket(ticket.Value);
            if (update.IsSuccess)
                return await _unitOfWork.SaveAllChanges() > 0 ? update : new BadRequestError("no rows effected");
            return update;
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

            var now = HelperDate.GetCurrentDate();
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
                    CreatedAt = HelperDate.GetCurrentDate(),
                    Id = Guid.NewGuid().ToString(),
                    PatientId = ticket.Value.PatientId,
                    Seen = false,
                    UpdatedAt = HelperDate.GetCurrentDate(),
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
            var now = HelperDate.GetCurrentDate();
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
            var now = HelperDate.GetCurrentDate();
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
                CreatedAt = HelperDate.GetCurrentDate(),
                UpdatedAt = HelperDate.GetCurrentDate(),
                PatientName = patient.Name,
                PatientNationalId = patient.NationalId,
            };

            await _ticketRepository.CreateTicket(ticket);
            return Result.Ok();
        }

        public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsForClinic(string clinicId, DateTime? date)
        {
            return await _hospitalRepository.GetAvailableTimeSlotsAsync<Clinic>(clinicId, date ?? HelperDate.GetCurrentDate());
        }

        public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsForMedicalLab(string medicalLabId, DateTime? date)
        {
            return await _hospitalRepository.GetAvailableTimeSlotsAsync<MedicalLab>(medicalLabId, date ?? HelperDate.GetCurrentDate());
        }

        public async Task<Result<List<DateTime>>> GetAvailableTimeSlotsForRadiologyCenter(string radiologyCenterId, DateTime? date)
        {
            return await _hospitalRepository.GetAvailableTimeSlotsAsync<RadiologyCenter>(radiologyCenterId, date ?? HelperDate.GetCurrentDate());
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
            var updateResult = await ChangeAppointmentState(appointmentId, AppointmentState.InProgress);
            if (updateResult.IsSuccess)
                BackgroundJob.Schedule(() => CheckAppointemnt(a.Id), a.ExpectedDuration.Add(TimeSpan.FromSeconds(60)));
            return updateResult;
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

            var updateResult = await ChangeAppointmentState(appointmentId, AppointmentState.InProgress);
            if (updateResult.IsSuccess)
                BackgroundJob.Schedule(() => CheckAppointemnt(a.Id), a.ExpectedDuration.Add(TimeSpan.FromSeconds(60)));
            return updateResult;
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

            var updateResult = await ChangeAppointmentState(appointmentId, AppointmentState.InProgress);
            if (updateResult.IsSuccess)
                BackgroundJob.Schedule(() => CheckAppointemnt(a.Id), a.ExpectedDuration.Add(TimeSpan.FromSeconds(60)));
            return updateResult;
        }

        private async Task<Result> ChangeAppointmentState(string appointmentId, AppointmentState newState)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId)
                .OnSuccessAsync(a => a.State = newState);
            if (appointment.IsFailed)
                return Result.Fail(appointment.Errors);
            return await _appointmentRepository.UpdateAppointmentAsync(appointment.Value);
        }

        public async Task CheckAppointemnt(Appointment app)
        {
            if (app.State == AppointmentState.InProgress)
            {
                app.State = AppointmentState.Terminated;
                await _appointmentRepository.UpdateAppointmentAsync(app);
            }
        }

        public async Task CheckAppointemnt(string appId)
        {
            var appResult = await _appointmentRepository.GetAppointmentByIdAsync(appId)
                .EnsureNoneAsync((a => a is null, new BadRequestError()));
            if (appResult.IsSuccess)
            {
                var app = appResult.Value;
                await CheckAppointemnt(app);
            }
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
