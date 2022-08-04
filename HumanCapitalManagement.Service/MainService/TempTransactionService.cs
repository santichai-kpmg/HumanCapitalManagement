using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HumanCapitalManagement.Service.MainService
{
    public class TempTransactionService : ServiceBase<TempTransaction>
    {
        public TempTransactionService(IRepository<TempTransaction> repo) : base(repo)
        {
            //
        }
        public TempTransaction Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
    } 
}
