namespace LiveChat.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LiveChat.Data.Common.Repositories;
    using LiveChat.Data.Models;
    using LiveChat.Services.Data;
    using LiveChat.Services.Data.Models.Messages;
    using LiveChat.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        private readonly IUserService userService;

        private readonly IDeletableEntityRepository<Message> messagerepairRepository;

        public ChatHub(
            IMessageService messageService,
            IUserService userService,
            IDeletableEntityRepository<Message> messagerepairRepository)
        {
            this.messageService = messageService;
            this.userService = userService;
            this.messagerepairRepository = messagerepairRepository;
        }

        public async Task Send(string message)
        {
            // Add Message to database
            var currentMessage = new CreateMessageServiceModel
            {
                SendingTime = DateTime.UtcNow,
                TextMessage = message,
                ChatRoomId = 1,
            };
            var userID = await this.userService.GetUserId(this.Context.User);
            var result = await this.messageService.Create(currentMessage, userID);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new MessageViewModel
                {
                    Id = result,
                    User = this.Context.User.Identity.Name,
                    Text = message,
                    DateOfMessage = DateTime.UtcNow.ToShortTimeString(),
                });
        }

        public async Task LoadHistory()
        {
            var messages = this.messagerepairRepository.
                AllAsNoTracking()
                .Where(m => m.ChatRoomId == 1).ToList();
            foreach (var message in messages)
            {
                await this.Clients.Caller.SendAsync(
                    "NewMessage",
                    new MessageViewModel
                {
                    Id = message.Id,
                    User = this.Context.User.Identity.Name,
                    Text = message.TextMessage,
                    DateOfMessage = message.SendingTime.ToString(),
                });
            }
        }

        public async Task LoadParticipants()
        {
            var users = ConnectedUsers.UserNames;
            foreach (var user in users)
            {
                await this.Clients.Caller.SendAsync(
                  "UsersList", user);
            }
        }

        public async Task DeleteMessage(string messageId)
        {
            var messageID = int.Parse(messageId);
            var message = this.messagerepairRepository.AllAsNoTracking().Where(m => m.Id == messageID).FirstOrDefault();
            this.messagerepairRepository.Delete(message);
            await this.messagerepairRepository.SaveChangesAsync();
        }

        public override Task OnConnectedAsync()
        {
            ConnectedUsers.UserNames.Add(this.Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.UserNames.Remove(this.Context.User.Identity.Name);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
