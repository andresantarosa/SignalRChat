using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Moq;
using SignalRChat.Domain.Dto;
using SignalRChat.Domain.Interfaces.Services.Chat;
using SignalRChat.Tests.Base;
using SignalRChat.UI.Hubs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SignalRChat.Tests.Unit.Hubs
{
    public class ChatTests : UnitTestFixture
    {
        private string _receiveMessageMethod = "ReceiveMessage";
        private string _setUsers = "SetUsers";
        private string _messageLimit = "SetMessageLimit";

        [Fact]
        public async Task SendMessage_WithValidText_ShouldSendMessage()
        {

            // Arrange
            var name = "andre@andre.com";
            var message = "myMessage";
            var mockContext = new Mock<HubCallerContext>();
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            var chatHub = Mocker.CreateInstance<Chat>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
            }));

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            mockContext.Setup(x => x.User).Returns(user);
            chatHub.Context = mockContext.Object;
            chatHub.Clients = mockClients.Object;

            // Act
            await chatHub.SendMessage(message);

            // Assert
            mockClients.Verify(clients => clients.All, Times.Once);

            mockClientProxy.Verify(
               clientProxy => clientProxy.SendCoreAsync(
                   _receiveMessageMethod,
                   It.Is<object[]>(callParams => callParams != null &&
                                   callParams.Length == 2
                                   && callParams[0].ToString() == name
                                   && callParams[1].ToString() == message),
                   default(CancellationToken)),
               Times.Once);
        }

        [Fact]
        public async Task OnConnectedAsync_With2ActiveUsers_ShouldReturnsListOfActiveUsers()
        {
            // Arrange
            var name = "andre@andre.com";
            var connectionId = "Connection3";
            var mockContext = new Mock<HubCallerContext>();
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            var chatHub = Mocker.CreateInstance<Chat>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
            }));

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            mockContext.Setup(x => x.User).Returns(user);
            mockContext.Setup(x => x.ConnectionId).Returns(connectionId);
            chatHub.Context = mockContext.Object;
            chatHub.Clients = mockClients.Object;

            var user1 = new ChatActiveUserDto("Connection1", "email1@email.com");
            var user2 = new ChatActiveUserDto("Connection2", "email2@email.com");
            var user3 = new ChatActiveUserDto(connectionId, name);


            Mocker.GetMock<IChatUsersService>()
                .Setup(x => x.ChatUsers)
                .Returns(new List<ChatActiveUserDto>
                {
                   user1,
                   user2,
                   user3,
                });

            Mocker.GetMock<IChatConfigurationService>()
                .Setup(x => x.GetMessageLimit())
                .Returns(10);
            // Act
            await chatHub.OnConnectedAsync();

            // Assert
            mockClients.Verify(clients => clients.All, Times.Exactly(2));

             mockClientProxy.Verify(
               clientProxy => clientProxy.SendCoreAsync(
                   _setUsers,
                   It.Is<object[]>(a => ((string[])a[0])[0] == user1.Email &&
                                        ((string[])a[0])[1] == user2.Email &&
                                        ((string[])a[0])[2] == user3.Email),
                   default(CancellationToken)),
               Times.Once);

            Mocker.GetMock<IChatUsersService>().Verify(x => x.AddUser(user3), Times.Once);
        }

        [Fact]
        public async Task OnConnectedAsync_ShouldReturnsMessageLimit()
        {
            // Arrange
            var name = "andre@andre.com";
            var connectionId = "Connection3";
            var mockContext = new Mock<HubCallerContext>();
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            var chatHub = Mocker.CreateInstance<Chat>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
            }));

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            mockContext.Setup(x => x.User).Returns(user);
            mockContext.Setup(x => x.ConnectionId).Returns(connectionId);
            chatHub.Context = mockContext.Object;
            chatHub.Clients = mockClients.Object;

            var user1 = new ChatActiveUserDto("Connection1", "email1@email.com");

            Mocker.GetMock<IChatUsersService>()
                .Setup(x => x.ChatUsers)
                .Returns(new List<ChatActiveUserDto>
                {
                   user1
                });

            Mocker.GetMock<IChatConfigurationService>()
                .Setup(x => x.GetMessageLimit())
                .Returns(10);
            // Act
            await chatHub.OnConnectedAsync();

            // Assert
            mockClients.Verify(clients => clients.All, Times.Exactly(2));

            mockClientProxy.Verify(
              clientProxy => clientProxy.SendCoreAsync(
                  _messageLimit,
                  It.Is<object[]>(a => ((int)a[0] == 10)),
                  default(CancellationToken)),
              Times.Once);

            Mocker.GetMock<IChatConfigurationService>().Verify(x => x.GetMessageLimit(), Times.Once);
        }

        [Fact]
        public async Task OnDisconnectedAsync_WithConnectedUser_ShouldRemoveUser()
        {
            // Arrange
            var connectionId = "Connection3";
            var mockContext = new Mock<HubCallerContext>();
            var mockClients = new Mock<IHubCallerClients>();
            var mockClientProxy = new Mock<IClientProxy>();
            var chatHub = Mocker.CreateInstance<Chat>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            mockContext.Setup(x => x.ConnectionId).Returns(connectionId);
            chatHub.Context = mockContext.Object;
            chatHub.Clients = mockClients.Object;

            // Act
            await chatHub.OnDisconnectedAsync(null);

            // Assert
            Mocker.GetMock<IChatUsersService>().Verify(x => x.RemoveUser(connectionId), Times.Once);
        }
    }
}
