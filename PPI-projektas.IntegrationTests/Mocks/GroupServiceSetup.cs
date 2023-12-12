using Moq;
using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Response;
using PPI_projektas.Services;

namespace PPI_projektas.IntegrationTests.Mocks
{
    public class GroupServiceSetup : UserServiceSetup
    {
        protected GroupService groupService;
        public GroupServiceSetup()
        {
            var mockODIF = new Mock<IObjectDataItemFactory>();
            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>())).Returns((Guid a, string b) => new ObjectDataItem(a, b));

            var mockGF = new Mock<IGroupFactory>();
            mockGF.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<List<User>>())).Returns((string a, User b, List<User> c) => new Group(a, b, c));

            groupService = new GroupService(mockODIF.Object, mockGF.Object);
        }
    }
}
