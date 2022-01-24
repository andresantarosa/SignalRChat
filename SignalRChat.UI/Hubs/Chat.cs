using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Domain.Dto;
using SignalRChat.Domain.Interfaces.Services.Chat;

namespace SignalRChat.UI.Hubs
{
    public class Chat : Hub
    {
        private readonly IChatUsersService _chatUsersService;
        public Chat(IChatUsersService chatUsersService)
        {
            _chatUsersService = chatUsersService;
            
        }

        public override async Task OnConnectedAsync()
        {
            var email = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            ChatActiveUserDto chatActiveUserDto = new ChatActiveUserDto(connectionId, email);
            _chatUsersService.AddUser(chatActiveUserDto);
            await SetUsers();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _chatUsersService.RemoveUser(Context.ConnectionId);
        }

        public async Task SendMessage(string message)
        {
            var user = Context.User.Identity.Name;
            await Clients.All.SendCoreAsync("ReceiveMessage", GetArray(user, message));
        }

        private async Task SetUsers() =>
            await Clients.All.SendCoreAsync("SetUsers",GetArray(_chatUsersService.ChatUsers.Select(x => x.Email).ToArray()));

        private object[] GetArray(string[] obj)
        {
            return new object[] { obj };
        }
        private object[] GetArray(params object[] args)
        {
            return args.ToArray();
        }
    }
}
