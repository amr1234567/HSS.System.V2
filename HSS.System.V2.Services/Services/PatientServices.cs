using FluentResults;

using HSS.System.V2.DataAccess.Contracts;
using HSS.System.V2.DataAccess.Helpers;
using HSS.System.V2.Domain.Enums;
using HSS.System.V2.Domain.Helpers.Methods;
using HSS.System.V2.Domain.Helpers.Models;
using HSS.System.V2.Domain.Models.Appointments;
using HSS.System.V2.Domain.Models.Facilities;
using HSS.System.V2.Domain.Models.Medical;
using HSS.System.V2.Domain.Models.Prescriptions;
using HSS.System.V2.Domain.ResultHelpers.Errors;
using HSS.System.V2.Services.DTOs.PatientDTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HSS.System.V2.DataAccess.Contexts;
using HSS.System.V2.Services.Contracts;
using Microsoft.Extensions.Options;

namespace HSS.System.V2.Services.Services
{
    public class PatientServices : IPatientService
    {
        private readonly IUserContext _userContext;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IQueueRepository _queueRepository;
        private readonly ITestsRepository _testsRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ITestRequiredRepository _testRequiredRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly IMedicalLabTestResultServices _medicalLabTestResultServices;
        private readonly ISpecializationReporitory _specializationReporitory;
        public readonly BaseUrls _baseUrlsModel;

        public PatientServices(IUserContext userContext, INotificationRepository notificationRepository,
            IPatientRepository patientRepository, IAppointmentRepository appointmentRepository,
            IQueueRepository queueRepository, ITestsRepository testsRepository,
            IHospitalRepository hospitalRepository, ITicketRepository ticketRepository,
            ITestRequiredRepository testRequiredRepository, IPrescriptionRepository prescriptionRepository,
            IWebHostEnvironment env, AppDbContext context, IMedicalLabTestResultServices medicalLabTestResultServices,
            ISpecializationReporitory specializationReporitory, IOptions<BaseUrls> options)
        {
            _userContext = userContext;
            _notificationRepository = notificationRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _queueRepository = queueRepository;
            _testsRepository = testsRepository;
            _hospitalRepository = hospitalRepository;
            _ticketRepository = ticketRepository;
            _testRequiredRepository = testRequiredRepository;
            _prescriptionRepository = prescriptionRepository;
            _env = env;
            _context = context;
            _medicalLabTestResultServices = medicalLabTestResultServices;
            _specializationReporitory = specializationReporitory;
            _baseUrlsModel = options.Value;
        }

        public async Task<Result<int>> NotificationCount()
        {
            var notifications = await _notificationRepository.GetAllNotificationsForUser(_userContext.ApiUserId, true);
            if (notifications.IsFailed)
                return Result.Fail(notifications.Errors);

            return Result.Ok(notifications.Value.Count());
        }

        public async Task<Result<List<NotificationDto>>> GetNotifications()
        {
            var allNotification = await _notificationRepository.GetAllNotificationsForUser(_userContext.ApiUserId, true);

            if (allNotification.IsFailed)
                return Result.Fail(allNotification.Errors);

            return allNotification.Value
                .OrderByDescending(x => x.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    MessageData = JsonSerializer.Deserialize<NotificationMessage>(n.MessageData),
                    CreatedAt = n.CreatedAt
                }).ToList();
        }

        public async Task<Result<NotificationData>> GetNotificationData(string notificationId)
        {
            try
            {
                var notResult = await _notificationRepository.GetNotificationById(notificationId);
                if (notResult.IsFailed)
                    return Result.Fail(notResult.Errors);
                return JsonSerializer.Deserialize<NotificationData>(notResult.Value.Data);
            }
            catch (Exception ex)
            {
                return new();
            }
        }

        public async Task<Result> SeenNotification(string notificationId)
        {
            return await _notificationRepository.MarkNotificationAsSeen(notificationId);
        }

