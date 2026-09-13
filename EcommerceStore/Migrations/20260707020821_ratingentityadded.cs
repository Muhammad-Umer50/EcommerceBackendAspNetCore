using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceStore.Migrations
{
    /// <inheritdoc />
    public partial class ratingentityadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "rating",
                table: "Products",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                table: "Products");
        }
    }
}
