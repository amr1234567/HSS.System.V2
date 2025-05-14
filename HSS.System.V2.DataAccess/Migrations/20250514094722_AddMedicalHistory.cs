using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Hospitals_HospitalId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "HospitalId",
                table: "Tickets",
                newName: "HospitalCreatedInId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_HospitalId",
                table: "Tickets",
                newName: "IX_Tickets_HospitalCreatedInId");

            migrationBuilder.AddColumn<string>(
                name: "MedicalHistoryId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientNationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstClinicAppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistories_Appointments_FirstClinicAppointmentId",
                        column: x => x.FirstClinicAppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalHistories_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalHistoryId",
                table: "Appointments",
                column: "MedicalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_FirstClinicAppointmentId",
                table: "MedicalHistories",
                column: "FirstClinicAppointmentId",
                unique: true,
                filter: "[FirstClinicAppointmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientId",
                table: "MedicalHistories",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MedicalHistories_MedicalHistoryId",
                table: "Appointments",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Hospitals_HospitalCreatedInId",
                table: "Tickets",
                column: "HospitalCreatedInId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MedicalHistories_MedicalHistoryId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Hospitals_HospitalCreatedInId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MedicalHistoryId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicalHistoryId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "HospitalCreatedInId",
                table: "Tickets",
                newName: "HospitalId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_HospitalCreatedInId",
                table: "Tickets",
                newName: "IX_Tickets_HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Hospitals_HospitalId",
                table: "Tickets",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
