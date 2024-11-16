using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1041_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updatedb7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_FoodItems_FoodItemID",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "FoodItemID",
                table: "CartItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ComboID",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComboID1",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ComboID",
                table: "CartItems",
                column: "ComboID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ComboID1",
                table: "CartItems",
                column: "ComboID1");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Combos_ComboID",
                table: "CartItems",
                column: "ComboID",
                principalTable: "Combos",
                principalColumn: "ComboID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Combos_ComboID1",
                table: "CartItems",
                column: "ComboID1",
                principalTable: "Combos",
                principalColumn: "ComboID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_FoodItems_FoodItemID",
                table: "CartItems",
                column: "FoodItemID",
                principalTable: "FoodItems",
                principalColumn: "FoodItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Combos_ComboID",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Combos_ComboID1",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_FoodItems_FoodItemID",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ComboID",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ComboID1",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ComboID",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ComboID1",
                table: "CartItems");

            migrationBuilder.AlterColumn<int>(
                name: "FoodItemID",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_FoodItems_FoodItemID",
                table: "CartItems",
                column: "FoodItemID",
                principalTable: "FoodItems",
                principalColumn: "FoodItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
