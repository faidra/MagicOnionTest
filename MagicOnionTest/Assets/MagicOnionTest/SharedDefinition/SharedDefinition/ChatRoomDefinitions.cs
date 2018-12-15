using MessagePack;
using MagicOnion;
using System.Threading.Tasks;

[MessagePackObject]
public struct ChatRoomMember
{
    [Key(0)]
    public int Id { get; set; }
    [Key(1)]
    public string Name { get; set; }
}

[MessagePackObject]
public struct JoinResult
{
    [Key(0)]
    public ChatRoomMember You;
    [Key(1)]
    public ChatRoomMember[] OtherMembers;
}

public interface IChatRoomHubReceiver
{
    void OnJoin(ChatRoomMember member);
    void OnLeave(int memberId);
    void OnSpeak(ChatRoomMember member, string message);
}

public interface IChatRoomHub : IStreamingHub<IChatRoomHub, IChatRoomHubReceiver>
{
    Task<JoinResult> JoinAsync(string name);
    Task LeaveAsync();
    Task SpeakAsync(string message);
}