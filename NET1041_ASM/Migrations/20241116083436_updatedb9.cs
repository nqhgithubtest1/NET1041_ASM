using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1041_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updatedb9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "OrderTime");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CartItems",
                newName: "TotalPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderTime",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "CartItems",
                newName: "Price");
        }
    }
}
