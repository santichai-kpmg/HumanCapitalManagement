using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class CompanyService : ServiceBase<TM_Company>
    {
        public CompanyService(IRepository<TM_Company> repo) : base(repo)
        {
            //
        }
        public TM_Company Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }


        public IEnumerable<TM_Company> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Company> GetTM_Pool(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.Company_name_en + "").Trim().ToLower() + (w.Company_name_th + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.Company_name_en + "").Trim().ToLower() + (w.Company_name_th + "").Trim().ToLower())));
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
        public bool CanSave(TM_Company TM_Company)
        {
            bool sCan = false;

            if (TM_Company.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.Company_name_en + "").Trim() == (TM_Company.Company_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.Company_name_en + "").Trim() == (TM_Company.Company_name_en + "").Trim() && w.Id != TM_Company.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        public int Update(TM_Company s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.Company_name_en = s.Company_name_en;
                _getData.Company_short_name_en = (s.Company_short_name_en + "").Trim();
                _getData.Company_description = s.Company_description;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
