using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.EducationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.EduService
{
    public class TM_Universitys_MajorService : ServiceBase<TM_Universitys_Major>
    {
        public TM_Universitys_MajorService(IRepository<TM_Universitys_Major> repo) : base(repo)
        {
            //
        }
        public TM_Universitys_Major Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Universitys_Major FindForEdit(int id, int nUni_id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.TM_Universitys_Faculty.TM_Universitys.Id== nUni_id);
        }
        public IEnumerable<TM_Universitys_Major> FindListByUniver(int id)
        {
            return Query().Where(s => s.TM_Universitys_Faculty.TM_Universitys.Id == id).ToList();
        }
        public IEnumerable<TM_Universitys_Major> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public bool CanSave(TM_Universitys_Major TM_Universitys_Major)
        {
            bool sCan = false;

            if (TM_Universitys_Major.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w =>
                w.TM_Universitys_Faculty.TM_Universitys.Id == TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.Id &&
                w.TM_Major.Id == TM_Universitys_Major.TM_Major.Id
                );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w =>
                //(w.universitys_faculty_name_en + "").Trim() == (TM_Universitys_Major.universitys_faculty_name_en + "").Trim() && 
                w.Id != TM_Universitys_Major.Id &&
                w.TM_Universitys_Faculty.TM_Universitys.Id == TM_Universitys_Major.TM_Universitys_Faculty.TM_Universitys.Id &&
                w.TM_Major.Id == TM_Universitys_Major.TM_Major.Id
                );
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Universitys_Major s)
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
                    _getData.universitys_major_name_en = (s.universitys_major_name_en + "").Trim();
                    _getData.universitys_major_description = s.universitys_major_description;
                    _getData.TM_Universitys_Faculty = s.TM_Universitys_Faculty;
                    _getData.TM_Major = s.TM_Major;
                }
            }
            sResult = SaveChanges();
            return sResult;
        }
        #endregion
    }
}
