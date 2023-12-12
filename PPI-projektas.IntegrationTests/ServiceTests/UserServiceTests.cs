using PPI_projektas.objects;
using PPI_projektas.Services.Response;

namespace PPI_projektas.IntegrationTests.ServiceTests
{
    [TestCaseOrderer(
    ordererTypeName: "PPI_projektas.IntegrationTests.PriorityOrderer",
    ordererAssemblyName: "PPI_projektas.IntegrationTests")]
    public class UserServiceTests : ServiceContext, IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _factory;

        public UserServiceTests(DatabaseFixture factory)
        {
            _factory = factory;
        }

        [Fact, TestPriority(0)]
        public void ValidateDataTest()
        {
            //Testing ValidateData()
            Assert.False(userService.ValidateData(""));
            Assert.False(userService.ValidateData(new List<User>()));
            Assert.True(userService.ValidateData(userData));
        }

        [Fact, TestPriority(0)]
        public void CreateUserTest()
        {
            var newUserId = userService.CreateUser(userData);

            //Testing CreateUser()
            Assert.True(newUserId != Guid.Empty);
        }

        [Fact, TestPriority(1)]
        public void GetUsersByNameTest()
        {
            var testUserId = userService.CreateUser(userData);
            List<ObjectDataItem> userList = userService.GetUsersByName(userData.Username);

            //Testing GetUsersByName()
            Assert.NotNull(userList);
            Assert.True(userList.Any());
            Assert.True(userList.Exists(x => x.Id == testUserId));
        }

        [Fact, TestPriority(1)]
        public void GetGroupsFromUser()
        {
            var testUserId = userService.CreateUser(userData);
            List<ObjectDataItem> groupList = userService.GetGroupsFromUser(testUserId);

            //Testing GetGroupsFromUser();
            Assert.NotNull(groupList);
        }

        [Fact, TestPriority(1)]
        public void DeleteUserTest()
        {
            var testUserId = userService.CreateUser(userData);
            userService.DeleteUser(testUserId);
            List<ObjectDataItem> userList = userService.GetUsersByName(userData.Username);

            //Testing DeleteUser()
            Assert.False(userList.Exists(x => x.Id == testUserId));
        }
    }
}