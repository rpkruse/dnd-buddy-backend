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
            await Clients.OthersInGroup(userMessageData.GroupName).SendAsync("sendConnectionNoticeToGroup", userMessageData);
        }

        public async Task LeaveGroup(UserMessageData userMessageData)
        {
            await Groups.RemoveAsync(Context.ConnectionId, userMessageData.GroupName);
            await Clients.Group(userMessageData.GroupName).SendAsync("sendDisconnectNoticeToGroup", userMessageData);
            await Clients.Client(Context.ConnectionId).SendAsync("okToStopConnection");
        }

        public async Task SetGroup(UserMessageData[] groupMembers, string groupName, string newMemberID)
        {
            //await Clients.OthersInGroup(groupName).SendAsync("updateLobby", groupMembers);
            await Clients.Client(newMemberID).SendAsync("updateLobby", groupMembers);
        }
    }
}
