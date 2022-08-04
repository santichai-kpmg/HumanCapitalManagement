using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.AddressRepository
{
    public class TM_CityRepository : RepositoryBase<TM_City>
    {
        public TM_CityRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
