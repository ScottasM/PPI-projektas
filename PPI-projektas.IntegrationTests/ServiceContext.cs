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
        protected UserCreateData memberData;
        protected UserService userService;
        protected GroupService groupService;


        public ServiceContext()
        {
            userData.Username = "IntegrationTest_username";
            userData.Password = "IntegrationTest_password";
            userData.Email = "IntegrationTest_email";
           
            memberData.Username = "memeber_username";
            memberData.Password = "memeber_password";
            memberData.Email = "memeber_email";

            var testUser = new User(userData.Username, userData.Password, userData.Email);

            var mockODIF = new Mock<IObjectDataItemFactory>();
            var mockUF = new Mock<IUserFactory>();
            var mockGF = new Mock<IGroupFactory>();

            mockGF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<User>(), It.IsAny<List<User>>())).Returns((String a, User b, List<User> c) => new Group(a, b, c));
            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<String>())).Returns((Guid a, String b) => new ObjectDataItem(a, b));
            mockUF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(testUser);

            userService = new UserService(mockODIF.Object, mockUF.Object);
            groupService = new GroupService(mockODIF.Object, mockGF.Object);
        }
    }
}
