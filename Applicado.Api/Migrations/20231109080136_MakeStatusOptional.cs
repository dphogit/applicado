using Applicado.Api.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Applicado.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeStatusOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Status>(
                name: "status",
                table: "job_applications",
                type: "status",
                nullable: true,
                oldClrType: typeof(Status),
                oldType: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Status>(
                name: "status",
                table: "job_applications",
                type: "status",
                nullable: false,
                defaultValue: Status.Open,
                oldClrType: typeof(Status),
                oldType: "status",
                oldNullable: true);
        }
    }
}
