using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJxDaCOle1qgSIaimQzdunI9W3Ztug9zDuikLI75hgpSI=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJxDaCOle1qgSIaimQzdunI9W3Ztug9zDuikLI75hgpSI=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJxDaCOle1qgSIaimQzdunI9W3Ztug9zDuikLI75hgpSI=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAE9L5uFq/gLSuE9LMOz38Xp2R7jNmOw2JBFcn3dR8n40uGldwgW7a1Wv8xw64RkQbUA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAE9L5uFq/gLSuE9LMOz38Xp2R7jNmOw2JBFcn3dR8n40uGldwgW7a1Wv8xw64RkQbUA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAE9L5uFq/gLSuE9LMOz38Xp2R7jNmOw2JBFcn3dR8n40uGldwgW7a1Wv8xw64RkQbUA==");
        }
    }
}
