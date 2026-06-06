using GraphQL_Test.Api;
using GraphQL_Test.Database;
using GraphQL_Test.Database.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register AppDbContext to use the In-Memory database provider
builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
    options.UseInMemoryDatabase("BookDb"));

builder.Services
       .AddGraphQLServer()
       .AddQueryType<Query>()
       .AddMutationType<Mutation>()
       .RegisterDbContextFactory<AppDbContext>()
       .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment())
       .AddFiltering()
       .AddSorting();




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Seed initial data into the In-Memory DB
using (var scope = app.Services.CreateScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using (var dbContext = dbContextFactory.CreateDbContext())
    {
        dbContext.Database.EnsureCreated();
        if (!dbContext.Books.Any())
        {
            dbContext.Books.AddRange(
                new Book { Title = "The C# Player's Guide", Author = "Jeremy Clark", Price = 29.99m },
                new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 39.99m },
                new Book { Title = "GraphQL in Action", Author = "Samer Buna", Price = 34.99m }
            );
            dbContext.SaveChanges();
        }
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGraphQL(); // Enable the GraphQL endpoint and Banana Cake Pop UI

app.Run();
