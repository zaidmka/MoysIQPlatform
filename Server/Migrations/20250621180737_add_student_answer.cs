using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoysIQPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_student_answer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "studentanswers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentid = table.Column<int>(type: "integer", nullable: false),
                    testid = table.Column<int>(type: "integer", nullable: false),
                    questionid = table.Column<int>(type: "integer", nullable: false),
                    answeroptionid = table.Column<int>(type: "integer", nullable: true),
                    writtenanswer = table.Column<string>(type: "text", nullable: true),
                    answeredat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentanswers", x => x.id);
                    table.ForeignKey(
                        name: "FK_studentanswers_answeroptions_answeroptionid",
                        column: x => x.answeroptionid,
                        principalTable: "answeroptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_studentanswers_questions_questionid",
                        column: x => x.questionid,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentanswers_students_studentid",
                        column: x => x.studentid,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentanswers_tests_testid",
                        column: x => x.testid,
                        principalTable: "tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_studentanswers_answeroptionid",
                table: "studentanswers",
                column: "answeroptionid");

            migrationBuilder.CreateIndex(
                name: "IX_studentanswers_questionid",
                table: "studentanswers",
                column: "questionid");

            migrationBuilder.CreateIndex(
                name: "IX_studentanswers_studentid",
                table: "studentanswers",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_studentanswers_testid",
                table: "studentanswers",
                column: "testid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentanswers");
        }
    }
}
