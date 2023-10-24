using PPI_projektas.objects.Factories;
using PPI_projektas.Services;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;


DataHandler dataHandler = new DataHandler();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:44488")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

/* Dependency injection */
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IObjectDataItemFactory, ObjectDataItemFactory>();
builder.Services.AddScoped<IOpenedNoteDataFactory, OpenedNoteDataFactory>();
builder.Services.AddScoped<IGroupFactory, GroupFactory>();
builder.Services.AddScoped<IUserFactory, UserFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
}



app.UseStaticFiles();
app.UseRouting();

// Use CORS
app.UseCors("AllowLocalhost");

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
