using Microsoft.EntityFrameworkCore.Migrations;

namespace LatinMedia.DataLayer.Migrations
{
    public partial class migUserEvent1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Courses_CourseId",
                table: "UserEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_EventTypes_EventId",
                table: "UserEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvent_Users_UserId",
                table: "UserEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEvent",
                table: "UserEvent");

            migrationBuilder.RenameTable(
                name: "UserEvent",
                newName: "UserEvents");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvent_UserId",
                table: "UserEvents",
                newName: "IX_UserEvents_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvent_EventId",
                table: "UserEvents",
                newName: "IX_UserEvents_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvent_CourseId",
                table: "UserEvents",
                newName: "IX_UserEvents_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEvents",
                table: "UserEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_Courses_CourseId",
                table: "UserEvents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_EventTypes_EventId",
                table: "UserEvents",
                column: "EventId",
                principalTable: "EventTypes",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_Users_UserId",
                table: "UserEvents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_Courses_CourseId",
                table: "UserEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_EventTypes_EventId",
                table: "UserEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_Users_UserId",
                table: "UserEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserEvents",
                table: "UserEvents");

            migrationBuilder.RenameTable(
                name: "UserEvents",
                newName: "UserEvent");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvents_UserId",
                table: "UserEvent",
                newName: "IX_UserEvent_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvents_EventId",
                table: "UserEvent",
                newName: "IX_UserEvent_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEvents_CourseId",
                table: "UserEvent",
                newName: "IX_UserEvent_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserEvent",
                table: "UserEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Courses_CourseId",
                table: "UserEvent",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_EventTypes_EventId",
                table: "UserEvent",
                column: "EventId",
                principalTable: "EventTypes",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvent_Users_UserId",
                table: "UserEvent",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
