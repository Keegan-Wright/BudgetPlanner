using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class DbStructureToHaveRelationsFurtherChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenBankingAccountId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropColumn(
                name: "OpenBankingAccountId",
                table: "OpenBankingAccountBalances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpenBankingAccountId",
                table: "OpenBankingTransactions",
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
    }
}
