using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1041_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updatedb5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboFoodItem_Combos_ComboID",
                table: "ComboFoodItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboFoodItem_FoodItems_FoodItemID",
                table: "ComboFoodItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboFoodItem",
                table: "ComboFoodItem");

            migrationBuilder.RenameTable(
                name: "ComboFoodItem",
                newName: "ComboFoodItems");

            migrationBuilder.RenameIndex(
                name: "IX_ComboFoodItem_FoodItemID",
                table: "ComboFoodItems",
                newName: "IX_ComboFoodItems_FoodItemID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboFoodItems",
                table: "ComboFoodItems",
                columns: new[] { "ComboID", "FoodItemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ComboFoodItems_Combos_ComboID",
                table: "ComboFoodItems",
                column: "ComboID",
                principalTable: "Combos",
                principalColumn: "ComboID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboFoodItems_FoodItems_FoodItemID",
                table: "ComboFoodItems",
                column: "FoodItemID",
                principalTable: "FoodItems",
                principalColumn: "FoodItemID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboFoodItems_Combos_ComboID",
                table: "ComboFoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboFoodItems_FoodItems_FoodItemID",
                table: "ComboFoodItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComboFoodItems",
                table: "ComboFoodItems");

            migrationBuilder.RenameTable(
                name: "ComboFoodItems",
                newName: "ComboFoodItem");

            migrationBuilder.RenameIndex(
                name: "IX_ComboFoodItems_FoodItemID",
                table: "ComboFoodItem",
                newName: "IX_ComboFoodItem_FoodItemID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComboFoodItem",
                table: "ComboFoodItem",
                columns: new[] { "ComboID", "FoodItemID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ComboFoodItem_Combos_ComboID",
                table: "ComboFoodItem",
                column: "ComboID",
                principalTable: "Combos",
                principalColumn: "ComboID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboFoodItem_FoodItems_FoodItemID",
                table: "ComboFoodItem",
                column: "FoodItemID",
                principalTable: "FoodItems",
                principalColumn: "FoodItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
