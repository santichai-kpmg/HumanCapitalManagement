using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.EduService
{
    public class TM_MajorService : ServiceBase<TM_Major>
    {
        public TM_MajorService(IRepository<TM_Major> repo) : base(repo)
        {
            //
        }
        public TM_Major Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Major> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Major> GetMajorForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_Major> GetMajor(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.major_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.major_name_en + "").Trim().ToLower())));
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
        public bool CanSave(TM_Major TM_Major)
        {
            bool sCan = false;

            if (TM_Major.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.major_name_en + "").Trim() == (TM_Major.major_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.major_name_en + "").Trim() == (TM_Major.major_name_en + "").Trim() && w.Id != TM_Major.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Major s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Major s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.major_name_en = (s.major_name_en + "").Trim();
                _getData.major_description = s.major_description;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
