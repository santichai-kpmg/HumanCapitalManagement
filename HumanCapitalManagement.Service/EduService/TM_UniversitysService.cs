using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.EduService
{
    public class TM_UniversitysService : ServiceBase<TM_Universitys>
    {
        public TM_UniversitysService(IRepository<TM_Universitys> repo) : base(repo)
        {
            //
        }
        public TM_Universitys Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Universitys> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Universitys> GetUniversityForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_Universitys> GetUniversity(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.universitys_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.universitys_name_en + "").Trim().ToLower())));
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
        public bool CanSave(TM_Universitys TM_Universitys)
        {
            bool sCan = false;

            if (TM_Universitys.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.universitys_name_en + "").Trim() == (TM_Universitys.universitys_name_en + "").Trim()
          
                );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => ((w.universitys_name_en + "").Trim() == (TM_Universitys.universitys_name_en + "").Trim()
               
                ) && w.Id != TM_Universitys.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Universitys s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Universitys s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.universitys_name_en = (s.universitys_name_en + "").Trim();
                _getData.universitys_aol_name = (s.universitys_aol_name + "").Trim();
                _getData.universitys_description = s.universitys_description;

                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
