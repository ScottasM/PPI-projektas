using Moq;
using PPI_projektas.objects;
using PPI_projektas.objects.Factories;
using PPI_projektas.Services.Response;
using PPI_projektas.Services;

namespace PPI_projektas.IntegrationTests.Mocks
{
    public class NoteServiceSetup : GroupServiceSetup
    {
        protected NoteService noteService;
        public NoteServiceSetup()
        {
            var mockODIF = new Mock<IObjectDataItemFactory>();
            mockODIF.Setup(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>())).Returns((Guid a, string b) => new ObjectDataItem(a, b));

            var mockONDF = new Mock<INoteDataFactory>();
            mockONDF.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<List<EntityStrings>>(), It.IsAny<string>())).Returns((string a, List<EntityStrings> b, string c) => new OpenedNoteData(a, b, c));

            var mockNF = new Mock<INoteFactory>();
            mockNF.Setup(x => x.Create(It.IsAny<Guid>())).Returns((Guid a) => new Note(a));

            noteService = new NoteService(mockODIF.Object, mockONDF.Object, mockNF.Object);
        }
    }
}