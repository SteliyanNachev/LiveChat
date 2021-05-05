namespace LiveChat.Data.Models
{
    using System;
    using System.Collections.Generic;

    using LiveChat.Data.Common.Models;

    public class ChatRoom : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        public virtual ICollection<UserChatRoom> Users { get; set; } = new HashSet<UserChatRoom>();
    }
}
