using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Migrations
{
    public partial class UpdateTblTransactionHistory_RenameColumnDateCreate_PurchasedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreate",
                table: "TransactionHistory",
                newName: "PurchasedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchasedDate",
                table: "TransactionHistory",
                newName: "DateCreate");
        }
    }
}
