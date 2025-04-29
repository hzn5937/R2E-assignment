using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookRequestDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "RequestId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 7, 1 },
                    { 3, 12, 1 },
                    { 4, 2, 2 },
                    { 5, 13, 2 },
                    { 6, 22, 2 },
                    { 7, 33, 2 },
                    { 8, 47, 2 },
                    { 9, 6, 3 },
                    { 10, 18, 3 },
                    { 11, 4, 4 },
                    { 12, 14, 4 },
                    { 13, 26, 4 },
                    { 14, 40, 4 },
                    { 15, 50, 5 },
                    { 16, 17, 6 },
                    { 17, 23, 6 },
                    { 18, 3, 7 },
                    { 19, 10, 7 },
                    { 20, 43, 7 }
                });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateRequested",
                value: new DateTime(2025, 4, 18, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3762));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateRequested",
                value: new DateTime(2025, 4, 23, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3779));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateRequested",
                value: new DateTime(2025, 4, 26, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3781));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateRequested",
                value: new DateTime(2025, 4, 27, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateRequested",
                value: new DateTime(2025, 4, 13, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateRequested",
                value: new DateTime(2025, 4, 20, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateRequested",
                value: new DateTime(2025, 4, 25, 17, 13, 56, 364, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN9pYo6vrGM8gDZaA+BK98PgJomHmmutwho951ecHDJ9f7JPVbxsdapaWizjNGYOQA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDiiExYoRpUNrLQC9hfBXYf7GYaXXSiNw/zCjJKNW3t3QmkMJaWBrEl5UEqNLSUAcg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM2sQFvgNJwPurcN9rJAAIItcN8lJfgiJ1JEMJyxHXXKIM4KArJrmOY2vTLVX4wxMg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELc15B8yNZziQtpb180m37aU46K9QshAIwk1XywUFBkjjiTvw/ECcSz1/NGbjzkHTw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJoqgKPKmbewuvsAL3j9Yx3fVp2H5mdom7OWY7c/2wDx5pw+48TVoWWUIBr9FaLbzg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHGmcg9qkALlm/0vEXXqdSkfcCSfbwRQyI1sEX2uk2ezK1+BIqAhKms9BwdAkJ+JrA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA5d1mOrnvALWYChdsDqnUN2rwrfFcUadIcXjhKbl/tuPmW+T4EUiGFztyfYmUsxow==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEO1NN9TXygVhAIa8lOOthBE1ukf/qKZ54gNMjNQ1q3ZxPKtMvsQgvAyvYj9HS8ZEcg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEL+H8kJtVImUBvT22MleiKaGxK6SjqAF1L4zAu8hmHy0UyvwsZ487TJEPPLgKQsvEg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC1JfKEsR9bX3T5922Kqo7DAdHa4XvZKsrD5falnDkBDCMe0lJyKSXn2hCzTrUmeYA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateRequested",
                value: new DateTime(2025, 4, 18, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1953));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateRequested",
                value: new DateTime(2025, 4, 23, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1973));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateRequested",
                value: new DateTime(2025, 4, 26, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1974));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateRequested",
                value: new DateTime(2025, 4, 27, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1975));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateRequested",
                value: new DateTime(2025, 4, 13, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1976));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateRequested",
                value: new DateTime(2025, 4, 20, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1978));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateRequested",
                value: new DateTime(2025, 4, 25, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1979));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBsPA4GdZc7PcJsUXMuBTRWKri6DvBQkA4/YWShkJMLMj9h12I0Mqle2YX2nTnbWIw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHEvgSsxPUNJKcuffdxeqY6qDnmFLmZg+ZrtsXxy5VcDXL3nfAbra77vrnpCZJuSGQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEF+5JrAxB9VJlxlDWClsQ9Spyc38qivSRFsxUuOoa4rYhh6+JF7PMEN/wGGE+Y0XjA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEORQCfGHVY+oGHJ4CGiv8/ssJziBFzfURGTaMcQOY/wQQFwdes8oiGDcDhcFs4PT7w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOXSFHPiBWDJ2uiKFKKAJrBmExwaLnV3+xOalJIK97KUxJPrEEpEMKed/ddzigUM6g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI3vOm821JiUX3WDhUCVIisb7JKEcEindsdxW3v1+EcX4mGjiAooUeIIL4f/yvks4g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENyHJA6oXTP2wWfYw0sRtb1B68uBJnTzy750eppi8GLRNY7gkFQnIYIVy9xD/o5mFw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHckA6cUz+dNZA3je3VMbV50lvGsXDoxAJHSCU9+N/gdbojXH5ib4XJeNGITsB9guw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK3ELCjv4gRUjFolXxFXuf1M8VbaKbFViJ2cZ3fs0PART2qLt/Y3ty2ruZkmHi+oIA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHIq/+nzyf0WiASEQhxfPdrvSUEzg7BAlQNi4ILas2ZFExQrdGiAN8d/ROrFSb6Lag==");
        }
    }
}
