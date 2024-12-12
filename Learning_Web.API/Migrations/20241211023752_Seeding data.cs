using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Learning_Web.API.Migrations
{
    /// <inheritdoc />
    public partial class Seedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficultys",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("25ffb097-718a-4287-a194-85fb24a4dbb6"), "Medium" },
                    { new Guid("e348bf44-9717-48ba-8c3e-27c74d0987a3"), "Hard" },
                    { new Guid("f2a3769d-2b91-4879-a1e9-ef6601dac153"), "Easy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficultys",
                keyColumn: "Id",
                keyValue: new Guid("25ffb097-718a-4287-a194-85fb24a4dbb6"));

            migrationBuilder.DeleteData(
                table: "Difficultys",
                keyColumn: "Id",
                keyValue: new Guid("e348bf44-9717-48ba-8c3e-27c74d0987a3"));

            migrationBuilder.DeleteData(
                table: "Difficultys",
                keyColumn: "Id",
                keyValue: new Guid("f2a3769d-2b91-4879-a1e9-ef6601dac153"));
        }
    }
}
