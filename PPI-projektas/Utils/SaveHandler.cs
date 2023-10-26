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
            using (var context = new EntityData(options)) {
                context.Set<T>().AddRange(obj);
                context.SaveChanges();
            }
        }

        public List<T> LoadList<T>(DbContextOptions<EntityData> options) where T: class
        {
            using (var context = new EntityData(options)) {
                return context.Set<T>().ToList();
            }
        }
        
        public void SaveObject<T>(T obj,DbContextOptions<EntityData> options) where T : Entity
        {
            var list = LoadList<T>(options);
            if (list == null)
                return;

            var index = list.FindIndex(inst => inst.Id == obj.Id) ;
            list[index] = obj;

            SaveList(list, options);
        }
    }
}
