using System.Net.Http.Json;
using System.Text.Json;
using GraphQL_Test.Database.Models;

namespace GraphQL_Test.WebApp.Services
{
    public class GraphQLClient
    {
        private readonly HttpClient _httpClient;

        public GraphQLClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> SendRequestAsync<T>(string query, object? variables = null)
        {
            var requestBody = new
            {
                query = query,
                variables = variables
            };

            var response = await _httpClient.PostAsJsonAsync("", requestBody);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("errors", out var errorsProp))
            {
                throw new Exception($"GraphQL Error: {errorsProp.GetRawText()}");
            }

            if (document.RootElement.TryGetProperty("data", out var dataProp))
            {
                return JsonSerializer.Deserialize<T>(dataProp.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return default;
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
