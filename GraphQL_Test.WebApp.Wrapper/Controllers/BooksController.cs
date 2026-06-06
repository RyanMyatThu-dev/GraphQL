using Microsoft.AspNetCore.Mvc;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL_Test.Database.Models;

namespace GraphQL_Test.WebApp.Wrapper.Controllers
{
    public class BooksController : Controller
    {
        private readonly GraphQLHttpClient _graphQLClient;

        public BooksController(GraphQLHttpClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var request = new GraphQLRequest
            {
                Query = @"
                    query {
                        books {
                            id
                            title
                            author
                            price
                        }
                    }"
            };

            var response = await _graphQLClient.SendQueryAsync<BooksResponse>(request);
            return View(response.Data?.Books ?? new List<Book>());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var request = new GraphQLRequest
            {
                Query = @"
                    query($id: Int!) {
                        book(id: $id) {
                            id
                            title
                            author
                            price
                        }
                    }",
                Variables = new { id }
            };

            var response = await _graphQLClient.SendQueryAsync<BookResponse>(request);
            if (response.Data?.Book == null)
            {
                return NotFound();
            }

            return View(response.Data.Book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        mutation($title: String!, $author: String!, $price: Decimal!) {
                            addBook(title: $title, author: $author, price: $price) {
                                id
                            }
                        }",
                    Variables = new
                    {
                        title = book.Title,
                        author = book.Author,
                        price = book.Price
                    }
                };

                var response = await _graphQLClient.SendMutationAsync<AddBookResponse>(request);
                if (response.Data?.AddBook != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var request = new GraphQLRequest
            {
                Query = @"
                    query($id: Int!) {
                        book(id: $id) {
                            id
                            title
                            author
                            price
                        }
                    }",
                Variables = new { id }
            };

            var response = await _graphQLClient.SendQueryAsync<BookResponse>(request);
            if (response.Data?.Book == null)
            {
                return NotFound();
            }
            return View(response.Data.Book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Price")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var request = new GraphQLRequest
                {
                    Query = @"
                        mutation($id: Int!, $title: String!, $author: String!, $price: Decimal!) {
                            updateBook(id: $id, title: $title, author: $author, price: $price) {
                                id
                            }
                        }",
                    Variables = new
                    {
                        id = book.Id,
                        title = book.Title,
                        author = book.Author,
                        price = book.Price
                    }
                };

                var response = await _graphQLClient.SendMutationAsync<UpdateBookResponse>(request);
                if (response.Data?.UpdateBook != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var request = new GraphQLRequest
            {
                Query = @"
                    query($id: Int!) {
                        book(id: $id) {
                            id
                            title
                            author
                            price
                        }
                    }",
                Variables = new { id }
            };

            var response = await _graphQLClient.SendQueryAsync<BookResponse>(request);
            if (response.Data?.Book == null)
            {
                return NotFound();
            }
            return View(response.Data.Book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = new GraphQLRequest
            {
                Query = @"
                    mutation($id: Int!) {
                        deleteBook(id: $id)
                    }",
                Variables = new { id }
            };

            var response = await _graphQLClient.SendMutationAsync<DeleteBookResponse>(request);
            if (response.Data != null && response.Data.DeleteBook)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class BooksResponse
    {
        public List<Book> Books { get; set; } = new();
    }

    public class BookResponse
    {
        public Book? Book { get; set; }
    }

    public class AddBookResponse
    {
        public Book AddBook { get; set; } = null!;
    }

    public class UpdateBookResponse
    {
        public Book? UpdateBook { get; set; }
    }

    public class DeleteBookResponse
    {
        public bool DeleteBook { get; set; }
    }
}
