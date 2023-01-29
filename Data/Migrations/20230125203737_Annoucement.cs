using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUZilla.Data.Migrations
{
    public partial class Annoucement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_AspNetUsers_StudentId",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Courses_CourseId",
                table: "Grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grade",
                table: "Grade");

            migrationBuilder.RenameTable(
                name: "Grade",
                newName: "Grades");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_StudentId",
                table: "Grades",
                newName: "IX_Grades_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_CourseId",
                table: "Grades",
                newName: "IX_Grades_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.RenameTable(
                name: "Grades",
                newName: "Grade");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_StudentId",
                table: "Grade",
                newName: "IX_Grade_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_CourseId",
                table: "Grade",
                newName: "IX_Grade_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grade",
                table: "Grade",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_AspNetUsers_StudentId",
                table: "Grade",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Courses_CourseId",
                table: "Grade",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
