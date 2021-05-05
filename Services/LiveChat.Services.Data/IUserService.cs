namespace LiveChat.Services.Data
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using LiveChat.Data.Models;
    using LiveChat.Services.Data.Models.Users;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<IdentityResult> Register(RegisterUserInputModel model);

        Task<ApplicationUser> Login(string username, string password);

        Task Logout();

        Task<IdentityResult> Delete(string userName);

        Task<IdentityResult> Update(UpdateUserInputModel model, string username);

        Task<string> GetUserId(ClaimsPrincipal user);
    }
}
