using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class migEditSurvay3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_SurvayDetails_Survays_SurvayId",
                table: "SurvayDetails",
                column: "SurvayId",
                principalTable: "Survays",
                principalColumn: "SurvayId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurvayDetails_Survays_SurvayId",
                table: "SurvayDetails");
        }
    }
}
