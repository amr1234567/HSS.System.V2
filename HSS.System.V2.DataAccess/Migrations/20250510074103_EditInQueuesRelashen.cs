using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInQueuesRelashen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_QueueId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PeriodPerAppointment",
                table: "SystemQueues",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments",
                column: "MedicalLabAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_QueueId",
                table: "Appointments",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_SystemQueueId",
                table: "Appointments",
                column: "SystemQueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_Appointments_SystemQueues_SystemQueueId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_SystemQueueId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PeriodPerAppointment",
                table: "SystemQueues");

            migrationBuilder.DropColumn(
                name: "SystemQueueId",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_MedicalLabAppointment_QueueId",
                table: "Appointments",
                column: "MedicalLabAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_QueueId",
                table: "Appointments",
                column: "QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_SystemQueues_RadiologyCeneterAppointment_QueueId",
                table: "Appointments",
                column: "RadiologyCeneterAppointment_QueueId",
                principalTable: "SystemQueues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
