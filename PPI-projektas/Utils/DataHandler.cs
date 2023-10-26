using PPI_projektas.objects;
using Microsoft.EntityFrameworkCore;
using System;

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

            var optionsBuilder = new DbContextOptionsBuilder<EntityData>();
            options = optionsBuilder.UseNpgsql(connectionString).Options;

            _state = FileState.Reading;

            AllUsers = _saveHandler.LoadList<User>(options);
            AllNotes = _saveHandler.LoadList<Note>(options);
            AllGroups = _saveHandler.LoadList<Group>(options);


            // assign loaded guids to actual objects
            foreach(var group in AllGroups) {
                group.Owner = AllUsers.Find(inst => inst.Id == group.OwnerGuid);

                group.LoadMembers();
                group.LoadNotes();
            }
            foreach(var user in AllUsers) {
                user.LoadCreatedNotes();
                user.LoadFavoriteNotes();
                user.LoadGroups();
            }
          
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

                _saveHandler.SaveList(AllGroups,options);
                _saveHandler.SaveList(AllUsers, options);
                _saveHandler.SaveList(AllNotes, options);

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
                Instance._saveHandler.SaveList(Instance.AllGroups, Instance.options);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Add(obje);
                Instance._saveHandler.SaveList(Instance.AllUsers, Instance.options);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Add(obje);
                Instance._saveHandler.SaveList(Instance.AllNotes, Instance.options);
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Remove(obje);
                Instance._saveHandler.SaveList(Instance.AllGroups, Instance.options);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Remove(obje);
                Instance._saveHandler.SaveList(Instance.AllUsers, Instance.options);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Remove(obje);
                Instance._saveHandler.SaveList(Instance.AllNotes, Instance.options);
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
