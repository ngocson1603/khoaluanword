using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Migrations
{
    public partial class UpdateTblTransactionHistory2_AddColumnPreviousWalletBalance_NewWalletBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewWalletBalance",
                table: "TransactionHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreviousWalletBalance",
                table: "TransactionHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewWalletBalance",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "PreviousWalletBalance",
                table: "TransactionHistory");
        }
    }
}
