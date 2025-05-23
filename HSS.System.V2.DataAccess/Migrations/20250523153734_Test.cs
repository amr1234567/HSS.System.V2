using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_SystemQueueId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SystemQueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "SystemQueueId",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "FinalDiagnosis",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalDiagnosis",
                table: "MedicalHistories");

            migrationBuilder.AddColumn<string>(
                name: "SystemQueueId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_SystemQueueId",
                table: "Appointments",
                column: "SystemQueueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_SystemQueueId",
                table: "Appointments",
                column: "SystemQueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");
        }
    }
}
