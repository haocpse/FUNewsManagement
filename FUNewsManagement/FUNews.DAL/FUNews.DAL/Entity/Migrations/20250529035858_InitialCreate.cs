using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FUNews.DAL.FUNews.DAL.Entity.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ParentCategoryId = table.Column<short>(type: "smallint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "SystemAccount",
                columns: table => new
                {
                    AccountId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountEmail = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    AccountRole = table.Column<int>(type: "int", nullable: true),
                    AccountPassword = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAccount", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "NewsArticle",
                columns: table => new
                {
                    NewsArticleId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NewsTitle = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Headline = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NewsContent = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    NewsSource = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CategoryId = table.Column<short>(type: "smallint", nullable: true),
                    NewsStatus = table.Column<bool>(type: "bit", nullable: true),
                    CreatedById = table.Column<short>(type: "smallint", nullable: true),
                    UpdatedById = table.Column<short>(type: "smallint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemAccountAccountId = table.Column<short>(type: "smallint", nullable: true),
                    SystemAccountAccountId1 = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticle", x => x.NewsArticleId);
                    table.ForeignKey(
                        name: "FK_NewsArticle_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsArticle_SystemAccount_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsArticle_SystemAccount_SystemAccountAccountId",
                        column: x => x.SystemAccountAccountId,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_NewsArticle_SystemAccount_SystemAccountAccountId1",
                        column: x => x.SystemAccountAccountId1,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_NewsArticle_SystemAccount_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsTag",
                columns: table => new
                {
                    NewsArticleId = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTag", x => new { x.NewsArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_NewsTag_NewsArticle_NewsArticleId",
                        column: x => x.NewsArticleId,
                        principalTable: "NewsArticle",
                        principalColumn: "NewsArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_CategoryId",
                table: "NewsArticle",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_CreatedById",
                table: "NewsArticle",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_SystemAccountAccountId",
                table: "NewsArticle",
                column: "SystemAccountAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_SystemAccountAccountId1",
                table: "NewsArticle",
                column: "SystemAccountAccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticle_UpdatedById",
                table: "NewsArticle",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTag_TagId",
                table: "NewsTag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsTag");

            migrationBuilder.DropTable(
                name: "NewsArticle");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "SystemAccount");
        }
    }
}
