﻿using SignalRChat.Domain.Entities;

namespace SignalRChat.Domain.Interfaces.Services.Chat
{
    public interface IChatService
    {
        Task<(bool success, string errors)> SaveMessage(Post post);
    }
}
