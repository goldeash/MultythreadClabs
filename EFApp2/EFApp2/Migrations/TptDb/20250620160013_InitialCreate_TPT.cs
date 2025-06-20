using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFApp2.Migrations.TptDb
{
    /// <inheritdoc />
    public partial class InitialCreate_TPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Ships_Base",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Ships_Base", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPT_Ships_Base_Lab8_TPT_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Lab8_TPT_Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Aircarriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AircraftCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Aircarriers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPT_Aircarriers_Lab8_TPT_Ships_Base_Id",
                        column: x => x.Id,
                        principalTable: "Lab8_TPT_Ships_Base",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Battleships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MainCaliberGuns = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Battleships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPT_Battleships_Lab8_TPT_Ships_Base_Id",
                        column: x => x.Id,
                        principalTable: "Lab8_TPT_Ships_Base",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Cruisers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MissileCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Cruisers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPT_Cruisers_Lab8_TPT_Ships_Base_Id",
                        column: x => x.Id,
                        principalTable: "Lab8_TPT_Ships_Base",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Destroyers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TorpedoTubes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Destroyers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPT_Destroyers_Lab8_TPT_Ships_Base_Id",
                        column: x => x.Id,
                        principalTable: "Lab8_TPT_Ships_Base",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lab8_TPT_Submarines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaxDepth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lab8_TPT_Submarines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lab8_TPT_Submarines_Lab8_TPT_Ships_Base_Id",
                        column: x => x.Id,
                        principalTable: "Lab8_TPT_Ships_Base",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lab8_TPT_Ships_Base_ManufacturerId",
                table: "Lab8_TPT_Ships_Base",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lab8_TPT_Aircarriers");

            migrationBuilder.DropTable(
                name: "Lab8_TPT_Battleships");

            migrationBuilder.DropTable(
                name: "Lab8_TPT_Cruisers");

            migrationBuilder.DropTable(
                name: "Lab8_TPT_Destroyers");

            migrationBuilder.DropTable(
                name: "Lab8_TPT_Submarines");

            migrationBuilder.DropTable(
                name: "Lab8_TPT_Ships_Base");

            migrationBuilder.DropTable(
                name: "Lab8_TPT_Manufacturers");
        }
    }
}
