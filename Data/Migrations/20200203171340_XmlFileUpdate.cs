using Microsoft.EntityFrameworkCore.Migrations;

namespace XmlSigner.Data.Migrations
{
    public partial class XmlFileUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_XmlFiles_SignerId",
                table: "XmlFiles",
                column: "SignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_XmlFiles_AspNetUsers_SignerId",
                table: "XmlFiles",
                column: "SignerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_XmlFiles_AspNetUsers_SignerId",
                table: "XmlFiles");

            migrationBuilder.DropIndex(
                name: "IX_XmlFiles_SignerId",
                table: "XmlFiles");
        }
    }
}
