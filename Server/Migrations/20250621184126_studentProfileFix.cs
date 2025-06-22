﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoysIQPlatform.Server.Migrations
{
    /// <inheritdoc />
    public partial class studentProfileFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "birthday",
                table: "students",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthday",
                table: "students");
        }
    }
}
