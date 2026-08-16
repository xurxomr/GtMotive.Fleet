using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GtMotive.Fleet.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "EndedOn",
                table: "Rentals",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndedOn",
                table: "Rentals");
        }
    }
}
