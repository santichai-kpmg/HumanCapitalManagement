using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TimeSheet_DetailRepository : RepositoryBase<TimeSheet_Detail>
    {
        public TimeSheet_DetailRepository(DbContext context) : base(context)
        {
            //
        }
    }
   
}
