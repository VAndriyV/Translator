using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataEFCore.Migrations
{
    public partial class Entitiesrename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutomaticRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alpha = table.Column<int>(nullable: false),
                    Lexeme = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Beta = table.Column<int>(nullable: true),
                    Stack = table.Column<int>(nullable: true),
                    Information = table.Column<string>(unicode: false, maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomaticRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LexemeClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Values = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LexemeClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lexemes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lexemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputConstants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputConstants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputIDNs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputIDNs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputLexemes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Row = table.Column<int>(nullable: false),
                    LexemeName = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    LexemeId = table.Column<int>(nullable: false),
                    IDNCode = table.Column<int>(nullable: true),
                    ConstantCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputLexemes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomaticRules");

            migrationBuilder.DropTable(
                name: "LexemeClasses");

            migrationBuilder.DropTable(
                name: "Lexemes");

            migrationBuilder.DropTable(
                name: "OutputConstants");

            migrationBuilder.DropTable(
                name: "OutputIDNs");

            migrationBuilder.DropTable(
                name: "OutputLexemes");
        }
    }
}
