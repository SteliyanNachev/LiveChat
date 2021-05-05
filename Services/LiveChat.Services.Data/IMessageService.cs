namespace LiveChat.Services.Data
{
    using System.Threading.Tasks;

    using LiveChat.Services.Data.Models.Messages;

    public interface IMessageService
    {
        Task<int> Create(CreateMessageServiceModel model, string userId);

        Task<CreateMessageServiceModel> Update(CreateMessageServiceModel model, string userId);
    }
}
