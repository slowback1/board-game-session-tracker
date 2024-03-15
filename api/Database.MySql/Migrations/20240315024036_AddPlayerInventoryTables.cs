using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerInventoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerItemGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    InventoryTypeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItemGroups", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerItemGroupId = table.Column<Guid>(type: "char(36)", nullable: false),
                    InventoryTypeOptionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItems_InventoryTypeOptions_InventoryTypeOptionId",
                        column: x => x.InventoryTypeOptionId,
                        principalTable: "InventoryTypeOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerItems_PlayerItemGroups_PlayerItemGroupId",
                        column: x => x.PlayerItemGroupId,
                        principalTable: "PlayerItemGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_InventoryTypeOptionId",
                table: "PlayerItems",
                column: "InventoryTypeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_PlayerItemGroupId",
                table: "PlayerItems",
                column: "PlayerItemGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerItems");

            migrationBuilder.DropTable(
                name: "PlayerItemGroups");
        }
    }
}
