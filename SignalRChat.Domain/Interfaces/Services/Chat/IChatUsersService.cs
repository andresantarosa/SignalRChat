using SignalRChat.Domain.Dto;

namespace SignalRChat.Domain.Interfaces.Services.Chat
{
    public interface IChatUsersService
    {
        List<ChatActiveUserDto> ChatUsers { get; }

        void AddUser(ChatActiveUserDto user);
        ChatActiveUserDto GetByConnection(string connectionId);
        ChatActiveUserDto GetByEmail(string email);
        void RemoveUser(ChatActiveUserDto user);
        void RemoveUser(string connectionid);
    }
}
