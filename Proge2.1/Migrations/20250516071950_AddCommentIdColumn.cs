using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proge2._1.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "seller",
                table: "Materials",
                newName: "Seller");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Materials",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Budgets",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "client",
                table: "Budgets",
                newName: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seller",
                table: "Materials",
                newName: "seller");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Materials",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Budgets",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Client",
                table: "Budgets",
                newName: "client");
        }
    }
}
