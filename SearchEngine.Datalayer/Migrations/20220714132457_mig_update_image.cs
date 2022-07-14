using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchEngine.Datalayer.Migrations
{
    public partial class mig_update_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePageUrl",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePageUrl",
                table: "Pages");
        }
    }
}
