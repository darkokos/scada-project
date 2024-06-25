using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScadaCore.ValuesAndAlarmsDbMigrations
{
    /// <inheritdoc />
    public partial class ModelFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EmittedValue",
                table: "TagLogs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "AlarmLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "AlarmLogs");

            migrationBuilder.AlterColumn<int>(
                name: "EmittedValue",
                table: "TagLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
