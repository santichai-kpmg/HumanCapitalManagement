using System;
using HumanCapitalManagement.Models.MainModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace HumanCapitalManagement.Data.MainRepository
{
     public class TM_SourcingChannelRepository : RepositoryBase<TM_SourcingChannel>
    {
        public TM_SourcingChannelRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
