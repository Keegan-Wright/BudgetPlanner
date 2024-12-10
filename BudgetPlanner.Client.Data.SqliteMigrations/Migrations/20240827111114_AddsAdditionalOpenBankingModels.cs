using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddsAdditionalOpenBankingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Logo",
                table: "OpenBankingProviders",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "OpenBankingProviderId",
                table: "OpenBankingProviders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OpenBankingAccountBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Available = table.Column<decimal>(type: "TEXT", nullable: false),
                    Current = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingAccountBalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    AccountType = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingDirectDebits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OpenBankingDirectDebitId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PreviousPaymentTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PreviousPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingDirectDebits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingStandingOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Frequency = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    NextPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NextPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FirstPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FinalPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinalPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingStandingOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionType = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionCategory = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingTransactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenBankingAccountBalances");

            migrationBuilder.DropTable(
                name: "OpenBankingAccounts");

            migrationBuilder.DropTable(
                name: "OpenBankingDirectDebits");

            migrationBuilder.DropTable(
                name: "OpenBankingStandingOrders");

            migrationBuilder.DropTable(
                name: "OpenBankingTransactions");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "OpenBankingProviders");

            migrationBuilder.DropColumn(
                name: "OpenBankingProviderId",
                table: "OpenBankingProviders");
        }
    }
}
