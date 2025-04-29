using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "admin1@example.com", "AQAAAAIAAYagAAAAEI5o2tx8L1/ulBfbhRj5m513L+UUFdP9iWRBktEgulH6/+zS7fSEtoRgUPdC9YKT4Q==", null, null, "Admin", "admin1" },
                    { 2, "admin2@example.com", "AQAAAAIAAYagAAAAECgzQZOqFuhfqVVnfpxrGQ7UJiZz+APhfF/Cr5Zwdr23amJGyveP/fenPccY9AhjBg==", null, null, "Admin", "admin2" },
                    { 3, "admin3@example.com", "AQAAAAIAAYagAAAAEGTUpPWY01z1X0A1iaqTna3G1AxySrn5lFavPtrPbR/ickZW4E3LTqV0+19OF27MGg==", null, null, "Admin", "admin3" },
                    { 5, "user1@example.com", "AQAAAAIAAYagAAAAEJNGLb648zyGrJ50UxMr2PzKRWecYnaTqHKAmzecDy/7ByooUQxoYzWwmolbEuXj8w==", null, null, "User", "user2" },
                    { 6, "user2@example.com", "AQAAAAIAAYagAAAAEONQw8BQJhvCb1rp0+OxGi/fAhxR5rqdcDzinJ2gVtLIzWuES8f5X/Y7UjezX1jrXQ==", null, null, "User", "user3" },
                    { 7, "user3@example.com", "AQAAAAIAAYagAAAAEA12zJqHrbFgMGvYbXCGZ+Yaqe+UAjjGDp8YeAg6EAN/6NipyvCQmZEogHzu2jf5FQ==", null, null, "User", "user4" },
                    { 8, "user4@example.com", "AQAAAAIAAYagAAAAEFUhmXKTmKBi7bUf7EJVHL5Sw9A2kgBVkRAYiEHA9Fk06EY6KaYvponAcYgVxxmRXw==", null, null, "User", "user5" },
                    { 9, "user5@example.com", "AQAAAAIAAYagAAAAEAhrU1ED168SdXnJDtQ3XxdDOr5Tvrowq2MgT0tqtPzK1FSaqcYideDZ5PEJ+hOZjA==", null, null, "User", "user6" },
                    { 10, "user6@example.com", "AQAAAAIAAYagAAAAEI2dDjzR+lfT+I8m147nJLnWsGvUqImb7tgleUL+rWEkwPxfewgIcPJvL2UM8Gvn3g==", null, null, "User", "user7" },
                    { 11, "user7@example.com", "AQAAAAIAAYagAAAAENZJD/od80BbALSTO64v4b9evezowYvbsenuvkW7P5rseGWbkShMDCSE3zdCxPX3gQ==", null, null, "User", "user8" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
