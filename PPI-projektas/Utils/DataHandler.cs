using PPI_projektas.objects;

namespace PPI_projektas.Utils
{
    public class DataHandler
    {
        public static DataHandler Instance;

        public List<User> _allUsers = new List<User>();
        public List<Group> _allGroups = new List<Group>();
        public List<Note> _allNotes = new List<Note>();

        private SaveHandler _saveHandler;

        public DataHandler()
        {
            #region Singleton
            if (Instance != null)
                Instance = null;
            Instance = this;
            #endregion

            _saveHandler = LazySingleton<SaveHandler>.Instance;

            _allUsers = _saveHandler.LoadList<User>();
            _allNotes = _saveHandler.LoadList<Note>();
            _allGroups = _saveHandler.LoadList<Group>();


            // assign loaded guids to actual objects
            foreach(Group group in _allGroups) {
                group.Owner = _allUsers.Find(inst => inst.Id == group.OwnerGuid);

                foreach (Guid guid in group.MembersGuid) group.AddUser(_allUsers.Find(inst => inst.Id == guid));
                foreach (Guid guid in group.NotesGuid) group.AddNote(_allNotes.Find(inst => inst.Id == guid));
            }
            foreach(User user in _allUsers) {
                foreach (Guid guid in user.CreatedNotesGuids) user.AddCreatedNote(_allNotes.Find(inst => inst.Id == guid));
                foreach (Guid guid in user.FavoriteNotesGuids) user.AddCreatedNote(_allNotes.Find(inst => inst.Id == guid));
                foreach (Guid guid in user.GroupsGuids) user.AddGroup(_allGroups.Find(inst => inst.Id == guid));
            }
            foreach (Note note in _allNotes) note.Author = _allUsers.Find(inst => inst.Id == note.AuthorGuid);

            SaveTimeout(15);
        }


        private async void SaveTimeout(int TimeoutSeconds)
        {
            while (true) {
                await Task.Delay(TimeoutSeconds * 1000);
                _saveHandler.SaveList(_allGroups);
                _saveHandler.SaveList(_allUsers);
                _saveHandler.SaveList(_allGroups);
            }

        }

        public static void Create<T>(T obj) // not pretty, but should work
        {
            if (obj == null)
                return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance._allGroups.Add(obje);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance._allUsers.Add(obje);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance._allNotes.Add(obje);
            }
        }

        public static void Delete<T>(T obj)
        {
            if(obj == null) return;

            if (obj is Group) {
                var obje = obj as Group;
                Instance._allGroups.Remove(obje);
            }
            else if (obj is User) {
                var obje = obj as User;
                Instance._allUsers.Remove(obje);
            }
            else if (obj is Note) {
                var obje = obj as Note;
                Instance._allNotes.Remove(obje);
            }
        }
    }
}
