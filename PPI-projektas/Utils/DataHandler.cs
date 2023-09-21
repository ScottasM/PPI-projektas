﻿using System.Text.Json;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public static class DataHandler
    {

        private static Dictionary<Type, string> filePaths = new Dictionary<Type, string>(3) {
            {typeof(User),"Users.txt"},
            {typeof(Group),"Groups.txt"},
            {typeof(Note),"Notes.txt"}
        };
        
        private static string? SerializeList<T>(List<T> obj)
        {
            try {
                return JsonSerializer.Serialize(obj);
            }
            catch(Exception err) {
                Console.WriteLine($"Error while serializing a list : {err.Message}");
                return null;
            }
            
        }

        public static void SaveList<T>(List<T> obj)
        {
            string? sr = SerializeList(obj);
            if (sr == null)
                return;

            try {
                File.WriteAllText(filePaths[obj.GetType()], sr);
            }
            catch (Exception err) {
                Console.WriteLine($"Failed to save file : {err.Message}");
            }
        }

        public static List<T> LoadList<T>()
        {
            if (File.Exists(filePaths[typeof(T)])) {
                var json = File.ReadAllText(filePaths[typeof(T)]);
                var list = JsonSerializer.Deserialize<List<T>>(json);

                if (list != null)
                    return list;
            }

            return new List<T>();
        }
        
        public static void SaveObject<T>(T obj) where T : Entity
        {
            List<T> list = LoadList<T>();
            if (list == null)
                return;

            T? tempObj = list.Find(inst => inst.Id == obj.Id);
        }
    }
}