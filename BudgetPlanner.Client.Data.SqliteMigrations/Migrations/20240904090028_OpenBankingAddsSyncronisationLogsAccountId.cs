using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class OpenBankingAddsSyncronisationLogsAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "OpenBankingSyncronisations",
                newName: "OpenBankingProviderId");

            migrationBuilder.AddColumn<string>(
                name: "OpenBankingAccountId",
                table: "OpenBankingSyncronisations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenBankingAccountId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.RenameColumn(
                name: "OpenBankingProviderId",
                table: "OpenBankingSyncronisations",
                newName: "ProviderId");
        }
    }
}
