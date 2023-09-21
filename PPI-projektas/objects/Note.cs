using PPI_projektas.objects.abstractions;

namespace PPI_projektas.objects;

public class Note : Entity
{
	public readonly User Author;

	public List<string> Tags { get; set; }
	
	public Note(User author)
	{
		Author = author;
		Tags = new List<string>();
	}
	
}