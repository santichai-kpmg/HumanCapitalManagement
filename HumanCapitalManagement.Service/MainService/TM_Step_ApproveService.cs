using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Step_ApproveService : ServiceBase<TM_Step_Approve>
    {
        public TM_Step_ApproveService(IRepository<TM_Step_Approve> repo) : base(repo)
        {
            //
        }
        public IEnumerable<TM_Step_Approve> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
