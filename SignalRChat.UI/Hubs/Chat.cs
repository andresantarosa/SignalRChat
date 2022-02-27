using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Domain.Dto;
using SignalRChat.Domain.Entities;
using SignalRChat.Domain.Interfaces.Persistence.UoW;
using SignalRChat.Domain.Interfaces.Services.Chat;

namespace SignalRChat.UI.Hubs
{
    public class Chat : Hub
    {
        private readonly IChatUsersService _chatUsersService;
        private readonly IChatConfigurationService _configuration;
        private readonly IChatService _chatService;
        public Chat(IChatUsersService chatUsersService,
                    IChatConfigurationService configuration,
                    IChatService chatService)
        {
            _chatUsersService = chatUsersService;
            _configuration = configuration;
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var email = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            ChatActiveUserDto chatActiveUserDto = new ChatActiveUserDto(connectionId, email);
            _chatUsersService.AddUser(chatActiveUserDto);
            await SetUsers();
            await SetMessageLimit();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _chatUsersService.RemoveUser(Context.ConnectionId);
        }

        public async Task SendMessage(string message)
        {

            if (message.StartsWith("/stock="))
            {
                var stockCode = message.Replace("/stock=", "");
                await _chatService.GetQuotation(stockCode, Context.ConnectionId);
                return;
            }

            var user = Context.User.Identity.Name;
            if (!await SavePost(message, user))
                return;

            await Clients.All.SendCoreAsync("ReceiveMessage", GetArray(user, message));
        }

        private async Task SetUsers() =>
            await Clients.All.SendCoreAsync("SetUsers",GetArray(_chatUsersService.ChatUsers.Select(x => x.Email).ToArray()));

        private async Task SetMessageLimit()
        {
            int messageLimit = _configuration.GetMessageLimit() ;
            await Clients.All.SendCoreAsync("SetMessageLimit", GetArray(messageLimit));

        }

        private object[] GetArray(string[] obj)
        {
            return new object[] { obj };
        }
        private object[] GetArray(params object[] args)
        {
            return args.ToArray();
        }

        private async Task<bool> SavePost(string message, string user)
        {
            var post = new Post()
            {
                PostContent = message,
                PostOwner = user,
            };

            var savePostResponse = _chatService.EnqueueChatMessageToBeSaved(post);
            if(savePostResponse.success == false)
                return false;
                
            return true;
        }
    }
}
