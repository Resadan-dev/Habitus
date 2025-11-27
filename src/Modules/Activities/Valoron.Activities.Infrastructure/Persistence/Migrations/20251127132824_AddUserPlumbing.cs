using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valoron.Activities.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPlumbing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "activities",
                table: "Books",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "activities",
                table: "Activities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "activities",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "activities",
                table: "Activities");
        }
    }
}
