using PPI_projektas.objects.abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class EntityString
{
	[Key]
	public readonly Guid Id;
    public string Value { get; set; } = null!;

    public EntityString(string value)
    {
	    Id = Guid.NewGuid();
	    Value = value;
    }
}

public class Note : Entity, IComparable<Note>
{

	public Guid UserId;
	public User User;

    public List<User> FavoriteByUsers;
    public Group Group;


    public string Name { get; set; }
	public List<EntityString> Tags { get; set; }
  
	public string Text { get; set; }
	
	public Note () {} // For deserialization

	public Note(Guid authorId, Guid groupId, bool createGUID = true) : base(createGUID)
	{

        UserId = authorId;
		Name = "";
		Tags = new List<EntityString>();
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
	    return Tags.Any(tag => tags.Contains(tag.Value));
    }

    public bool ContainsAll(IEnumerable<string> tags)
    {
	    return Tags.Count(tag => tags.Contains(tag.Value)) == tags.Count();
    }	
}