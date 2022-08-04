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
    public class Probation_With_OutRepository : RepositoryBase<Probation_With_Out>
    {
        public Probation_With_OutRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
