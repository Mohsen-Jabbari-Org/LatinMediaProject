using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class migEditSurvay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survays_Educations_EducationEduId",
                table: "Survays");

            migrationBuilder.DropForeignKey(
                name: "FK_Survays_Employments_EmploymentEmpId",
                table: "Survays");

            migrationBuilder.DropIndex(
                name: "IX_Survays_EducationEduId",
                table: "Survays");

            migrationBuilder.DropIndex(
                name: "IX_Survays_EmploymentEmpId",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "EducationEduId",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "EmploymentEmpId",
                table: "Survays");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Survays",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EduId",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmpId",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Poll1",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Poll2",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Poll3",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Poll4",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Poll5",
                table: "Survays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SurvayDetails",
                columns: table => new
                {
                    SurvayId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    Poll1 = table.Column<int>(nullable: false),
                    Poll2 = table.Column<int>(nullable: false),
                    Poll3 = table.Column<int>(nullable: false),
                    Poll4 = table.Column<int>(nullable: false),
                    Poll5 = table.Column<int>(nullable: false),
                    Poll6 = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvayDetails", x => new { x.SurvayId, x.TeacherId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Survays_EduId",
                table: "Survays",
                column: "EduId");

            migrationBuilder.CreateIndex(
                name: "IX_Survays_EmpId",
                table: "Survays",
                column: "EmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Survays_Educations_EduId",
                table: "Survays",
                column: "EduId",
                principalTable: "Educations",
                principalColumn: "EduId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Survays_Employments_EmpId",
                table: "Survays",
                column: "EmpId",
                principalTable: "Employments",
                principalColumn: "EmpId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Survays_Educations_EduId",
                table: "Survays");

            migrationBuilder.DropForeignKey(
                name: "FK_Survays_Employments_EmpId",
                table: "Survays");

            migrationBuilder.DropTable(
                name: "SurvayDetails");

            migrationBuilder.DropIndex(
                name: "IX_Survays_EduId",
                table: "Survays");

            migrationBuilder.DropIndex(
                name: "IX_Survays_EmpId",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "EduId",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "EmpId",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "Poll1",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "Poll2",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "Poll3",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "Poll4",
                table: "Survays");

            migrationBuilder.DropColumn(
                name: "Poll5",
                table: "Survays");

            migrationBuilder.AddColumn<int>(
                name: "EducationEduId",
                table: "Survays",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmploymentEmpId",
                table: "Survays",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Survays_EducationEduId",
                table: "Survays",
                column: "EducationEduId");

            migrationBuilder.CreateIndex(
                name: "IX_Survays_EmploymentEmpId",
                table: "Survays",
                column: "EmploymentEmpId");

            migrationBuilder.AddForeignKey(
                name: "FK_Survays_Educations_EducationEduId",
                table: "Survays",
                column: "EducationEduId",
                principalTable: "Educations",
                principalColumn: "EduId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Survays_Employments_EmploymentEmpId",
                table: "Survays",
                column: "EmploymentEmpId",
                principalTable: "Employments",
                principalColumn: "EmpId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
