using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class DbStructureToHaveRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "OpenBankingTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "OpenBankingAccountId",
                table: "OpenBankingSyncronisations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "OpenBankingSyncronisations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "OpenBankingStandingOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "OpenBankingDirectDebits",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "OpenBankingAccountBalances",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingTransactions_AccountId",
                table: "OpenBankingTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingTransactions_ProviderId",
                table: "OpenBankingTransactions",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingSyncronisations_OpenBankingAccountId",
                table: "OpenBankingSyncronisations",
                column: "OpenBankingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingSyncronisations_ProviderId",
                table: "OpenBankingSyncronisations",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingStandingOrders_AccountId",
                table: "OpenBankingStandingOrders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingProviderScopes_ProviderId",
                table: "OpenBankingProviderScopes",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingDirectDebits_AccountId",
                table: "OpenBankingDirectDebits",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingAccounts_ProviderId",
                table: "OpenBankingAccounts",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingAccountBalances_AccountId",
                table: "OpenBankingAccountBalances",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingAccountBalances_OpenBankingAccounts_AccountId",
                table: "OpenBankingAccountBalances",
                column: "AccountId",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingAccounts_OpenBankingProviders_ProviderId",
                table: "OpenBankingAccounts",
                column: "ProviderId",
                principalTable: "OpenBankingProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingDirectDebits_OpenBankingAccounts_AccountId",
                table: "OpenBankingDirectDebits",
                column: "AccountId",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingProviderScopes_OpenBankingProviders_ProviderId",
                table: "OpenBankingProviderScopes",
                column: "ProviderId",
                principalTable: "OpenBankingProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingStandingOrders_OpenBankingAccounts_AccountId",
                table: "OpenBankingStandingOrders",
                column: "AccountId",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_OpenBankingAccountId",
                table: "OpenBankingSyncronisations",
                column: "OpenBankingAccountId",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingProviders_ProviderId",
                table: "OpenBankingSyncronisations",
                column: "ProviderId",
                principalTable: "OpenBankingProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingTransactions_OpenBankingAccounts_AccountId",
                table: "OpenBankingTransactions",
                column: "AccountId",
                principalTable: "OpenBankingAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingTransactions_OpenBankingProviders_ProviderId",
                table: "OpenBankingTransactions",
                column: "ProviderId",
                principalTable: "OpenBankingProviders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingAccountBalances_OpenBankingAccounts_AccountId",
                table: "OpenBankingAccountBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingAccounts_OpenBankingProviders_ProviderId",
                table: "OpenBankingAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingDirectDebits_OpenBankingAccounts_AccountId",
                table: "OpenBankingDirectDebits");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingProviderScopes_OpenBankingProviders_ProviderId",
                table: "OpenBankingProviderScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingStandingOrders_OpenBankingAccounts_AccountId",
                table: "OpenBankingStandingOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_OpenBankingAccountId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingSyncronisations_OpenBankingProviders_ProviderId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingTransactions_OpenBankingAccounts_AccountId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingTransactions_OpenBankingProviders_ProviderId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingTransactions_AccountId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingTransactions_ProviderId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingSyncronisations_OpenBankingAccountId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingSyncronisations_ProviderId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingStandingOrders_AccountId",
                table: "OpenBankingStandingOrders");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingProviderScopes_ProviderId",
                table: "OpenBankingProviderScopes");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingDirectDebits_AccountId",
                table: "OpenBankingDirectDebits");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingAccounts_ProviderId",
                table: "OpenBankingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingAccountBalances_AccountId",
                table: "OpenBankingAccountBalances");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OpenBankingTransactions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OpenBankingSyncronisations");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OpenBankingStandingOrders");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OpenBankingDirectDebits");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "OpenBankingAccountBalances");

            migrationBuilder.AlterColumn<string>(
                name: "OpenBankingAccountId",
                table: "OpenBankingSyncronisations",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
