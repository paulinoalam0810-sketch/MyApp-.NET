using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteVisits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Persons_PersonId",
                table: "Visits");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonEntityId",
                table: "Visits",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PersonEntityId",
                table: "Visits",
                column: "PersonEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Persons_PersonEntityId",
                table: "Visits",
                column: "PersonEntityId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Persons_PersonId",
                table: "Visits",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Persons_PersonEntityId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Persons_PersonId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_PersonEntityId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "PersonEntityId",
                table: "Visits");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Persons_PersonId",
                table: "Visits",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
