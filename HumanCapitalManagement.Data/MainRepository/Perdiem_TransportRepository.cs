using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Perdiem_TransportRepository : RepositoryBase<Perdiem_Transport>
    {
        public Perdiem_TransportRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
