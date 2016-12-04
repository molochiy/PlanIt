using PlanIt.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanIt.Services.Abstract
{
    public interface IProfileService
    {
        Profile GetProfileById(int id);
        void SaveProfile(Profile profile);
        void UpdateProfile(Profile profile);
    }
}
