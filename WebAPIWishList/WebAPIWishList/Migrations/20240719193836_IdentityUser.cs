using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIWishList.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "wishItems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_wishItems_UserId",
                table: "wishItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_wishItems_AspNetUsers_UserId",
                table: "wishItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wishItems_AspNetUsers_UserId",
                table: "wishItems");

            migrationBuilder.DropIndex(
                name: "IX_wishItems_UserId",
                table: "wishItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "wishItems");
        }
    }
}
