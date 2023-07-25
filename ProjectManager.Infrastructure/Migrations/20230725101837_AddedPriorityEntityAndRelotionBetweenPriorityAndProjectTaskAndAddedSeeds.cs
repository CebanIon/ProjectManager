using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    public partial class AddedPriorityEntityAndRelotionBetweenPriorityAndProjectTaskAndAddedSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriorityId",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PriorityValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Priority",
                columns: new[] { "Id", "Name", "PriorityValue" },
                values: new object[,]
                {
                    { 1, "Low", 1 },
                    { 2, "Medium", 2 },
                    { 3, "High", 3 },
                    { 4, "Urgent", 4 }
                });

            migrationBuilder.InsertData(
                table: "ProjectStates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 4, null, "Done" });

            migrationBuilder.InsertData(
                table: "ProjectTaskStates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 5, null, "In progress" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_PriorityId",
                table: "ProjectTasks",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Priority_PriorityId",
                table: "ProjectTasks",
                column: "PriorityId",
                principalTable: "Priority",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Priority_PriorityId",
                table: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_PriorityId",
                table: "ProjectTasks");

            migrationBuilder.DeleteData(
                table: "ProjectStates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProjectTaskStates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "ProjectTasks");
        }
    }
}
