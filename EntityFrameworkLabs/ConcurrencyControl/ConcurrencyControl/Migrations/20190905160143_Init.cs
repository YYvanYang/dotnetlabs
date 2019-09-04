﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConcurrencyControl.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConcurrentAccountsWithRowVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<double>(nullable: false),
                    Timestamp = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrentAccountsWithRowVersion", x => x.Id);
                });

            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetTimestampOnUpdate
                    AFTER UPDATE ON ConcurrentAccountsWithRowVersion
                    BEGIN
                        UPDATE ConcurrentAccountsWithRowVersion
                        SET Timestamp = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetTimestampOnInsert
                    AFTER INSERT ON ConcurrentAccountsWithRowVersion
                    BEGIN
                        UPDATE ConcurrentAccountsWithRowVersion
                        SET Timestamp = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");

            migrationBuilder.CreateTable(
                name: "ConcurrentAccountsWithToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrentAccountsWithToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonconcurrentAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonconcurrentAccounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConcurrentAccountsWithRowVersion");

            migrationBuilder.DropTable(
                name: "ConcurrentAccountsWithToken");

            migrationBuilder.DropTable(
                name: "NonconcurrentAccounts");
        }
    }
}
