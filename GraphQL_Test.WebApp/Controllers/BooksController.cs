using Microsoft.AspNetCore.Mvc;
using GraphQL_Test.Database.Models;
using GraphQL_Test.WebApp.Services;

namespace GraphQL_Test.WebApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly GraphQLClient _graphQLClient;

        public BooksController(GraphQLClient graphQLClient)
        {
            _graphQLClient = graphQLClient;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var query = @"
                query {
                    books {
                        id
                        title
                        author
                        price
                    }
                }";

            var response = await _graphQLClient.SendRequestAsync<BooksResponse>(query);
            return View(response?.Books ?? new List<Book>());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var query = @"
                query($id: Int!) {
                    book(id: $id) {
                        id
                        title
                        author
                        price
                    }
                }";

            var response = await _graphQLClient.SendRequestAsync<BookResponse>(query, new { id });
            if (response?.Book == null)
            {
                return NotFound();
            }

            return View(response.Book);
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
                var mutation = @"
                    mutation($title: String!, $author: String!, $price: Decimal!) {
                        addBook(title: $title, author: $author, price: $price) {
                            id
                        }
                    }";

                var response = await _graphQLClient.SendRequestAsync<AddBookResponse>(mutation, new
                {
                    title = book.Title,
                    author = book.Author,
                    price = book.Price
                });

                if (response?.AddBook != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var query = @"
                query($id: Int!) {
                    book(id: $id) {
                        id
                        title
                        author
                        price
                    }
                }";

            var response = await _graphQLClient.SendRequestAsync<BookResponse>(query, new { id });
            if (response?.Book == null)
            {
                return NotFound();
            }
            return View(response.Book);
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
                var mutation = @"
                    mutation($id: Int!, $title: String!, $author: String!, $price: Decimal!) {
                        updateBook(id: $id, title: $title, author: $author, price: $price) {
                            id
                        }
                    }";

                var response = await _graphQLClient.SendRequestAsync<UpdateBookResponse>(mutation, new
                {
                    id = book.Id,
                    title = book.Title,
                    author = book.Author,
                    price = book.Price
                });

                if (response?.UpdateBook != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var query = @"
                query($id: Int!) {
                    book(id: $id) {
                        id
                        title
                        author
                        price
                    }
                }";

            var response = await _graphQLClient.SendRequestAsync<BookResponse>(query, new { id });
            if (response?.Book == null)
            {
                return NotFound();
            }
            return View(response.Book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mutation = @"
                mutation($id: Int!) {
                    deleteBook(id: $id)
                }";

            var response = await _graphQLClient.SendRequestAsync<DeleteBookResponse>(mutation, new { id });
            if (response != null && response.DeleteBook)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
