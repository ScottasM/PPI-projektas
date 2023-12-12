using PPI_projektas.objects;
using PPI_projektas.Utils;
using PPI_projektas.IntegrationTests.Mocks;

namespace PPI_projektas.IntegrationTests.ServiceTests
{
    [TestCaseOrderer(
    ordererTypeName: "PPI_projektas.IntegrationTests.PriorityOrderer",
    ordererAssemblyName: "PPI_projektas.IntegrationTests")]
    public class NoteServiceTests : NoteServiceSetup, IClassFixture<DatabaseFixture>
    {

        [Fact, TestPriority(5)]
        public void CreateNoteTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "CreateNoteTest_group", members);

            //CreateNote() correctly creates a note
            var noteId = noteService.CreateNote(groupId, ownerId);
            Assert.True(noteId != Guid.Empty);
        }

        [Fact, TestPriority(6)]
        public void GetNotesTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "GetNotesTest_group", members);

            //GetNotes() returns no notes for new groups
            var noteList = noteService.GetNotes(groupId);
            Assert.False(noteList.Any());

            //GetNotes() returns correct amount of notes
            noteService.CreateNote(groupId, ownerId);
            noteService.CreateNote(groupId, ownerId);
            noteList = noteService.GetNotes(groupId);
            Assert.True(noteList.Any());
            Assert.True(noteList.Count() == 2);
        }

        [Fact, TestPriority(7)]
        public void UpdateNoteTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "UpdateNoteTest_group", members);

            var noteId = noteService.CreateNote(groupId, ownerId);
            var name = "UpdateNoteTest_note";
            var tags = new List<EntityStrings>();
            var text = "UpdateNoteTesting";

            //UpdateNote() correctly updates note's attributes
            noteService.UpdateNote(noteId, ownerId, name, tags, text);
            var note = DataHandler.FindObjectById(noteService.GetNotes(groupId).First().Id, DataHandler.Instance.AllNotes);
            Assert.True(note.Name == name);
            Assert.True(note.Tags == tags);
            Assert.True(note.Text == text);

        }

        [Fact, TestPriority(7)]
        public void DeleteNoteTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "CreateNoteTest_group", members);
            var noteId = noteService.CreateNote(groupId, ownerId);

            //Testing DeleteNote() 
            noteService.DeleteNote(noteId, ownerId);
            var noteList = noteService.GetNotes(groupId);
            Assert.False(noteList.Any());
        }
    }
}
