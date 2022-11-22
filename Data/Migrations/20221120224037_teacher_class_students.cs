using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUZilla.Data.Migrations
{
    public partial class teacher_class_students : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_TeacherId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Classes",
                newName: "TutorId");

            migrationBuilder.AddColumn<int>(
                name: "TutorClassId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TutorId",
                table: "Classes",
                column: "TutorId",
                unique: true,
                filter: "[TutorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_TutorId",
                table: "Classes",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_AspNetUsers_TutorId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_TutorId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "TutorClassId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TutorId",
                table: "Classes",
                newName: "TeacherId");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TeacherId",
                table: "Classes",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_AspNetUsers_TeacherId",
                table: "Classes",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
