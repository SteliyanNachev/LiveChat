namespace LiveChat.Data.Models
{
    using System;

    using LiveChat.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        public DateTime SendingTime { get; set; }

        public string TextMessage { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ChatRoomId { get; set; }

        public ChatRoom ChatRoom { get; set; }
    }
}
