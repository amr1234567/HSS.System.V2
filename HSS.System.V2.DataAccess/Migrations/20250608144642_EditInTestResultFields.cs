using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInTestResultFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalLabTestResults");

            migrationBuilder.CreateTable(
                name: "MedicalLabTestResultFieldValues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FieldId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResultFieldType = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalLabTestResultFieldValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalLabTestResultFieldValues_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalLabTestResultFieldValues_MedicalLabTestResultFields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "MedicalLabTestResultFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabTestResultFieldValues_AppointmentId",
                table: "MedicalLabTestResultFieldValues",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabTestResultFieldValues_FieldId",
                table: "MedicalLabTestResultFieldValues",
                column: "FieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalLabTestResultFieldValues");

            migrationBuilder.CreateTable(
                name: "MedicalLabTestResults",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FieldId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "IX_MedicalLabTestResults_AppointmentId",
                table: "MedicalLabTestResults",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabTestResults_FieldId",
                table: "MedicalLabTestResults",
                column: "FieldId");
        }
    }
}
