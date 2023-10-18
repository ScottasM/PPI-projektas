using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace PPI_projektas.objects;

public class Note : Entity, IComparable<Note>
{
	public string Name { get; set; }

	[JsonIgnore] public User Author;

	[JsonInclude] public Guid AuthorGuid;
  
	public List<string> Tags { get; set; }
	
	[JsonInclude] public string Text;
	
	public Note () {} // For deserialization

	public Note(User author, bool createGUID = true) : base(createGUID)
  {
    Name = "";
		Author = author;
		AuthorGuid = author.Id;
		Tags = new List<string>();
		Text = "";
	}
  
    public int CompareTo(Note otherNote)
    {
        int tagComparison = Tags.Count.CompareTo(otherNote.Tags.Count);
        if (tagComparison != 0) {
            return tagComparison;
        }

        int authorComparison = String.Compare(Author.GetUsername(), otherNote.Author.GetUsername(), StringComparison.OrdinalIgnoreCase);
        if (authorComparison != 0) {
            return authorComparison;
        }
        int textComparison = String.Compare(Text, otherNote.Text, StringComparison.OrdinalIgnoreCase);
        if (textComparison != 0) {
            return textComparison;
        }

        return 0;
    }
}