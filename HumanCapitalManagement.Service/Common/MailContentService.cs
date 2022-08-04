using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class MailContentService : ServiceBase<MailContent>
    {
        public MailContentService(IRepository<MailContent> repo) : base(repo)
        {
            //
        }
        public MailContent Find(int id)
        {
            return Query().Any(s => (s.nCId.HasValue ? s.nCId == id : s.Id == id)) ? Query().FirstOrDefault(s => (s.nCId.HasValue ? s.Id == id : s.Id == id)) : null;
        }
        public MailContent FindByType(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<MailContent> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<MailContent> GetSubGroupForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<MailContent> GetMailContent(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.mail_type + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.mail_type + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            return sQuery.ToList();
        }

        #region Save Edit Delect 

        public int Update(MailContent s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
