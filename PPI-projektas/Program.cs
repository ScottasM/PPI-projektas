
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PPI_projektas.objects;

using PPI_projektas.objects.Factories;
using PPI_projektas.Services;
using PPI_projektas.Services.Interfaces;
using PPI_projektas.Services.Response;

using PPI_projektas.Utils;




var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllersWithViews();

var connectionString = "server=185.34.52.6;user=NotesApp;password=AlioValioIrInternetas;database=NotesApp";//builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(connectionString);

var serverVersion = MariaDbServerVersion.AutoDetect(connectionString);



// Replace 'YourDbContext' with the name of your own DbContext derived class.
builder.Services.AddDbContext<EntityData>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
); 


/*builder.Services.AddDbContext<EntityData>(options => {
    options.UseMySql(ServerVersion.AutoDetect(connectionString));
});

if (connectionString == null) {
    Console.WriteLine("connectionString is empty?!?!?!???!!!!!?!?!?");
}*/

DataHandler dataHandler = new DataHandler(connectionString);

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
builder.Services.AddScoped<ICustomAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IObjectDataItemFactory, ObjectDataItemFactory>();
builder.Services.AddScoped<IOpenedNoteDataFactory, OpenedNoteDataFactory>();
builder.Services.AddScoped<IAuthReturnFactory, AuthReturnFactory>();
builder.Services.AddScoped<IGroupFactory, GroupFactory>();
builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddScoped<INoteFactory, NoteFactory>();


var app = builder.Build();




/*using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    try {
        var context = services.GetRequiredService<EntityData>();
        context.Database.Migrate();
    }
    catch (Exception ex) {
        // Log error
    }
}*/



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
