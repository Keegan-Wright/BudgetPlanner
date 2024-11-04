using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class TrackClassificationsSyncLogsv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_AccountId1",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingSyncronisations_AccountId1",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "OpenBankingSyncronisations");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingSyncronisations_AccountId",
                table: "OpenBankingSyncronisations",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_AccountId",
                table: "OpenBankingSyncronisations",
                column: "AccountId",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_AccountId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingSyncronisations_AccountId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId1",
                table: "OpenBankingSyncronisations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingSyncronisations_AccountId1",
                table: "OpenBankingSyncronisations",
                column: "AccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_AccountId1",
                table: "OpenBankingSyncronisations",
                column: "AccountId1",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
