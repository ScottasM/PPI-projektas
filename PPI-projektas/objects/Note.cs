using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Note : Entity
{
	public string Name { get; set; }

	[JsonIgnore] public User Author;

	[JsonInclude] public Guid AuthorGuid;

	public List<string> Tags { get; set; }
	
	[JsonInclude] public string Text;
	
	public Note () {} // For deserialization

	public Note(User author)
	{
		Name = "";
		Author = author;
		AuthorGuid = author.Id;
		Tags = new List<string>();
		Text = "";
	}
}