using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Note : Entity, IComparable<Note>
{
	[JsonInclude] public Guid AuthorId;
  
	public string Name { get; set; }

	public List<string> Tags { get; set; }
  
	[JsonInclude] public string Text;
	
	public Note () {} // For deserialization

	public Note(Guid authorId, bool createGUID = true) : base(createGUID)
	{
		AuthorId = authorId;
		Name = "";
		Tags = new List<string>();
		Text = "";
	}
  
    public int CompareTo(Note otherNote)
    {
        var tagComparison = Tags.Count.CompareTo(otherNote.Tags.Count);
        if (tagComparison != 0)
            return tagComparison;
        
        return String.Compare(Text, otherNote.Text, StringComparison.OrdinalIgnoreCase);
    }
}