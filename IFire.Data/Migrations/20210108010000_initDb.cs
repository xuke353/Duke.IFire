using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IFire.Data.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsLock = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account_Auth_Info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    RefreshTokenExpiredTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LoginTime = table.Column<long>(type: "bigint", nullable: false),
                    LoginIP = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Auth_Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account_Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audit_Info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Controller = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true),
                    Action = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true),
                    Parameters = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Result = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ExecutionTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExecutionDuration = table.Column<int>(type: "int", nullable: false),
                    ClientIp = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true),
                    BrowserInfo = table.Column<string>(type: "varchar(1000) CHARACTER SET utf8mb4", maxLength: 1000, nullable: true),
                    Exception = table.Column<string>(type: "varchar(500) CHARACTER SET utf8mb4", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit_Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Login_Log",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    LoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IP = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Success = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Error = table.Column<string>(type: "varchar(2000) CHARACTER SET utf8mb4", maxLength: 2000, nullable: true),
                    UserAgent = table.Column<string>(type: "varchar(1000) CHARACTER SET utf8mb4", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Route = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: true),
                    Url = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: true),
                    Icon = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Show = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role_Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role_Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionCode = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Permission", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "Deleted", "IsLock", "Name", "Password", "Status", "Type", "Username" },
                values: new object[] { 1, false, false, "管理员", "F0DD923B1DB060E4DFB4AA10CB855FBA", 1, 1, "admin" });

            migrationBuilder.InsertData(
                table: "Account_Role",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Role_Permission",
                columns: new[] { "Id", "PermissionCode", "RoleId" },
                values: new object[,]
                {
                    { 1, "WeatherForecast_Get_Get", 1 },
                    { 2, "Permission_Tree_Get", 1 },
                    { 3, "Auth_AuthInfo_Get", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Account_Auth_Info");

            migrationBuilder.DropTable(
                name: "Account_Role");

            migrationBuilder.DropTable(
                name: "Audit_Info");

            migrationBuilder.DropTable(
                name: "Login_Log");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Role_Menu");

            migrationBuilder.DropTable(
                name: "Role_Permission");
        }
    }
}
