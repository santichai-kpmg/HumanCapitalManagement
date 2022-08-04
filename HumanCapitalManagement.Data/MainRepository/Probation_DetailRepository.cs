using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Probation_DetailRepository : RepositoryBase<Probation_Details>
    {
        public Probation_DetailRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
