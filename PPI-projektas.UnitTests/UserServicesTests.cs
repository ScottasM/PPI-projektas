using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services;
using PPI_projektas.Services.Response;
using Moq;
using PPI_projektas.Utils;

namespace PPI_projektas.UnitTests
{
    public class UserServicesTests
    {
        DataHandler dataHandler = new DataHandler();

        [Fact]
        public void UserServicesTest()
        {
            UserCreateData userData = new UserCreateData();
            userData.Username = "TestData_username";
            userData.Password = "TestData_password";
            userData.Email = "TestData_email";

            User testUser = new User(userData.Username, userData.Password, userData.Email);

            var mockODIF = new Mock<IObjectDataItemFactory>();
            var mockUF = new Mock<IUserFactory>();

            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<String>())).Returns((Guid a, String b) => new ObjectDataItem(a, b));
            mockUF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(testUser);

            var service = new UserService(mockODIF.Object, mockUF.Object);

            //Testing ValidateData()
            Assert.False(service.ValidateData(""));
            Assert.False(service.ValidateData(new List<User>()));
            Assert.True(service.ValidateData(userData));

            var newUserId = service.CreateUser(userData);

            //Testing CreateUser()
            Assert.True(newUserId != Guid.Empty);
            Assert.True(newUserId == testUser.Id);

            var userList = service.GetUsersByName(testUser.Username);

            //Testing GetUsersByName()
            Assert.NotNull(userList);
            Assert.True(userList.Any());
            Assert.True(userList.Exists(x => x.Id == testUser.Id));

            service.DeleteUser(newUserId);
            userList = service.GetUsersByName(testUser.Username);

            //Testing DeleteUser()
            Assert.False(userList.Exists(x => x.Id == testUser.Id));
        }
    }
}