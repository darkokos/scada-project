using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScadaCore.ValuesAndAlarmsDbMigrations
{
    /// <inheritdoc />
    public partial class LogsChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlarmLogs_Alarm_AlarmId",
                table: "AlarmLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TagLogs_Tag_TagName",
                table: "TagLogs");

            migrationBuilder.DropTable(
                name: "Alarm");

            migrationBuilder.DropTable(
                name: "AnalogOutputTags");

            migrationBuilder.DropTable(
                name: "DigitalInputTags");

            migrationBuilder.DropTable(
                name: "DigitalOutputTags");

            migrationBuilder.DropTable(
                name: "AnalogInputTags");

            migrationBuilder.DropTable(
                name: "OutputTags");

            migrationBuilder.DropTable(
                name: "InputTags");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_TagLogs_TagName",
                table: "TagLogs");

            migrationBuilder.DropIndex(
                name: "IX_AlarmLogs_AlarmId",
                table: "AlarmLogs");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "TagLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "EmittedValue",
                table: "TagLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AlarmLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ValueName",
                table: "AlarmLogs",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmittedValue",
                table: "TagLogs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AlarmLogs");

            migrationBuilder.DropColumn(
                name: "ValueName",
                table: "AlarmLogs");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "TagLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InputOutputAddress = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "InputTags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Driver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsScanned = table.Column<bool>(type: "bit", nullable: false),
                    ScanTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputTags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_InputTags_Tag_Name",
                        column: x => x.Name,
                        principalTable: "Tag",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutputTags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InitialValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputTags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_OutputTags_Tag_Name",
                        column: x => x.Name,
                        principalTable: "Tag",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalogInputTags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HighLimit = table.Column<int>(type: "int", nullable: false),
                    LowLimit = table.Column<int>(type: "int", nullable: false),
                    Units = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogInputTags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_AnalogInputTags_InputTags_Name",
                        column: x => x.Name,
                        principalTable: "InputTags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalInputTags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalInputTags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_DigitalInputTags_InputTags_Name",
                        column: x => x.Name,
                        principalTable: "InputTags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalogOutputTags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HighLimit = table.Column<int>(type: "int", nullable: false),
                    LowLimit = table.Column<int>(type: "int", nullable: false),
                    Units = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalogOutputTags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_AnalogOutputTags_OutputTags_Name",
                        column: x => x.Name,
                        principalTable: "OutputTags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DigitalOutputTags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalOutputTags", x => x.Name);
                    table.ForeignKey(
                        name: "FK_DigitalOutputTags_OutputTags_Name",
                        column: x => x.Name,
                        principalTable: "OutputTags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alarm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalogInputTagName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Threshold = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ValueName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alarm_AnalogInputTags_AnalogInputTagName",
                        column: x => x.AnalogInputTagName,
                        principalTable: "AnalogInputTags",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagLogs_TagName",
                table: "TagLogs",
                column: "TagName");

            migrationBuilder.CreateIndex(
                name: "IX_AlarmLogs_AlarmId",
                table: "AlarmLogs",
                column: "AlarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Alarm_AnalogInputTagName",
                table: "Alarm",
                column: "AnalogInputTagName");

            migrationBuilder.AddForeignKey(
                name: "FK_AlarmLogs_Alarm_AlarmId",
                table: "AlarmLogs",
                column: "AlarmId",
                principalTable: "Alarm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagLogs_Tag_TagName",
                table: "TagLogs",
                column: "TagName",
                principalTable: "Tag",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
