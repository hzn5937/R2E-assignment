using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "BookBorrowingRequests",
                columns: new[] { "Id", "ApproverId", "DateRequested", "RequestorId", "Status" },
                values: new object[,]
                {
                    { 19, 1, new DateTime(2025, 4, 5, 10, 30, 0, 0, DateTimeKind.Utc), 4, "Approved" },
                    { 20, 2, new DateTime(2025, 4, 12, 14, 45, 0, 0, DateTimeKind.Utc), 4, "Approved" },
                    { 21, 3, new DateTime(2025, 4, 20, 9, 15, 0, 0, DateTimeKind.Utc), 4, "Approved" },
                    { 22, 1, new DateTime(2025, 4, 25, 16, 30, 0, 0, DateTimeKind.Utc), 4, "Rejected" },
                    { 23, 2, new DateTime(2025, 3, 15, 11, 20, 0, 0, DateTimeKind.Utc), 5, "Approved" },
                    { 24, 3, new DateTime(2025, 3, 22, 13, 40, 0, 0, DateTimeKind.Utc), 5, "Approved" },
                    { 25, 1, new DateTime(2025, 3, 28, 9, 50, 0, 0, DateTimeKind.Utc), 5, "Approved" },
                    { 26, 3, new DateTime(2025, 4, 4, 14, 30, 0, 0, DateTimeKind.Utc), 5, "Approved" },
                    { 27, 2, new DateTime(2025, 4, 18, 10, 15, 0, 0, DateTimeKind.Utc), 5, "Approved" },
                    { 28, null, new DateTime(2025, 4, 6, 9, 30, 0, 0, DateTimeKind.Utc), 7, "Waiting" },
                    { 29, 1, new DateTime(2025, 4, 14, 16, 45, 0, 0, DateTimeKind.Utc), 7, "Rejected" },
                    { 30, 2, new DateTime(2025, 4, 22, 11, 20, 0, 0, DateTimeKind.Utc), 7, "Approved" },
                    { 31, 1, new DateTime(2025, 2, 10, 10, 30, 0, 0, DateTimeKind.Utc), 9, "Approved" },
                    { 32, 2, new DateTime(2025, 3, 5, 14, 20, 0, 0, DateTimeKind.Utc), 9, "Approved" },
                    { 33, 3, new DateTime(2025, 3, 15, 9, 45, 0, 0, DateTimeKind.Utc), 9, "Approved" },
                    { 34, 1, new DateTime(2025, 4, 3, 11, 30, 0, 0, DateTimeKind.Utc), 9, "Approved" },
                    { 35, 2, new DateTime(2025, 4, 10, 15, 45, 0, 0, DateTimeKind.Utc), 9, "Approved" },
                    { 36, null, new DateTime(2025, 4, 20, 9, 15, 0, 0, DateTimeKind.Utc), 9, "Waiting" },
                    { 37, 1, new DateTime(2025, 5, 1, 10, 30, 0, 0, DateTimeKind.Utc), 11, "Approved" },
                    { 38, 2, new DateTime(2025, 5, 2, 14, 15, 0, 0, DateTimeKind.Utc), 11, "Approved" },
                    { 39, null, new DateTime(2025, 5, 3, 9, 45, 0, 0, DateTimeKind.Utc), 11, "Waiting" }
                });

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

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "RequestId" },
                values: new object[,]
                {
                    { 45, 11, 19 },
                    { 46, 15, 19 },
                    { 47, 21, 20 },
                    { 48, 39, 20 },
                    { 49, 48, 20 },
                    { 50, 37, 21 },
                    { 51, 21, 22 },
                    { 52, 5, 23 },
                    { 53, 32, 23 },
                    { 54, 3, 24 },
                    { 55, 27, 24 },
                    { 56, 29, 24 },
                    { 57, 15, 25 },
                    { 58, 18, 25 },
                    { 59, 31, 25 },
                    { 60, 35, 25 },
                    { 61, 6, 26 },
                    { 62, 9, 26 },
                    { 63, 12, 26 },
                    { 64, 25, 26 },
                    { 65, 43, 26 },
                    { 66, 14, 27 },
                    { 67, 20, 27 },
                    { 68, 1, 28 },
                    { 69, 13, 28 },
                    { 70, 16, 29 },
                    { 71, 4, 30 },
                    { 72, 22, 30 },
                    { 73, 45, 30 },
                    { 74, 2, 31 },
                    { 75, 8, 32 },
                    { 76, 10, 32 },
                    { 77, 17, 32 },
                    { 78, 24, 32 },
                    { 79, 33, 33 },
                    { 80, 36, 34 },
                    { 81, 38, 34 },
                    { 82, 40, 34 },
                    { 83, 46, 34 },
                    { 84, 49, 34 },
                    { 85, 44, 35 },
                    { 86, 7, 36 },
                    { 87, 2, 37 },
                    { 88, 16, 37 },
                    { 89, 28, 37 },
                    { 90, 47, 37 },
                    { 91, 23, 38 },
                    { 92, 6, 39 },
                    { 93, 19, 39 },
                    { 94, 26, 39 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateRequested",
                value: new DateTime(2025, 4, 23, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4794));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateRequested",
                value: new DateTime(2025, 4, 28, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4829));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateRequested",
                value: new DateTime(2025, 5, 1, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4831));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4833));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateRequested",
                value: new DateTime(2025, 4, 18, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4834));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateRequested",
                value: new DateTime(2025, 4, 25, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4837));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateRequested",
                value: new DateTime(2025, 4, 30, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4839));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateRequested",
                value: new DateTime(2025, 5, 3, 6, 32, 24, 253, DateTimeKind.Utc).AddTicks(4840));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateRequested",
                value: new DateTime(2025, 4, 29, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 23, 20, 24, 253, DateTimeKind.Utc).AddTicks(4861));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 11,
                column: "DateRequested",
                value: new DateTime(2025, 4, 27, 18, 32, 24, 253, DateTimeKind.Utc).AddTicks(4863));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 12,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 6, 32, 24, 253, DateTimeKind.Utc).AddTicks(4865));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 13,
                column: "DateRequested",
                value: new DateTime(2025, 5, 2, 13, 44, 24, 253, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 14,
                column: "DateRequested",
                value: new DateTime(2025, 4, 30, 6, 32, 24, 253, DateTimeKind.Utc).AddTicks(4869));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 15,
                column: "DateRequested",
                value: new DateTime(2025, 4, 30, 13, 44, 24, 253, DateTimeKind.Utc).AddTicks(4871));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 16,
                column: "DateRequested",
                value: new DateTime(2025, 4, 30, 23, 20, 24, 253, DateTimeKind.Utc).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 17,
                column: "DateRequested",
                value: new DateTime(2025, 5, 1, 13, 44, 24, 253, DateTimeKind.Utc).AddTicks(4875));

            migrationBuilder.UpdateData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 18,
                column: "DateRequested",
                value: new DateTime(2025, 5, 1, 23, 20, 24, 253, DateTimeKind.Utc).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPTT95ZoG4A1YLWx/ZNulKJnxjPRCHF0YTDIsrWxWmRP758ZKIXwdJeSlJmMUBdBNg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENU2b+si1vmTif26kOAaQRMGUII5FhFFsfXTj+4Q0EpyKfr3N5sMfyVdPtvXgdAaxw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGcCqumrEjHVfWf7fvN4k3Ay8wrKe+HGQeLKLTLcoQBhg8HpByBxmnfrfn1EI+y1/w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIUwCm938Qh/1WSiFC7ErKwWnDRIxHoLapxEsciuLwR69HmvmO0N0r+956rlB7aYiw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGN+8nQU5cTdsBc14AvnaPa0lcmShqWEibpFiu7DuLyidQZ9rUQIeZZKeeSFt6D8Nw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHcZ+9aopLOUH9JJYeBY0NFrtz6jG0fRhkKGCnUCPBlSJpxYWQuXzVlCaA8XeCBnAA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDTB1D5b/VBWdMCgvHBDKavUXWLUifGYGxKvctESoamJb3OgfGPoTRlC45CwgXniCw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGeTHbnBhb/wg208idw/LeLTieyIPsbd+XuVB64omUXrVfR5l4J2tHywl7hlr7X/Fg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEG8Yh9n4YfMWgPAGCslqe/3F02k/co4bSVfBfLxBWKZ5MiT05utpDgOft5L9MTaodA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMeEtFyNMhWR2xj12Mas+X+jJvQJZunTTEjT+EszzlgGuNokNmGe7TWbUeFH+7QGlQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAfZb2Nm5E9F78qj/i+H261pOrDd5n+GmNzX1MGSJOxaAmLWK1KKj6SWdHuOUFZa3Q==");
        }
    }
}
