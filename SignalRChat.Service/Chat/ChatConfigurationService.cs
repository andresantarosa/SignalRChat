
using Microsoft.Extensions.Configuration;
using SignalRChat.Domain.Interfaces.Services.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Service.Chat
{
    public class ChatConfigurationService : IChatConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ChatConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetMessageLimit()
        {
            var limit = _configuration["ChatConfiguration:MaxAllowedMessage"];
            return Convert.ToInt32(limit);
        }
    }
}
