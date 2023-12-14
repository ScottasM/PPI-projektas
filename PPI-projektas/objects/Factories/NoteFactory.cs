namespace PPI_projektas.objects.Factories;

public interface INoteFactory
{
    Note Create(Guid authorId);
}

public class NoteFactory : INoteFactory
{
    public Note Create(Guid authorId)
    {
        return new Note(authorId);
    }
}