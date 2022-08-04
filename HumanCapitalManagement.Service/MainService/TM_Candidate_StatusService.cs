using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_StatusService : ServiceBase<TM_Candidate_Status>
    {
        public TM_Candidate_StatusService(IRepository<TM_Candidate_Status> repo) : base(repo)
        {
            //
        }
        public TM_Candidate_Status Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Candidate_Status> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_Status> GetDataForBox(int nID)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id != nID);
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_Status> GetCandidateStatus(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.candidate_status_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.candidate_status_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_Status> GetStatusForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
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
        public int Update(TM_Candidate_Status s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
