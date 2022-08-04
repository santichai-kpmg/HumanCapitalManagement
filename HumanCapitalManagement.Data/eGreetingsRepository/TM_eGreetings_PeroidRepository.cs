using HumanCapitalManagement.Models.eGreetings;


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.eGreetingsRepository
{
    public class TM_eGreetings_PeroidRepository : RepositoryBase<TM_eGreetings_Peroid>
    {
        public TM_eGreetings_PeroidRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
