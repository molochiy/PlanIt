using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanIt.Entities
{
    public partial class PlanItEntities
    {
        public PlanItEntities(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
        }
    }
}
