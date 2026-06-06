using GraphQL_Test.Database;
using GraphQL_Test.Database.Models;
using HotChocolate.Data;


namespace GraphQL_Test.Api
{
    public class Query
    {

        [UseFiltering]
        [UseSorting]
        public IQueryable<Book> GetBooks(AppDbContext context) => context.Books;

        public Book? GetBook(AppDbContext context, int id) =>
            context.Books.FirstOrDefault(b => b.Id == id);
    }
}
