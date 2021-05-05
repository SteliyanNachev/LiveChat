namespace LiveChat.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using LiveChat.Data.Common.Models;
    using LiveChat.Data.Common.Repositories;
    using LiveChat.Data.Models;
    using LiveChat.Services.Data;
    using LiveChat.Services.Data.Models.Messages;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("/api/message")]
    public class MessageController : ControllerBase
    {
        private readonly IDeletableEntityRepository<Message> messagerepairRepository;
        private readonly IUserService userService;
        private readonly IMessageService messageService;

        public MessageController(
            IDeletableEntityRepository<Message> messagerepairRepository,
            IUserService userService,
            IMessageService messageService)
        {
            this.messagerepairRepository = messagerepairRepository;
            this.userService = userService;
            this.messageService = messageService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IEnumerable<Message>> Get(int id)
        {
            var userId = await this.userService.GetUserId(this.User);

            return this.messagerepairRepository.
                AllAsNoTracking()
                .Where(x => x.Id == id && x.UserId == userId).ToList();
        }

        [Authorize]
        [HttpPost("/api/[controller]/Create")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> CreateMessage([FromBody]CreateMessageServiceModel message)
        {
            var userId = await this.userService.GetUserId(this.User);
            var result = await this.messageService.Create(message, userId);

            return this.Ok(result);
        }

        [Authorize]
        [HttpPut("/api/[controller]/Update")]
        public async Task<ActionResult> UpdateMessage([FromBody] CreateMessageServiceModel message)
        {
            var userId = await this.userService.GetUserId(this.User);
            var result = await this.messageService.Update(message, userId);
            if (result == null)
            {
                return this.BadRequest();
            }

            return this.Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = this.messagerepairRepository.
                 AllAsNoTracking()
                 .Where(x => x.Id == id).FirstOrDefault();
            this.messagerepairRepository.Delete(message);
            await this.messagerepairRepository.SaveChangesAsync();
            return this.NoContent();
        }
    }
}
