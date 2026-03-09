using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDoWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLabelAndConfigureTaskCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Labels_LabelId",
                table: "TaskItems");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.RenameColumn(
                name: "LabelId",
                table: "TaskItems",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_LabelId",
                table: "TaskItems",
                newName: "IX_TaskItems_CategoryId");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "None", "None", "Default" },
                    { 2, "Blue", "A node.js course", "COMP1511" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Categories_CategoryId",
                table: "TaskItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Categories_CategoryId",
                table: "TaskItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "TaskItems",
                newName: "LabelId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_CategoryId",
                table: "TaskItems",
                newName: "IX_TaskItems_LabelId");

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Labels_LabelId",
                table: "TaskItems",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
