using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SignalRChat.Domain.Dto;
using SignalRChat.Domain.Entities;
using SignalRChat.Domain.Interfaces.Infra.Messaging;
using SignalRChat.Domain.Interfaces.Persistence.Repository;
using SignalRChat.Domain.Interfaces.Services.Chat;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SignalRChat.Service.Chat
{


    public class ChatService : IChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMessaging _messaging;
        public ChatService(IHttpClientFactory httpClientFactory,
                           IMessaging messaging)
        {
            _httpClientFactory = httpClientFactory;
            _messaging = messaging;
        }

        public (bool success, string errors) EnqueueChatMessageToBeSaved(Post post)
        {
            if (post == null)
                return (false, "Post cannot be null");

            if (string.IsNullOrWhiteSpace(post.PostOwner))
                return (false, "Owner is required");

            if (string.IsNullOrWhiteSpace(post.PostContent))
                return (false, "Message cannot be empty");

            if (post.PostContent.Length > 1000)
                return (false, "Post maximum lenght is 1000 chars");

            _messaging.Enqueue<Post>("SaveChatMessageQueue", post);

            return (true, null);
        }

        public async Task<bool> GetQuotation(string stockCode, string caller)
        {
            var client = _httpClientFactory.CreateClient("StockGateway");
            Console.WriteLine("===============================");
            Console.WriteLine(client.BaseAddress);
            Console.WriteLine("===============================");
            var response = await client.GetAsync($"/GetStock/{stockCode}/{caller}");
            return response.IsSuccessStatusCode;
        }

    }
}
