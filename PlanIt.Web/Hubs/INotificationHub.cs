using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PlanIt.Web.Hubs
{
    public interface INotificationHub
    {
        void UpdateNotification(List<string> receivers);
        void AddNewCommentToList(List<string> receivers, JsonResult data);
    }
}
