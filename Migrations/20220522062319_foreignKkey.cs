using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace groupCW.Migrations
{
    public partial class foreignKkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers");

            migrationBuilder.AlterColumn<int>(
                name: "MembershipCategoryTotalLoans",
                table: "MembershipCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers",
                columns: new[] { "DVDNumber", "ActorNumber" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers");

            migrationBuilder.AlterColumn<string>(
                name: "MembershipCategoryTotalLoans",
                table: "MembershipCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CastMembers",
                table: "CastMembers",
                column: "DVDNumber");
        }
    }
}
