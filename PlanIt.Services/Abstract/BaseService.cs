using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanIt.Repositories.Abstract;

namespace PlanIt.Services.Abstract
{
    public abstract class BaseService
    {
        protected readonly IRepository _repository;

        protected BaseService(IRepository repository)
        {
            _repository = repository;
        }
    }
}
