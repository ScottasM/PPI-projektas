namespace PPI_projektas.Services.Response;


public interface IGroupEditDataFactory
{
    GroupEditData Create(List<ObjectDataItem> memberData, List<ObjectDataItem> administratorData);
}

public class GroupEditDataFactory : IGroupEditDataFactory
{
    public GroupEditData Create(List<ObjectDataItem> memberData, List<ObjectDataItem> administratorData)
    {
        return new GroupEditData(memberData, administratorData);
    }
}

public class GroupEditData
{
    public List<ObjectDataItem> MemberData { get; set; }

    public List<ObjectDataItem> AdministratorData { get; set; }

    public GroupEditData(List<ObjectDataItem> memberData, List<ObjectDataItem> administratorData)
    {
        MemberData = memberData;
        AdministratorData = administratorData;
    }
}