using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lms.Data.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Module_CourseId",
                table: "Module",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_Module_CourseId",
                table: "Module");
        }
    }
}
