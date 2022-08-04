using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.EduService
{
    public class TM_Universitys_FacultyService : ServiceBase<TM_Universitys_Faculty>
    {
        public TM_Universitys_FacultyService(IRepository<TM_Universitys_Faculty> repo) : base(repo)
        {
            //
        }
        public TM_Universitys_Faculty Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Universitys_Faculty FindForEdit(int id, int nUni_id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.TM_Universitys.Id == nUni_id);
        }
      
        public IEnumerable<TM_Universitys_Faculty> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Universitys_Faculty> GetMajorForSave(int[] aID)//,bool isAdmin
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
        public bool CanSave(TM_Universitys_Faculty TM_Universitys_Faculty)
        {
            bool sCan = false;

            if (TM_Universitys_Faculty.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w =>
                w.TM_Universitys.Id == TM_Universitys_Faculty.TM_Universitys.Id &&
                w.TM_Faculty.Id == TM_Universitys_Faculty.TM_Faculty.Id
                );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w =>
                //(w.universitys_faculty_name_en + "").Trim() == (TM_Universitys_Faculty.universitys_faculty_name_en + "").Trim() && 
                w.Id != TM_Universitys_Faculty.Id &&
                w.TM_Universitys.Id == TM_Universitys_Faculty.TM_Universitys.Id &&
                w.TM_Faculty.Id == TM_Universitys_Faculty.TM_Faculty.Id
                );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Universitys_Faculty s)
        {
            var sResult = 0;
            if (s.Id == 0)
            {
                Add(s);
            }
            else
            {
                var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
                if (_getData != null)
                {
                    _getData.active_status = s.active_status;
                    _getData.update_date = s.update_date;
                    _getData.update_user = s.update_user;
                    _getData.universitys_faculty_name_en = (s.universitys_faculty_name_en + "").Trim();
                    _getData.universitys_faculty_description = s.universitys_faculty_description;
                    _getData.TM_Faculty = s.TM_Faculty;
                }
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Universitys_Faculty s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.universitys_faculty_name_en = (s.universitys_faculty_name_en + "").Trim();
                _getData.universitys_faculty_description = s.universitys_faculty_description;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
