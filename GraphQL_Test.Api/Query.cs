using GraphQL_Test.Database;
using GraphQL_Test.Database.Models;
using HotChocolate.Data;


namespace GraphQL_Test.Api
{
    public class Query
    {
        private readonly AppDbContext _dbContext;
        public Query(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [UseFiltering]
        [UseSorting]
        public IQueryable<Book> GetBooks() => _dbContext.Books;
        public Book? GetBook(int id) =>
            _dbContext.Books.FirstOrDefault(b => b.Id == id);
    }
}
