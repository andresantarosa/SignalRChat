using FluentAssertions;
using SignalRChat.Domain.Dto;
using SignalRChat.Service.Chat;
using SignalRChat.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalRChat.Tests.Unit.Services
{
    public class ChatUserServiceTests : UnitTestFixture
    {
        public ChatUserServiceTests():base()
        {
        }

        [Fact]
        public void AddUser_WithValidData_ShouldAddNewUser()
        {
            //Arrange
            var chatUserService = Mocker.CreateInstance<ChatUsersService>();
            var user = new ChatActiveUserDto("connection1", "email@email.com");

            //Act
            chatUserService.AddUser(user);

            //Assert
            chatUserService.GetCount().Should().Be(1);
        }

        [Fact]
        public void AddUser_WithExistingEmailValidData_ShouldNotAddNewUser()
        {
            //Arrange
            var chatUserService = Mocker.CreateInstance<ChatUsersService>();
            var user = new ChatActiveUserDto("connection1", "email@email.com");
            var user2 = new ChatActiveUserDto("connection2", "email@email.com");
            chatUserService.AddUser(user);

            //Act
            chatUserService.AddUser(user2);

            //Assert
            chatUserService.GetCount().Should().Be(1);
        }

        [Fact]
        public void RemoveUser_WithValidData_ShouldAddNewUser()
        {
            //Arrange
            var chatUserService = Mocker.CreateInstance<ChatUsersService>();
            var user = new ChatActiveUserDto("connection1", "email@email.com");
            chatUserService.AddUser(user);

            //Act
            chatUserService.RemoveUser(user);

            //Assert
            chatUserService.GetCount().Should().Be(0);
        }

        [Fact]
        public void GetByConnection_ShouldReturnSingleUser()
        {
            //Arrange
            var chatUserService = Mocker.CreateInstance<ChatUsersService>();
            var user = new ChatActiveUserDto("connection1", "email@email.com");
            var user2 = new ChatActiveUserDto("connection2", "email2@email.com");

            chatUserService.AddUser(user);
            chatUserService.AddUser(user2);


            //Act
            var foundUser = chatUserService.GetByConnection(user.ConnectionId);

            //Assert
            foundUser.Should().NotBeNull();
            foundUser.ConnectionId.Should().Be(user.ConnectionId);
        }

        [Fact]
        public void GetByEmail_ShouldReturnSingleUser_WithNoErrors()
        {
            //Arrange
            var chatUserService = Mocker.CreateInstance<ChatUsersService>();
            var user = new ChatActiveUserDto("connection1", "email@email.com");
            var user2 = new ChatActiveUserDto("connection2", "email2@email.com");

            chatUserService.AddUser(user);
            chatUserService.AddUser(user2);


            //Act
            var foundUser = chatUserService.GetByEmail(user.Email);

            //Assert
            foundUser.Should().NotBeNull();
            foundUser.Email.Should().Be(user.Email);
        }
    }
}
