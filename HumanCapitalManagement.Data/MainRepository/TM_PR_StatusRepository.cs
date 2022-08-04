using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_PR_StatusRepository : RepositoryBase<TM_PR_Status>
    {
        public TM_PR_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
