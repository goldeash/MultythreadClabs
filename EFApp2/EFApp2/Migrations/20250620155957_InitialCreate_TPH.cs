using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFApp2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_TPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lab8_TPH_Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPH_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPH_Ships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    ShipType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AircraftCapacity = table.Column<int>(type: "int", nullable: true),
                    MainCaliberGuns = table.Column<int>(type: "int", nullable: true),
                    MissileCount = table.Column<int>(type: "int", nullable: true),
                    TorpedoTubes = table.Column<int>(type: "int", nullable: true),
                    MaxDepth = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPH_Ships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPH_Ships_Lab8_TPH_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPH_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPH_Ships_ManufacturerId",
                table: "Lab8_TPH_Ships",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lab8_TPH_Ships");

            migrationBuilder.DropTable(
                name: "Lab8_TPH_Manufacturers");
        }
    }
}