        public async Task<Result<DiabetesResponse?>> SugerTest(DiabetesTestModel model)
        {
            var userResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
            if (userResult.IsFailed)
                return Result.Fail(userResult.Errors);
            var user = userResult.Value;
            var nationalId = user.NationalId;
            int century = nationalId[0] == '2' ? 1900 : 2000;
            int year = century + int.Parse(nationalId.Substring(1, 2));
            int month = int.Parse(nationalId.Substring(3, 2));
            int day = int.Parse(nationalId.Substring(5, 2));

            DateTime birthDate = new DateTime(year, month, day);
            int age = DateTime.Today.Year - birthDate.Year;

            if (birthDate > DateTime.Today.AddYears(-age))
            {
                age--; // Adjust if birthday hasn't occurred yet this year
            }
            var ageForTest = 0;
            if (age >= 18 && age < 24)
            {
                ageForTest = 1;
            }
            else if (age >= 24 && age < 29)
            {
                ageForTest = 2;
            }
            else if (age >= 29 && age < 34)
            {
                ageForTest = 3;
            }
            else if (age >= 34 && age < 39)
            {
                ageForTest = 4;
            }
            else if (age >= 39 && age < 44)
            {
                ageForTest = 5;
            }
            else if (age >= 44 && age < 49)
            {
                ageForTest = 6;
            }
            else if (age >= 49 && age < 54)
            {
                ageForTest = 7;
            }
            else if (age >= 54 && age < 59)
            {
                ageForTest = 8;
            }
            else if (age >= 59 && age < 64)
            {
                ageForTest = 9;
            }
            else if (age >= 64 && age < 69)
            {
                ageForTest = 10;
            }
            else if (age >= 69 && age < 74)
            {
                ageForTest = 11;
            }
            else if (age >= 74 && age < 79)
            {
                ageForTest = 12;
            }
            else
            {
                ageForTest = 13;
            }
            using (HttpClient client = new HttpClient())
            {
                string url = "http://127.0.0.1:8000/predict";



                var json = new
                {
                    input_data = new
                    {
                        BMI = model.Weight / (model.Height / 100 * (model.Height / 100)),
                        Age = ageForTest,
                        model.HighBP,
                        model.HighChol,
                        model.CholCheck,
                        model.Smoker,
                        model.Stroke,
                        model.HeartDiseaseorAttack,
                        model.PhysActivity,
                        model.Fruits,
                        Veggies = model.Vegetables,
                        model.HvyAlcoholConsump,
                        model.AnyHealthcare,
                        model.NoDocbcCost,
                        model.GenHlth,
                        model.MentHlth,
                        model.PhysHlth,
                        model.DiffWalk,
                        Sex = model.Gender,
                        model.Education,
                        model.Income
                    }
                };

                string jsonString = JsonSerializer.Serialize(json);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throw exception if status code is not successful

                string responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<DiabetesResponse>(responseBody);

                return Result.Ok(result);

            }
        }

        public async Task<Result<PagedResult<AppointmentView>>> GetCurrentAppontments(PaginationRequest pagination)
        {
            try
            {
                var appointmentsResult = await _appointmentRepository.GetAllAppointmentsForUser(_userContext.ApiUserId, pagination, AppointmentState.NotStarted);

                if (appointmentsResult.IsFailed)
                    return Result.Fail(appointmentsResult.Errors);

                var appointments = appointmentsResult.Value;

                if (appointments.TotalCount == 0)
                    return PagedResult<AppointmentView>.Empty;

                var allTicketAppintments = appointments.Items.Select(a => new AppointmentView()
                {
                    Id = a.Id,
                    DepartmentName = a.EmployeeName,
                    HospitalName = a.HospitalName,
                    EmployeeName = a.EmployeeName,
                    StartAt = a.ActualStartAt ?? a.SchaudleStartAt,
                }).OrderBy(x => x.StartAt);

                var result = new PagedResult<AppointmentView>(allTicketAppintments, appointments.TotalPages, appointments.CurrentPage, appointments.PageSize);

                return result;
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<object>> GetAppointmentDetails(string appointmentId)
        {
            var appointmentResult = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);

            if (appointmentResult.IsFailed)
                return Result.Fail(appointmentResult.Errors);

            var appointment = appointmentResult.Value;


            if (appointment == null)
                return Result.Fail("this appointment don't have details");

            var details = await _queueRepository.GetAppointemntCustomDetails(appointment);

            return Result.Ok<object>(new
            {
                appointment.Hospital.Address,
                AppointmentNumber = details.Index,
                AppoitnmentStartAt = details.StartAt
            });
        }

        public async Task<Result<List<SpecialzationDto>>> GetAllSpecialzations()
        {
            return await _specializationReporitory.GetAllAsync()
                .MapAsync(data => data.Select(s => new SpecialzationDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Icon = s.Icon
                }).ToList());
        }

