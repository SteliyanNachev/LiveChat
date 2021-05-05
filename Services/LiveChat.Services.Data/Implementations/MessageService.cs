namespace LiveChat.Services.Data.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using LiveChat.Data.Common.Repositories;
    using LiveChat.Data.Models;
    using LiveChat.Services.Data.Models.Messages;

    public class MessageService : IMessageService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessageService(IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task<int> Create(CreateMessageServiceModel model, string userId)
        {
            var message = new Message
            {
                SendingTime = model.SendingTime,
                TextMessage = model.TextMessage,
                ChatRoomId = model.ChatRoomId,
                UserId = userId,
            };
            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();
            return message.Id;
        }

        public async Task<CreateMessageServiceModel> Update(CreateMessageServiceModel model, string userId)
        {
            var message = new Message
            {
                SendingTime = model.SendingTime,
                TextMessage = model.TextMessage,
                ChatRoomId = model.ChatRoomId,
                UserId = userId,
            };
            this.messagesRepository.Update(message);
            await this.messagesRepository.SaveChangesAsync();
            return model;
        }
    }
}
