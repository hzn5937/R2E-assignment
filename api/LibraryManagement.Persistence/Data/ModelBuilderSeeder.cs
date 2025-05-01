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
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "History" },
                new Category { Id = 4, Name = "Fantasy" },
                new Category { Id = 5, Name = "Biography" },
                new Category { Id = 6, Name = "Technology" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "To Kill a Mockingbird", Author = "Harper Lee", CategoryId = 1, TotalQuantity = 10, AvailableQuantity = 7 },
                new Book { Id = 2, Title = "1984", Author = "George Orwell", CategoryId = 1, TotalQuantity = 12, AvailableQuantity = 10 },
                new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", CategoryId = 1, TotalQuantity = 8, AvailableQuantity = 5 },
                new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen", CategoryId = 1, TotalQuantity = 7, AvailableQuantity = 7 },
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

            var hasher = new PasswordHasher<User>();
            var users = new List<User>
            {
                // Admins (Id 1-3)
                new User { Id = 1, Username = "admin1", Role = UserRole.Admin, Email = "admin1@example.com" },
                new User { Id = 2, Username = "admin2", Role = UserRole.Admin, Email = "admin2@example.com" },
                new User { Id = 3, Username = "admin3", Role = UserRole.Admin, Email = "admin3@example.com" },
                // Users (Id 4-10)
                new User { Id = 5, Username = "user2", Role = UserRole.User, Email = "user1@example.com" },
                new User { Id = 6, Username = "user3", Role = UserRole.User, Email = "user2@example.com" },
                new User { Id = 7, Username = "user4", Role = UserRole.User, Email = "user3@example.com" },
                new User { Id = 8, Username = "user5", Role = UserRole.User, Email = "user4@example.com" },
                new User { Id = 9, Username = "user6", Role = UserRole.User, Email = "user5@example.com" },
                new User { Id = 10, Username = "user7", Role = UserRole.User, Email = "user6@example.com" },
                new User { Id = 11, Username = "user8", Role = UserRole.User, Email = "user7@example.com" }
            };

            // Hash passwords (using a common password "password123" for seeding)
            foreach (var user in users)
            {
                user.PasswordHash = hasher.HashPassword(user, "password123");
            }

            modelBuilder.Entity<User>().HasData(users);

            var requests = new List<BookBorrowingRequest>
            {
                new BookBorrowingRequest { Id = 1, RequestorId = 4, ApproverId = 1, DateRequested = DateTime.UtcNow.AddDays(-10), Status = RequestStatus.Approved },
                new BookBorrowingRequest { Id = 2, RequestorId = 5, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-5), Status = RequestStatus.Waiting },
                new BookBorrowingRequest { Id = 3, RequestorId = 6, ApproverId = 2, DateRequested = DateTime.UtcNow.AddDays(-2), Status = RequestStatus.Rejected },
                new BookBorrowingRequest { Id = 4, RequestorId = 4, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-1), Status = RequestStatus.Waiting },
                new BookBorrowingRequest { Id = 5, RequestorId = 7, ApproverId = 1, DateRequested = DateTime.UtcNow.AddDays(-15), Status = RequestStatus.Approved },
                new BookBorrowingRequest { Id = 6, RequestorId = 8, ApproverId = 3, DateRequested = DateTime.UtcNow.AddDays(-8), Status = RequestStatus.Approved },
                new BookBorrowingRequest { Id = 7, RequestorId = 9, ApproverId = null, DateRequested = DateTime.UtcNow.AddDays(-3), Status = RequestStatus.Waiting },
            };
            modelBuilder.Entity<BookBorrowingRequest>().HasData(requests);

            // --- Borrowing Request Detail Seed Data ---
            var requestDetails = new List<BookBorrowingRequestDetail>
            {
                // Request 1 (Approved, User 4, Admin 1) - 3 books
                new BookBorrowingRequestDetail { Id = 1, RequestId = 1, BookId = 1 }, // To Kill a Mockingbird
                new BookBorrowingRequestDetail { Id = 2, RequestId = 1, BookId = 7 }, // A Brief History of Time
                new BookBorrowingRequestDetail { Id = 3, RequestId = 1, BookId = 12 }, // The Hobbit

                // Request 2 (Waiting, User 5) - 5 books
                new BookBorrowingRequestDetail { Id = 4, RequestId = 2, BookId = 2 },  // 1984
                new BookBorrowingRequestDetail { Id = 5, RequestId = 2, BookId = 13 }, // Harry Potter
                new BookBorrowingRequestDetail { Id = 6, RequestId = 2, BookId = 22 }, // Clean Code
                new BookBorrowingRequestDetail { Id = 7, RequestId = 2, BookId = 33 }, // Origin of Species
                new BookBorrowingRequestDetail { Id = 8, RequestId = 2, BookId = 47 }, // Alexander Hamilton

                // Request 3 (Rejected, User 6, Admin 2) - 2 books
                new BookBorrowingRequestDetail { Id = 9, RequestId = 3, BookId = 6 }, // Sapiens
                new BookBorrowingRequestDetail { Id = 10, RequestId = 3, BookId = 18 }, // Becoming

                // Request 4 (Waiting, User 4) - 4 books
                new BookBorrowingRequestDetail { Id = 11, RequestId = 4, BookId = 4 }, // Pride and Prejudice
                new BookBorrowingRequestDetail { Id = 12, RequestId = 4, BookId = 14 }, // A Game of Thrones
                new BookBorrowingRequestDetail { Id = 13, RequestId = 4, BookId = 26 }, // Introduction to Algorithms
                new BookBorrowingRequestDetail { Id = 14, RequestId = 4, BookId = 40 }, // SPQR

                // Request 5 (Approved, User 7, Admin 1) - 1 book
                new BookBorrowingRequestDetail { Id = 15, RequestId = 5, BookId = 50 }, // Cracking the Coding Interview

                // Request 6 (Approved, User 8, Admin 3) - 2 books
                new BookBorrowingRequestDetail { Id = 16, RequestId = 6, BookId = 17 }, // Steve Jobs
                new BookBorrowingRequestDetail { Id = 17, RequestId = 6, BookId = 23 }, // The Pragmatic Programmer

                 // Request 7 (Waiting, User 9) - 3 books
                new BookBorrowingRequestDetail { Id = 18, RequestId = 7, BookId = 3 }, // The Great Gatsby
                new BookBorrowingRequestDetail { Id = 19, RequestId = 7, BookId = 10 }, // Guns, Germs, and Steel
                new BookBorrowingRequestDetail { Id = 20, RequestId = 7, BookId = 43 } // The Fellowship of the Ring
            };
            modelBuilder.Entity<BookBorrowingRequestDetail>().HasData(requestDetails);
        }
    }
}