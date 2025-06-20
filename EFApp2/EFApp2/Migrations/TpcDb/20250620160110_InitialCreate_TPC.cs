using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFApp2.Migrations.TpcDb
{
    /// <inheritdoc />
    public partial class InitialCreate_TPC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ShipSequence");

            migrationBuilder.CreateTable(
                name: "Lab8_TPC_Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPC_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPC_Aircarriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ShipSequence]"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    AircraftCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPC_Aircarriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPC_Aircarriers_Lab8_TPC_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPC_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPC_Battleships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ShipSequence]"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    MainCaliberGuns = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPC_Battleships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPC_Battleships_Lab8_TPC_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPC_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPC_Cruisers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ShipSequence]"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    MissileCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPC_Cruisers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPC_Cruisers_Lab8_TPC_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPC_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPC_Destroyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ShipSequence]"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    TorpedoTubes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPC_Destroyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPC_Destroyers_Lab8_TPC_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPC_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPC_Submarines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ShipSequence]"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false),
                    MaxDepth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPC_Submarines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPC_Submarines_Lab8_TPC_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPC_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPC_Aircarriers_ManufacturerId",
                table: "Lab8_TPC_Aircarriers",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPC_Battleships_ManufacturerId",
                table: "Lab8_TPC_Battleships",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPC_Cruisers_ManufacturerId",
                table: "Lab8_TPC_Cruisers",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPC_Destroyers_ManufacturerId",
                table: "Lab8_TPC_Destroyers",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPC_Submarines_ManufacturerId",
                table: "Lab8_TPC_Submarines",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lab8_TPC_Aircarriers");

            migrationBuilder.DropTable(
                name: "Lab8_TPC_Battleships");

            migrationBuilder.DropTable(
                name: "Lab8_TPC_Cruisers");

            migrationBuilder.DropTable(
                name: "Lab8_TPC_Destroyers");

            migrationBuilder.DropTable(
                name: "Lab8_TPC_Submarines");

            migrationBuilder.DropTable(
                name: "Lab8_TPC_Manufacturers");

            migrationBuilder.DropSequence(
                name: "ShipSequence");
        }
    }
}
