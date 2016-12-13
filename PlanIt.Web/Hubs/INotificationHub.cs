using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanIt.Web.Hubs
{
    public interface INotificationHub
    {
        void UpdateNotification(List<string> receivers);
    }
}
