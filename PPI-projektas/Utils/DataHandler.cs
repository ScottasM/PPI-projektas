using PPI_projektas.objects;
using Microsoft.EntityFrameworkCore;
using System;
using PPI_projektas.objects.abstractions;

namespace PPI_projektas.Utils
{
    public class DataHandler
    {

        public static DataHandler Instance;

        public List<User> AllUsers = new();
        public List<Group> AllGroups = new();
        public List<Note> AllNotes = new();

        private SaveHandler _saveHandler;

        enum FileState
        {
            Ready,
            Saving,
            Reading
        }

        private FileState _state = FileState.Ready;

        private DbContextOptions<EntityData> options;

        public DataHandler(string connectionString)
        {
            #region Singleton
            if (Instance != null)
                Instance = null;
            Instance = this;
            #endregion

            _saveHandler = LazySingleton<SaveHandler>.Instance;



            _state = FileState.Reading;

            AllUsers = _saveHandler.LoadList<User>();
            AllNotes = _saveHandler.LoadList<Note>();
            AllGroups = _saveHandler.LoadList<Group>();


            // assign loaded guids to actual objects
          
            _state = FileState.Ready;

            SaveTimeout(15);
        }


        private async void SaveTimeout(int TimeoutSeconds)
        {
            while (true) {
                await Task.Delay(TimeoutSeconds * 1000);

                if(_state != FileState.Ready) { // dont save if we're reading from the files
                    continue;
                }

                _state = FileState.Saving;

                _saveHandler.Save();

                _state = FileState.Ready;
            }
        }

        public static void Create<T>(T obj) 
        {
            if (obj == null)
                return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Add(obje);
                Instance._saveHandler.SaveObject(obje);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Add(obje);
                Instance._saveHandler.SaveObject(obje);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Add(obje);
                Instance._saveHandler.SaveObject(obje);
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Remove(obje);
                Instance._saveHandler.RemoveObject(obje);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Remove(obje);
                Instance._saveHandler.RemoveObject(obje);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Remove(obje);
                Instance._saveHandler.RemoveObject(obje);
            }
        }

        public static bool userExists(string username)
        {
            return Instance.AllUsers.Any(inst => inst.GetUsername() == username);
        }

        public static User? userExistsObject(string username)
        {
            return Instance.AllUsers.Find(inst => inst.GetUsername() == username);
        }
    }
}
