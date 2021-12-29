using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class migEditEvent1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_CourseId",
                table: "Events",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Courses_CourseId",
                table: "Events",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Courses_CourseId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CourseId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Events");
        }
    }
}
