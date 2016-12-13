using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace PlanIt.Web.Hubs
{


    [Authorize]
    public class NotificationHub : Hub, INotificationHub
    {
        private static readonly ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        
        public void UpdateNotification(List<string> receivers)
        {
            foreach (var receiver in receivers)
            {
                foreach (var connectionId in _connections.GetConnections(receiver))
                {
                    try
                    {
                        GlobalHost.ConnectionManager.GetHubContext<NotificationHub>()
                            .Clients.Client(connectionId)
                            .updateNotificationCount();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}