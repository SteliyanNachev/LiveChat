namespace LiveChat.Web.ViewModels.Chat
{
    using System;

    public class MessageViewModel
    {
        public int Id { get; set; }

        public string User { get; set; }

        public string Text { get; set; }

        public string DateOfMessage { get; set; }
    }
}
