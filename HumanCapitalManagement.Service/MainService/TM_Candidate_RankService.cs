using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_RankService : ServiceBase<TM_Candidate_Rank>
    {
        public TM_Candidate_RankService(IRepository<TM_Candidate_Rank> repo) : base(repo)
        {
            //
        }
        public TM_Candidate_Rank Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Candidate_Rank FindForSelect(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.active_status == "Y");
        }
        public IEnumerable<TM_Candidate_Rank> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }   
        public IEnumerable<TM_Candidate_Rank> GetCRank(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.crank_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.crank_name_en + "").Trim().ToLower())));
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
        public bool CanSave(TM_Candidate_Rank TM_Candidate_Rank)
        {
            bool sCan = false;

            if (TM_Candidate_Rank.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.crank_name_en + "").Trim() == (TM_Candidate_Rank.crank_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.crank_name_en + "").Trim() == (TM_Candidate_Rank.crank_name_en + "").Trim() && w.Id != TM_Candidate_Rank.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Candidate_Rank s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Candidate_Rank s)
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
