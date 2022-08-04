using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_PositionRepository : RepositoryBase<TM_Position>
    {
        public TM_PositionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
