using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInQueueSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_Clinics_DepartmentId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_MedicalLabs_DepartmentId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_DepartmentId",
                table: "SystemQueues");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "SystemQueues",
                newName: "RadiologyCeneterId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemQueues_DepartmentId",
                table: "SystemQueues",
                newName: "IX_SystemQueues_RadiologyCeneterId");

            migrationBuilder.AddColumn<string>(
                name: "ClinicId",
                table: "SystemQueues",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalLabId",
                table: "SystemQueues",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_ClinicId",
                table: "SystemQueues",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_MedicalLabId",
                table: "SystemQueues",
                column: "MedicalLabId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_Clinics_ClinicId",
                table: "SystemQueues",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_MedicalLabs_MedicalLabId",
                table: "SystemQueues",
                column: "MedicalLabId",
                principalTable: "MedicalLabs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_RadiologyCeneterId",
                table: "SystemQueues",
                column: "RadiologyCeneterId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_Clinics_ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_MedicalLabs_MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_RadiologyCeneterId",
                table: "SystemQueues");

            migrationBuilder.DropIndex(
                name: "IX_SystemQueues_ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropIndex(
                name: "IX_SystemQueues_MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.RenameColumn(
                name: "RadiologyCeneterId",
                table: "SystemQueues",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_SystemQueues_RadiologyCeneterId",
                table: "SystemQueues",
                newName: "IX_SystemQueues_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_Clinics_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_MedicalLabs_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "MedicalLabs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id");
        }
    }
}
