using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Screens.Hosting.WebTerm
{
    public class WebTermHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var client = Clients.Caller;
            var connectionId = Context.ConnectionId;
            WebTermHost.Instance.ClientConnected(connectionId, client);
            await Task.CompletedTask;
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = Context.ConnectionId;
            WebTermHost.Instance.ClientDisconnected(connectionId);
            await Task.CompletedTask;
        }

        public async Task SendKey(string key)
        {
            var connectionId = Context.ConnectionId;
            WebTermHost.Instance.ProcessKey(connectionId, key);
            await Task.CompletedTask;
        }

    }
}
