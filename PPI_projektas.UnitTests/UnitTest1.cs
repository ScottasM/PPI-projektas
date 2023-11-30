using PPI_projektas.objects;
using System.Collections;
using Group = PPI_projektas.objects.Group;

namespace PPI_projektas.UnitTests
{
    public class UnitTest1
    {
        [Theory]
        [ClassData(typeof(CompareGroupParameters))]
        public void GroupCompareTo(Group groupForCompare, IntResult expectedResult)
        {
            var group = new Group("unitTest_group0", new User(), new List<User>());

            var result = group.CompareTo(groupForCompare);
            Assert.Equal(expectedResult.result, result);
        }

        [Theory]
        [ClassData(typeof(CompareNoteParameters))]
        public void NoteCompareTo(Note noteForCompare, IntResult expectedResult)
        {
            var note = new Note(new Guid());
            note.Text = "unitTest_noteText0";
            note.Tags = new List<EntityStrings>();

            var result = note.CompareTo(noteForCompare);
            Assert.Equal(expectedResult.result, result);
        }
    }
    public class IntResult
    {
        public int result;
    }
    public class CompareGroupParameters : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Group { Name = "unitTest_group0" },
                new IntResult { result = 0 },
            };
            yield return new object[]
{
                new Group { Name = "unitTest_group1" },
                new IntResult { result = -1 },
};
            yield return new object[]
            {
                new Group { Name = "otherGroupName" },
                new IntResult { result = 1 },
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)GetEnumerator();
    }
    public class CompareNoteParameters : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Note { Text = "unitTest_noteText0", Tags = new List<EntityStrings>() },
                new IntResult { result = 0 },
            };
            yield return new object[]
            {
                new Note { Text = "unitTest_NOTETEXT0", Tags = new List<EntityStrings>() },
                new IntResult { result = 0 },
            };
            yield return new object[]
            {
                new Note { Text = "unitTest_noteText1", Tags = new List <EntityStrings>()},
                new IntResult { result = -1 },
            };
            yield return new object[]
            {
                new Note { Text = "someOtherNote", Tags = new List <EntityStrings>()},
                new IntResult { result = 2 },
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)GetEnumerator();
    }


}