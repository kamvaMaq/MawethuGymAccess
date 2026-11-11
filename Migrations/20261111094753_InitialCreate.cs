using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MawethuGym.Web.Migrations;

/// <inheritdoc />
public partial class _20261111094753_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Members",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                MemberNumber = table.Column<string>(type: "TEXT", nullable: false),
                FullName = table.Column<string>(type: "TEXT", nullable: false),
                Phone = table.Column<string>(type: "TEXT", nullable: false),
                Email = table.Column<string>(type: "TEXT", nullable: false),
                PlanType = table.Column<string>(type: "TEXT", nullable: false),
                ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                BiometricConsentGiven = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CheckIns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                Method = table.Column<string>(type: "TEXT", nullable: false),
                IsOverride = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CheckIns", x => x.Id);
                table.ForeignKey(
                    name: "FK_CheckIns_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CheckIns_MemberId",
            table: "CheckIns",
            column: "MemberId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CheckIns");

        migrationBuilder.DropTable(
            name: "Members");
    }
}
