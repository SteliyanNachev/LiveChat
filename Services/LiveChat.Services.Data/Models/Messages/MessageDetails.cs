namespace LiveChat.Services.Data.Models.Messages
{
    using System;

    public class MessageDetails
    {
        public DateTime SendingTime { get; set; }

        public string TextMessage { get; set; }

        public string UserId { get; set; }

        public int ChatRoomId { get; set; }
    }
}
