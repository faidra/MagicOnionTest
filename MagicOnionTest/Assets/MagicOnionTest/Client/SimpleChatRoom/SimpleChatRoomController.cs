using UnityEngine;
using System.Collections.Generic;
using MagicOnion.Client;
using Grpc.Core;
using System.Threading.Tasks;

public class SimpleChatRoomController : MonoBehaviour, IChatRoomHubReceiver
{
    [SerializeField]
    SimpleChatLoginContext LoginContext;

    IChatRoomHub _client;

    ChatRoomMember Me;
    Dictionary<int, ChatRoomMember> Others = new Dictionary<int, ChatRoomMember>();

    async void Start()
    {
        var channel = new Channel(LoginContext.Host, LoginContext.Port, ChannelCredentials.Insecure);
        _client = StreamingHubClient.Connect<IChatRoomHub, IChatRoomHubReceiver>(channel, this);

        await JoinAsync();
    }

    async Task JoinAsync()
    {
        var result = await _client.JoinAsync(LoginContext.UserName);
        Me = result.You;
        foreach (var member in result.OtherMembers)
        {
            OnJoin(member);
        }
    }

    async Task SpeakAsync(string message)
    {
        if (_client == null) throw new System.Exception("not connected");

        await _client.SpeakAsync(message);
    }

    async void OnDestroy()
    {
        if (_client != null) await _client.DisposeAsync();
    }

    void OnJoin(ChatRoomMember member)
    {
        Debug.LogFormat("Join:{0}", member.Name);
        Others.Add(member.Id, member);
    }

    void IChatRoomHubReceiver.OnJoin(ChatRoomMember member) => OnJoin(member);

    void IChatRoomHubReceiver.OnLeave(int memberId)
    {
        ChatRoomMember member;
        if (Others.TryGetValue(memberId, out member))
        {
            Debug.LogFormat("Leave:{0}", member.Name);
            Others.Remove(memberId);
        }
    }

    void IChatRoomHubReceiver.OnSpeak(ChatRoomMember member, string message)
    {
        Debug.LogFormat("{0}:{1}", member.Name, message);
    }
}
