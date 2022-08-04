using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class RankService : ServiceBase<TM_Rank>
    {
        public RankService(IRepository<TM_Rank> repo) : base(repo)
        {
            //
        }
        public TM_Rank Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Rank> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Rank> GetRank(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.rank_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.rank_name_en + "").Trim().ToLower())));
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
        public bool CanSave(TM_Rank TM_Rank)
        {
            bool sCan = false;

            if (TM_Rank.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.rank_name_en + "").Trim() == (TM_Rank.rank_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.rank_name_en + "").Trim() == (TM_Rank.rank_name_en + "").Trim() && w.Id != TM_Rank.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Rank s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Rank s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.rank_name_en = (s.rank_name_en + "").Trim();
                _getData.rank_short_name_en = (s.rank_short_name_en + "").Trim();
                _getData.rank_description = s.rank_description;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
