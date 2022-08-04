using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
   
    public class TM_Time_TypeRepository : RepositoryBase<TM_Time_Type>
    {
        public TM_Time_TypeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
