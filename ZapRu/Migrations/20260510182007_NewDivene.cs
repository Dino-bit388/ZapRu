using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZapRu.Migrations
{
    /// <inheritdoc />
    public partial class NewDivene : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "Cars",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Cars",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Cars",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Cars",
                newName: "Created_At");
        }
    }
}
