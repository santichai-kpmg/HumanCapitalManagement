using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.EduRepository
{
    public class TM_Education_HistoryRepository : RepositoryBase<TM_Education_History>
    {
        public TM_Education_HistoryRepository(DbContext context) : base(context)
        {
            //
        }
    }



}
