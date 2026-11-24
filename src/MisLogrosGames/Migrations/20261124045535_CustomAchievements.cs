using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MisLogrosGames.Migrations
{
    /// <inheritdoc />
    public partial class CustomAchievements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "achievements",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "owner_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "owner_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "owner_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "owner_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "achievements",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "owner_id",
                value: null);

            migrationBuilder.CreateIndex(
                name: "ix_achievements_owner_id",
                table: "achievements",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_achievements_users_owner_id",
                table: "achievements",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_achievements_users_owner_id",
                table: "achievements");

            migrationBuilder.DropIndex(
                name: "ix_achievements_owner_id",
                table: "achievements");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "achievements");
        }
    }
}
