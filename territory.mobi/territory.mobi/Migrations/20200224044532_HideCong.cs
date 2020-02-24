using Microsoft.EntityFrameworkCore.Migrations;

namespace territory.mobi.Migrations
{
    public partial class HideCong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "streetNo",
                schema: "app",
                table: "DoNotCall",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 10);

            migrationBuilder.AddColumn<bool>(
                name: "hide",
                schema: "app",
                table: "Cong",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hide",
                schema: "app",
                table: "Cong");

            migrationBuilder.AlterColumn<string>(
                name: "streetNo",
                schema: "app",
                table: "DoNotCall",
                unicode: false,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 100);
        }
    }
}
