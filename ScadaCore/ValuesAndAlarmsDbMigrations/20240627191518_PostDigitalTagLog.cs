using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScadaCore.ValuesAndAlarmsDbMigrations
{
    /// <inheritdoc />
    public partial class PostDigitalTagLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmittedValue",
                table: "TagLogs");

            migrationBuilder.CreateTable(
                name: "AnalogTagLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmittedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogTagLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalogTagLogs_TagLogs_Id",
                        column: x => x.Id,
                        principalTable: "TagLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalTagLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EmittedValue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalTagLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DigitalTagLogs_TagLogs_Id",
                        column: x => x.Id,
                        principalTable: "TagLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalogTagLogs");

            migrationBuilder.DropTable(
                name: "DigitalTagLogs");

            migrationBuilder.AddColumn<decimal>(
                name: "EmittedValue",
                table: "TagLogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
