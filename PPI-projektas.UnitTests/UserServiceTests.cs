using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services;
using PPI_projektas.Services.Response;
using Moq;
using PPI_projektas.Utils;

namespace PPI_projektas.UnitTests
{
    [TestCaseOrderer(
    ordererTypeName: "PPI_projektas.UnitTests.PriorityOrderer",
    ordererAssemblyName: "PPI_projektas.UnitTests")]
    public class UserServiceTests
    {
        DataHandler dataHandler = new DataHandler();
        UserCreateData userData;
        UserService userService;
        List<ObjectDataItem>? userList;

        public UserServiceTests() 
        {
            userData.Username = "TestData_username";
            userData.Password = "TestData_password";
            userData.Email = "TestData_email";

            var testUser = new User(userData.Username, userData.Password, userData.Email);

            var mockODIF = new Mock<IObjectDataItemFactory>();
            var mockUF = new Mock<IUserFactory>();

            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<String>())).Returns((Guid a, String b) => new ObjectDataItem(a, b));
            mockUF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(testUser);
            
            userService = new UserService(mockODIF.Object, mockUF.Object);

        }

        [Fact, TestPriority(0)]
        public void ValidateDataTest()
        {
            //Testing ValidateData()
            Assert.False(userService.ValidateData(""));
            Assert.False(userService.ValidateData(new List<User>()));
            Assert.True(userService.ValidateData(userData));
        }

        [Fact, TestPriority(1)]
        public void CreateUserTest()
        {
            var newUserId = userService.CreateUser(userData);

            //Testing CreateUser()
            Assert.True(newUserId != Guid.Empty);
        }

        [Fact, TestPriority(2)]
        public void GetUsersByNameTest()
        {
            var testUserId = userService.CreateUser(userData);
            userList = userService.GetUsersByName(userData.Username);

            //Testing GetUsersByName()
            Assert.NotNull(userList);
            Assert.True(userList.Any());
            Assert.True(userList.Exists(x => x.Id == testUserId));
        }

        [Fact, TestPriority(3)]
        public void GetGroupsFromUser() 
        {
            var testUserId = userService.CreateUser(userData);
            List<ObjectDataItem> groupList = userService.GetGroupsFromUser(testUserId);

            //Testing GetGroupsFromUser();
            Assert.NotNull(groupList);
        }

        [Fact, TestPriority(4)]
        public void DeleteUserTest()
        {
            var testUserId = userService.CreateUser(userData);
            userService.DeleteUser(testUserId);
            userList = userService.GetUsersByName(userData.Username);

            //Testing DeleteUser()
            Assert.False(userList.Exists(x => x.Id == testUserId));
            dataHandler.AllUsers.Clear();
        }
    }
}