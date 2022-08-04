using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
 
    public class TM_MaritalStatusService : ServiceBase<TM_MaritalStatus>
    {
        public TM_MaritalStatusService(IRepository<TM_MaritalStatus> repo) : base(repo)
        {
            //
        }

        public TM_MaritalStatus Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_MaritalStatus> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public IEnumerable<TM_MaritalStatus> GetMaritalStatusForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }


    }
}



