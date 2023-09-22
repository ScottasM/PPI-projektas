using PPI_projektas.objects;

namespace PPI_projektas.Utils
{
    public class DataHandler
    {
        public static DataHandler Instance;

        private List<User> allUsers = new List<User>();
        private List<Group> allGroups = new List<Group>();
        private List<Note> allNotes = new List<Note>();

        private SaveHandler saveHandler;

        public DataHandler()
        {
            #region Singleton
            if (Instance != null)
                Instance = null;
            Instance = this;
            #endregion

            saveHandler = LazySingleton<SaveHandler>.Instance;

            allUsers = saveHandler.LoadList<User>();
            allNotes = saveHandler.LoadList<Note>();
            allGroups = saveHandler.LoadList<Group>();


            // assign loaded guids to actual objects
            foreach(Group group in allGroups) {
                group.Owner = allUsers.Find(inst => inst.Id == group.OwnerGuid);

                foreach (Guid guid in group.MembersGuid) group.AddUser(allUsers.Find(inst => inst.Id == guid));
                foreach (Guid guid in group.NotesGuid) group.AddNote(allNotes.Find(inst => inst.Id == guid));
            }
            foreach(User user in allUsers) {
                foreach (Guid guid in user.CreatedNotesGuids) user.AddCreatedNote(allNotes.Find(inst => inst.Id == guid));
                foreach (Guid guid in user.FavoriteNotesGuids) user.AddCreatedNote(allNotes.Find(inst => inst.Id == guid));
                foreach (Guid guid in user.GroupsGuids) user.AddGroup(allGroups.Find(inst => inst.Id == guid));
            }
            foreach (Note note in allNotes) note.Author = allUsers.Find(inst => inst.Id == note.AuthorGuid);

            SaveTimeout(15);
        }


        private async void SaveTimeout(int TimeoutSeconds)
        {
            while (true) {
                await Task.Delay(TimeoutSeconds * 1000);
                saveHandler.SaveList(allGroups);
                saveHandler.SaveList(allUsers);
                saveHandler.SaveList(allGroups);
            }

        }

        public void Update<T>(T obj) 
        {
            
        }

    }
}
