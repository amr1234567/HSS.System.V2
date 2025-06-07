using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addNewTableToSaveMedicalLabTestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Appointments_ClinicAppointmentId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_ClinicAppointmentId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PrescriptionId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "ClinicAppointmentId",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "MedicalLabTestResults",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FieldId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalLabTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalLabTestResults_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalLabTestResults_MedicalLabTestResultFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "MedicalLabTestResultFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PrescriptionId",
                table: "Appointments",
                column: "PrescriptionId",
                unique: true,
                filter: "[PrescriptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabTestResults_AppointmentId",
                table: "MedicalLabTestResults",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabTestResults_FieldId",
                table: "MedicalLabTestResults",
                column: "FieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalLabTestResults");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_PrescriptionId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "ClinicAppointmentId",
                table: "Prescriptions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ClinicAppointmentId",
                table: "Prescriptions",
                column: "ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PrescriptionId",
                table: "Appointments",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Appointments_ClinicAppointmentId",
                table: "Prescriptions",
                column: "ClinicAppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
