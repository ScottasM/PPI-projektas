namespace PPI_projektas.objects;

public class Tag : IEquatable<Tag>
{
    public readonly string Text;
    
    public string Color { get; set; }
    
    public Tag(string text)
    {
        Text = text;
    }
    
    public bool Equals(Tag? tag)
    {
        return tag != null && Text == tag.Text;
    }
}