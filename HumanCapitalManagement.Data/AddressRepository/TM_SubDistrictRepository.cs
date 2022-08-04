using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.AddressRepository
{
    public class TM_SubDistrictRepository : RepositoryBase<TM_SubDistrict>
    {
        public TM_SubDistrictRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
