using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class OpenBankingModelUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "OpenBankingTransactions",
                newName: "TransactionTime");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "OpenBankingAccounts",
                newName: "OpenBankingProviderId");

            migrationBuilder.AddColumn<string>(
                name: "OpenBankingAccountId",
                table: "OpenBankingTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Pending",
                table: "OpenBankingTransactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "OpenBankingTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OpenBankingAccountId",
                table: "OpenBankingAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OpenBankingAccountId",
                table: "OpenBankingAccountBalances",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenBankingAccountId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropColumn(
                name: "Pending",
                table: "OpenBankingTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropColumn(
                name: "OpenBankingAccountId",
                table: "OpenBankingAccounts");

            migrationBuilder.DropColumn(
                name: "OpenBankingAccountId",
                table: "OpenBankingAccountBalances");

            migrationBuilder.RenameColumn(
                name: "TransactionTime",
                table: "OpenBankingTransactions",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "OpenBankingProviderId",
                table: "OpenBankingAccounts",
                newName: "AccountId");
        }
    }
}
