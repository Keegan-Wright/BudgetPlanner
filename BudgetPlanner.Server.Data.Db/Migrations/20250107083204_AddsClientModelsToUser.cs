using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Server.Data.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddsClientModelsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "OpenBankingAccounts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "HouseholdMembers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Debts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "CustomClassifications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "BudgetCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingAccounts_ApplicationUserId",
                table: "OpenBankingAccounts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HouseholdMembers_ApplicationUserId",
                table: "HouseholdMembers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Debts_ApplicationUserId",
                table: "Debts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomClassifications_ApplicationUserId",
                table: "CustomClassifications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetCategories_ApplicationUserId",
                table: "BudgetCategories",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetCategories_AspNetUsers_ApplicationUserId",
                table: "BudgetCategories",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomClassifications_AspNetUsers_ApplicationUserId",
                table: "CustomClassifications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_AspNetUsers_ApplicationUserId",
                table: "Debts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseholdMembers_AspNetUsers_ApplicationUserId",
                table: "HouseholdMembers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenBankingAccounts_AspNetUsers_ApplicationUserId",
                table: "OpenBankingAccounts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetCategories_AspNetUsers_ApplicationUserId",
                table: "BudgetCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomClassifications_AspNetUsers_ApplicationUserId",
                table: "CustomClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Debts_AspNetUsers_ApplicationUserId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_HouseholdMembers_AspNetUsers_ApplicationUserId",
                table: "HouseholdMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_OpenBankingAccounts_AspNetUsers_ApplicationUserId",
                table: "OpenBankingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_OpenBankingAccounts_ApplicationUserId",
                table: "OpenBankingAccounts");

            migrationBuilder.DropIndex(
                name: "IX_HouseholdMembers_ApplicationUserId",
                table: "HouseholdMembers");

            migrationBuilder.DropIndex(
                name: "IX_Debts_ApplicationUserId",
                table: "Debts");

            migrationBuilder.DropIndex(
                name: "IX_CustomClassifications_ApplicationUserId",
                table: "CustomClassifications");

            migrationBuilder.DropIndex(
                name: "IX_BudgetCategories_ApplicationUserId",
                table: "BudgetCategories");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "OpenBankingAccounts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "HouseholdMembers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CustomClassifications");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BudgetCategories");
        }
    }
}
