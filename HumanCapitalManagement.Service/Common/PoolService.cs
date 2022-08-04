using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class PoolService : ServiceBase<TM_Pool>
    {
        public PoolService(IRepository<TM_Pool> repo) : base(repo)
        {
            //
        }
        public TM_Pool Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Pool FindByCode(string code, string company_code)
        {
            return Query().SingleOrDefault(s => s.Pool_code == code && s.TM_Company.Country_code == company_code);
        }

        public IEnumerable<TM_Pool> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Pool> GetTM_Pool(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.Pool_name_en + "").Trim().ToLower() + (w.Pool_name_th + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.Pool_name_en + "").Trim().ToLower() + (w.Pool_name_th + "").Trim().ToLower())));
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
        public bool CanSave(TM_Pool TM_Pool)
        {
            bool sCan = false;

            if (TM_Pool.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.Pool_name_en + "").Trim() == (TM_Pool.Pool_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.Pool_name_en + "").Trim() == (TM_Pool.Pool_name_en + "").Trim() && w.Id != TM_Pool.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        public int Update(TM_Pool s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.Pool_name_en = s.Pool_name_en;
                _getData.Pool_short_name_en = (s.Pool_short_name_en + "").Trim();
                _getData.Pool_description = s.Pool_description;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
