using PPI_projektas.objects;

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

        public DataHandler()
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
            foreach (var note in AllNotes) note.Author = AllUsers.Find(inst => inst.Id == note.AuthorGuid);

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

                _saveHandler.SaveList(AllGroups);
                _saveHandler.SaveList(AllUsers);
                _saveHandler.SaveList(AllNotes);

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
                Instance._saveHandler.SaveList(Instance.AllGroups);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Add(obje);
                Instance._saveHandler.SaveList(Instance.AllUsers);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Add(obje);
                Instance._saveHandler.SaveList(Instance.AllNotes);
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance.AllGroups.Remove(obje);
                Instance._saveHandler.SaveList(Instance.AllGroups);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance.AllUsers.Remove(obje);
                Instance._saveHandler.SaveList(Instance.AllUsers);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance.AllNotes.Remove(obje);
                Instance._saveHandler.SaveList(Instance.AllNotes);
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
