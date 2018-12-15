using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;


namespace MagicOnionTestServer.SimpleChat
{
    class SimpleChatHub : StreamingHubBase<IChatRoomHub, IChatRoomHubReceiver>, IChatRoomHub
    {
        const string GroupIdentifier = "SimpleChatHub";
        IGroup _room;
        ChatRoomMember _self;
        IInMemoryStorage<ChatRoomMember> _members;

        public async Task<JoinResult> JoinAsync(string name)
        {
            _self = new ChatRoomMember() { Name = name };

            (_room, _members) = await Group.AddAsync(GroupIdentifier, _self);
            _self.Id = _members.AllValues.Count;

            Broadcast(_room).OnJoin(_self);

            return new JoinResult()
            {
                You = _self,
                OtherMembers = _members.AllValues.Where(mem => mem.Id != _self.Id).ToArray()
            };
        }

        public async Task LeaveAsync()
        {
            await _room.RemoveAsync(Context);
            Broadcast(_room).OnLeave(_self.Id);
        }

        public async Task SpeakAsync(string message)
        {
            Broadcast(_room).OnSpeak(_self, message);
        }
    }
}
