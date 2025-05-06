using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Data
{
    public static class ModelBuilderSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // --- Category Seed Data ---
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "History" },
                new Category { Id = 4, Name = "Fantasy" },
                new Category { Id = 5, Name = "Biography" },
                new Category { Id = 6, Name = "Technology" }
            );

            // --- Book Seed Data (AvailableQuantity reflects existing approved requests) ---
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "To Kill a Mockingbird", Author = "Harper Lee", CategoryId = 1, TotalQuantity = 10, AvailableQuantity = 7 },
                new Book { Id = 2, Title = "1984", Author = "George Orwell", CategoryId = 1, TotalQuantity = 12, AvailableQuantity = 10 },
                new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", CategoryId = 1, TotalQuantity = 8, AvailableQuantity = 5 },
                new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", CategoryId = 1, TotalQuantity = 10, AvailableQuantity = 7 },
                new Book { Id = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", CategoryId = 1, TotalQuantity = 6, AvailableQuantity = 3 },
                new Book { Id = 6, Title = "Sapiens: A Brief History of Humankind", Author = "Yuval Noah Harari", CategoryId = 3, TotalQuantity = 15, AvailableQuantity = 11 },
                new Book { Id = 7, Title = "A Brief History of Time", Author = "Stephen Hawking", CategoryId = 2, TotalQuantity = 9, AvailableQuantity = 8 },
                new Book { Id = 8, Title = "Cosmos", Author = "Carl Sagan", CategoryId = 2, TotalQuantity = 7, AvailableQuantity = 6 },
                new Book { Id = 9, Title = "The Selfish Gene", Author = "Richard Dawkins", CategoryId = 2, TotalQuantity = 5, AvailableQuantity = 5 },
                new Book { Id = 10, Title = "Guns, Germs, and Steel", Author = "Jared Diamond", CategoryId = 3, TotalQuantity = 10, AvailableQuantity = 9 },
                new Book { Id = 11, Title = "A People's History of the United States", Author = "Howard Zinn", CategoryId = 3, TotalQuantity = 7, AvailableQuantity = 4 },
                new Book { Id = 12, Title = "The Hobbit", Author = "J.R.R. Tolkien", CategoryId = 4, TotalQuantity = 20, AvailableQuantity = 18 },
                new Book { Id = 13, Title = "Harry Potter and the Sorcerer's Stone", Author = "J.K. Rowling", CategoryId = 4, TotalQuantity = 25, AvailableQuantity = 22 },
                new Book { Id = 14, Title = "A Game of Thrones", Author = "George R.R. Martin", CategoryId = 4, TotalQuantity = 18, AvailableQuantity = 15 },
                new Book { Id = 15, Title = "The Name of the Wind", Author = "Patrick Rothfuss", CategoryId = 4, TotalQuantity = 14, AvailableQuantity = 10 },
                new Book { Id = 16, Title = "Mistborn: The Final Empire", Author = "Brandon Sanderson", CategoryId = 4, TotalQuantity = 16, AvailableQuantity = 16 },
                new Book { Id = 17, Title = "Steve Jobs", Author = "Walter Isaacson", CategoryId = 5, TotalQuantity = 11, AvailableQuantity = 9 },
                new Book { Id = 18, Title = "Becoming", Author = "Michelle Obama", CategoryId = 5, TotalQuantity = 13, AvailableQuantity = 13 },
                new Book { Id = 19, Title = "Unbroken", Author = "Laura Hillenbrand", CategoryId = 5, TotalQuantity = 9, AvailableQuantity = 7 },
                new Book { Id = 20, Title = "Educated", Author = "Tara Westover", CategoryId = 5, TotalQuantity = 10, AvailableQuantity = 8 },
                new Book { Id = 21, Title = "The Diary of a Young Girl", Author = "Anne Frank", CategoryId = 5, TotalQuantity = 8, AvailableQuantity = 8 },
                new Book { Id = 22, Title = "Clean Code: A Handbook of Agile Software Craftsmanship", Author = "Robert C. Martin", CategoryId = 6, TotalQuantity = 15, AvailableQuantity = 12 },
                new Book { Id = 23, Title = "The Pragmatic Programmer: Your Journey to Mastery", Author = "Andrew Hunt, David Thomas", CategoryId = 6, TotalQuantity = 12, AvailableQuantity = 11 },
                new Book { Id = 24, Title = "Code Complete", Author = "Steve McConnell", CategoryId = 6, TotalQuantity = 10, AvailableQuantity = 9 },
                new Book { Id = 25, Title = "Design Patterns: Elements of Reusable Object-Oriented Software", Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides", CategoryId = 6, TotalQuantity = 9, AvailableQuantity = 6 },
                new Book { Id = 26, Title = "Introduction to Algorithms", Author = "Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, Clifford Stein", CategoryId = 6, TotalQuantity = 7, AvailableQuantity = 5 },
                new Book { Id = 27, Title = "Brave New World", Author = "Aldous Huxley", CategoryId = 1, TotalQuantity = 11, AvailableQuantity = 10 },
                new Book { Id = 28, Title = "Lord of the Flies", Author = "William Golding", CategoryId = 1, TotalQuantity = 9, AvailableQuantity = 7 },
                new Book { Id = 29, Title = "Fahrenheit 451", Author = "Ray Bradbury", CategoryId = 1, TotalQuantity = 13, AvailableQuantity = 11 },
                new Book { Id = 30, Title = "The Hitchhiker's Guide to the Galaxy", Author = "Douglas Adams", CategoryId = 1, TotalQuantity = 15, AvailableQuantity = 14 },
                new Book { Id = 31, Title = "Moby Dick", Author = "Herman Melville", CategoryId = 1, TotalQuantity = 6, AvailableQuantity = 6 },
                new Book { Id = 32, Title = "War and Peace", Author = "Leo Tolstoy", CategoryId = 1, TotalQuantity = 5, AvailableQuantity = 3 },
                new Book { Id = 33, Title = "The Origin of Species", Author = "Charles Darwin", CategoryId = 2, TotalQuantity = 8, AvailableQuantity = 7 },
                new Book { Id = 34, Title = "The Double Helix", Author = "James D. Watson", CategoryId = 2, TotalQuantity = 6, AvailableQuantity = 5 },
                new Book { Id = 35, Title = "Silent Spring", Author = "Rachel Carson", CategoryId = 2, TotalQuantity = 7, AvailableQuantity = 7 },
                new Book { Id = 36, Title = "The Immortal Life of Henrietta Lacks", Author = "Rebecca Skloot", CategoryId = 2, TotalQuantity = 10, AvailableQuantity = 8 },
                new Book { Id = 37, Title = "The Structure of Scientific Revolutions", Author = "Thomas S. Kuhn", CategoryId = 2, TotalQuantity = 5, AvailableQuantity = 4 },
                new Book { Id = 38, Title = "1776", Author = "David McCullough", CategoryId = 3, TotalQuantity = 9, AvailableQuantity = 9 },
                new Book { Id = 39, Title = "The Rise and Fall of the Third Reich", Author = "William L. Shirer", CategoryId = 3, TotalQuantity = 6, AvailableQuantity = 5 },
                new Book { Id = 40, Title = "SPQR: A History of Ancient Rome", Author = "Mary Beard", CategoryId = 3, TotalQuantity = 8, AvailableQuantity = 7 },
                new Book { Id = 41, Title = "Rubicon: The Last Years of the Roman Republic", Author = "Tom Holland", CategoryId = null, TotalQuantity = 7, AvailableQuantity = 6 },
                new Book { Id = 42, Title = "The Guns of August", Author = "Barbara W. Tuchman", CategoryId = 3, TotalQuantity = 6, AvailableQuantity = 6 },
                new Book { Id = 43, Title = "The Fellowship of the Ring", Author = "J.R.R. Tolkien", CategoryId = 4, TotalQuantity = 19, AvailableQuantity = 17 },
                new Book { Id = 44, Title = "The Chronicles of Narnia", Author = "C.S. Lewis", CategoryId = 4, TotalQuantity = 15, AvailableQuantity = 13 },
                new Book { Id = 45, Title = "American Gods", Author = "Neil Gaiman", CategoryId = 4, TotalQuantity = 12, AvailableQuantity = 10 },
                new Book { Id = 46, Title = "The Color of Magic", Author = "Terry Pratchett", CategoryId = 4, TotalQuantity = 14, AvailableQuantity = 14 },
                new Book { Id = 47, Title = "Alexander Hamilton", Author = "Ron Chernow", CategoryId = null, TotalQuantity = 10, AvailableQuantity = 8 },
                new Book { Id = 48, Title = "Team of Rivals: The Political Genius of Abraham Lincoln", Author = "Doris Kearns Goodwin", CategoryId = 5, TotalQuantity = 8, AvailableQuantity = 7 },
                new Book { Id = 49, Title = "The Innovators: How a Group of Hackers, Geniuses, and Geeks Created the Digital Revolution", Author = "Walter Isaacson", CategoryId = 6, TotalQuantity = 11, AvailableQuantity = 10 },
                new Book { Id = 50, Title = "Cracking the Coding Interview", Author = "Gayle Laakmann McDowell", CategoryId = null, TotalQuantity = 20, AvailableQuantity = 15 }
            );

            // --- User Seed Data ---
            var hasher = new PasswordHasher<User>();
            var users = new List<User>
            {
                // Admins (Id 1-3)
                new User { Id = 1, Username = "admin1", Role = UserRole.Admin, Email = "admin1@example.com" },
                new User { Id = 2, Username = "admin2", Role = UserRole.Admin, Email = "admin2@example.com" },
                new User { Id = 3, Username = "admin3", Role = UserRole.Admin, Email = "admin3@example.com" },
                // Users (Id 4-11)
                new User { Id = 4, Username = "user1", Role = UserRole.User, Email = "user1@example.com" }, // Existing: 1 Approved, 1 Waiting
                new User { Id = 5, Username = "user2", Role = UserRole.User, Email = "user2@example.com" }, // Existing: 1 Waiting
                new User { Id = 6, Username = "user3", Role = UserRole.User, Email = "user3@example.com" }, // Existing: 1 Rejected
                new User { Id = 7, Username = "user4", Role = UserRole.User, Email = "user4@example.com" }, // Existing: 1 Approved
                new User { Id = 8, Username = "user5", Role = UserRole.User, Email = "user5@example.com" }, // Existing: 1 Approved
                new User { Id = 9, Username = "user6", Role = UserRole.User, Email = "user6@example.com" }, // Existing: 1 Waiting
                new User { Id = 10, Username = "user7", Role = UserRole.User, Email = "user7@example.com" }, // Existing: 0 Active
                new User { Id = 11, Username = "user8", Role = UserRole.User, Email = "user8@example.com" }  // Existing: 0 Active
            };

            // Hash passwords (using a common password "password123" for seeding)
            foreach (var user in users)
            {
                user.PasswordHash = hasher.HashPassword(user, "password123");
            }
            modelBuilder.Entity<User>().HasData(users);

            // --- Borrowing Request Seed Data ---
            var requests = new List<BookBorrowingRequest>
            {
                // --- Existing Requests (Id 1-7) ---
                new BookBorrowingRequest { Id = 1, RequestorId = 4, ApproverId = 1, DateRequested = DateTime.UtcNow.AddDays(-10), Status = RequestStatus.Approved }, // User 4 - Active #1
                new BookBorrowingRequest { Id = 2, RequestorId = 5, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-5), Status = RequestStatus.Waiting },  // User 5 - Active #1
                new BookBorrowingRequest { Id = 3, RequestorId = 6, ApproverId = 2, DateRequested = DateTime.UtcNow.AddDays(-2), Status = RequestStatus.Rejected }, // User 6 - Not Active
                new BookBorrowingRequest { Id = 4, RequestorId = 4, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-1), Status = RequestStatus.Waiting },  // User 4 - Active #2
                new BookBorrowingRequest { Id = 5, RequestorId = 7, ApproverId = 1, DateRequested = DateTime.UtcNow.AddDays(-15), Status = RequestStatus.Approved }, // User 7 - Active #1
                new BookBorrowingRequest { Id = 6, RequestorId = 8, ApproverId = 3, DateRequested = DateTime.UtcNow.AddDays(-8), Status = RequestStatus.Approved },  // User 8 - Active #1
                new BookBorrowingRequest { Id = 7, RequestorId = 9, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-3), Status = RequestStatus.Waiting },   // User 9 - Active #1

                // --- New Requests (Id 8+) ---

                // User 4 already has 2 active, add 1 more (Waiting)
                new BookBorrowingRequest { Id = 8, RequestorId = 4, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-0.5), Status = RequestStatus.Waiting }, // User 4 - Active #3 (Total: 3)

                // User 5 has 1 active, add 2 more (1 Approved, 1 Waiting)
                new BookBorrowingRequest { Id = 9, RequestorId = 5, ApproverId = 1, DateRequested = DateTime.UtcNow.AddDays(-4), Status = RequestStatus.Approved }, // User 5 - Active #2
                new BookBorrowingRequest { Id = 10, RequestorId = 5, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-0.8), Status = RequestStatus.Waiting },// User 5 - Active #3 (Total: 3)

                // User 6 has 0 active (1 rejected), add 3 (1 Rejected, 1 Waiting, 1 Approved)
                new BookBorrowingRequest { Id = 11, RequestorId = 6, ApproverId = 2, DateRequested = DateTime.UtcNow.AddDays(-6), Status = RequestStatus.Rejected },// User 6 - Not Active
                new BookBorrowingRequest { Id = 12, RequestorId = 6, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-1.5), Status = RequestStatus.Waiting }, // User 6 - Active #1
                new BookBorrowingRequest { Id = 13, RequestorId = 6, ApproverId = 3, DateRequested = DateTime.UtcNow.AddDays(-1.2), Status = RequestStatus.Approved },// User 6 - Active #2 (Total: 2)

                // User 10 has 0 active, add 3 (1 Waiting, 2 Approved)
                new BookBorrowingRequest { Id = 14, RequestorId = 10, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-3.5), Status = RequestStatus.Waiting }, // User 10 - Active #1
                new BookBorrowingRequest { Id = 15, RequestorId = 10, ApproverId = 1, DateRequested = DateTime.UtcNow.AddDays(-3.2), Status = RequestStatus.Approved },// User 10 - Active #2
                new BookBorrowingRequest { Id = 16, RequestorId = 10, ApproverId = 2, DateRequested = DateTime.UtcNow.AddDays(-2.8), Status = RequestStatus.Approved },// User 10 - Active #3 (Total: 3)

                // User 11 has 0 active, add 2 (1 Waiting, 1 Rejected)
                new BookBorrowingRequest { Id = 17, RequestorId = 11, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-2.2), Status = RequestStatus.Waiting }, // User 11 - Active #1 (Total: 1)
                new BookBorrowingRequest { Id = 18, RequestorId = 11, ApproverId = 3, DateRequested = DateTime.UtcNow.AddDays(-1.8), Status = RequestStatus.Rejected }, // User 11 - Not Active

                // --- Additional Requests to Demonstrate Monthly Quota System ---
                
                // User 4 - Previous month requests (April 2025 - quota should reset in May)
                new BookBorrowingRequest { Id = 19, RequestorId = 4, ApproverId = 1, DateRequested = new DateTime(2025, 4, 5, 10, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 4, 15, 14, 30, 0, DateTimeKind.Utc) }, // April - Returned #1
                new BookBorrowingRequest { Id = 20, RequestorId = 4, ApproverId = 2, DateRequested = new DateTime(2025, 4, 12, 14, 45, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 4, 25, 16, 00, 0, DateTimeKind.Utc) }, // April - Returned #2
                new BookBorrowingRequest { Id = 21, RequestorId = 4, ApproverId = 3, DateRequested = new DateTime(2025, 4, 20, 9, 15, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // April - Active #3 (Max quota reached for April)
                new BookBorrowingRequest { Id = 22, RequestorId = 4, ApproverId = 1, DateRequested = new DateTime(2025, 4, 25, 16, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Rejected }, // April - Rejected (over quota)
                
                // User 5 - Multiple months showing quota reset
                new BookBorrowingRequest { Id = 23, RequestorId = 5, ApproverId = 2, DateRequested = new DateTime(2025, 3, 15, 11, 20, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 3, 30, 9, 45, 0, DateTimeKind.Utc) }, // March - Returned #1
                new BookBorrowingRequest { Id = 24, RequestorId = 5, ApproverId = 3, DateRequested = new DateTime(2025, 3, 22, 13, 40, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 4, 5, 10, 20, 0, DateTimeKind.Utc) }, // March - Returned #2
                new BookBorrowingRequest { Id = 25, RequestorId = 5, ApproverId = 1, DateRequested = new DateTime(2025, 3, 28, 9, 50, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // March - Active #3 (Max quota reached for March)
                new BookBorrowingRequest { Id = 26, RequestorId = 5, ApproverId = 3, DateRequested = new DateTime(2025, 4, 4, 14, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 4, 20, 11, 30, 0, DateTimeKind.Utc) }, // April - Returned #1 (Quota reset)
                new BookBorrowingRequest { Id = 27, RequestorId = 5, ApproverId = 2, DateRequested = new DateTime(2025, 4, 18, 10, 15, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // April - Active #2
                
                // User 7 - Mixture of approved, waiting, and rejected across multiple months
                new BookBorrowingRequest { Id = 28, RequestorId = 7, ApproverId = null, DateRequested = new DateTime(2025, 4, 6, 9, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Waiting }, // April - Active #1
                new BookBorrowingRequest { Id = 29, RequestorId = 7, ApproverId = 1, DateRequested = new DateTime(2025, 4, 14, 16, 45, 0, DateTimeKind.Utc), Status = RequestStatus.Rejected }, // April - Not active (rejected)
                new BookBorrowingRequest { Id = 30, RequestorId = 7, ApproverId = 2, DateRequested = new DateTime(2025, 4, 22, 11, 20, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // April - Active #2
                
                // User 9 - Showing 0, 1, 2, and 3 active requests in different months
                new BookBorrowingRequest { Id = 31, RequestorId = 9, ApproverId = 1, DateRequested = new DateTime(2025, 2, 10, 10, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 2, 28, 15, 15, 0, DateTimeKind.Utc) }, // Feb - Returned #1 (Only 1 in Feb)
                new BookBorrowingRequest { Id = 32, RequestorId = 9, ApproverId = 2, DateRequested = new DateTime(2025, 3, 5, 14, 20, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 3, 25, 10, 45, 0, DateTimeKind.Utc) }, // March - Returned #1
                new BookBorrowingRequest { Id = 33, RequestorId = 9, ApproverId = 3, DateRequested = new DateTime(2025, 3, 15, 9, 45, 0, DateTimeKind.Utc), Status = RequestStatus.Returned, DateReturned = new DateTime(2025, 4, 1, 14, 00, 0, DateTimeKind.Utc) }, // March - Returned #2 (2 in March)
                new BookBorrowingRequest { Id = 34, RequestorId = 9, ApproverId = 1, DateRequested = new DateTime(2025, 4, 3, 11, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // April - Active #1
                new BookBorrowingRequest { Id = 35, RequestorId = 9, ApproverId = 2, DateRequested = new DateTime(2025, 4, 10, 15, 45, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // April - Active #2
                new BookBorrowingRequest { Id = 36, RequestorId = 9, ApproverId = null, DateRequested = new DateTime(2025, 4, 20, 9, 15, 0, DateTimeKind.Utc), Status = RequestStatus.Waiting }, // April - Active #3 (3 in April - max quota)
                
                // User 11 - Using all quota in May (current month)
                new BookBorrowingRequest { Id = 37, RequestorId = 11, ApproverId = 1, DateRequested = new DateTime(2025, 5, 1, 10, 30, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // May - Active #1
                new BookBorrowingRequest { Id = 38, RequestorId = 11, ApproverId = 2, DateRequested = new DateTime(2025, 5, 2, 14, 15, 0, DateTimeKind.Utc), Status = RequestStatus.Approved }, // May - Active #2
                new BookBorrowingRequest { Id = 39, RequestorId = 11, ApproverId = null, DateRequested = new DateTime(2025, 5, 3, 9, 45, 0, DateTimeKind.Utc), Status = RequestStatus.Waiting }  // May - Active #3 (max quota reached)
            };
            modelBuilder.Entity<BookBorrowingRequest>().HasData(requests);

            // --- Borrowing Request Detail Seed Data ---
            var requestDetails = new List<BookBorrowingRequestDetail>
            {
                // --- Details for Existing Requests (Id 1-7 corresponding to Detail Id 1-20) ---
                // Request 1 (Approved, User 4) - 3 books
                new BookBorrowingRequestDetail { Id = 1, RequestId = 1, BookId = 1 }, // To Kill a Mockingbird (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 2, RequestId = 1, BookId = 7 }, // A Brief History of Time (Avail: 8 -> 7)
                new BookBorrowingRequestDetail { Id = 3, RequestId = 1, BookId = 12 }, // The Hobbit (Avail: 18 -> 17)

                // Request 2 (Waiting, User 5) - 5 books
                new BookBorrowingRequestDetail { Id = 4, RequestId = 2, BookId = 2 },  // 1984 (Avail: 10 -> 9)
                new BookBorrowingRequestDetail { Id = 5, RequestId = 2, BookId = 13 }, // Harry Potter (Avail: 22 -> 21)
                new BookBorrowingRequestDetail { Id = 6, RequestId = 2, BookId = 22 }, // Clean Code (Avail: 12 -> 11)
                new BookBorrowingRequestDetail { Id = 7, RequestId = 2, BookId = 33 }, // Origin of Species (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 8, RequestId = 2, BookId = 47 }, // Alexander Hamilton (Avail: 8 -> 7)

                // Request 3 (Rejected, User 6) - 2 books (No change in Availability)
                new BookBorrowingRequestDetail { Id = 9, RequestId = 3, BookId = 6 }, // Sapiens (Avail: 11)
                new BookBorrowingRequestDetail { Id = 10, RequestId = 3, BookId = 18 }, // Becoming (Avail: 13)

                // Request 4 (Waiting, User 4) - 4 books
                new BookBorrowingRequestDetail { Id = 11, RequestId = 4, BookId = 4 }, // Pride and Prejudice (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 12, RequestId = 4, BookId = 14 }, // A Game of Thrones (Avail: 15 -> 14)
                new BookBorrowingRequestDetail { Id = 13, RequestId = 4, BookId = 26 }, // Introduction to Algorithms (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 14, RequestId = 4, BookId = 40 }, // SPQR (Avail: 7 -> 6)

                // Request 5 (Approved, User 7) - 1 book
                new BookBorrowingRequestDetail { Id = 15, RequestId = 5, BookId = 50 }, // Cracking the Coding Interview (Avail: 15 -> 14)

                // Request 6 (Approved, User 8) - 2 books
                new BookBorrowingRequestDetail { Id = 16, RequestId = 6, BookId = 17 }, // Steve Jobs (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 17, RequestId = 6, BookId = 23 }, // The Pragmatic Programmer (Avail: 11 -> 10)

                 // Request 7 (Waiting, User 9) - 3 books
                new BookBorrowingRequestDetail { Id = 18, RequestId = 7, BookId = 3 }, // The Great Gatsby (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 19, RequestId = 7, BookId = 10 }, // Guns, Germs, and Steel (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 20, RequestId = 7, BookId = 43 }, // The Fellowship of the Ring (Avail: 17 -> 16)


                // --- Details for New Requests (Id 8+ corresponding to Detail Id 21+) ---

                // Request 8 (Waiting, User 4) - 2 books
                new BookBorrowingRequestDetail { Id = 21, RequestId = 8, BookId = 8 }, // Cosmos (Avail: 6 -> 5)
                new BookBorrowingRequestDetail { Id = 22, RequestId = 8, BookId = 27 }, // Brave New World (Avail: 10 -> 9)

                // Request 9 (Approved, User 5) - 3 books
                new BookBorrowingRequestDetail { Id = 23, RequestId = 9, BookId = 16 }, // Mistborn (Avail: 16 -> 15)
                new BookBorrowingRequestDetail { Id = 24, RequestId = 9, BookId = 36 }, // The Immortal Life of Henrietta Lacks (Avail: 8 -> 7)
                new BookBorrowingRequestDetail { Id = 25, RequestId = 9, BookId = 44 }, // The Chronicles of Narnia (Avail: 13 -> 12)

                // Request 10 (Waiting, User 5) - 1 book
                new BookBorrowingRequestDetail { Id = 26, RequestId = 10, BookId = 30 },// The Hitchhiker's Guide (Avail: 14 -> 13)

                // Request 11 (Rejected, User 6) - 2 books (No change in Availability)
                new BookBorrowingRequestDetail { Id = 27, RequestId = 11, BookId = 5 }, // The Catcher in the Rye (Avail: 3)
                new BookBorrowingRequestDetail { Id = 28, RequestId = 11, BookId = 32 },// War and Peace (Avail: 3)

                // Request 12 (Waiting, User 6) - 3 books
                new BookBorrowingRequestDetail { Id = 29, RequestId = 12, BookId = 28 },// Lord of the Flies (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 30, RequestId = 12, BookId = 29 },// Fahrenheit 451 (Avail: 11 -> 10)
                new BookBorrowingRequestDetail { Id = 31, RequestId = 12, BookId = 31 },// Moby Dick (Avail: 6 -> 5)

                // Request 13 (Approved, User 6) - 1 book
                new BookBorrowingRequestDetail { Id = 32, RequestId = 13, BookId = 46 },// The Color of Magic (Avail: 14 -> 13)

                // Request 14 (Waiting, User 10) - 5 books
                new BookBorrowingRequestDetail { Id = 33, RequestId = 14, BookId = 9 }, // The Selfish Gene (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 34, RequestId = 14, BookId = 24 },// Code Complete (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 35, RequestId = 14, BookId = 34 },// The Double Helix (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 36, RequestId = 14, BookId = 38 },// 1776 (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 37, RequestId = 14, BookId = 42 },// The Guns of August (Avail: 6 -> 5)

                // Request 15 (Approved, User 10) - 2 books
                new BookBorrowingRequestDetail { Id = 38, RequestId = 15, BookId = 19 },// Unbroken (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 39, RequestId = 15, BookId = 20 },// Educated (Avail: 8 -> 7)

                // Request 16 (Approved, User 10) - 1 book
                new BookBorrowingRequestDetail { Id = 40, RequestId = 16, BookId = 49 },// The Innovators (Avail: 10 -> 9)

                // Request 17 (Waiting, User 11) - 3 books
                new BookBorrowingRequestDetail { Id = 41, RequestId = 17, BookId = 35 },// Silent Spring (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 42, RequestId = 17, BookId = 45 },// American Gods (Avail: 10 -> 9)
                new BookBorrowingRequestDetail { Id = 43, RequestId = 17, BookId = 25 },// Design Patterns (Avail: 6 -> 5)

                // Request 18 (Rejected, User 11) - 1 book (No change in Availability)
                new BookBorrowingRequestDetail { Id = 44, RequestId = 18, BookId = 41 }, // Rubicon (Avail: 6)

                // --- Details for Additional Monthly Quota Demonstration Requests (Id 19-39) ---
                
                // Request 19 (Approved, User 4, April) - 2 books 
                new BookBorrowingRequestDetail { Id = 45, RequestId = 19, BookId = 11 }, // A People's History (Avail: 4 -> 3)
                new BookBorrowingRequestDetail { Id = 46, RequestId = 19, BookId = 15 }, // The Name of the Wind (Avail: 10 -> 9)
                
                // Request 20 (Approved, User 4, April) - 3 books
                new BookBorrowingRequestDetail { Id = 47, RequestId = 20, BookId = 21 }, // The Diary of a Young Girl (Avail: 8 -> 7)
                new BookBorrowingRequestDetail { Id = 48, RequestId = 20, BookId = 39 }, // The Rise and Fall of the Third Reich (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 49, RequestId = 20, BookId = 48 }, // Team of Rivals (Avail: 7 -> 6)
                
                // Request 21 (Approved, User 4, April) - 1 book (reaching max quota for April)
                new BookBorrowingRequestDetail { Id = 50, RequestId = 21, BookId = 37 }, // The Structure of Scientific Revolutions (Avail: 4 -> 3)
                
                // Request 22 (Rejected, User 4, April) - 1 book (over quota, so rejected)
                new BookBorrowingRequestDetail { Id = 51, RequestId = 22, BookId = 21 }, // The Diary of a Young Girl (still Avail: 7)
                
                // Request 23 (Approved, User 5, March) - 2 books
                new BookBorrowingRequestDetail { Id = 52, RequestId = 23, BookId = 5 },  // The Catcher in the Rye (Avail: 3 -> 2)
                new BookBorrowingRequestDetail { Id = 53, RequestId = 23, BookId = 32 }, // War and Peace (Avail: 3 -> 2)
                
                // Request 24 (Approved, User 5, March) - 3 books
                new BookBorrowingRequestDetail { Id = 54, RequestId = 24, BookId = 3 },  // The Great Gatsby (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 55, RequestId = 24, BookId = 27 }, // Brave New World (Avail: 10 -> 9)
                new BookBorrowingRequestDetail { Id = 56, RequestId = 24, BookId = 29 }, // Fahrenheit 451 (Avail: 11 -> 10)
                
                // Request 25 (Approved, User 5, March) - 4 books (max quota for March)
                new BookBorrowingRequestDetail { Id = 57, RequestId = 25, BookId = 15 }, // The Name of the Wind (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 58, RequestId = 25, BookId = 18 }, // Becoming (Avail: 13 -> 12)
                new BookBorrowingRequestDetail { Id = 59, RequestId = 25, BookId = 31 }, // Moby Dick (Avail: 6 -> 5)
                new BookBorrowingRequestDetail { Id = 60, RequestId = 25, BookId = 35 }, // Silent Spring (Avail: 7 -> 6)
                
                // Request 26 (Approved, User 5, April) - 5 books (quota reset for April)
                new BookBorrowingRequestDetail { Id = 61, RequestId = 26, BookId = 6 },  // Sapiens (Avail: 11 -> 10)
                new BookBorrowingRequestDetail { Id = 62, RequestId = 26, BookId = 9 },  // The Selfish Gene (Avail: 5 -> 4)
                new BookBorrowingRequestDetail { Id = 63, RequestId = 26, BookId = 12 }, // The Hobbit (Avail: 18 -> 17)
                new BookBorrowingRequestDetail { Id = 64, RequestId = 26, BookId = 25 }, // Design Patterns (Avail: 6 -> 5)
                new BookBorrowingRequestDetail { Id = 65, RequestId = 26, BookId = 43 }, // The Fellowship of the Ring (Avail: 17 -> 16)
                
                // Request 27 (Approved, User 5, April) - 2 books
                new BookBorrowingRequestDetail { Id = 66, RequestId = 27, BookId = 14 }, // A Game of Thrones (Avail: 15 -> 14)
                new BookBorrowingRequestDetail { Id = 67, RequestId = 27, BookId = 20 }, // Educated (Avail: 8 -> 7)
                
                // Request 28 (Waiting, User 7, April) - 2 books
                new BookBorrowingRequestDetail { Id = 68, RequestId = 28, BookId = 1 },  // To Kill a Mockingbird (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 69, RequestId = 28, BookId = 13 }, // Harry Potter (Avail: 22 -> 21)
                
                // Request 29 (Rejected, User 7, April) - 1 book (no change to availability)
                new BookBorrowingRequestDetail { Id = 70, RequestId = 29, BookId = 16 }, // Mistborn (still Avail: 16)
                
                // Request 30 (Approved, User 7, April) - 3 books
                new BookBorrowingRequestDetail { Id = 71, RequestId = 30, BookId = 4 },  // Pride and Prejudice (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 72, RequestId = 30, BookId = 22 }, // Clean Code (Avail: 12 -> 11)
                new BookBorrowingRequestDetail { Id = 73, RequestId = 30, BookId = 45 }, // American Gods (Avail: 10 -> 9)
                
                // Request 31 (Approved, User 9, February) - 1 book
                new BookBorrowingRequestDetail { Id = 74, RequestId = 31, BookId = 2 },  // 1984 (Avail: 10 -> 9)
                
                // Request 32 (Approved, User 9, March) - 4 books
                new BookBorrowingRequestDetail { Id = 75, RequestId = 32, BookId = 8 },  // Cosmos (Avail: 6 -> 5)
                new BookBorrowingRequestDetail { Id = 76, RequestId = 32, BookId = 10 }, // Guns, Germs, and Steel (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 77, RequestId = 32, BookId = 17 }, // Steve Jobs (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 78, RequestId = 32, BookId = 24 }, // Code Complete (Avail: 9 -> 8)
                
                // Request 33 (Approved, User 9, March) - 1 book
                new BookBorrowingRequestDetail { Id = 79, RequestId = 33, BookId = 33 }, // The Origin of Species (Avail: 7 -> 6)
                
                // Request 34 (Approved, User 9, April) - 5 books
                new BookBorrowingRequestDetail { Id = 80, RequestId = 34, BookId = 36 }, // The Immortal Life of Henrietta Lacks (Avail: 8 -> 7)
                new BookBorrowingRequestDetail { Id = 81, RequestId = 34, BookId = 38 }, // 1776 (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 82, RequestId = 34, BookId = 40 }, // SPQR (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 83, RequestId = 34, BookId = 46 }, // The Color of Magic (Avail: 14 -> 13)
                new BookBorrowingRequestDetail { Id = 84, RequestId = 34, BookId = 49 }, // The Innovators (Avail: 10 -> 9)
                
                // Request 35 (Approved, User 9, April) - 1 book
                new BookBorrowingRequestDetail { Id = 85, RequestId = 35, BookId = 44 }, // The Chronicles of Narnia (Avail: 13 -> 12)
                
                // Request 36 (Waiting, User 9, April) - 1 book (max quota for April)
                new BookBorrowingRequestDetail { Id = 86, RequestId = 36, BookId = 7 },  // A Brief History of Time (Avail: 8 -> 7)
                
                // Request 37 (Approved, User 11, May) - 4 books
                new BookBorrowingRequestDetail { Id = 87, RequestId = 37, BookId = 2 },  // 1984 (Avail: 9 -> 8)
                new BookBorrowingRequestDetail { Id = 88, RequestId = 37, BookId = 16 }, // Mistborn (Avail: 16 -> 15)
                new BookBorrowingRequestDetail { Id = 89, RequestId = 37, BookId = 28 }, // Lord of the Flies (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 90, RequestId = 37, BookId = 47 }, // Alexander Hamilton (Avail: 8 -> 7)
                
                // Request 38 (Approved, User 11, May) - 1 book
                new BookBorrowingRequestDetail { Id = 91, RequestId = 38, BookId = 23 }, // The Pragmatic Programmer (Avail: 11 -> 10)
                
                // Request 39 (Waiting, User 11, May) - 3 books (max quota for May)
                new BookBorrowingRequestDetail { Id = 92, RequestId = 39, BookId = 6 },  // Sapiens (Avail: 10 -> 9)
                new BookBorrowingRequestDetail { Id = 93, RequestId = 39, BookId = 19 }, // Unbroken (Avail: 7 -> 6)
                new BookBorrowingRequestDetail { Id = 94, RequestId = 39, BookId = 26 }  // Introduction to Algorithms (Avail: 5 -> 4)
            };
            modelBuilder.Entity<BookBorrowingRequestDetail>().HasData(requestDetails);

            // Note: The AvailableQuantity in the Book seed data above does NOT reflect the new requests added here.
            // The logic in the application (e.g., RequestService) should handle the decrementing
            // of AvailableQuantity when a request is Approved or created as Waiting, and potentially
            // incrementing it if a Waiting request is Rejected or an Approved request is completed/returned.
            // The seeding here just sets the initial state and ensures we don't seed requests for books
            // that are already seeded with AvailableQuantity = 0.
        }
    }
}