namespace LiveChat.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using LiveChat.Data.Models;
    using LiveChat.Services.Data.Models.Messages;
    using LiveChat.Web.Hubs;
    using LiveChat.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    // [Route("[controller]")]

    // [ApiController]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> chatHub;

        public ChatController(IHubContext<ChatHub> chatHub)
        {
            this.chatHub = chatHub;
        }

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public Task Notification([FromBody] string message)
        {
            var username = this.User.Identity.Name;
            var result = this.chatHub.Clients.All.SendAsync("Notification", message, username);
            return result;
        }
    }
}
