using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Edits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MedicalLabAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_RadiologyCeneterAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ClinicName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicalLabAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MedicalLabName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RadiologyCeneterAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RadiologyCeneterAppointment_TesterName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "RadiologyCeneterName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TesterName",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClinicName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalLabAppointment_QueueId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalLabName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiologyCeneterAppointment_TesterName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RadiologyCeneterName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TesterName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MedicalLabAppointment_QueueId",
                table: "Appointments",
                column: "MedicalLabAppointment_QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_QueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments",
                column: "MedicalLabAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");
        }
    }
}
