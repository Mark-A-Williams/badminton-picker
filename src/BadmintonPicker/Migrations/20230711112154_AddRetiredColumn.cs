using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonPicker.Migrations
{
    /// <inheritdoc />
    public partial class AddRetiredColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Retired",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Retired",
                table: "Players");
        }
    }
}
