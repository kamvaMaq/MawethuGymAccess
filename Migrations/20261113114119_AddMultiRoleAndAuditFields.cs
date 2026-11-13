using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MawethuGym.Web.Migrations;

/// <inheritdoc />
public partial class _20261113114119_AddMultiRoleAndAuditFields : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<double>(
            name: "CurrentWeightKg",
            table: "Members",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<string>(
            name: "FaceDescriptorJson",
            table: "Members",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "RegisteredByStaffId",
            table: "Members",
            type: "INTEGER",
            nullable: true);

        migrationBuilder.AddColumn<double>(
            name: "WeightGoalKg",
            table: "Members",
            type: "REAL",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.CreateTable(
            name: "Staff",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Username = table.Column<string>(type: "TEXT", nullable: false),
                FullName = table.Column<string>(type: "TEXT", nullable: false),
                Role = table.Column<string>(type: "TEXT", nullable: false),
                PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Staff", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                MemberId = table.Column<int>(type: "INTEGER", nullable: false),
                Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                Method = table.Column<string>(type: "TEXT", nullable: false),
                Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                StaffId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Payments_Staff_StaffId",
                    column: x => x.StaffId,
                    principalTable: "Staff",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Members_RegisteredByStaffId",
            table: "Members",
            column: "RegisteredByStaffId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_MemberId",
            table: "Payments",
            column: "MemberId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_StaffId",
            table: "Payments",
            column: "StaffId");

        migrationBuilder.AddForeignKey(
            name: "FK_Members_Staff_RegisteredByStaffId",
            table: "Members",
            column: "RegisteredByStaffId",
            principalTable: "Staff",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Members_Staff_RegisteredByStaffId",
            table: "Members");

        migrationBuilder.DropTable(
            name: "Payments");

        migrationBuilder.DropTable(
            name: "Staff");

        migrationBuilder.DropIndex(
            name: "IX_Members_RegisteredByStaffId",
            table: "Members");

        migrationBuilder.DropColumn(
            name: "CurrentWeightKg",
            table: "Members");

        migrationBuilder.DropColumn(
            name: "FaceDescriptorJson",
            table: "Members");

        migrationBuilder.DropColumn(
            name: "RegisteredByStaffId",
            table: "Members");

        migrationBuilder.DropColumn(
            name: "WeightGoalKg",
            table: "Members");
    }
}
