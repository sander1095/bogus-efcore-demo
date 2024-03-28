using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcore_practical_example.Migrations
{
    /// <inheritdoc />
    public partial class initialwithseed3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Sander's .NET Blog!!");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Blogs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Sander's .NET Blog");
        }
    }
}
