using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSS.System.V2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalLabResultFieldsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalLabTestResultFields",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KeyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResultFieldType = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalLabTestResultFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalLabTestMedicalLabTestResultField",
                columns: table => new
                {
                    FieldsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalLabTestMedicalLabTestResultField", x => new { x.FieldsId, x.TestsId });
                    table.ForeignKey(
                        name: "FK_MedicalLabTestMedicalLabTestResultField_MedicalLabTestResultFields_FieldsId",
                        column: x => x.FieldsId,
                        principalTable: "MedicalLabTestResultFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalLabTestMedicalLabTestResultField_Tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalLabTestMedicalLabTestResultField_TestsId",
                table: "MedicalLabTestMedicalLabTestResultField",
                column: "TestsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalLabTestMedicalLabTestResultField");

            migrationBuilder.DropTable(
                name: "MedicalLabTestResultFields");
        }
    }
}
