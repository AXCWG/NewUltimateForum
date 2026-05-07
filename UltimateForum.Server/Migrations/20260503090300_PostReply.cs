using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateForum.Server.Migrations
{
    /// <inheritdoc />
    public partial class PostReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RepliedUnderId",
                table: "Replies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_RepliedUnderId",
                table: "Replies",
                column: "RepliedUnderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Posts_RepliedUnderId",
                table: "Replies",
                column: "RepliedUnderId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Posts_RepliedUnderId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_RepliedUnderId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "RepliedUnderId",
                table: "Replies");
        }
    }
}
