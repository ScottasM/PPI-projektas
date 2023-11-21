namespace PPI_projektas.objects.Factories;

public interface INoteFactory
{
    Note Create(Guid authorId, Guid groupId);
}

public class NoteFactory : INoteFactory
{
    public Note Create(Guid authorId, Guid groupId)
    {
        return new Note(authorId, groupId);
    }
}