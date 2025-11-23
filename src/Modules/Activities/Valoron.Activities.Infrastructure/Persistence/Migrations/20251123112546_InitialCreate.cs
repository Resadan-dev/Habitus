using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valoron.Activities.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "activities");

            migrationBuilder.CreateTable(
                name: "Activities",
                schema: "activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Category_Code = table.Column<string>(type: "text", nullable: false),
                    Category_Name = table.Column<string>(type: "text", nullable: false),
                    Difficulty_Value = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Measurement_Unit = table.Column<int>(type: "integer", nullable: false),
                    Measurement_Target = table.Column<decimal>(type: "numeric", nullable: false),
                    Measurement_Current = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TotalPages = table.Column<int>(type: "integer", nullable: false),
                    CurrentPage = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities",
                schema: "activities");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "activities");
        }
    }
}
