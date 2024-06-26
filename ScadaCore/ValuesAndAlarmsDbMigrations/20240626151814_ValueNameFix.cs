using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScadaCore.ValuesAndAlarmsDbMigrations
{
    /// <inheritdoc />
    public partial class ValueNameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueName",
                table: "AlarmLogs",
                newName: "Unit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "AlarmLogs",
                newName: "ValueName");
        }
    }
}
