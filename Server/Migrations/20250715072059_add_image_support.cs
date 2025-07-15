using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoysIQPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_image_support : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageurl",
                table: "questions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageurl",
                table: "answeroptions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_studentanswersnapshots_questionid",
                table: "studentanswersnapshots",
                column: "questionid");

            migrationBuilder.CreateIndex(
                name: "IX_studentanswersnapshots_testid",
                table: "studentanswersnapshots",
                column: "testid");

            migrationBuilder.AddForeignKey(
                name: "FK_studentanswersnapshots_questions_questionid",
                table: "studentanswersnapshots",
                column: "questionid",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentanswersnapshots_tests_testid",
                table: "studentanswersnapshots",
                column: "testid",
                principalTable: "tests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentanswersnapshots_questions_questionid",
                table: "studentanswersnapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_studentanswersnapshots_tests_testid",
                table: "studentanswersnapshots");

            migrationBuilder.DropIndex(
                name: "IX_studentanswersnapshots_questionid",
                table: "studentanswersnapshots");

            migrationBuilder.DropIndex(
                name: "IX_studentanswersnapshots_testid",
                table: "studentanswersnapshots");

            migrationBuilder.DropColumn(
                name: "imageurl",
                table: "questions");

            migrationBuilder.DropColumn(
                name: "imageurl",
                table: "answeroptions");
        }
    }
}
