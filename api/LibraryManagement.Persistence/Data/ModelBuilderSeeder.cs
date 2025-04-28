using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}