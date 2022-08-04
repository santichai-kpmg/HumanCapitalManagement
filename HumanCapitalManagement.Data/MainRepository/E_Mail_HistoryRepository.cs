using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class E_Mail_HistoryRepository : RepositoryBase<E_Mail_History>
    {
        public E_Mail_HistoryRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
