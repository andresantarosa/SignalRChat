using SignalRChat.Domain.Entities;
using SignalRChat.Domain.Interfaces.Persistence.Repository;
using SignalRChat.Domain.Interfaces.Services.Chat;
using System.Net.Http;

namespace SignalRChat.Service.Chat
{


    public class ChatService : IChatService
    {
        private readonly IPostRepository _postRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatService(IPostRepository postRepository, IHttpClientFactory httpClientFactory)
        {
            _postRepository = postRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool success, string errors)> SaveMessage(Post post)
        {
            if (post == null)
                return (false, "Post cannot be null");

            if (string.IsNullOrWhiteSpace(post.PostOwner))
                return (false, "Owner is required");

            if (string.IsNullOrWhiteSpace(post.PostContent))
                return (false, "Message cannot be empty");

            if (post.PostContent.Length > 1000)
                return (false, "Post maximum lenght is 1000 chars");

            await _postRepository.Add(post);
            return (true, null);
        }

        public async Task<bool> GetQuotation(string stockCode, string caller)
        {
            var client = _httpClientFactory.CreateClient("StockGateway");
            var response = await client.GetAsync($"/GetStock/{stockCode}/{caller}");
            return response.IsSuccessStatusCode;
        }
    }
}
