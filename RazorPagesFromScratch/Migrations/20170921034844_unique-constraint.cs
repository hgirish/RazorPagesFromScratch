using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RazorPagesFromScratch.Migrations
{
    public partial class uniqueconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_TodoLists_ListId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ListId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Items",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ListId",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AltenateKey_TextListId",
                table: "Items",
                columns: new[] { "ListId", "Text" });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_TodoLists_ListId",
                table: "Items",
                column: "ListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_TodoLists_ListId",
                table: "Items");

            migrationBuilder.DropUniqueConstraint(
                name: "AltenateKey_TextListId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Items",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "ListId",
                table: "Items",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
    }
}
