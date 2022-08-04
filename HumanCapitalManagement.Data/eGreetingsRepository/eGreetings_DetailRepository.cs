using HumanCapitalManagement.Models.eGreetings;


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.eGreetingsRepository
{
    public class eGreetings_DetailRepository : RepositoryBase<eGreetings_Detail>
    {
        public eGreetings_DetailRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
