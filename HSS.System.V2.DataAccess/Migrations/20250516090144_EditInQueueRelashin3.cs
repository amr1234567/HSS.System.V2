using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInQueueRelashin3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_SystemQueues_QueueId",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalLabs_SystemQueues_QueueId",
                table: "MedicalLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_RadiologyCenters_SystemQueues_QueueId",
                table: "RadiologyCenters");

            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "RadiologyCenters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "MedicalLabs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "Clinics",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_SystemQueues_QueueId",
                table: "Clinics",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalLabs_SystemQueues_QueueId",
                table: "MedicalLabs",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RadiologyCenters_SystemQueues_QueueId",
                table: "RadiologyCenters",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_SystemQueues_QueueId",
                table: "Clinics");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalLabs_SystemQueues_QueueId",
                table: "MedicalLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_RadiologyCenters_SystemQueues_QueueId",
                table: "RadiologyCenters");

            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "RadiologyCenters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "MedicalLabs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "Clinics",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_SystemQueues_QueueId",
                table: "Clinics",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalLabs_SystemQueues_QueueId",
                table: "MedicalLabs",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RadiologyCenters_SystemQueues_QueueId",
                table: "RadiologyCenters",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
