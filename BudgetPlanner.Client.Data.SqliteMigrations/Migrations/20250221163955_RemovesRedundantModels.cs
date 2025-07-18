using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPlanner.Client.Data.SqliteMigrations.Migrations
{
    /// <inheritdoc />
    public partial class RemovesRedundantModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetCategories");

            migrationBuilder.DropTable(
                name: "CustomClassifications");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "Debts");

            migrationBuilder.DropTable(
                name: "HouseholdMembers");

            migrationBuilder.DropTable(
                name: "OpenBankingAccessTokens");

            migrationBuilder.DropTable(
                name: "OpenBankingAccountBalances");

            migrationBuilder.DropTable(
                name: "OpenBankingDirectDebits");

            migrationBuilder.DropTable(
                name: "OpenBankingProviderScopes");

            migrationBuilder.DropTable(
                name: "OpenBankingStandingOrders");

            migrationBuilder.DropTable(
                name: "OpenBankingSyncronisations");

            migrationBuilder.DropTable(
                name: "OpenBankingTransactionClassifications");

            migrationBuilder.DropTable(
                name: "OpenBankingTransactions");

            migrationBuilder.DropTable(
                name: "OpenBankingAccounts");

            migrationBuilder.DropTable(
                name: "OpenBankingProviders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvailableFunds = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GoalCompletionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MonthlyStart = table.Column<decimal>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SavingsGoal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomClassifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(type: "TEXT", nullable: true),
                    Xml = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Debts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinalPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MonthlyPayment = table.Column<decimal>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PayOffGoal = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseholdMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    Income = table.Column<decimal>(type: "TEXT", nullable: true),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseholdMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingAccessTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccessToken = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresIn = table.Column<int>(type: "INTEGER", nullable: false),
                    ProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingAccessTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingProviders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccessCode = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Logo = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OpenBankingProviderId = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountType = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    OpenBankingAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingAccounts_OpenBankingProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "OpenBankingProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingProviderScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingProviderScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingProviderScopes_OpenBankingProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "OpenBankingProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingAccountBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Available = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Current = table.Column<decimal>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingAccountBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingAccountBalances_OpenBankingAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "OpenBankingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingDirectDebits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OpenBankingDirectDebitId = table.Column<string>(type: "TEXT", nullable: false),
                    PreviousPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    PreviousPaymentTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingDirectDebits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingDirectDebits_OpenBankingAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "OpenBankingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingStandingOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    FinalPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FinalPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FirstPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FirstPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Frequency = table.Column<string>(type: "TEXT", nullable: false),
                    NextPaymentAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    NextPaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Payee = table.Column<string>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingStandingOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingStandingOrders_OpenBankingAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "OpenBankingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingSyncronisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OpenBankingAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    SyncronisationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SyncronisationType = table.Column<int>(type: "INTEGER", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingSyncronisations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingSyncronisations_OpenBankingAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "OpenBankingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenBankingSyncronisations_OpenBankingProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "OpenBankingProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProviderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Pending = table.Column<bool>(type: "INTEGER", nullable: false),
                    TransactionCategory = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TransactionType = table.Column<string>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingTransactions_OpenBankingAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "OpenBankingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenBankingTransactions_OpenBankingProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "OpenBankingProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenBankingTransactionClassifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Classification = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCustomClassification = table.Column<bool>(type: "INTEGER", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenBankingTransactionClassifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenBankingTransactionClassifications_OpenBankingTransactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "OpenBankingTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingAccountBalances_AccountId",
                table: "OpenBankingAccountBalances",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingAccounts_ProviderId",
                table: "OpenBankingAccounts",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingDirectDebits_AccountId",
                table: "OpenBankingDirectDebits",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingProviderScopes_ProviderId",
                table: "OpenBankingProviderScopes",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingStandingOrders_AccountId",
                table: "OpenBankingStandingOrders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingSyncronisations_AccountId",
                table: "OpenBankingSyncronisations",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingSyncronisations_ProviderId",
                table: "OpenBankingSyncronisations",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingTransactionClassifications_TransactionId",
                table: "OpenBankingTransactionClassifications",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingTransactions_AccountId",
                table: "OpenBankingTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenBankingTransactions_ProviderId",
                table: "OpenBankingTransactions",
                column: "ProviderId");
        }
    }
}
