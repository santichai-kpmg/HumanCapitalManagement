using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_Pool_Approve_PermitService : ServiceBase<TM_Pool_Approve_Permit>
    {

        public TM_Pool_Approve_PermitService(IRepository<TM_Pool_Approve_Permit> repo) : base(repo)
        {
            //
        }
        public TM_Pool_Approve_Permit Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public int AddAndUpdateApprover(TM_Pool_Approve_Permit s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.user_no == s.user_no && w.TM_Pool.Id == s.TM_Pool.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.update_user = s.update_user;
                _getData.update_date = s.update_date;
                _getData.active_status = s.active_status;
                if (s.active_status + "" == "Y")
                {
                    _getData.description = s.description;
                }

            }
            else
            {
                Add(s);
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
