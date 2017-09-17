using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RazorPagesFromScratch.Migrations
{
    public partial class todolist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "Items",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TodoLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ListId",
                table: "Items",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_TodoLists_ListId",
                table: "Items",
                column: "ListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_TodoLists_ListId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "TodoLists");

            migrationBuilder.DropIndex(
                name: "IX_Items_ListId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "Items");
        }
    }
}
