namespace PPI_projektas.Services.Response;


public interface IGroupEditDataFactory
{
    GroupUserData Create(List<ObjectDataItem> memberData, List<ObjectDataItem> administratorData);
}

public class GroupEditDataFactory : IGroupEditDataFactory
{
    public GroupUserData Create(List<ObjectDataItem> memberData, List<ObjectDataItem> administratorData)
    {
        return new GroupUserData(memberData, administratorData);
    }
}

public class GroupUserData
{
    public List<ObjectDataItem> MemberData { get; set; }

    public List<ObjectDataItem> AdministratorData { get; set; }

    public GroupUserData(List<ObjectDataItem> memberData, List<ObjectDataItem> administratorData)
    {
        MemberData = memberData;
        AdministratorData = administratorData;
    }
}