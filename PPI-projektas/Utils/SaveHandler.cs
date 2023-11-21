using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PPI_projektas.objects;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public class SaveHandler
    {

        

        private Dictionary<Type, string> _filePaths = new Dictionary<Type, string>(3) {
            {typeof(User),"Users.json"},
            {typeof(Group),"Groups.json"},
            {typeof(Note),"Notes.json"}
        };

        private string? SerializeList<T>(List<T> obj)
        {
            try {
                return JsonSerializer.Serialize(obj);
            }
            catch(Exception err) {
                Console.WriteLine($"Error while serializing a list : {err.Message}");
                return null;
            }
            
        }

        public void SaveList<T>(List<T> obj, DbContextOptions<EntityData> options) where T: class
        {
            using (var context = new EntityData()) {
                context.Set<T>().RemoveRange(context.Set<T>());

                // Add the new entities to the DbSet
                context.Set<T>().AddRange(obj);
                context.SaveChanges();
            }
        }

        public List<T> LoadList<T>(DbContextOptions<EntityData> options) where T: class
        {
            using (var context = new EntityData()) {
                return context.Set<T>().ToList();
            }
        }
        
        public void SaveObject<T>(T obj,DbContextOptions<EntityData> options) where T : Entity
        {
            using (var context = new EntityData()) {
                context.Set<T>().Add(obj);
                context.SaveChanges();
            }
        }
    }
}
