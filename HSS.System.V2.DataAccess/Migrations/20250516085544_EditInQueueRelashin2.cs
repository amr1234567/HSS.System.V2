using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInQueueRelashin2 : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "SystemQueues",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "SystemQueues",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_Clinics_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_MedicalLabs_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "MedicalLabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemQueues_RadiologyCenters_DepartmentId",
                table: "SystemQueues",
                column: "DepartmentId",
                principalTable: "RadiologyCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
