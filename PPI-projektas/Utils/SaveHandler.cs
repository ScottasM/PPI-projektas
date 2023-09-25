using System.Text.Json;
using PPI_projektas.objects;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public class SaveHandler
    {
        private Dictionary<Type, string> _filePaths = new Dictionary<Type, string>(3) {
            {typeof(User),"Users.txt"},
            {typeof(Group),"Groups.txt"},
            {typeof(Note),"Notes.txt"}
        };



        
        private string? SerializeList<T>(List<T> obj)
        {
            try {
                Group grp = new Group();
                DataHandler.Create(grp);
                return JsonSerializer.Serialize(obj);
            }
            catch(Exception err) {
                Console.WriteLine($"Error while serializing a list : {err.Message}");
                return null;
            }
            
        }

        public void SaveList<T>(List<T> obj)
        {
            string? sr = SerializeList(obj);
            if (sr == null)
                return;

            try {
                File.WriteAllText(_filePaths[obj.GetType()], sr);
            }
            catch (Exception err) {
                Console.WriteLine($"Failed to save file : {err.Message}");
            }
        }

        public List<T> LoadList<T>()
        {
            if (File.Exists(_filePaths[typeof(T)])) {
                var json = File.ReadAllText(_filePaths[typeof(T)]);
                var list = JsonSerializer.Deserialize<List<T>>(json);

                if (list != null)
                    return list;
            }

            return new List<T>();
        }
        
        public void SaveObject<T>(T obj) where T : Entity
        {
            List<T> list = LoadList<T>();
            if (list == null)
                return;

            int index = list.FindIndex(inst => inst.Id == obj.Id) ;
            list[index] = obj;

            SaveList(list);
        }
    }
}
