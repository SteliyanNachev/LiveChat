namespace LiveChat.Services.Data.Models.Messages
{
    using System;

    public class CreateMessageServiceModel
    {
        public DateTime SendingTime { get; set; }

        public string TextMessage { get; set; }

        public int ChatRoomId { get; set; }
    }
}
