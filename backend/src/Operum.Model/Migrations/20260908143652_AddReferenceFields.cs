using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operum.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferencedEntryId",
                table: "FieldValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferencedDisplayFieldId",
                table: "Fields",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferencedTrackerId",
                table: "Fields",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_FieldId_ReferencedEntryId",
                table: "FieldValues",
                columns: new[] { "FieldId", "ReferencedEntryId" });

            migrationBuilder.CreateIndex(
                name: "IX_FieldValues_ReferencedEntryId",
                table: "FieldValues",
                column: "ReferencedEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ReferencedDisplayFieldId",
                table: "Fields",
                column: "ReferencedDisplayFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ReferencedTrackerId",
                table: "Fields",
                column: "ReferencedTrackerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Fields_ReferencedDisplayFieldId",
                table: "Fields",
                column: "ReferencedDisplayFieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Trackers_ReferencedTrackerId",
                table: "Fields",
                column: "ReferencedTrackerId",
                principalTable: "Trackers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldValues_Entries_ReferencedEntryId",
                table: "FieldValues",
                column: "ReferencedEntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Fields_ReferencedDisplayFieldId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Trackers_ReferencedTrackerId",
                table: "Fields");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldValues_Entries_ReferencedEntryId",
                table: "FieldValues");

            migrationBuilder.DropIndex(
                name: "IX_FieldValues_FieldId_ReferencedEntryId",
                table: "FieldValues");

            migrationBuilder.DropIndex(
                name: "IX_FieldValues_ReferencedEntryId",
                table: "FieldValues");

            migrationBuilder.DropIndex(
                name: "IX_Fields_ReferencedDisplayFieldId",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_ReferencedTrackerId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ReferencedEntryId",
                table: "FieldValues");

            migrationBuilder.DropColumn(
                name: "ReferencedDisplayFieldId",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ReferencedTrackerId",
                table: "Fields");
        }
    }
}
