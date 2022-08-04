using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_Pool_RankService : ServiceBase<TM_Pool_Rank>
    {
        public TM_Pool_RankService(IRepository<TM_Pool_Rank> repo) : base(repo)
        {
            //
        }
        public TM_Pool_Rank Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Pool_Rank> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Pool_Rank> GetPool_Rank(string name, string status, int? pool_id)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.Pool_rank_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.Pool_rank_name_en + "").Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            if (pool_id.HasValue && pool_id != 0)
            {

                sQuery = sQuery.Where(w => w.TM_Pool.Id == pool_id.Value);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public bool CanSave(TM_Pool_Rank TM_Pool_Rank)
        {
            bool sCan = false;

            if (TM_Pool_Rank.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.Pool_rank_name_en + "").Trim() == (TM_Pool_Rank.Pool_rank_name_en + "").Trim() && w.TM_Pool.Id == TM_Pool_Rank.TM_Pool.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.Pool_rank_name_en + "").Trim() == (TM_Pool_Rank.Pool_rank_name_en + "").Trim() && w.Id != TM_Pool_Rank.Id && w.TM_Pool.Id == TM_Pool_Rank.TM_Pool.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Pool_Rank s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Pool_Rank s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.Pool_rank_name_en = (s.Pool_rank_name_en + "").Trim();
                _getData.Pool_rank_short_name_en = (s.Pool_rank_short_name_en + "").Trim();
                _getData.Pool_rank_description = s.Pool_rank_description;
                // _getData.TM_Pool = s.TM_Pool;
                _getData.TM_Rank = s.TM_Rank;

                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
