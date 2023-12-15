using PPI_projektas.IntegrationTests.Mocks;

namespace PPI_projektas.IntegrationTests.ServiceTests
{
    public class AuthenticationServiceTests : AuthServiceSetup, IClassFixture<DatabaseFixture>
    {
        [Theory]
        [InlineData("username", "password")]
        [InlineData(" ", " ")]
        [InlineData("", "")]
        public void TryRegisterTest(string username, string password)
        {
            var authRezult = authService.TryRegister(username, password);

            Assert.NotNull(authRezult.User);
            Assert.True(authRezult.Success);
            Assert.True(authRezult.ErrorMessage == "");
        }
    }
}
