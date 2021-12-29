using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class migsurvay1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationEduId",
                table: "Survays",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EduId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EduName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EduId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Survays_EducationEduId",
                table: "Survays",
                column: "EducationEduId");

            migrationBuilder.AddForeignKey(
                name: "FK_Survays_Educations_EducationEduId",
                table: "Survays",
                column: "EducationEduId",
                principalTable: "Educations",
                principalColumn: "EduId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survays_Educations_EducationEduId",
                table: "Survays");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropIndex(
                name: "IX_Survays_EducationEduId",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "EducationEduId",
                table: "Survays");
        }
    }
}
