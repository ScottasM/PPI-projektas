using PPI_projektas.objects.abstractions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class EntityStrings
{
    [Key]
    public int Id { get; set; }
    public string value { get; set; } = null!;
}

public class Note : Entity, IComparable<Note>
{
	public Guid UserId;
	public User Author;
	public List<User> EditingPriveleges;
	public List<User> AdminstratorPrivileges;

    public List<User> FavoriteByUsers;
    public Group Group;


    public string Name { get; set; }
	public List<EntityStrings> Tags { get; set; }
  
	public string Text { get; set; }
	
	public Note () {} // For deserialization

	public Note(Guid authorId, bool createGUID = true) : base(createGUID)
	{
        UserId = authorId;
		Name = "";
		Tags = new List<EntityStrings>();
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