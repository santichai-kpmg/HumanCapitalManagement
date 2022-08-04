using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TimeSheet_FormRepository : RepositoryBase<TimeSheet_Form>
    {
        public TimeSheet_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
   
}
