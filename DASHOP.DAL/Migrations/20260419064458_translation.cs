using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DASHOP.DAL.Migrations
{
    /// <inheritdoc />
    public partial class translation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categoryTranslation_categories_CategoryId",
                table: "categoryTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categoryTranslation",
                table: "categoryTranslation");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "categoryTranslation",
                newName: "CategoryTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_categoryTranslation_CategoryId",
                table: "CategoryTranslations",
                newName: "IX_CategoryTranslations_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTranslations",
                table: "CategoryTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTranslations_Categories_CategoryId",
                table: "CategoryTranslations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTranslations_Categories_CategoryId",
                table: "CategoryTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTranslations",
                table: "CategoryTranslations");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "CategoryTranslations",
                newName: "categoryTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryTranslations_CategoryId",
                table: "categoryTranslation",
                newName: "IX_categoryTranslation_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categoryTranslation",
                table: "categoryTranslation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categoryTranslation_categories_CategoryId",
                table: "categoryTranslation",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
