using PPI_projektas.objects.abstractions;
using PPI_projektas.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PPI_projektas.objects;

public class Tag : Entity
{
    public string Value { get; set; } = null!;

    public Tag(string value)
    {
        Value = value;
    }
}

public class Note : Entity, IComparable<Note>
{

    public Guid UserId;
    public User User;

    public List<User> FavoriteByUsers { get; set; }
    public Group Group { get; set; }

    public string Name { get; set; }
    public List<Tag> Tags { get; set; }
  
    public string Text { get; set; }
    public DateTime LastEditTime { get; set; }

    public Note () {} // For deserialization

    public Note(Guid authorId, Guid groupId, bool createGUID = true) : base(createGUID)
    {

        UserId = authorId;
        Name = "";
        Tags = new List<Tag>();
        Text = "";
        Group = DataHandler.FindObjectById(groupId, DataHandler.Instance.AllGroups);
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