using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditInMedinicesWithPharmacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicinePharmacy");

            migrationBuilder.DropColumn(
                name: "ExpectedDuration",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalLabMedicalLabTest",
                columns: table => new
                {
                    MedicalLabsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalLabMedicalLabTest", x => new { x.MedicalLabsId, x.TestsId });
                    table.ForeignKey(
                        name: "FK_MedicalLabMedicalLabTest_MedicalLabs_MedicalLabsId",
                        column: x => x.MedicalLabsId,
                        principalTable: "MedicalLabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalLabMedicalLabTest_Tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePharmacies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePharmacies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicinePharmacies_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicinePharmacies_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabMedicalLabTest_TestsId",
                table: "MedicalLabMedicalLabTest",
                column: "TestsId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePharmacies_MedicineId",
                table: "MedicinePharmacies",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePharmacies_PharmacyId",
                table: "MedicinePharmacies",
                column: "PharmacyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalLabMedicalLabTest");

            migrationBuilder.DropTable(
                name: "MedicinePharmacies");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Tickets");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExpectedDuration",
                table: "Tickets",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "MedicinePharmacy",
                columns: table => new
                {
                    MedicinesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PharmaciesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePharmacy", x => new { x.MedicinesId, x.PharmaciesId });
                    table.ForeignKey(
                        name: "FK_MedicinePharmacy_Medicines_MedicinesId",
                        column: x => x.MedicinesId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinePharmacy_Pharmacies_PharmaciesId",
                        column: x => x.PharmaciesId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePharmacy_PharmaciesId",
                table: "MedicinePharmacy",
                column: "PharmaciesId");
        }
    }
}
