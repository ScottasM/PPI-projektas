using PPI_projektas.objects;

namespace PPI_projektas.Services.Request;

public class NoteSearchData
{
    public Guid UserId { get; set; }
    
    public SearchType SearchType { get; set; }
    
    public string? TagFilter { get; set; }
    
    public string? NameFilter { get; set; }
    
    public Guid? GroupId { get; set; }

    public NoteSearchData(Guid userId, SearchType searchType, string tagFilter, string nameFilter, Guid? groupId = null)
    {
        UserId = userId;
        TagFilter = tagFilter;
        NameFilter = nameFilter;
        GroupId = groupId;
        SearchType = searchType;
    }
}