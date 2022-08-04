using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.AddressRepository
{
    public class TM_DistrictRepository : RepositoryBase<TM_District>
    {
        public TM_DistrictRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
