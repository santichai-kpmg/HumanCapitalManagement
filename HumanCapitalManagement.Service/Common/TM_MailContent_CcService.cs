using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_MailContent_CcService : ServiceBase<TM_MailContent_Cc>
    {
        public TM_MailContent_CcService(IRepository<TM_MailContent_Cc> repo) : base(repo)
        {
            //
        }
        public TM_MailContent_Cc Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public int AddAndUpdate(TM_MailContent_Cc s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.user_no == s.user_no && w.MailContent.Id == s.MailContent.Id).FirstOrDefault();
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
