using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class OpenBankingModelUpdatesSODD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Payee",
                table: "OpenBankingStandingOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "OpenBankingStandingOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "OpenBankingDirectDebits",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payee",
                table: "OpenBankingStandingOrders");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "OpenBankingStandingOrders");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "OpenBankingDirectDebits");
        }
    }
}
