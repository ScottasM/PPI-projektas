using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Note : Entity, IComparable<Note>
{
	public string Name { get; set; }

	[JsonIgnore] public User Author;

	[JsonInclude] public Guid AuthorGuid;

  [JsonInclude] public string text;

	public List<string> Tags { get; set; } 

	[JsonInclude] public string Text;
	
	public Note () {} // For deserialization

	public Note(User author, bool createGuid = true) : base(createGuid)
  {
    Name = "";
		Author = author;
		AuthorGuid = author.Id;
		Tags = new List<string>();
		Text = "";
	}
  
    public int CompareTo(Note otherNote)
    {
        var tagComparison = Tags.Count.CompareTo(otherNote.Tags.Count);
        if (tagComparison != 0) {
            return tagComparison;
        }

        var authorComparison = String.Compare(Author.GetUsername(), otherNote.Author.GetUsername(), StringComparison.OrdinalIgnoreCase);
        if (authorComparison != 0) {
            return authorComparison;
        }
        var textComparison = String.Compare(Text, otherNote.Text, StringComparison.OrdinalIgnoreCase);
        if (textComparison != 0) {
            return textComparison;
        }

        return 0;
    }
}