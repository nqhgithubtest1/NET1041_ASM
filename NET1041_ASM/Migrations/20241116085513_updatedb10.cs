using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1041_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updatedb10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "CartItems",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CartItems",
                newName: "TotalPrice");
        }
    }
}
