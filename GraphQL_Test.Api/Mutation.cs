using GraphQL_Test.Database;
using GraphQL_Test.Database.Models;

namespace GraphQL_Test.Api
{
    public class Mutation
    {
        private readonly AppDbContext _dbContext;
        public Mutation(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Book> AddBook(
            string title, 
            string author, 
            decimal price
            )
        {
            var book = new Book
            {
                Title = title,
                Author = author,
                Price = price
            };

            _dbContext.Books.Add(book);
            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> UpdateBook(
            int id, 
            string title, 
            string author, 
            decimal price
            )
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            book.Title = title;
            book.Author = author;
            book.Price = price;

            await _dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
