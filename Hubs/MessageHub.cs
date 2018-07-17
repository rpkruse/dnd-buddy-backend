using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dnd_buddy_backend.Models;
using Microsoft.AspNetCore.SignalR;

namespace dnd_buddy_backend.Hubs
{ //https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups

    public class MessageHub : Hub
    {
        public override Task OnConnectedAsync()
        {


            Clients.Client(Context.ConnectionId).SendAsync("connected");

            return base.OnConnectedAsync();
        }

        public async Task JoinGroup(UserMessageData userMessageData)
        {
            userMessageData.ID = Context.ConnectionId;
            await Groups.AddAsync(Context.ConnectionId, userMessageData.GroupName);
            await Clients.Client(userMessageData.ID).SendAsync("sendConnectionNoticeToSelf", userMessageData);
            await Clients.OthersInGroup(userMessageData.GroupName).SendAsync("sendConnectionNoticeToGroup", userMessageData);
        }

        public async Task LeaveGroup(UserMessageData userMessageData)
        {
            await Groups.RemoveAsync(Context.ConnectionId, userMessageData.GroupName);
            await Clients.OthersInGroup(userMessageData.GroupName).SendAsync("sendDisconnectNoticeToGroup", userMessageData);
            await Clients.Client(Context.ConnectionId).SendAsync("okToStopConnection");
        }

        public async Task SetGroup(OnlineUser[] groupMembers, string groupName, string newMemberID)
        {
            //await Clients.OthersInGroup(groupName).SendAsync("updateLobby", groupMembers);
            await Clients.Client(newMemberID).SendAsync("updateLobby", groupMembers);
        }

        public async Task SendRoll(RollMessageData rollMessageData)
        {
            await Clients.OthersInGroup(rollMessageData.GroupName).SendAsync("sendRollNoticeToGroup", rollMessageData);
        }

        public async Task SendItem(ItemMessageData itemMessageData)
        {
            await Clients.Client(itemMessageData.connectionId).SendAsync("getItem", itemMessageData);
        }

        public async Task SendGridPlacement(GridMessageData gridMessageData)
        {
            await Clients.OthersInGroup(gridMessageData.GroupName).SendAsync("sendGridUpdateToGroup", gridMessageData);
        }

        public async Task SendGroupMessage(ChatMessageData chatMessageData)
        {
            await Clients.OthersInGroup(chatMessageData.GroupName).SendAsync("sendChatMessageToGroup", chatMessageData);
        }

        public async Task SendPrivateMessage(ChatMessageData chatMessageData) //Group name will be a username
        {
            await Clients.Client(chatMessageData.ConnectionID).SendAsync("sendChatMessageToGroup", chatMessageData);
        }
    }
}
