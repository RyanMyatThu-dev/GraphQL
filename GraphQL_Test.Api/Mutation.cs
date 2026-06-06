using GraphQL_Test.Database;
using GraphQL_Test.Database.Models;

namespace GraphQL_Test.Api
{
    public class Mutation
    {
        public async Task<Book> AddBook(
            string title,
            string author,
            decimal price,
            AppDbContext context
            )
        {
            var book = new Book
            {
                Title = title,
                Author = author,
                Price = price
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> UpdateBook(
            int id,
            string title,
            string author,
            decimal price,
            AppDbContext context
            )
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            book.Title = title;
            book.Author = author;
            book.Price = price;

            await context.SaveChangesAsync();

            return book;
        }

        public async Task<bool> DeleteBook(int id,
            AppDbContext context
        )
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
