using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.OldTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class DivisionService : ServiceBase<TM_Divisions>
    {
        public DivisionService(IRepository<TM_Divisions> repo) : base(repo)
        {
            //
        }

        public TM_Divisions Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Divisions FindByCode(string code)
        {
            return Query().SingleOrDefault(s => s.division_code == code);
        }
        public IEnumerable<TM_Divisions> GetDivisionForSave(string[] aUnitcode)//,bool isAdmin
        {
            var sQuery = Query();
            if (aUnitcode != null)
            {
                sQuery = sQuery.Where(w => aUnitcode.Contains(w.division_code));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }

        public IEnumerable<TM_Divisions> GetAll()//,bool isAdmin
        {
            return Query().ToList();
        }

        public IEnumerable<TM_Divisions> GetForFeedback()//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Pool.TM_Company.Company_code == "4100" && w.TM_Pool.Pool_code == "4");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Divisions> GetTM_Divisions(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.division_name_th + "").Trim().ToLower() + (w.division_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.division_name_th + "").Trim().ToLower() + (w.division_name_en + "").Trim().ToLower())));

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
        public IEnumerable<TM_Divisions> GetDivisionForReport()//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Pool.TM_Company.Company_code == "4100");
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<TM_Divisions> GetDivisionForPES()//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public bool CanSave(TM_Divisions TM_Divisions)
        {
            bool sCan = false;

            if (TM_Divisions.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.division_name_en + "").Trim() == (TM_Divisions.division_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.division_name_en + "").Trim() == (TM_Divisions.division_name_en + "").Trim() && w.Id != TM_Divisions.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Divisions s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Divisions s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                //_getData.active_status = s.active_status;
                //_getData.division_name_en =( s.division_name_en + "").Trim();
                //_getData.division_short_name_en = s.division_short_name_en;
                //_getData.update_date = s.update_date;
                //_getData.update_user = s.update_user;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
