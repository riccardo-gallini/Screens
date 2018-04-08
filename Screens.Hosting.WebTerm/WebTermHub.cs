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
            var connection = Context.Connection;
            var client = Clients.Client(connection.ConnectionId);

            await WebTermHost.Instance.ClientConnectedAsync(connection, client);
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var connection = Context.Connection;
            WebTermHost.Instance.ClientDisconnectedAsync(connection);
            await Task.CompletedTask;
        }

        public async Task SendKey(KeyInfo key)
        {
            var connectionId = Context.ConnectionId;
            WebTermHost.Instance.ProcessKey(connectionId, key);
            await Task.CompletedTask;
        }

    }
}
