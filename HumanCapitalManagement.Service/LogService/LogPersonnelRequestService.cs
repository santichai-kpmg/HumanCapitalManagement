using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.LogModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.LogService
{
    public class LogPersonnelRequestService : ServiceBase<LogPersonnelRequest>
    {
        public LogPersonnelRequestService(IRepository<LogPersonnelRequest> repo) : base(repo)
        {
            //
        }
        public LogPersonnelRequest Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<LogPersonnelRequest> GetListData(int[] pr_id)//,bool isAdmin
        {
            int[] aStatus = new int[] { 2, 3, 4 };
            var sQuery = Query();
            if (pr_id.Length > 0)
            {
                sQuery = sQuery.Where(w => pr_id.Contains((int)w.PersonnelRequest_Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public int CreateNew(LogPersonnelRequest s)
        {

            int nSeq = 1;
            if (Query().Where(w => w.PersonnelRequest_Id == s.PersonnelRequest_Id).Any())
            {
                nSeq = Query().Where(w => w.PersonnelRequest_Id == s.PersonnelRequest_Id).Max(m => m.seq.HasValue ? m.seq.Value : 0) + 1;
            }
            s.seq = nSeq;

            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
    }
}
