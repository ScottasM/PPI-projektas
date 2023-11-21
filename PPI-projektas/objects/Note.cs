using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Note : Entity, IComparable<Note>
{
	[JsonInclude] public Guid AuthorId;

	[JsonInclude] public Guid GroupId;
  
	public string Name { get; set; }

	public List<Tag> Tags { get; set; }
  
	[JsonInclude] public string Text;
	
	public Note () {} // For deserialization

	public Note(Guid authorId, Guid groupId, bool createGUID = true) : base(createGUID)
	{
		AuthorId = authorId;
		GroupId = groupId;
		Name = "";
		Tags = new List<Tag>();
		Text = "";
	}
  
    public int CompareTo(Note otherNote)
    {
        var tagComparison = Tags.Count.CompareTo(otherNote.Tags.Count);
        if (tagComparison != 0)
            return tagComparison;
        
        return String.Compare(Text, otherNote.Text, StringComparison.OrdinalIgnoreCase);
    }

    public bool ContainsAny(IEnumerable<string> tags)
    {
	    return tags.Any(tag => Tags.Contains(new Tag (tag)));
    }

    public bool ContainsAll(IEnumerable<string> tags)
    {
	    return tags.All(tag => Tags.Contains(new Tag(tag)));
    }	
}