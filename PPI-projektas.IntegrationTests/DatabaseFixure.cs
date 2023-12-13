using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PPI_projektas.Utils;

namespace PPI_projektas.IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var builder = WebApplication.CreateBuilder();
            var connectionString = "server=185.34.52.6;user=NotesApp;password=AlioValioIrInternetas;database=NotesApp";
            var serverVersion = MariaDbServerVersion.AutoDetect(connectionString);

            DataHandler dataHandler = new DataHandler(connectionString);

            builder.Services.AddDbContext<EntityData>(
                dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion)
            );
        }

        public void Dispose()
        {
            
        }
    }
}
