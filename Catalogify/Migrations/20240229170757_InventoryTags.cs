using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalogify.Migrations
{
    /// <inheritdoc />
    public partial class InventoryTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InventoryId",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_InventoryId",
                table: "Categories",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Inventories_InventoryId",
                table: "Categories",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Inventories_InventoryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_InventoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Categories");
        }
    }
}
