using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1041_ASM.Migrations
{
    /// <inheritdoc />
    public partial class updatedb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboFoodItems");

            migrationBuilder.CreateTable(
                name: "ComboFoodItem",
                columns: table => new
                {
                    ComboID = table.Column<int>(type: "int", nullable: false),
                    FoodItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboFoodItem", x => new { x.ComboID, x.FoodItemID });
                    table.ForeignKey(
                        name: "FK_ComboFoodItem_Combos_ComboID",
                        column: x => x.ComboID,
                        principalTable: "Combos",
                        principalColumn: "ComboID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboFoodItem_FoodItems_FoodItemID",
                        column: x => x.FoodItemID,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboFoodItem_FoodItemID",
                table: "ComboFoodItem",
                column: "FoodItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComboFoodItem");

            migrationBuilder.CreateTable(
                name: "ComboFoodItems",
                columns: table => new
                {
                    CombosComboID = table.Column<int>(type: "int", nullable: false),
                    FoodItemsFoodItemID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboFoodItems", x => new { x.CombosComboID, x.FoodItemsFoodItemID });
                    table.ForeignKey(
                        name: "FK_ComboFoodItems_Combos_CombosComboID",
                        column: x => x.CombosComboID,
                        principalTable: "Combos",
                        principalColumn: "ComboID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboFoodItems_FoodItems_FoodItemsFoodItemID",
                        column: x => x.FoodItemsFoodItemID,
                        principalTable: "FoodItems",
                        principalColumn: "FoodItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboFoodItems_FoodItemsFoodItemID",
                table: "ComboFoodItems",
                column: "FoodItemsFoodItemID");
        }
    }
}
