using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class IdLinkingAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenBankingProviderId",
                table: "OpenBankingSyncronisations",
                newName: "ProviderId");

            migrationBuilder.RenameColumn(
                name: "OpenBankingProviderId",
                table: "OpenBankingAccounts",
                newName: "ProviderId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderId",
                table: "OpenBankingTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "OpenBankingTransactions");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "OpenBankingSyncronisations",
                newName: "OpenBankingProviderId");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "OpenBankingAccounts",
                newName: "OpenBankingProviderId");
        }
    }
}
