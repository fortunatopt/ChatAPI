using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatAPI
{
    public class ChatHub : Hub
    {
        public void SendMessage(string name, string message)
        {
            Guid id = Guid.NewGuid();
            Clients.All.broadcastMessage(id, name, message);
        }
    }
}