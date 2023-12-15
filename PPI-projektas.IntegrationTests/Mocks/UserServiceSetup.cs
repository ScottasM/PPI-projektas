using Moq;
using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services;
using PPI_projektas.Services.Response;

namespace PPI_projektas.IntegrationTests.Mocks
{
    public class UserServiceSetup
    {
        protected UserCreateData userData;
        protected UserService userService;
        public UserServiceSetup() {

            userData.Username = "IntegrationTest_username";
            userData.Password = "IntegrationTest_password";
            userData.Email = "IntegrationTest_email";

            var mockODIF = new Mock<IObjectDataItemFactory>();
            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>())).Returns((Guid a, string b) => new ObjectDataItem(a, b));

            var mockUF = new Mock<IUserFactory>();
            mockUF.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b, string c) => new User(a, b, c));

            userService = new UserService(mockODIF.Object, mockUF.Object);
        }
    }
}
