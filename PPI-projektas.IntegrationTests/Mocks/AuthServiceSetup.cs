using Moq;
using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Response;
using PPI_projektas.Services;

namespace PPI_projektas.IntegrationTests.Mocks
{
    public class AuthServiceSetup
    {
        protected AuthenticationService authService;
        public AuthServiceSetup()
        {
            var mockARF = new Mock<IAuthReturnFactory>();
            mockARF.Setup(x => x.Create(It.IsAny<User>(), It.IsAny<bool>(), It.IsAny<String>())).Returns((User a, bool b, String c) => new AuthReturn(a, b, c));

            var mockUF = new Mock<IUserFactory>();
            mockUF.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => new User(a, b));

            authService = new AuthenticationService(mockARF.Object, mockUF.Object);
        }
    }
}