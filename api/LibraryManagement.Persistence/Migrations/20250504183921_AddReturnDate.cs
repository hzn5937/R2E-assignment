using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateReturned",
                table: "BookBorrowingRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 4, 24, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1935), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 4, 29, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1958), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 2, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1959), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 3, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1960), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 4, 19, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1962), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 4, 26, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1963), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 1, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1964), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 4, 6, 39, 21, 621, DateTimeKind.Utc).AddTicks(1965), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 4, 30, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1973), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 3, 23, 27, 21, 621, DateTimeKind.Utc).AddTicks(1974), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 4, 28, 18, 39, 21, 621, DateTimeKind.Utc).AddTicks(1975), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 3, 6, 39, 21, 621, DateTimeKind.Utc).AddTicks(1976), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 3, 13, 51, 21, 621, DateTimeKind.Utc).AddTicks(1979), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 1, 6, 39, 21, 621, DateTimeKind.Utc).AddTicks(1979), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 1, 13, 51, 21, 621, DateTimeKind.Utc).AddTicks(1980), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 1, 23, 27, 21, 621, DateTimeKind.Utc).AddTicks(1982), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 2, 13, 51, 21, 621, DateTimeKind.Utc).AddTicks(1982), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DateRequested", "DateReturned" },
                values: new object[] { new DateTime(2025, 5, 2, 23, 27, 21, 621, DateTimeKind.Utc).AddTicks(1983), null });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 4, 15, 14, 30, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 4, 25, 16, 0, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 21,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 22,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 3, 30, 9, 45, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 4, 5, 10, 20, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 25,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 4, 20, 11, 30, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 27,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 28,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 29,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 30,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 2, 28, 15, 15, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 3, 25, 10, 45, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "DateReturned", "Status" },
                values: new object[] { new DateTime(2025, 4, 1, 14, 0, 0, 0, DateTimeKind.Utc), "Returned" });

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 34,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 35,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 36,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 37,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 38,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 39,
                column: "DateReturned",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEv7U7O2Q3Z/2Nm8wZfw6aJq756m+d8pFLBqXzJOATjuFXnzhS8OT4TlDSz70NzVqg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPm125SSHwDDdw4poNqW/QBBfFJ6C9lXDGwZiuLfmulrgrGM7NSr1u2zCybfTB4ixg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBGR3Th99+by66Gq7cthXnZqFEx6wMrij3wH/HXmRBJW6+O2/RjtmCL48zqEuCi0LA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECfmdzh0HVoZdeQGweDriV/ouMKR2LBkN2Xj4ji/cN0GKZ4KFrtucXxJ0bfOnX7sOQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEE20Q+xUmBgasj5zXu0T0ThIDmfJ+m8pJI+xKeNfN4xj8TFAAFjFOjFeEbrrNmESgw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPGXR625wtxuPnOGD2IMKnhVGSIfm2Mnnx8tVqoG/TschkziDCBMnxu+jsqaWcZ1hQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEjL0xJKfB4u1DY4A0DoHxn7oAU4RmyHUvg6SIdvQrtA/uae5yGRnOo2w7bcOzD2Ng==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBMZbMrhNZWNRU/E2+CRxQjoiD/8sliQzJ+Lv7p0D+nNuzwzhnKDD5BJo6U1U2Rq9w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC4ZNAhL3COSN60Q/TRhBOLHFuaOVc2LQpGF7sNsQCBsyMQbCPYCjC1VXVXFZuLVdQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIL5OxstRzuiZ06Ai9fyOQmCHkIARnBTQCfyfY62eTgZdx3JOopWYJiBabLP4hgzyw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG62CAc04OJgzoQ64z1pHVTPA2b7/0mX6KZ+m+66aUuLO9XH5FJvJynu8ZcSJJdL/Q==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateReturned",
                table: "BookBorrowingRequests");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateRequested",
                value: new DateTime(2025, 4, 24, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9161));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateRequested",
                value: new DateTime(2025, 4, 29, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9184));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9186));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateRequested",
                value: new DateTime(2025, 5, 3, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9187));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateRequested",
                value: new DateTime(2025, 4, 19, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9188));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateRequested",
                value: new DateTime(2025, 4, 26, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9193));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateRequested",
                value: new DateTime(2025, 5, 1, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9194));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateRequested",
                value: new DateTime(2025, 5, 3, 20, 49, 10, 78, DateTimeKind.Utc).AddTicks(9195));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateRequested",
                value: new DateTime(2025, 4, 30, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9203));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateRequested",
                value: new DateTime(2025, 5, 3, 13, 37, 10, 78, DateTimeKind.Utc).AddTicks(9205));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 11,
                column: "DateRequested",
                value: new DateTime(2025, 4, 28, 8, 49, 10, 78, DateTimeKind.Utc).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 12,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 20, 49, 10, 78, DateTimeKind.Utc).AddTicks(9207));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 13,
                column: "DateRequested",
                value: new DateTime(2025, 5, 3, 4, 1, 10, 78, DateTimeKind.Utc).AddTicks(9210));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 14,
                column: "DateRequested",
                value: new DateTime(2025, 4, 30, 20, 49, 10, 78, DateTimeKind.Utc).AddTicks(9210));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 15,
                column: "DateRequested",
                value: new DateTime(2025, 5, 1, 4, 1, 10, 78, DateTimeKind.Utc).AddTicks(9211));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 16,
                column: "DateRequested",
                value: new DateTime(2025, 5, 1, 13, 37, 10, 78, DateTimeKind.Utc).AddTicks(9214));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 17,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 4, 1, 10, 78, DateTimeKind.Utc).AddTicks(9214));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 18,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 13, 37, 10, 78, DateTimeKind.Utc).AddTicks(9216));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 19,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 20,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 23,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 24,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 26,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 31,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 32,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 33,
                column: "Status",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELwsQaUF+ysJUXmF07IFu+U1d//YMokJmO00P+MJpIPiC/J+d0ekuoamewoz0HsKOg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELseI/ehDZ9OvhFs87sRDDah1RDMyPPv5iJOxxtD81ISC1DGtsG5bkIoSz2GxWfkvg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHzLyBGvkVRsMlc6k8Zw7csywxC54ZjxfrG/uqSzpSie6Y/U46x4Xc5ELg90pMr/mA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKGc8UPb/9EjwVN5uyShJqzWmg0UfU5LmIN7X98vzHaDbdnAnB4/uQeR5cMEOrq+NQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJSnyVThepovWECRpHLcblopv2fHumv09KvF8RizgGuPaJmhqc5XagVJC/QbRTZ0XQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDfrrDIlykilXMOHQIylJD6gLqRA+4Mz8X4pv51ay4LLbEKGMGEbSzfgnws+iL3FDw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJQVIUhmLkean/8ybOq9iw+HP7//OeGLFwKAXmlljXPGyz2QIlEyuFrCroy+WwnONg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPnM3TTrw0NWAXwRbmKNWecYao3waWIaO8JUpmpXRI1UdVoIFxdcL4m//z/y3joYwQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIQqP5VYdyccJH4tQvnmbzClPyqNf43SW112ohFIwnF6N3pgcfz/4QCEO5uZsiAshQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOGQy6XTKyS2xhZx1l27rPYMOUCohRfuFqQBgceF/5cYpwybymGBPTIEIuantI5i0w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBjWnS+ACwarJFN0U+9sn5g7u1WT+qkzW60g4dOnI7MYgDmJVz2aPYLXuHABC9Nyww==");
        }
    }
}
