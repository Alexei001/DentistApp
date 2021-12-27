using Microsoft.EntityFrameworkCore.Migrations;

namespace DentistApp.Migrations
{
    public partial class secondMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Doctors_DoctorId",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Doctors_DoctorId",
                table: "Clients",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Doctors_DoctorId",
                table: "Clients");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Doctors_DoctorId",
                table: "Clients",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