        public async Task<Result<PagedResult<RadiologyTestDto>>> GetAllRadiologyTest(PaginationRequest pagination)
        {
            return await _testsRepository.GetAllTestsAsync<RadiologyTest>(pagination)
                .MapAsync(data => new PagedResult<RadiologyTestDto>(data.Items.Select(t => new RadiologyTestDto
                {
                    Id = t.Id,
                    Name = t.Name,
                }), data.TotalCount, data.CurrentPage, data.PageSize));
        }

        public async Task<Result<PagedResult<MedicalLabTestDto>>> GetAllMedicalLabTest(PaginationRequest pagination)
        {
            return await _testsRepository.GetAllTestsAsync<MedicalLabTest>(pagination)
                .MapAsync(data => new PagedResult<MedicalLabTestDto>(data.Items.Select(t => new MedicalLabTestDto
                {
                    Id = t.Id,
                    Name = t.Name,
                }), data.TotalCount, data.CurrentPage, data.PageSize));
        }

        public async Task<Result<PagedResult<HospitalDto>>> GetHospitalsBySpecificationId(string specializationId, PaginationRequest pagination)
        {
            var userResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
            if (userResult.IsFailed)
                return Result.Fail(userResult.Errors);
            var patient = userResult.Value;

            var hospitalsResult = await _hospitalRepository.GetHospitalsBySpecificationId(specializationId);
            if (hospitalsResult.IsFailed)
                return Result.Fail(hospitalsResult.Errors);

            return hospitalsResult.Value
                .DistinctBy(h => h.Id)
                .OrderBy(h => DistanceHelper.GetDistance(patient.Lat, patient.Lng, h.Lat, h.Lng))
                .Select(h => new HospitalDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address
                }).GetPaged(pagination);
        }

        public async Task<Result<PagedResult<HospitalDto>>> GetHospitalsByRadiologyTestId(string radiologyTestId, PaginationRequest pagination)
        {
            var userResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
            if (userResult.IsFailed)
                return Result.Fail(userResult.Errors);
            var patient = userResult.Value;

            var hospitalsResult = await _testsRepository.GetAllHospitalsDoTest<RadiologyTest>(radiologyTestId);
            if (hospitalsResult.IsFailed)
                return Result.Fail(hospitalsResult.Errors);
            return hospitalsResult.Value
                .DistinctBy(h => h.Id)
                .OrderBy(h => DistanceHelper.GetDistance(patient.Lat, patient.Lng, h.Lat, h.Lng))
                .Select(h => new HospitalDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address
                }).GetPaged(pagination);
        }

        public async Task<Result<PagedResult<HospitalDto>>> GetHospitalsByMedicalLabTestId(string medicalLabTestId, PaginationRequest pagination)
        {
            var userResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
            if (userResult.IsFailed)
                return Result.Fail(userResult.Errors);
            var patient = userResult.Value;

            var hospitalsResult = await _testsRepository.GetAllHospitalsDoTest<MedicalLabTest>(medicalLabTestId);
            if (hospitalsResult.IsFailed)
                return Result.Fail(hospitalsResult.Errors);
            return hospitalsResult.Value
                .OrderBy(h => DistanceHelper.GetDistance(patient.Lat, patient.Lng, h.Lat, h.Lng))
                .Select(h => new HospitalDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address
                }).GetPaged(pagination);
        }

        public async Task<Result<PagedResult<TicketViewDto>>> GetActiveTicketForPatient(PaginationRequest pagination)
        {
            var ticketsResult = await _ticketRepository.GetOpenTicketsForPatient(_userContext.ApiUserId, page: pagination.Page, size: pagination.Size);

            if (ticketsResult.IsFailed)
                return Result.Fail(ticketsResult.Errors);
            var tickets = ticketsResult.Value.Items.Select(t => new TicketViewDto
            {
                Id = t.Id,
                TicketState = t.State.ToString(),
                AppointmentCount = t.Appointments != null ? t.Appointments.Count : 0,
                CreatedAt = t.CreatedAt
            }).ToList();
            var result = new PagedResult<TicketViewDto>(tickets, ticketsResult.Value.TotalCount, ticketsResult.Value.CurrentPage, ticketsResult.Value.PageSize);
            return Result.Ok(result);
        }

        public async Task<Result<List<DebartmentDto>>> GetClinics(string hospitalId, string SpecificationId)
        {
            return await _hospitalRepository.GetHospitalDepartmentItems<Clinic>(hospitalId)
                .MapAsync(data =>
                    data.Where(c => c.SpecializationId == SpecificationId)
                    .Select(c => new DebartmentDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).ToList());
        }

        public async Task<Result<List<DebartmentDto>>> GetRadiologyCenter(string hospitalId, string TestId)
        {
            return await _hospitalRepository.GetHospitalDepartmentItems<RadiologyCenter>(hospitalId)
                .MapAsync(data =>
                    data.Where(c => c.Tests.Any(t => t.Id == TestId))
                    .Select(c => new DebartmentDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).ToList()); throw new NotImplementedException();
        }

        public async Task<Result<List<DebartmentDto>>> GetMedicalLabs(string hospitalId, string TestId)
        {
            return await _hospitalRepository.GetHospitalDepartmentItems<MedicalLab>(hospitalId)
               .MapAsync(data =>
                   data.Where(c => c.Tests.Any(t => t.Id == TestId))
                   .Select(c => new DebartmentDto
                   {
                       Id = c.Id,
                       Name = c.Name,
                   }).ToList());
        }

        public async Task<Result> CreateTicket(string hospitalId)
        {
            var patientResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
            if (patientResult.IsFailed)
                return Result.Fail(patientResult.Errors);
            var patient = patientResult.Value;

            var hospitalResult = await _hospitalRepository.GetHospitalById(hospitalId);
            if (hospitalResult.IsFailed)
                return Result.Fail(hospitalResult.Errors);
            var hospital = hospitalResult.Value;    

            var ticket = new Ticket
            {
                Id = Guid.NewGuid().ToString(),
                PatientId = patient.Id,
                PatientNationalId = patient.NationalId,
                PatientName = patient.Name,
                HospitalCreatedInId = hospital.Id,
                State = TicketState.Active,
                CreatedAt = DateTime.UtcNow,
            };
            return await _ticketRepository.CreateTicket(ticket);
        }

        public async Task<Result> CreateClinicAppointment(CreateClinicAppointmentModelForPatient model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientById(_userContext.ApiUserId);
            if(patient.IsFailed)
                return Result.Fail(patient.Errors);
            var clinic = await _hospitalRepository.GetHospitalDepartmentItem<Clinic>(model.ClinicId);
            if(clinic.IsFailed)
                return Result.Fail(clinic.Errors);
            var ticket = await _ticketRepository.GetTicketById(model.TicketId);
            if(ticket.IsFailed)
                return Result.Fail(ticket.Errors);
            var clinicAppointment = ticket.Value.FirstClinicAppointment;
            var entity = model.ToModel();
            entity.DepartmentName = clinic.Value.Name;
            entity.HospitalName = clinic.Value.Hospital.Name;
            entity.HospitalId = clinic.Value.HospitalId;
            entity.PatientNationalId = patient.Value.NationalId;
            entity.PatientName = patient.Value.Name;
            entity.ExpectedDuration = clinic.Value.PeriodPerAppointment;
            entity.EmployeeName = string.Empty;

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

        public async Task<Result> CreateRadiologyAppointMent(CreateRadiologyAppointmentModelForPatient model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientById(_userContext.ApiUserId);
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

            return await _appointmentRepository.CreateAppointmentAsync(entity)
                .ThenAsync(() => _ticketRepository.UpdateTicket(ticket.Value));
        }

        public async Task<Result> CreateMedicalLabAppointment(CreateMedicalLabAppointmentModelForPatient model)
        {
            if (model.ExpectedTimeForStart < DateTime.UtcNow)
                return new BadArgumentsError("لا يمكن بدأ الحجز في الماضي");
            var patient = await _patientRepository.GetPatientById(_userContext.ApiUserId);
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

            return await _appointmentRepository.CreateAppointmentAsync(entity)
                .ThenAsync(() => _ticketRepository.UpdateTicket(ticket.Value));
        }

        public async Task<Result<List<TestRquired>>> GetMedicalLabTestRequired()
        {
            var testsResult = await _testRequiredRepository.GetAllTestsRequiredAvailableForUser(_userContext.ApiUserId);
            if (testsResult.IsFailed)
                return Result.Fail(testsResult.Errors);
            var tests = testsResult.Value;

            return tests.Where(t => t.Test is MedicalLabTest)
                .Select(r => new TestRquired
                {
                    TestName = r.TestName,
                    ClinicName = r.ClinicAppointment.DepartmentName,
                    DoctorName = r.ClinicAppointment.EmployeeName,
                    HospitalName = r.ClinicAppointment.HospitalName,
                    Date = r.ClinicAppointment.SchaudleStartAt
                }).ToList();
        }

        public async Task<Result<List<TestRquired>>> GetRadiologyTestRequired()
        {
            var testsResult = await _testRequiredRepository.GetAllTestsRequiredAvailableForUser(_userContext.ApiUserId);
            if (testsResult.IsFailed)
                return Result.Fail(testsResult.Errors);
            var tests = testsResult.Value;

            return tests.Where(t => t.Test is RadiologyTest)
                .Select(r => new TestRquired
                {
                    TestName = r.TestName,
                    ClinicName = r.ClinicAppointment.DepartmentName,
                    DoctorName = r.ClinicAppointment.EmployeeName,
                    HospitalName = r.ClinicAppointment.HospitalName,
                    Date = r.ClinicAppointment.SchaudleStartAt
                }).ToList();
        }

        public async Task<Result<PagedResult<PrescriptionDto>>> GetAllPrescriptionsRequired(PaginationRequest pagination)
        {
            var preResult = await _prescriptionRepository.GetAllMedicalPrescription(_userContext.ApiUserId);
            if (preResult.IsFailed)
                return Result.Fail(preResult.Errors);
            var prescriptions = preResult.Value;
            return prescriptions
                 .Select(p => new PrescriptionDto
                 {
                     PrescriptionId = p.Id,
                     ClinicName = p.ClinicAppointment.DepartmentName,
                     DoctorName = p.ClinicAppointment.EmployeeName,
                     HospitalName = p.ClinicAppointment.Hospital.Name,
                     MedicineCount = p.Items.Count(),
                     Date = p.ClinicAppointment.ActualStartAt ?? p.ClinicAppointment.SchaudleStartAt
                 }).GetPaged(pagination);
        }

        public async Task<Result<List<MedicineDto>>> GetMedicineByPrescriptionId(string prescriptionId)
        {
            var result = await _prescriptionRepository.GetMedicalPrescriptionById(prescriptionId);
            if (result.IsFailed)
                return Result.Fail(result.Errors);
            return result.Value.Items
                .Select(m => new MedicineDto
                {
                    MedicineName = m.Medicine.Name,
                    MedicineQuantity = m.Quantity
                }).ToList();
        }

        public async Task<Result<PagedResult<AppointmentView>>> GetCurrentClinicAppointments(PaginationRequest pagination)
        {
            try
            {
                var appsResult = await _appointmentRepository.GetAllAppointmentsForUser(_userContext.ApiUserId, AppointmentState.NotStarted);
                if (appsResult.IsFailed)
                    return Result.Fail(appsResult.Errors);
                return appsResult.Value
                    .OfType<ClinicAppointment>()
                    .OrderBy(c => c.ActualStartAt ?? c.SchaudleStartAt)
                    .Select(c => new AppointmentView
                    {
                        Id = c.Id,
                        DepartmentName = c.DepartmentName,
                        HospitalName = c.Hospital.Name,
                        EmployeeName = c.EmployeeName,
                        StartAt = c.ActualStartAt ?? c.SchaudleStartAt,
                    }).GetPaged(pagination);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        /// <inheritdoc/>
        public async Task<Result<PagedResult<AppointmentView>>> GetCurrentMedicalLabAppointments(PaginationRequest pagination)
        {
            try
            {
                var appsResult = await _appointmentRepository.GetAllAppointmentsForUser(_userContext.ApiUserId, AppointmentState.NotStarted);
                if (appsResult.IsFailed)
                    return Result.Fail(appsResult.Errors);
                return appsResult.Value
                    .OfType<MedicalLabAppointment>()
                    .OrderBy(c => c.ActualStartAt ?? c.SchaudleStartAt)
                    .Select(c => new AppointmentView
                    {
                        Id = c.Id,
                        DepartmentName = c.DepartmentName,
                        HospitalName = c.Hospital.Name,
                        EmployeeName = c.EmployeeName,
                        StartAt = c.ActualStartAt ?? c.SchaudleStartAt,
                    }).GetPaged(pagination);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<PagedResult<AppointmentView>>> GetCurrentRadiologyCenterAppointments(PaginationRequest pagination)
        {
            try
            {
                var appsResult = await _appointmentRepository.GetAllAppointmentsForUser(_userContext.ApiUserId, AppointmentState.NotStarted);
                if (appsResult.IsFailed)
                    return Result.Fail(appsResult.Errors);
                return appsResult.Value
                    .OfType<RadiologyCeneterAppointment>()
                    .OrderBy(c => c.ActualStartAt ?? c.SchaudleStartAt)
                    .Select(c => new AppointmentView
                    {
                        Id = c.Id,
                        DepartmentName = c.DepartmentName,
                        HospitalName = c.Hospital.Name,
                        EmployeeName = c.EmployeeName,
                        StartAt = c.ActualStartAt ?? c.SchaudleStartAt,
                    }).GetPaged(pagination);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }
       
        public async Task<Result> UploadProfilePicture(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Result.Fail("there are not selected file to upload");

                var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif", "image/bmp", "image/webp" };
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                var contentType = file.ContentType.ToLower();

                if (!allowedTypes.Contains(contentType) || !allowedExtensions.Contains(fileExtension))
                    return Result.Fail("This file is NOT Image");

                var imagePath = await UploadFiles.Upload(file, _env.WebRootPath, "ProfilePictures");

                if (imagePath.IsFailed || string.IsNullOrEmpty(imagePath.Value))
                    return Result.Fail("Upload Image is Fail");

                var userResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
                if (userResult.IsFailed)
                    return Result.Fail(userResult.Errors);
                var user = userResult.Value;

                if (!string.IsNullOrEmpty(user.UrlOfProfilePicutre) && File.Exists(user.UrlOfProfilePicutre))
                    File.Delete(user.UrlOfProfilePicutre);

                return await _patientRepository.UpdatePatientPicture(_userContext.ApiUserId, imagePath.Value);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<ProfileInformationDto>> GetPatientProfileInformation()
        {
            try
            {
                var userResult = await _patientRepository.GetPatientById(_userContext.ApiUserId);
                if (userResult.IsFailed)
                    return Result.Fail(userResult.Errors);
                var user = userResult.Value;

                var information = new ProfileInformationDto
                {
                    UserName = user.Name,
                    Gender = user.Gender.ToString(),
                    Age = user.GetAge(),
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    UrlOfProfilePicture = _baseUrlsModel.Default + user.UrlOfProfilePicutre
                };

                return Result.Ok(information);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<List<TicketInformationDto>>> GetAllActiveTicketsOfPatient()
        {
            try
            {
                var ticketsResult = await _ticketRepository.GetAllTicketForPatient(_userContext.ApiUserId);
                if (ticketsResult.IsFailed)
                    return Result.Fail(ticketsResult.Errors);

                return ticketsResult.Value
                    .Where(t => t.State == TicketState.Active)
                    .OrderBy(t => t.CreatedAt)
                    .Select(t => new TicketInformationDto
                    {
                        TicketId = t.Id,
                        CreateAt = t.CreatedAt,
                        CurrentState = t.State.ToString(),
                    }).ToList();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<List<TicketInformationDto>>> GetAllInactiviTicketsOfPatient()
        {
            try
            {
                var ticketsResult = await _ticketRepository.GetAllTicketForPatient(_userContext.ApiUserId);
                if (ticketsResult.IsFailed)
                    return Result.Fail(ticketsResult.Errors);

                return ticketsResult.Value
                    .Where(t => t.State != TicketState.Active)
                    .OrderBy(t => t.CreatedAt)
                    .Select(t => new TicketInformationDto
                    {
                        TicketId = t.Id,
                        CreateAt = t.CreatedAt,
                        CurrentState = t.State.ToString(),
                    }).ToList();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<PagedResult<AppointmentView>>> GetTicketContent(string ticketId, PaginationRequest pagination)
        {
            try
            {
                var appointmentsResult = await _appointmentRepository.GetAllAppointmentsForUser(_userContext.ApiUserId, AppointmentState.NotStarted);

                if (appointmentsResult.IsFailed)
                    return Result.Fail(appointmentsResult.Errors);

                var appointments = appointmentsResult.Value;

                if (appointments.Count() == 0)
                    return Result.Fail("there are not appointments yet!");

                return appointments
                    .Where(a => a.TicketId == ticketId)
                    .Select(a => new AppointmentView()
                    {
                        Id = a.Id,
                        DepartmentName = a.EmployeeName,
                        HospitalName = a.HospitalName,
                        EmployeeName = a.EmployeeName,
                        StartAt = a.ActualStartAt ?? a.SchaudleStartAt,
                    }).OrderBy(x => x.StartAt).GetPaged(pagination);
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<object>> GetAppointmentContent(string appointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(appointmentId))
                    return Result.Fail("null Id");

                var appointment = await _context.Appointments.FindAsync(appointmentId);

                if (appointment is null)
                    return EntityNotExistsError.Happen<Appointment>(appointmentId);

                if (appointment is ClinicAppointment)
                {
                    var clinicAppointment = await _context.Appointments.AsNoTracking()
                        .OfType<ClinicAppointment>()
                        .Where(x => x.Id == appointmentId)
                        .Include(c => c.TestsRequired)
                        .Include(c => c.Disease)
                        .Include(c => c.Prescription)
                            .ThenInclude(p => p.Items)
                        .Select(c => new
                        {
                            c.Diagnosis,
                            MedicalLabTestsRequired = c.TestsRequired != null && c.TestsRequired.Any() ?
                            c.TestsRequired.Select(m => m.TestName).ToList() :
                            null,
                            RadiologyTestsRequired = c.TestsRequired != null && c.TestsRequired.Any() ?
                            c.TestsRequired.Select(r => r.TestName).ToList() :
                            null,
                            Prescriptions = c.Prescription != null ?
                            c.Prescription.Items.Select(m => m.MedicineName).ToList() : null,
                            Disease = c.Disease!.Name
                        }).FirstOrDefaultAsync();

                    if (clinicAppointment == null)
                        return EntityNotExistsError.Happen<ClinicAppointment>(appointmentId);

                    bool allPropertiesNull = clinicAppointment.GetType().GetProperties()
                        .All(p => p.GetValue(clinicAppointment) == null);

                    if (allPropertiesNull)
                        return Result.Fail("No content found for this appointment!");

                    return Result.Ok<object>(clinicAppointment);
                }
                else if (appointment is RadiologyCeneterAppointment)
                {
                    var radiologyAppointment = await _context.RadiologyCeneterAppointments.AsNoTracking()
                        .Where(x => x.Id.Equals(appointment.Id))
                        .Select(x => x.Result)
                        .ToListAsync();

                    if (radiologyAppointment is null || !radiologyAppointment.Any())
                        return Result.Fail("this appointment not found");

                    return Result.Ok<object>(radiologyAppointment);
                }
                else if (appointment is MedicalLabAppointment)
                {
                    var medicalLabAppointment = await _medicalLabTestResultServices.GetTestResultForMedicalLabAppointment(appointment.Id);

                    if (medicalLabAppointment.IsFailed)
                        return Result.Fail($"{medicalLabAppointment.Errors.FirstOrDefault()}");

                    return Result.Ok<object>(medicalLabAppointment.ValueOrDefault);
                }
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return new UnKnownError(ex);
            }
        }

        public async Task<Result<PagedResult<SpecialzationDto>>> GetAllSpecifications(PaginationRequest pagination)
        {
            var specsResult = await _specializationReporitory.GetAllAsync();
            if (specsResult.IsFailed)
                return Result.Fail(specsResult.Errors);
            var specs = specsResult.Value;
            return specs
                .Select(s => new SpecialzationDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Icon = s.Icon
                }).GetPaged(pagination);
        }

        public async Task<Result<IEnumerable<TicketViewDto>>> GetActiveTicketInHospital(string hospitalId)
        {
            var ticketsResult = await _ticketRepository.GetAllOpenedTicketInHospitalForPatient(hospitalId, _userContext.ApiUserId);
            if (ticketsResult.IsFailed)
                return Result.Fail(ticketsResult.Errors);
            var tickets = ticketsResult.Value;
            if (!tickets.Any())
                return Result.Fail("there are not ticket used in this hospital");
            return tickets.Where(t => t.State == TicketState.Active)
                 .Select(t => new TicketViewDto
                 {
                     Id = t.Id,
                     TicketState = t.State.ToString(),
                     AppointmentCount = t.Appointments.Count,
                     CreatedAt = t.CreatedAt
                 }).ToList();
        }

        public async Task<Result> CancelAppointment(string appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment.IsFailed)
                return Result.Fail(appointment.Errors);
            appointment.Value.State = AppointmentState.Cancelled;
            appointment.Value.UpdatedAt = DateTime.UtcNow;
            return await _appointmentRepository.UpdateAppointmentAsync(appointment.Value);
        }

    }
}
