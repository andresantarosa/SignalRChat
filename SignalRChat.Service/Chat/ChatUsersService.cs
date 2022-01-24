using SignalRChat.Domain.Dto;
using SignalRChat.Domain.Interfaces.Services.Chat;

namespace SignalRChat.Service.Chat
{
    public class ChatUsersService : IChatUsersService
    {
        public List<ChatActiveUserDto> ChatUsers { get; } = new List<ChatActiveUserDto>();

        public void AddUser(ChatActiveUserDto user)
        {
            if (GetByEmail(user.Email) == null)
                ChatUsers.Add(user);
        }


        public void RemoveUser(ChatActiveUserDto user) =>
            ChatUsers.Remove(user);

        public ChatActiveUserDto GetByConnection(string connectionId) =>
            ChatUsers.FirstOrDefault(x => x.ConnectionId == connectionId);

        public ChatActiveUserDto GetByEmail(string email) =>
            ChatUsers.FirstOrDefault(x => x.Email == email);

        public int GetCount() =>
            ChatUsers.Count();

        public void RemoveUser(string connectionid)
        {
            var user = GetByConnection(connectionid);
            if (user == null)
                return;
            
            RemoveUser(user);
        }
    }
}
