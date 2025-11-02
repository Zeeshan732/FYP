using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace neuro_kinetic_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTestRecordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTestRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TestResult = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accuracy = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VoiceRecordingUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AnalysisNotes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTestRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTestRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTestRecords_Status",
                table: "UserTestRecords",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestRecords_TestDate",
                table: "UserTestRecords",
                column: "TestDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestRecords_TestResult",
                table: "UserTestRecords",
                column: "TestResult");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestRecords_UserId",
                table: "UserTestRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTestRecords_UserName",
                table: "UserTestRecords",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTestRecords");
        }
    }
}
