using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Note : Entity
{
	[JsonIgnore] public User Author;

	[JsonInclude] public Guid AuthorGuid;

	public string Name { get; set; }

	public List<string> Tags { get; set; }
	
	public Note () {} // For deserialization
	
	public Note(User author)
	{
		Name = "";
		Author = author;
		AuthorGuid = author.Id;
		Tags = new List<string>();
	}

	
	
}