using PPI_projektas.objects;

namespace PPI_projektas.Utils
{
    public class UpdateSyncing
    {
        private List<User> allUsers = new List<User>();
        private List<Group> allGroups = new List<Group>();
        private List<Note> allNotes = new List<Note>();

        private SaveHandler saveHandler;

        public UpdateSyncing()
        {
            saveHandler = LazySingleton<SaveHandler>.Instance;

            allUsers = saveHandler.LoadList<User>();
            allNotes = saveHandler.LoadList<Note>();
            allGroups = saveHandler.LoadList<Group>();


            // assign loaded guids to actual objects
            foreach(Group group in allGroups) {

                group.Owner = allUsers.Find(inst => inst.Id == group.OwnerGuid);

                foreach(Guid guid in group.MembersGuid) group.AddUser(allUsers.Find(inst => inst.Id == guid));
                
                foreach(Guid guid in group.NotesGuid) group.AddNote(allNotes.Find(inst => inst.Id == guid))
            }
        }

        





    }
}
