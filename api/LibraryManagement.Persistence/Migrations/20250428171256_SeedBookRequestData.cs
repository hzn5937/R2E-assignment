using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookRequestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookBorrowingRequests",
                columns: new[] { "Id", "ApproverId", "DateRequested", "RequestorId", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 4, 18, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1953), 4, 1 },
                    { 2, null, new DateTime(2025, 4, 23, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1973), 5, 0 },
                    { 3, 2, new DateTime(2025, 4, 26, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1974), 6, 2 },
                    { 4, null, new DateTime(2025, 4, 27, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1975), 4, 0 },
                    { 5, 1, new DateTime(2025, 4, 13, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1976), 7, 1 },
                    { 6, 3, new DateTime(2025, 4, 20, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1978), 8, 1 },
                    { 7, null, new DateTime(2025, 4, 25, 17, 12, 56, 672, DateTimeKind.Utc).AddTicks(1979), 9, 0 }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI5o2tx8L1/ulBfbhRj5m513L+UUFdP9iWRBktEgulH6/+zS7fSEtoRgUPdC9YKT4Q==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECgzQZOqFuhfqVVnfpxrGQ7UJiZz+APhfF/Cr5Zwdr23amJGyveP/fenPccY9AhjBg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGTUpPWY01z1X0A1iaqTna3G1AxySrn5lFavPtrPbR/ickZW4E3LTqV0+19OF27MGg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJNGLb648zyGrJ50UxMr2PzKRWecYnaTqHKAmzecDy/7ByooUQxoYzWwmolbEuXj8w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEONQw8BQJhvCb1rp0+OxGi/fAhxR5rqdcDzinJ2gVtLIzWuES8f5X/Y7UjezX1jrXQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEA12zJqHrbFgMGvYbXCGZ+Yaqe+UAjjGDp8YeAg6EAN/6NipyvCQmZEogHzu2jf5FQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFUhmXKTmKBi7bUf7EJVHL5Sw9A2kgBVkRAYiEHA9Fk06EY6KaYvponAcYgVxxmRXw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAhrU1ED168SdXnJDtQ3XxdDOr5Tvrowq2MgT0tqtPzK1FSaqcYideDZ5PEJ+hOZjA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI2dDjzR+lfT+I8m147nJLnWsGvUqImb7tgleUL+rWEkwPxfewgIcPJvL2UM8Gvn3g==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENZJD/od80BbALSTO64v4b9evezowYvbsenuvkW7P5rseGWbkShMDCSE3zdCxPX3gQ==");
        }
    }
}
