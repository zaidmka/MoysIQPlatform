using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoysIQPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class StudentAnswerSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "studentanswersnapshots",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentid = table.Column<int>(type: "integer", nullable: false),
                    testid = table.Column<int>(type: "integer", nullable: false),
                    questionid = table.Column<int>(type: "integer", nullable: false),
                    studentanswertext = table.Column<string>(type: "text", nullable: true),
                    studentansweroptionid = table.Column<int>(type: "integer", nullable: true),
                    correctanswertextatsubmission = table.Column<string>(type: "text", nullable: true),
                    correctansweroptionidatsubmission = table.Column<int>(type: "integer", nullable: true),
                    iscorrect = table.Column<bool>(type: "boolean", nullable: false),
                    questionweight = table.Column<double>(type: "double precision", nullable: false),
                    submittedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentanswersnapshots", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentanswersnapshots");
        }
    }
}
