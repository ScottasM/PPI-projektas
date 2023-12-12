using Moq;
using PPI_projektas.objects.Factories;
using PPI_projektas.objects;
using PPI_projektas.Services;
using PPI_projektas.Services.Response;

namespace PPI_projektas.IntegrationTests
{
    public class ServiceContext
    {
        protected UserCreateData userData;
        protected UserService userService;
        protected GroupService groupService;
        protected NoteService noteService;

        public ServiceContext()
        {
            userData.Username = "IntegrationTest_username";
            userData.Password = "IntegrationTest_password";
            userData.Email = "IntegrationTest_email";

            var mockODIF = new Mock<IObjectDataItemFactory>();
            var mockUF = new Mock<IUserFactory>();
            var mockGF = new Mock<IGroupFactory>();
            var mockONDF = new Mock<IOpenedNoteDataFactory>();
            var mockNF = new Mock<INoteFactory>();
            
            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<String>())).Returns((Guid a, String b) => new ObjectDataItem(a, b));
            mockUF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns((String a, String b, String c) => new User(a, b, c));
            mockGF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<User>(), It.IsAny<List<User>>())).Returns((String a, User b, List<User> c) => new Group(a, b, c));
            mockONDF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<List<EntityStrings>>(), It.IsAny<String>())).Returns((String a, List<EntityStrings> b, String c) => new OpenedNoteData(a, b, c));
            mockNF.Setup(x => x.Create(It.IsAny<Guid>())).Returns((Guid a) => new Note(a));

            userService = new UserService(mockODIF.Object, mockUF.Object);
            groupService = new GroupService(mockODIF.Object, mockGF.Object);
            noteService = new NoteService(mockODIF.Object, mockONDF.Object, mockNF.Object);
        }
    }
}
