namespace LiveChat.Services.Data.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using LiveChat.Data.Common.Repositories;
    using LiveChat.Data.Models;
    using LiveChat.Services.Data.Models.Messages;
    using LiveChat.Services.Data.Models.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> Delete(string userName)
        {
            var currentUser = await this.userManager.FindByNameAsync(userName);
            var result = await this.userManager.DeleteAsync(currentUser);
            return result;
        }

        public async Task<ApplicationUser> Login(string username, string password)
        {
            var result = await this.signInManager.PasswordSignInAsync(username, password, true, true);
            if (!result.Succeeded)
            {
                return null;
            }

            var user = await this.userManager.FindByNameAsync(username);
            return user;
        }

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(RegisterUserInputModel userInput)
        {
            var user = new ApplicationUser
            {
                Email = userInput.Email,
                UserName = userInput.UserName,
            };
            var result = await this.userManager.CreateAsync(user, userInput.Password);
            return result;
        }

        public async Task<IdentityResult> Update(UpdateUserInputModel model, string username)
        {
            var currentUser = await this.userManager.FindByNameAsync(username);

            currentUser.UserName = model.UserName;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.PhoneNumber;

            var result = await this.userManager.UpdateAsync(currentUser);
            return result;
        }

        public async Task<string> GetUserId(ClaimsPrincipal user)
        {
            return this.userManager.GetUserId(user);
        }
    }
}
