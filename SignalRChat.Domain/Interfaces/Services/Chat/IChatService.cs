using SignalRChat.Domain.Dto.External;
using SignalRChat.Domain.Entities;

namespace SignalRChat.Domain.Interfaces.Services.Chat
{
    public interface IChatService
    {
        (bool success, string errors) EnqueueChatMessageToBeSaved(Post post);
        Task<bool> GetQuotation(string stockCode, string caller);
    }
}
