using PPI_projektas.objects;

namespace PPI_projektas.Services.Request;

public class NoteSearchData
{
    public Guid UserId { get; set; }
    
    public Guid? GroupId { get; set; }
    
    public IEnumerable<string>? Tags { get; set; }
    
    public SearchType? SearchType { get; set; }
    
    public string? NameFilter { get; set; }

    public NoteSearchData(Guid userId, Guid? groupId = null, IEnumerable<string>? tags = null, SearchType? searchType = null, string? nameFilter = null)
    {
        UserId = userId;
        GroupId = groupId;
        Tags = tags;
        SearchType = searchType;
        NameFilter = nameFilter;
    }
}