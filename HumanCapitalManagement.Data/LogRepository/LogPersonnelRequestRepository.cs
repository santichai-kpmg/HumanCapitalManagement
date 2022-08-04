using HumanCapitalManagement.Models.LogModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.LogRepository
{
    public class LogPersonnelRequestRepository : RepositoryBase<LogPersonnelRequest>
    {
        public LogPersonnelRequestRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
