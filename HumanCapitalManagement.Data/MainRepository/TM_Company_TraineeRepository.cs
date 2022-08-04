using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Company_TraineeRepository : RepositoryBase<TM_Company_Trainee>
    {
        public TM_Company_TraineeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
