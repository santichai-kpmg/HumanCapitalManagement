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
    public class Probation_FormRepository : RepositoryBase<Probation_Form>
    {
        public Probation_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
