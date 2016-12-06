using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;

namespace PlanIt.Web.SignalR
{
    public class RealtimeNotifierHub : Hub
    {
        public int recordsToBeProcessed = 100000;

        public void DoLongOperation()
        {
            for (int record = 0; record <= recordsToBeProcessed; record++)
            {
                if (ShouldNotifyClient(record))
                {
                    Clients.Caller.sendMessage(string.Format
                    ("Processing item {0} of {1}", record, recordsToBeProcessed));
                    Thread.Sleep(10);
                }
            }
        }

        private static bool ShouldNotifyClient(int record)
        {
            return record % 10 == 0;
        }
    }
}