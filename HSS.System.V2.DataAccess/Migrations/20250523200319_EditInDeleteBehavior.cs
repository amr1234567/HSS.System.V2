using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Appointments_PreExamiationClinicAppointemntId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Appointments_ReExamiationClinicAppointemntId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Employees_CurrentWorkingDoctorId",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Appointments_FirstClinicAppointmentId",
                table: "MedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalLabs_Employees_CurrentWorkingTesterId",
                table: "MedicalLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_RadiologyCenters_Employees_CurrentWorkingTesterId",
                table: "RadiologyCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Appointments_FirstClinicAppointmentId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Appointments_PreExamiationClinicAppointemntId",
                table: "Appointments",
                column: "PreExamiationClinicAppointemntId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Appointments_ReExamiationClinicAppointemntId",
                table: "Appointments",
                column: "ReExamiationClinicAppointemntId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Employees_CurrentWorkingDoctorId",
                table: "Clinics",
                column: "CurrentWorkingDoctorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Appointments_FirstClinicAppointmentId",
                table: "MedicalHistories",
                column: "FirstClinicAppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalLabs_Employees_CurrentWorkingTesterId",
                table: "MedicalLabs",
                column: "CurrentWorkingTesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RadiologyCenters_Employees_CurrentWorkingTesterId",
                table: "RadiologyCenters",
                column: "CurrentWorkingTesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Appointments_FirstClinicAppointmentId",
                table: "Tickets",
                column: "FirstClinicAppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Appointments_PreExamiationClinicAppointemntId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Appointments_ReExamiationClinicAppointemntId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_Employees_CurrentWorkingDoctorId",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Appointments_FirstClinicAppointmentId",
                table: "MedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalLabs_Employees_CurrentWorkingTesterId",
                table: "MedicalLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_RadiologyCenters_Employees_CurrentWorkingTesterId",
                table: "RadiologyCenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Appointments_FirstClinicAppointmentId",
                table: "Tickets");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Appointments_PreExamiationClinicAppointemntId",
                table: "Appointments",
                column: "PreExamiationClinicAppointemntId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Appointments_ReExamiationClinicAppointemntId",
                table: "Appointments",
                column: "ReExamiationClinicAppointemntId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_Employees_CurrentWorkingDoctorId",
                table: "Clinics",
                column: "CurrentWorkingDoctorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Appointments_FirstClinicAppointmentId",
                table: "MedicalHistories",
                column: "FirstClinicAppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalLabs_Employees_CurrentWorkingTesterId",
                table: "MedicalLabs",
                column: "CurrentWorkingTesterId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RadiologyCenters_Employees_CurrentWorkingTesterId",
                table: "RadiologyCenters",
                column: "CurrentWorkingTesterId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Appointments_FirstClinicAppointmentId",
                table: "Tickets",
                column: "FirstClinicAppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
