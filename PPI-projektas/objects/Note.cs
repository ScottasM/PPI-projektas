using PPI_projektas.objects.abstractions;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Note : Entity
{
	[JsonIgnore] public User Author;

	[JsonInclude] public Guid AuthorGuid;


	public List<string> Tags { get; set; }
	
	public Note(User author)
	{
		Author = author;
		AuthorGuid = author.Id;
		Tags = new List<string>();
	}

	
	
}