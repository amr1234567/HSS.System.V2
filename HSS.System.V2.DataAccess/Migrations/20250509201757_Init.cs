
#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diseases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    OpenAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveIngredient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthOfDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestPrice = table.Column<double>(type: "float", nullable: false),
                    EstimatedDurationInMinutes = table.Column<double>(type: "float", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    SampleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyPart = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiresContrast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreparationInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfShifts = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    PharmacyType = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pharmacies_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfShifts = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    HospitalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receptions_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    SchaudleStartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDuration = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExpectedDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReExaminationNeeded = table.Column<bool>(type: "bit", nullable: true),
                    ReExamiationClinicAppointemntId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PreExamiationClinicAppointemntId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DiseaseId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClinicId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DoctorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QueueId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PrescriptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicalLabAppointment_Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalLabAppointmentState = table.Column<int>(type: "int", nullable: true),
                    ReceiveResultDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClinicAppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicalLabId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicalLabAppointment_QueueId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TesterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadiologyCeneterAppointment_Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadiologyCeneterAppointment_ClinicAppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCeneterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCeneterAppointment_TesterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCeneterAppointment_TestId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCeneterAppointment_QueueId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Appointments_ClinicAppointmentId",
                        column: x => x.ClinicAppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Appointments_PreExamiationClinicAppointemntId",
                        column: x => x.PreExamiationClinicAppointemntId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Appointments_RadiologyCeneterAppointment_ClinicAppointmentId",
                        column: x => x.RadiologyCeneterAppointment_ClinicAppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Appointments_ReExamiationClinicAppointemntId",
                        column: x => x.ReExamiationClinicAppointemntId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_Tests_RadiologyCeneterAppointment_TestId",
                        column: x => x.RadiologyCeneterAppointment_TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Tests_TesterId",
                        column: x => x.TesterId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicAppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Appointments_ClinicAppointmentId",
                        column: x => x.ClinicAppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestsRequired",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicAppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsRequired", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestsRequired_Appointments_ClinicAppointmentId",
                        column: x => x.ClinicAppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestsRequired_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    FirstClinicAppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Appointments_FirstClinicAppointmentId",
                        column: x => x.FirstClinicAppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionMedicineItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TimesPerDay = table.Column<int>(type: "int", nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrescriptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionMedicineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicineItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionMedicineItems_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfShifts = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    QueueId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodPerAppointment = table.Column<TimeSpan>(type: "time", nullable: false),
                    SpecializationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentWorkingDoctorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HospitalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clinics_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clinics_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    SpecializationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SpecializationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClinicId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicalLabName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalLabId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PharmacyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCenterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadiologyCenterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthOfDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Receptions_ReceptionId",
                        column: x => x.ReceptionId,
                        principalTable: "Receptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalLabs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfShifts = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    QueueId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodPerAppointment = table.Column<TimeSpan>(type: "time", nullable: false),
                    CurrentWorkingTesterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HospitalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalLabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalLabs_Employees_CurrentWorkingTesterId",
                        column: x => x.CurrentWorkingTesterId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalLabs_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyCenterRadiologyTest",
                columns: table => new
                {
                    RadiologyCentersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyCenterRadiologyTest", x => new { x.RadiologyCentersId, x.TestsId });
                    table.ForeignKey(
                        name: "FK_RadiologyCenterRadiologyTest_Tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RadiologyCenters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfShifts = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndAt = table.Column<TimeSpan>(type: "time", nullable: false),
                    QueueId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodPerAppointment = table.Column<TimeSpan>(type: "time", nullable: false),
                    CurrentWorkingTesterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    HospitalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RadiologyCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RadiologyCenters_Employees_CurrentWorkingTesterId",
                        column: x => x.CurrentWorkingTesterId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RadiologyCenters_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemQueues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ClinicId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MedicalLabId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RadiologyCenterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemQueues_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemQueues_MedicalLabs_MedicalLabId",
                        column: x => x.MedicalLabId,
                        principalTable: "MedicalLabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SystemQueues_RadiologyCenters_RadiologyCenterId",
                        column: x => x.RadiologyCenterId,
                        principalTable: "RadiologyCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicAppointmentId",
                table: "Appointments",
                column: "ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicId",
                table: "Appointments",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DiseaseId",
                table: "Appointments",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalLabAppointment_QueueId",
                table: "Appointments",
                column: "MedicalLabAppointment_QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalLabId",
                table: "Appointments",
                column: "MedicalLabId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PreExamiationClinicAppointemntId",
                table: "Appointments",
                column: "PreExamiationClinicAppointemntId",
                unique: true,
                filter: "[PreExamiationClinicAppointemntId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PrescriptionId",
                table: "Appointments",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_QueueId",
                table: "Appointments",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RadiologyCeneterAppointment_ClinicAppointmentId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RadiologyCeneterAppointment_TesterId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_TesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RadiologyCeneterAppointment_TestId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RadiologyCeneterId",
                table: "Appointments",
                column: "RadiologyCeneterId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ReExamiationClinicAppointemntId",
                table: "Appointments",
                column: "ReExamiationClinicAppointemntId",
                unique: true,
                filter: "[ReExamiationClinicAppointemntId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TesterId",
                table: "Appointments",
                column: "TesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TicketId",
                table: "Appointments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_CurrentWorkingDoctorId",
                table: "Clinics",
                column: "CurrentWorkingDoctorId",
                unique: true,
                filter: "[CurrentWorkingDoctorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_HospitalId",
                table: "Clinics",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_QueueId",
                table: "Clinics",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_SpecializationId",
                table: "Clinics",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ClinicId",
                table: "Employees",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_HospitalId",
                table: "Employees",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_MedicalLabId",
                table: "Employees",
                column: "MedicalLabId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PharmacyId",
                table: "Employees",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RadiologyCenterId",
                table: "Employees",
                column: "RadiologyCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReceptionId",
                table: "Employees",
                column: "ReceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SpecializationId",
                table: "Employees",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabs_CurrentWorkingTesterId",
                table: "MedicalLabs",
                column: "CurrentWorkingTesterId",
                unique: true,
                filter: "[CurrentWorkingTesterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabs_HospitalId",
                table: "MedicalLabs",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabs_QueueId",
                table: "MedicalLabs",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_HospitalId",
                table: "Pharmacies",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicineItems_MedicineId",
                table: "PrescriptionMedicineItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicineItems_PrescriptionId",
                table: "PrescriptionMedicineItems",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ClinicAppointmentId",
                table: "Prescriptions",
                column: "ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenterRadiologyTest_TestsId",
                table: "RadiologyCenterRadiologyTest",
                column: "TestsId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenters_CurrentWorkingTesterId",
                table: "RadiologyCenters",
                column: "CurrentWorkingTesterId",
                unique: true,
                filter: "[CurrentWorkingTesterId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenters_HospitalId",
                table: "RadiologyCenters",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_RadiologyCenters_QueueId",
                table: "RadiologyCenters",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Receptions_HospitalId",
                table: "Receptions",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_ClinicId",
                table: "SystemQueues",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_MedicalLabId",
                table: "SystemQueues",
                column: "MedicalLabId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_RadiologyCenterId",
                table: "SystemQueues",
                column: "RadiologyCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_TestsRequired_ClinicAppointmentId",
                table: "TestsRequired",
                column: "ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TestsRequired_TestId",
                table: "TestsRequired",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FirstClinicAppointmentId",
                table: "Tickets",
                column: "FirstClinicAppointmentId",
                unique: true,
                filter: "[FirstClinicAppointmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PatientId",
                table: "Tickets",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clinics_ClinicId",
                table: "Appointments",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_RadiologyCeneterAppointment_TesterId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_TesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_TesterId",
                table: "Appointments",
                column: "TesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MedicalLabs_MedicalLabId",
                table: "Appointments",
                column: "MedicalLabId",
                principalTable: "MedicalLabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Prescriptions_PrescriptionId",
                table: "Appointments",
                column: "PrescriptionId",
                principalTable: "Prescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_RadiologyCenters_RadiologyCeneterId",
                table: "Appointments",
                column: "RadiologyCeneterId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments",
                column: "MedicalLabAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_QueueId",
                table: "Appointments",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Tickets_TicketId",
                table: "Appointments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Employees_CurrentWorkingDoctorId",
                table: "Clinics",
                column: "CurrentWorkingDoctorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_SystemQueues_QueueId",
                table: "Clinics",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_MedicalLabs_MedicalLabId",
                table: "Employees",
                column: "MedicalLabId",
                principalTable: "MedicalLabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_RadiologyCenters_RadiologyCenterId",
                table: "Employees",
                column: "RadiologyCenterId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalLabs_SystemQueues_QueueId",
                table: "MedicalLabs",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RadiologyCenterRadiologyTest_RadiologyCenters_RadiologyCentersId",
                table: "RadiologyCenterRadiologyTest",
                column: "RadiologyCentersId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RadiologyCenters_SystemQueues_QueueId",
                table: "RadiologyCenters",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clinics_ClinicId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_Clinics_ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Diseases_DiseaseId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_RadiologyCeneterAppointment_TesterId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_TesterId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalLabs_Employees_CurrentWorkingTesterId",
                table: "MedicalLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_RadiologyCenters_Employees_CurrentWorkingTesterId",
                table: "RadiologyCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MedicalLabs_MedicalLabId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_MedicalLabs_MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Prescriptions_PrescriptionId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_RadiologyCenters_RadiologyCeneterId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_RadiologyCenterId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Tests_RadiologyCeneterAppointment_TestId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Tests_TesterId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Tickets_TicketId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "PrescriptionMedicineItems");

            migrationBuilder.DropTable(
                name: "RadiologyCenterRadiologyTest");

            migrationBuilder.DropTable(
                name: "TestsRequired");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Receptions");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "MedicalLabs");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "RadiologyCenters");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "SystemQueues");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
