using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Science-Fiction" },
                    { 2, "Fantasy" },
                    { 3, "Mystery & Thriller" },
                    { 4, "Romance" },
                    { 5, "Non-Fiction" },
                    { 6, "History" },
                    { 7, "Biography" },
                    { 8, "Children" },
                    { 9, "Self-Help" },
                    { 10, "Technology" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "11111111-1111-1111-1111-111111111111", "user1@demo.local", true, false, null, "USER1@DEMO.LOCAL", "USER1", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "11111111-1111-1111-1111-111111111111", false, "user1" },
                    { 2, 0, "22222222-2222-2222-2222-222222222222", "user2@demo.local", true, false, null, "USER2@DEMO.LOCAL", "USER2", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "22222222-2222-2222-2222-222222222222", false, "user2" },
                    { 3, 0, "33333333-3333-3333-3333-333333333333", "user3@demo.local", true, false, null, "USER3@DEMO.LOCAL", "USER3", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "33333333-3333-3333-3333-333333333333", false, "user3" },
                    { 4, 0, "44444444-4444-4444-4444-444444444444", "user4@demo.local", true, false, null, "USER4@DEMO.LOCAL", "USER4", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "44444444-4444-4444-4444-444444444444", false, "user4" },
                    { 5, 0, "55555555-5555-5555-5555-555555555555", "user5@demo.local", true, false, null, "USER5@DEMO.LOCAL", "USER5", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "55555555-5555-5555-5555-555555555555", false, "user5" },
                    { 6, 0, "66666666-6666-6666-6666-666666666666", "user6@demo.local", true, false, null, "USER6@DEMO.LOCAL", "USER6", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "66666666-6666-6666-6666-666666666666", false, "user6" },
                    { 7, 0, "77777777-7777-7777-7777-777777777777", "user7@demo.local", true, false, null, "USER7@DEMO.LOCAL", "USER7", "AQAAAAEAACcQAAAAELp13dXz+6l4t+J9JsOlxkh7m50kaxQoyJMylSdtE/XUPSm58KRjsfUwaUxMVoajJw==", null, false, 0, "77777777-7777-7777-7777-777777777777", false, "user7" },
                    { 8, 0, "88888888-8888-8888-8888-888888888888", "admin1@demo.local", true, false, null, "ADMIN1@DEMO.LOCAL", "ADMIN1", "AQAAAAEAACcQAAAAE9L5uFq/gLSuE9LMOz38Xp2R7jNmOw2JBFcn3dR8n40uGldwgW7a1Wv8xw64RkQbUA==", null, false, 1, "88888888-8888-8888-8888-888888888888", false, "admin1" },
                    { 9, 0, "99999999-9999-9999-9999-999999999999", "admin2@demo.local", true, false, null, "ADMIN2@DEMO.LOCAL", "ADMIN2", "AQAAAAEAACcQAAAAE9L5uFq/gLSuE9LMOz38Xp2R7jNmOw2JBFcn3dR8n40uGldwgW7a1Wv8xw64RkQbUA==", null, false, 1, "99999999-9999-9999-9999-999999999999", false, "admin2" },
                    { 10, 0, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", "admin3@demo.local", true, false, null, "ADMIN3@DEMO.LOCAL", "ADMIN3", "AQAAAAEAACcQAAAAE9L5uFq/gLSuE9LMOz38Xp2R7jNmOw2JBFcn3dR8n40uGldwgW7a1Wv8xw64RkQbUA==", null, false, 1, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", false, "admin3" }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequests",
                columns: new[] { "Id", "ApproverId", "DateRequested", "RequestorId", "Status" },
                values: new object[,]
                {
                    { 1, 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0 },
                    { 2, 9, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 3, 10, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2 },
                    { 4, 8, new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 0 },
                    { 5, 9, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1 },
                    { 6, 10, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 2 },
                    { 7, 8, new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 0 },
                    { 8, 9, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 9, 10, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 },
                    { 10, 8, new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 0 },
                    { 11, 9, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1 },
                    { 12, 10, new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2 },
                    { 13, 8, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 0 },
                    { 14, 9, new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1 },
                    { 15, 10, new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 16, 8, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 0 },
                    { 17, 9, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1 },
                    { 18, 10, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2 },
                    { 19, 8, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 0 },
                    { 20, 9, new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AvailableQuantity", "CategoryId", "Title", "TotalQuantity" },
                values: new object[,]
                {
                    { 1, 20, 1, "Galactic Odyssey", 20 },
                    { 2, 20, 1, "Mars Frontier", 20 },
                    { 3, 20, 1, "Quantum Rift", 20 },
                    { 4, 20, 1, "The Last Starship", 20 },
                    { 5, 20, 1, "Echoes of Andromeda", 20 },
                    { 6, 20, 2, "Dragon's Oath", 20 },
                    { 7, 20, 2, "Sword of Silverwood", 20 },
                    { 8, 20, 2, "Chronicles of Eldoria", 20 },
                    { 9, 20, 2, "The Forgotten Kingdom", 20 },
                    { 10, 20, 2, "Mageborn Legacy", 20 },
                    { 11, 20, 3, "Midnight in Maple Town", 20 },
                    { 12, 20, 3, "The Vanishing Key", 20 },
                    { 13, 20, 3, "Whispers in the Attic", 20 },
                    { 14, 20, 3, "Shadow at Rivermere", 20 },
                    { 15, 20, 3, "Cold Case Harbor", 20 },
                    { 16, 20, 4, "Letters to Vienna", 20 },
                    { 17, 20, 4, "Summer at Willow Lake", 20 },
                    { 18, 20, 4, "Falling for Florence", 20 },
                    { 19, 20, 4, "Tides of the Heart", 20 },
                    { 20, 20, 4, "Paris in Bloom", 20 },
                    { 21, 20, 5, "Mindset Matters", 20 },
                    { 22, 20, 5, "The Power of Habits", 20 },
                    { 23, 20, 5, "Climate Change 101", 20 },
                    { 24, 20, 5, "Digital Minimalism", 20 },
                    { 25, 20, 5, "Nutrition Decoded", 20 },
                    { 26, 20, 6, "Empire of Sands", 20 },
                    { 27, 20, 6, "The Silk Road Story", 20 },
                    { 28, 20, 6, "Revolution 1776", 20 },
                    { 29, 20, 6, "World War Chronicles", 20 },
                    { 30, 20, 6, "Ancient Egypt Revealed", 20 },
                    { 31, 20, 7, "The Steve Jobs Way", 20 },
                    { 32, 20, 7, "Long Walk to Freedom", 20 },
                    { 33, 20, 7, "Becoming Elon", 20 },
                    { 34, 20, 7, "The Ruth Bader Story", 20 },
                    { 35, 20, 7, "Mozart: A Life", 20 },
                    { 36, 20, 8, "The Lost Unicorn", 20 },
                    { 37, 20, 8, "Adventures of Tiny Tim", 20 },
                    { 38, 20, 8, "Space Pups", 20 },
                    { 39, 20, 8, "The Rainbow Treehouse", 20 },
                    { 40, 20, 8, "Pirates of Jellybean Bay", 20 },
                    { 41, 20, 9, "Atomic Focus", 20 },
                    { 42, 20, 9, "Unbreakable Confidence", 20 },
                    { 43, 20, 9, "30-Day Productivity", 20 },
                    { 44, 20, 9, "The Joy of Saying No", 20 },
                    { 45, 20, 9, "Stress-Free Living", 20 },
                    { 46, 20, 10, "AI for Everyone", 20 },
                    { 47, 20, 10, "Building with .NET 8", 20 },
                    { 48, 20, 10, "Cyber-Security Essentials", 20 },
                    { 49, 20, 10, "Data Science Crash Course", 20 },
                    { 50, 20, 10, "Intro to Quantum Computing", 20 }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "RequestId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 4, 1 },
                    { 5, 6, 2 },
                    { 6, 7, 2 },
                    { 7, 8, 2 },
                    { 8, 9, 2 },
                    { 9, 11, 3 },
                    { 10, 12, 3 },
                    { 11, 13, 3 },
                    { 12, 14, 3 },
                    { 13, 16, 4 },
                    { 14, 17, 4 },
                    { 15, 18, 4 },
                    { 16, 19, 4 },
                    { 17, 21, 5 },
                    { 18, 22, 5 },
                    { 19, 23, 5 },
                    { 20, 24, 5 },
                    { 21, 26, 6 },
                    { 22, 27, 6 },
                    { 23, 28, 6 },
                    { 24, 29, 6 },
                    { 25, 31, 7 },
                    { 26, 32, 7 },
                    { 27, 33, 7 },
                    { 28, 34, 7 },
                    { 29, 36, 8 },
                    { 30, 37, 8 },
                    { 31, 38, 8 },
                    { 32, 39, 8 },
                    { 33, 41, 9 },
                    { 34, 42, 9 },
                    { 35, 43, 9 },
                    { 36, 44, 9 },
                    { 37, 46, 10 },
                    { 38, 47, 10 },
                    { 39, 48, 10 },
                    { 40, 49, 10 },
                    { 41, 1, 11 },
                    { 42, 2, 11 },
                    { 43, 3, 11 },
                    { 44, 6, 12 },
                    { 45, 7, 12 },
                    { 46, 8, 12 },
                    { 47, 11, 13 },
                    { 48, 12, 13 },
                    { 49, 13, 13 },
                    { 50, 16, 14 },
                    { 51, 17, 14 },
                    { 52, 18, 14 },
                    { 53, 21, 15 },
                    { 54, 22, 15 },
                    { 55, 23, 15 },
                    { 56, 26, 16 },
                    { 57, 27, 16 },
                    { 58, 28, 16 },
                    { 59, 31, 17 },
                    { 60, 32, 17 },
                    { 61, 33, 17 },
                    { 62, 36, 18 },
                    { 63, 37, 18 },
                    { 64, 38, 18 },
                    { 65, 41, 19 },
                    { 66, 42, 19 },
                    { 67, 43, 19 },
                    { 68, 46, 20 },
                    { 69, 47, 20 },
                    { 70, 48, 20 }
                });
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

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequestDetails",
                keyColumn: "Id",
                keyValue: 44);

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
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 50);

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

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "BookBorrowingRequests",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

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
                keyValue: 4);

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
        }
    }
}
