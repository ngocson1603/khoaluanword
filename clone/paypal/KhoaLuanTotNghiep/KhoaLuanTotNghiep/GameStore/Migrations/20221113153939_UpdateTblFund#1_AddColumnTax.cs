using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Migrations
{
    public partial class UpdateTblFund1_AddColumnTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Tax",
                table: "Fund",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Fund");
        }
    }
}
