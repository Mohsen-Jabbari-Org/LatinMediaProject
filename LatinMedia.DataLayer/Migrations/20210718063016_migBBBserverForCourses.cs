using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class migBBBserverForCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BBBServersId",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServerId",
                table: "Courses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_BBBServersId",
                table: "Courses",
                column: "BBBServersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_BBBServers_BBBServersId",
                table: "Courses",
                column: "BBBServersId",
                principalTable: "BBBServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_BBBServers_BBBServersId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_BBBServersId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "BBBServersId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "Courses");
        }
    }
}
