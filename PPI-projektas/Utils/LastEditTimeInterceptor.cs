using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PPI_projektas.objects;

namespace PPI_projektas.Utils
{
    public class LastEditTimeInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            var entries = context.ChangeTracker.Entries<Note>();

            foreach (var entry in entries) {
                if (entry.State == EntityState.Modified) {
                    entry.Entity.LastEditTime = DateTime.Now;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
