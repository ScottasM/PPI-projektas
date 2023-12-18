using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPI_projektas.Migrations
{
    /// <inheritdoc />
    public partial class migratePrivileges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupUser1",
                columns: table => new
                {
                    AdministratorOfGroupsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AdministratorsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser1", x => new { x.AdministratorOfGroupsId, x.AdministratorsId });
                    table.ForeignKey(
                        name: "FK_GroupUser1_Groups_AdministratorOfGroupsId",
                        column: x => x.AdministratorOfGroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser1_Users_AdministratorsId",
                        column: x => x.AdministratorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NoteUser1",
                columns: table => new
                {
                    EditorOfNotesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EditorsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteUser1", x => new { x.EditorOfNotesId, x.EditorsId });
                    table.ForeignKey(
                        name: "FK_NoteUser1_Notes_EditorOfNotesId",
                        column: x => x.EditorOfNotesId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteUser1_Users_EditorsId",
                        column: x => x.EditorsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser1_AdministratorsId",
                table: "GroupUser1",
                column: "AdministratorsId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteUser1_EditorsId",
                table: "NoteUser1",
                column: "EditorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupUser1");

            migrationBuilder.DropTable(
                name: "NoteUser1");
        }
    }
}
