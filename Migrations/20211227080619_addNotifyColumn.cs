using Microsoft.EntityFrameworkCore.Migrations;

namespace DentistApp.Migrations
{
    public partial class addNotifyColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Notify",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notify",
                table: "Clients");
        }
    }
}
