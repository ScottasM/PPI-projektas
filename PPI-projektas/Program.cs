using Microsoft.EntityFrameworkCore;
using PPI_projektas.Utils;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EntityData>(options => options.UseMySql(ServerVersion.AutoDetect(connectionString)));

if(connectionString == null) {
    Console.WriteLine("connectionString is empty?!?!?!???!!!!!?!?!?");
}

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

var app = builder.Build();




using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    try {
        var context = services.GetRequiredService<EntityData>();
        context.Database.Migrate();
    }
    catch (Exception ex) {
        // Log error
    }
}



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
