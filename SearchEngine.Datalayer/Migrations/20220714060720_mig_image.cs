using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SearchEngine.Datalayer.Migrations
{
    public partial class mig_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsImage",
                table: "Pages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsImage",
                table: "Pages");
        }
    }
}
