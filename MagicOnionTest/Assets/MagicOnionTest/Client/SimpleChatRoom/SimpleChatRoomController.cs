using UnityEngine;
using System.Collections.Generic;
using MagicOnion.Client;
using Grpc.Core;
using System.Threading.Tasks;
using UniRx;

public class SimpleChatRoomController : MonoBehaviour, IChatRoomHubReceiver
{
    [SerializeField]
    SimpleChatLoginContext LoginContext;

    [SerializeField]
    SimpleChatRoomView View;

    IChatRoomHub _client;

    ChatRoomMember Me;
    Dictionary<int, ChatRoomMember> Others = new Dictionary<int, ChatRoomMember>();

    async void Start()
    {
        var channel = new Channel(LoginContext.Host, LoginContext.Port, ChannelCredentials.Insecure);
        _client = StreamingHubClient.Connect<IChatRoomHub, IChatRoomHubReceiver>(channel, this);

        await JoinAsync();

        View.OnLogoutClickedAsObservable().Subscribe(_ => LogoutAsync()).AddTo(this);
        View.OnInputAsObservable().Subscribe(mes => SpeakAsync(mes)).AddTo(this);
    }

    async Task JoinAsync()
    {
        var result = await _client.JoinAsync(LoginContext.UserName);
        Me = result.You;
        View.SetMyName(Me.Name);

        foreach (var member in result.OtherMembers)
        {
            OnJoin(member);
        }

    }

    async Task SpeakAsync(string message)
    {
        if (_client == null) throw new System.Exception("not connected");

        await _client.SpeakAsync(message);

        View.ClearInputField();
    }

    async void OnDestroy()
    {
        if (_client != null) await _client.DisposeAsync();
    }

    void OnJoin(ChatRoomMember member)
    {
        View.OnJoin(member);
        Others.Add(member.Id, member);
    }

    void IChatRoomHubReceiver.OnJoin(ChatRoomMember member) => OnJoin(member);

    void IChatRoomHubReceiver.OnLeave(int memberId)
    {
        ChatRoomMember member;
        if (Others.TryGetValue(memberId, out member))
        {
            View.OnLeave(member);
            Others.Remove(memberId);
        }
    }

    void IChatRoomHubReceiver.OnSpeak(ChatRoomMember member, string message)
    {
        View.OnSpeak(member, message);
    }

    async Task LogoutAsync()
    {
        await _client.LeaveAsync();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Index");
    }
}
