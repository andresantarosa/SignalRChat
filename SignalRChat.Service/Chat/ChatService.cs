using SignalRChat.Domain.Entities;
using SignalRChat.Domain.Interfaces.Persistence.Repository;
using SignalRChat.Domain.Interfaces.Services.Chat;

namespace SignalRChat.Service.Chat
{


    public class ChatService : IChatService
    {
        private readonly IPostRepository _postRepository;

        public ChatService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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
    }
}
