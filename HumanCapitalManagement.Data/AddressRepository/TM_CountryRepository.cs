using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.AddressRepository
{
 
    public class TM_CountryRepository : RepositoryBase<TM_Country>
    {
        public TM_CountryRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
