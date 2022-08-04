using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Models.MainModels;

namespace HumanCapitalManagement.Data.MainRepository
{    
    public class TM_FY_DetailRepository : RepositoryBase<TM_FY_Detail>
    {
        public TM_FY_DetailRepository(DbContext context) : base(context)
        {

        }
    }



}
