using Moq;
using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services;
using PPI_projektas.Services.Response;

namespace PPI_projektas.IntegrationTests
{
    public class GroupServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _factory;

        public GroupServiceTests(DatabaseFixture factory)
        {
            _factory = factory;
        }

        [Fact, TestPriority(5)]
        public void GroupServiceTest() 
        {

            UserCreateData userData = new UserCreateData();
            userData.Username = "IntegrationTest_username";
            userData.Password = "IntegrationTest_password";
            userData.Email = "IntegrationTest_email";
            var testUser = new User(userData.Username, userData.Password, userData.Email);

            var mockODIF = new Mock<IObjectDataItemFactory>();
            var mockUF = new Mock<IUserFactory>();
            var mockGF = new Mock<IGroupFactory>();

            mockGF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<User>(), It.IsAny<List<User>>())).Returns((String a, User b, List<User> c) => new Group(a, b, c));
            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<String>())).Returns((Guid a, String b) => new ObjectDataItem(a, b));
            mockUF.Setup(x => x.Create(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(testUser);

            var userService = new UserService(mockODIF.Object, mockUF.Object);
            var groupService = new GroupService(mockODIF.Object, mockGF.Object);

            var ownerId = userService.CreateUser(userData);
            Assert.True(ownerId != Guid.Empty);

            List<Guid> memID = new List<Guid>();
            memID.Add(userService.CreateUser(userData));
            memID.Add(userService.CreateUser(userData));
            IEnumerable<Guid> members = memID;


            //CreateGroup() assigns id to the new group
            var groupId = groupService.CreateGroup(ownerId, "CreateGroupTest_group", members);
            Assert.True(groupId != Guid.Empty);


            //GetGroupsByOwner() returns not empty list
            var groupList = groupService.GetGroupsByOwner(ownerId);
            Assert.NotNull(groupList);
            Assert.True(groupList.Any());
            

            //Edited group's id doesnt change
            groupService.EditGroup(groupId, "EditGroup_group", members, ownerId);
            var editedGroup = groupList.Find(x => x.Id == groupId);
            Assert.NotNull(editedGroup);


            //GetUsersInGroup() returns the correct lists of users
            var originalGroupUsers = groupService.GetUsersInGroup(groupId);
            var editedGroupUsers = groupService.GetUsersInGroup(editedGroup.Id);
            Assert.True(originalGroupUsers.Any());
            Assert.True(editedGroupUsers.Any());
            Assert.True(originalGroupUsers.All(editedGroupUsers.Contains));


            //Testing DeleteGroup()
            groupService.DeleteGroup(groupId);
            List<ObjectDataItem> newgroupList = groupService.GetGroupsByOwner(ownerId);
            Assert.False(newgroupList.Exists(x => x.Id == groupId));
        }
    }
}
