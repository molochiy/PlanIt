﻿using PlanIt.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;

namespace PlanIt.Services.Concrete
{
    public class ProfileService : BaseService, IProfileService
    {
        public ProfileService(IRepository repository) : base(repository)
        {
        }
        public Profile GetProfileById(int id)
        {
            return _repository.GetSingle<Profile>(u => u.Id == id);
        }
    
        public Profile UpdateProfile(Profile profile)
        {
           return  _repository.Update<Profile>(profile);
        }
    }
}
