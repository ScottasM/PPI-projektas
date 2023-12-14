using PPI_projektas.objects;
using PPI_projektas.Services.Response;
using PPI_projektas.Utils;

namespace PPI_projektas.Services.Request;

public class UpdatePrivilegesData
{
    public Guid UserId { get; set; }
    
    public IEnumerable<PrivilegeData> UpdatePrivileges { get; set; }

    public UpdatePrivilegesData(Guid userId, IEnumerable<PrivilegeData> updatePrivileges)
    {
        UserId = userId;
        UpdatePrivileges = updatePrivileges;
    }
}
