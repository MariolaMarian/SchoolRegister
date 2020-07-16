using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRegister.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ChatHub(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task SendMessageToEveryUser(ChatMessageVM chatMessage)
        {
            var currentUser = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            chatMessage.SetAuthor(currentUser);

            await Clients.All.SendAsync("ShowMessage", chatMessage);
        }

        public async Task SendMessageToSingleUser(ChatMessageVM chatMessage)
        {
            var currentUser = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            chatMessage.SetAuthor(currentUser);

            int recipientId = 0;
            if(Int32.TryParse(chatMessage.RecipientId, out recipientId))
            {
                var recipient = _dbContext.Users.FirstOrDefault(u => u.Id == recipientId);
                if (recipient != null)
                {
                    await Clients.User(recipient.Id.ToString()).SendAsync("ShowMessage", chatMessage);
                }
            }
        }

        public async Task SendMessageToGroup(ChatMessageVM chatMessage)
        {
            var currentUser = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            chatMessage.SetAuthor(currentUser);

            int groupId = 0;
            if (Int32.TryParse(chatMessage.RecipientId, out groupId))
            {
                await Clients.Group(groupId.ToString()).SendAsync("ShowMessage", chatMessage);
            }

        }

        private async Task SetGroupsAsync()
        {
            var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
            if(user is Student student)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, student.GroupId.ToString());
            }
        }

        public override async Task OnConnectedAsync()
        {
            await SetGroupsAsync();
            await base.OnConnectedAsync();
        }
    }
}
