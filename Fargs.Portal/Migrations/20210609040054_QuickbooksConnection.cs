using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fargs.Portal.Migrations
{
    public partial class QuickbooksConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToken_User_UserId",
                schema: "Identity",
                table: "UserToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserToken",
                schema: "Identity",
                table: "UserToken");

            migrationBuilder.EnsureSchema(
                name: "ServiceConnection");

            migrationBuilder.RenameTable(
                name: "UserToken",
                schema: "Identity",
                newName: "QuickbooksConnection",
                newSchema: "ServiceConnection");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuickbooksConnection",
                schema: "ServiceConnection",
                table: "QuickbooksConnection",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.CreateTable(
                name: "QuickbooksConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RealmId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickbooksConnections", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_QuickbooksConnection_User_UserId",
                schema: "ServiceConnection",
                table: "QuickbooksConnection",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuickbooksConnection_User_UserId",
                schema: "ServiceConnection",
                table: "QuickbooksConnection");

            migrationBuilder.DropTable(
                name: "QuickbooksConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuickbooksConnection",
                schema: "ServiceConnection",
                table: "QuickbooksConnection");

            migrationBuilder.RenameTable(
                name: "QuickbooksConnection",
                schema: "ServiceConnection",
                newName: "UserToken",
                newSchema: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserToken",
                schema: "Identity",
                table: "UserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserToken_User_UserId",
                schema: "Identity",
                table: "UserToken",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
