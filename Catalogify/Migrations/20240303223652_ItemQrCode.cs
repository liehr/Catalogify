using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalogify.Migrations
{
    /// <inheritdoc />
    public partial class ItemQrCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeBytes",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeBytes",
                table: "Items");
        }
    }
}
