using Microsoft.EntityFrameworkCore.Migrations;

namespace GameStore.Migrations
{
    public partial class UpdateTblAccount2_AddColumnSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Account");
        }
    }
}
