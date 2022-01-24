using SignalRChat.Domain.Entities.Base;

namespace SignalRChat.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string PostOwner { get; set; } 
        public string PostContent { get; set; } = "";
    }
}
