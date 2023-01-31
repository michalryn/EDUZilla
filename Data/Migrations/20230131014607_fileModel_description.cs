using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDUZilla.Data.Migrations
{
    public partial class fileModel_description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileDescription",
                table: "FileModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileDescription",
                table: "FileModels");
        }
    }
}
