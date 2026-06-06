var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var graphqlUrl = builder.Configuration["GraphQLApiUrl"] ?? "http://localhost:5011/graphql";
builder.Services.AddHttpClient<GraphQL_Test.WebApp.Services.GraphQLClient>(client =>
{
    client.BaseAddress = new Uri(graphqlUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");


app.Run();
