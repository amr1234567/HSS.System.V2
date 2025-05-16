using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInQueueRelashin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_Clinics_ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_MedicalLabs_MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_RadiologyCenterId",
                table: "SystemQueues");

            migrationBuilder.DropIndex(
                name: "IX_SystemQueues_ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropIndex(
                name: "IX_SystemQueues_MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.DropIndex(
                name: "IX_SystemQueues_RadiologyCenterId",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "MedicalLabId",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "RadiologyCenterId",
                table: "SystemQueues");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "SystemQueues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_Clinics_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_MedicalLabs_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "MedicalLabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_SystemQueues_DepartmentId",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "SystemQueues");

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

            migrationBuilder.AddColumn<string>(
                name: "RadiologyCenterId",
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

            migrationBuilder.CreateIndex(
                name: "IX_SystemQueues_RadiologyCenterId",
                table: "SystemQueues",
                column: "RadiologyCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_Clinics_ClinicId",
                table: "SystemQueues",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_MedicalLabs_MedicalLabId",
                table: "SystemQueues",
                column: "MedicalLabId",
                principalTable: "MedicalLabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_RadiologyCenterId",
                table: "SystemQueues",
                column: "RadiologyCenterId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
