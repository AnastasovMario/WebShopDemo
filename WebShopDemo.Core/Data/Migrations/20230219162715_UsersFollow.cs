using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShopDemo.Core.Data.Migrations
{
    public partial class UsersFollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_UserCreatedId",
                table: "Users",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserUpdatedId",
                table: "Users",
                column: "UserUpdatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserCreatedId",
                table: "Users",
                column: "UserCreatedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserUpdatedId",
                table: "Users",
                column: "UserUpdatedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserCreatedId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserUpdatedId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserCreatedId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserUpdatedId",
                table: "Users");
        }
    }
}
