namespace PPI_projektas.objects;

public class Tag : IEquatable<Tag>
{
    public readonly string Text;
    
    public string Color { get; set; }
    
    public Tag () {} // For deserialization
    
    public Tag(string text)
    {
        Text = text;
        Color = String.Empty;
    }

    public Tag(string text, string color)
    {
        Text = text;
        Color = color;
    }
    
    public bool Equals(Tag? tag)
    {
        return tag != null && Text == tag.Text;
    }
}