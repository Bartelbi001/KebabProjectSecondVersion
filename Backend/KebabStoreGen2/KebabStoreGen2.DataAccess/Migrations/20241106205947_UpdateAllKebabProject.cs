using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KebabStoreGen2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllKebabProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KebabName",
                table: "KebabEntities",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "KebabDescription",
                table: "KebabEntities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Calories",
                table: "KebabEntities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "KebabEntities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stuffing",
                table: "KebabEntities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wrap",
                table: "KebabEntities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IngredientEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    WeightInGrams = table.Column<int>(type: "int", nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false),
                    Protein = table.Column<int>(type: "int", nullable: false),
                    Fat = table.Column<int>(type: "int", nullable: false),
                    Carbs = table.Column<int>(type: "int", nullable: false),
                    KebabEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IngredientEntities_KebabEntities_KebabEntityId",
                        column: x => x.KebabEntityId,
                        principalTable: "KebabEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientEntities_KebabEntityId",
                table: "IngredientEntities",
                column: "KebabEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientEntities");

            migrationBuilder.DropColumn(
                name: "Calories",
                table: "KebabEntities");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "KebabEntities");

            migrationBuilder.DropColumn(
                name: "Stuffing",
                table: "KebabEntities");

            migrationBuilder.DropColumn(
                name: "Wrap",
                table: "KebabEntities");

            migrationBuilder.AlterColumn<string>(
                name: "KebabName",
                table: "KebabEntities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "KebabDescription",
                table: "KebabEntities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
