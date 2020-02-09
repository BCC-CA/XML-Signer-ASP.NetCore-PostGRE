using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XmlSigner.Data.Migrations
{
    public partial class CreateTimeErrorFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "XmlFiles",
                type: "TIMESTAMPTZ",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMPTZ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "XmlFiles",
                type: "TIMESTAMPTZ",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMPTZ",
                oldNullable: true);
        }
    }
}
