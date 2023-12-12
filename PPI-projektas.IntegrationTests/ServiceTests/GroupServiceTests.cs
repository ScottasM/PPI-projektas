using PPI_projektas.Services;
using PPI_projektas.Services.Response;
using PPI_projektas.IntegrationTests.Mocks;

namespace PPI_projektas.IntegrationTests.ServiceTests
{
    [TestCaseOrderer(
    ordererTypeName: "PPI_projektas.IntegrationTests.PriorityOrderer",
    ordererAssemblyName: "PPI_projektas.IntegrationTests")]
    public class GroupServiceTests : GroupServiceSetup, IClassFixture<DatabaseFixture>
    {
        UserCreateData memberData;

        public GroupServiceTests()
        {
            memberData.Username = "member_username";
            memberData.Password = "member_password";
            memberData.Email = "member_email";
        }

        [Fact, TestPriority(2)]
        public void CreateGroupTest()
        {
            var ownerId = userService.CreateUser(userData);
            Assert.True(ownerId != Guid.Empty);

            List<Guid> memID = new List<Guid>();
            memID.Add(userService.CreateUser(memberData));
            IEnumerable<Guid> members = memID;
            Assert.NotNull(members);

            //CreateGroup() assigns id to the new group
            var groupId = groupService.CreateGroup(ownerId, "CreateGroupTest_group", members);
            Assert.True(groupId != Guid.Empty);
        }

        [Fact, TestPriority(3)]
        public void GetGroupsByOwnerTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            memID.Add(userService.CreateUser(memberData));
            IEnumerable<Guid> members = memID;

            groupService.CreateGroup(ownerId, "GetGroupsByOwnerTest_group", members);

            //GetGroupsByOwner() returns not empty list
            var groupList = groupService.GetGroupsByOwner(ownerId);
            Assert.NotNull(groupList);
            Assert.True(groupList.Any());
        }

        [Fact, TestPriority(4)]
        public void EditGroupTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            memID.Add(userService.CreateUser(memberData));
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "EditGroupTest_group", members);
            var groupList = groupService.GetGroupsByOwner(ownerId);

            //Edited group's id doesnt change
            groupService.EditGroup(groupId, "EditGroup_group", members, ownerId);
            ObjectDataItem editedGroup = groupList.Find(x => x.Id == groupId);
            Assert.NotNull(editedGroup);
            Assert.True(editedGroup.Id == groupId);
        }

        [Fact, TestPriority(4)]
        public void GetUsersInGroupTest()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            var memberId = userService.CreateUser(memberData);
            memID.Add(memberId);
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "GetUsersInGroupTest_group", members);

            //GetUsersInGroup() returns the correct lists of users
            var originalGroupUsers = groupService.GetUsersInGroup(groupId);
            Assert.Contains(new ObjectDataItem(memberId, memberData.Username), originalGroupUsers);
        }

        [Fact, TestPriority(4)]
        public void DeleteGroup()
        {
            var ownerId = userService.CreateUser(userData);

            List<Guid> memID = new List<Guid>();
            memID.Add(userService.CreateUser(memberData));
            IEnumerable<Guid> members = memID;

            var groupId = groupService.CreateGroup(ownerId, "DeleteGroup_group", members);

            //Testing DeleteGroup()
            groupService.DeleteGroup(groupId);
            List<ObjectDataItem> newgroupList = groupService.GetGroupsByOwner(ownerId);
            Assert.False(newgroupList.Exists(x => x.Id == groupId));
        }
    }
}
