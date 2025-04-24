using System;
using System.Collections.Generic;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Data
{
    public static class ModelBuilderSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            /*───────────────────────────────────────
              1.  C A T E G O R I E S
            ───────────────────────────────────────*/
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Science-Fiction" },
                new Category { Id = 2, Name = "Fantasy" },
                new Category { Id = 3, Name = "Mystery & Thriller" },
                new Category { Id = 4, Name = "Romance" },
                new Category { Id = 5, Name = "Non-Fiction" },
                new Category { Id = 6, Name = "History" },
                new Category { Id = 7, Name = "Biography" },
                new Category { Id = 8, Name = "Children" },
                new Category { Id = 9, Name = "Self-Help" },
                new Category { Id = 10, Name = "Technology" }
            );

            /*───────────────────────────────────────
              2.  B O O K S   (50)
            ───────────────────────────────────────*/
            var books = new List<Book>
            {
                // Sci-Fi
                new Book { Id =  1, Title = "Galactic Odyssey",         TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 1 },
                new Book { Id =  2, Title = "Mars Frontier",            TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 1 },
                new Book { Id =  3, Title = "Quantum Rift",             TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 1 },
                new Book { Id =  4, Title = "The Last Starship",        TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 1 },
                new Book { Id =  5, Title = "Echoes of Andromeda",      TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 1 },

                // Fantasy
                new Book { Id =  6, Title = "Dragon's Oath",            TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 2 },
                new Book { Id =  7, Title = "Sword of Silverwood",      TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 2 },
                new Book { Id =  8, Title = "Chronicles of Eldoria",    TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 2 },
                new Book { Id =  9, Title = "The Forgotten Kingdom",    TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 2 },
                new Book { Id = 10, Title = "Mageborn Legacy",          TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 2 },

                // Mystery
                new Book { Id = 11, Title = "Midnight in Maple Town",   TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 3 },
                new Book { Id = 12, Title = "The Vanishing Key",        TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 3 },
                new Book { Id = 13, Title = "Whispers in the Attic",    TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 3 },
                new Book { Id = 14, Title = "Shadow at Rivermere",      TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 3 },
                new Book { Id = 15, Title = "Cold Case Harbor",         TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 3 },

                // Romance
                new Book { Id = 16, Title = "Letters to Vienna",        TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 4 },
                new Book { Id = 17, Title = "Summer at Willow Lake",    TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 4 },
                new Book { Id = 18, Title = "Falling for Florence",     TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 4 },
                new Book { Id = 19, Title = "Tides of the Heart",       TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 4 },
                new Book { Id = 20, Title = "Paris in Bloom",           TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 4 },

                // Non-Fiction
                new Book { Id = 21, Title = "Mindset Matters",          TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 5 },
                new Book { Id = 22, Title = "The Power of Habits",      TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 5 },
                new Book { Id = 23, Title = "Climate Change 101",       TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 5 },
                new Book { Id = 24, Title = "Digital Minimalism",       TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 5 },
                new Book { Id = 25, Title = "Nutrition Decoded",        TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 5 },

                // History
                new Book { Id = 26, Title = "Empire of Sands",          TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 6 },
                new Book { Id = 27, Title = "The Silk Road Story",      TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 6 },
                new Book { Id = 28, Title = "Revolution 1776",          TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 6 },
                new Book { Id = 29, Title = "World War Chronicles",     TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 6 },
                new Book { Id = 30, Title = "Ancient Egypt Revealed",   TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 6 },

                // Biography
                new Book { Id = 31, Title = "The Steve Jobs Way",       TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 7 },
                new Book { Id = 32, Title = "Long Walk to Freedom",     TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 7 },
                new Book { Id = 33, Title = "Becoming Elon",            TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 7 },
                new Book { Id = 34, Title = "The Ruth Bader Story",     TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 7 },
                new Book { Id = 35, Title = "Mozart: A Life",           TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 7 },

                // Children
                new Book { Id = 36, Title = "The Lost Unicorn",         TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 8 },
                new Book { Id = 37, Title = "Adventures of Tiny Tim",   TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 8 },
                new Book { Id = 38, Title = "Space Pups",               TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 8 },
                new Book { Id = 39, Title = "The Rainbow Treehouse",    TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 8 },
                new Book { Id = 40, Title = "Pirates of Jellybean Bay", TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 8 },

                // Self-Help
                new Book { Id = 41, Title = "Atomic Focus",             TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 9 },
                new Book { Id = 42, Title = "Unbreakable Confidence",  TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 9 },
                new Book { Id = 43, Title = "30-Day Productivity",      TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 9 },
                new Book { Id = 44, Title = "The Joy of Saying No",     TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 9 },
                new Book { Id = 45, Title = "Stress-Free Living",       TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 9 },

                // Technology
                new Book { Id = 46, Title = "AI for Everyone",          TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 10 },
                new Book { Id = 47, Title = "Building with .NET 8",     TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 10 },
                new Book { Id = 48, Title = "Cyber-Security Essentials",TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 10 },
                new Book { Id = 49, Title = "Data Science Crash Course",TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 10 },
                new Book { Id = 50, Title = "Intro to Quantum Computing",TotalQuantity = 20, AvailableQuantity = 20, CategoryId = 10 },
            };
            modelBuilder.Entity<Book>().HasData(books);

            /*───────────────────────────────────────
              3.  U S E R S   (10)
            ───────────────────────────────────────*/
            // Hash của "user123"
            const string userHash = "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJ50uwTC9wQFvOvRAv6AXaph3HPE6S3jSnUFTevNgE1f8=";
            // Hash của "admin123"
            const string adminHash = "AQEAAAAQJwAAEAAAACAAAAAX0H6XuxvVHqyaYJJ1yNDJxDaCOle1qgSIaimQzdunI9W3Ztug9zDuikLI75hgpSI=";

            modelBuilder.Entity<User>().HasData(
                // users 1-7
                MakeUser(1, "user1", userHash, UserRole.User, "11111111-1111-1111-1111-111111111111"),
                MakeUser(2, "user2", userHash, UserRole.User, "22222222-2222-2222-2222-222222222222"),
                MakeUser(3, "user3", userHash, UserRole.User, "33333333-3333-3333-3333-333333333333"),
                MakeUser(4, "user4", userHash, UserRole.User, "44444444-4444-4444-4444-444444444444"),
                MakeUser(5, "user5", userHash, UserRole.User, "55555555-5555-5555-5555-555555555555"),
                MakeUser(6, "user6", userHash, UserRole.User, "66666666-6666-6666-6666-666666666666"),
                MakeUser(7, "user7", userHash, UserRole.User, "77777777-7777-7777-7777-777777777777"),

                // admins 8-10
                MakeUser(8, "admin1", adminHash, UserRole.Admin, "88888888-8888-8888-8888-888888888888"),
                MakeUser(9, "admin2", adminHash, UserRole.Admin, "99999999-9999-9999-9999-999999999999"),
                MakeUser(10, "admin3", adminHash, UserRole.Admin, "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
            );

            /*───────────────────────────────────────
              4.  B O R R O W I N G   R E Q U E S T S
            ───────────────────────────────────────*/
            var requests = new List<BookBorrowingRequest>();
            for (int i = 1; i <= 20; i++)
            {
                requests.Add(new BookBorrowingRequest
                {
                    Id = i,
                    RequestorId = 1 + ((i - 1) % 7),
                    ApproverId = 8 + ((i - 1) % 3),
                    DateRequested = new DateTime(2025, 1, 1).AddDays(-(i - 1)),
                    Status = (RequestStatus)((i - 1) % 3)
                });
            }
            modelBuilder.Entity<BookBorrowingRequest>().HasData(requests);

            /*───────────────────────────────────────
              5.  R E Q U E S T   D E T A I L S
            ───────────────────────────────────────*/
            var details = new List<BookBorrowingRequestDetail>();
            int detailId = 1;

            for (int reqId = 1; reqId <= 20; reqId++)
            {
                int booksInReq = reqId <= 10 ? 4 : 3;
                for (int j = 0; j < booksInReq; j++)
                {
                    int bookId = ((reqId - 1) * 5 + j) % 50 + 1;
                    details.Add(new BookBorrowingRequestDetail
                    {
                        Id = detailId++,
                        RequestId = reqId,
                        BookId = bookId
                    });
                }
            }
            modelBuilder.Entity<BookBorrowingRequestDetail>().HasData(details);
        }

        /* helper to avoid repeating initialization boilerplate */
        private static User MakeUser(
            int id,
            string userName,
            string passwordHash,
            UserRole role,
            string staticGuid)
        {
            return new User
            {
                Id = id,
                Role = role,
                UserName = userName,
                NormalizedUserName = userName.ToUpperInvariant(),
                Email = $"{userName}@demo.local",
                NormalizedEmail = $"{userName}@demo.local".ToUpperInvariant(),
                EmailConfirmed = true,
                SecurityStamp = staticGuid,        // static
                ConcurrencyStamp = staticGuid,        // static
                PasswordHash = passwordHash
            };
        }
    }
}
