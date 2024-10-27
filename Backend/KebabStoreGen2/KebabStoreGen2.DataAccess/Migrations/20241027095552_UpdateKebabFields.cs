using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KebabStoreGen2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKebabFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "KebabEntities",
                newName: "KebabName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "KebabEntities",
                newName: "KebabDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KebabName",
                table: "KebabEntities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "KebabDescription",
                table: "KebabEntities",
                newName: "Description");
        }
    }
}
