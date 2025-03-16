using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddNullProductCategoryToProduct : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ProductCategoryId",
            table: "Products",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Products_ProductCategoryId",
            table: "Products",
            column: "ProductCategoryId");

        migrationBuilder.AddForeignKey(
            name: "FK_Products_ProductCategories_ProductCategoryId",
            table: "Products",
            column: "ProductCategoryId",
            principalTable: "ProductCategories",
            principalColumn: "Id");

        migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM ProductCategories WHERE Name = 'Uncategorized')
                BEGIN
                    INSERT INTO ProductCategories (Name) VALUES ('Uncategorized')
                END
            ");

        migrationBuilder.Sql(@"
                UPDATE Products
                SET ProductCategoryId = (SELECT Id FROM ProductCategories WHERE Name = 'Uncategorized')
                WHERE ProductCategoryId IS NULL;
            ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Products_ProductCategories_ProductCategoryId",
            table: "Products");

        migrationBuilder.DropIndex(
            name: "IX_Products_ProductCategoryId",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "ProductCategoryId",
            table: "Products");
    }
}
