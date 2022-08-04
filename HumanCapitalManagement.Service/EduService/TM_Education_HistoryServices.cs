using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.EduService
{


    //public class TM_Education_HistoryService : ServiceBase<TM_Education_History>
    //{
    public class TM_Education_HistoryServices : ServiceBase<TM_Education_History>
    {
        public TM_Education_HistoryServices(IRepository<TM_Education_History> repo) : base(repo)
        {
            //
        }
        public TM_Education_History Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Education_History> GetDataForSelect()
        {
            var sQuery = Query();
            return sQuery.ToList();
        }
        public IEnumerable<TM_Education_History> GetFacultyForSave(int[] aID)//,bool isAdmin
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
        //public IEnumerable<TM_Education_History> GetFaculty(string name, string status)//,bool isAdmin
        //{
        //    var sQuery = Query().Where(w => 1 == 1);

        //    if (!string.IsNullOrEmpty(name))
        //    {
        //        sQuery = sQuery.Where(w => ((w.faculty_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.faculty_name_en + "").Trim().ToLower())));
        //    }
        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        sQuery = sQuery.Where(w => w.active_status == status);
        //    }
        //    //if(!isAdmin)
        //    //{
        //    //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
        //    //}
        //    return sQuery.ToList();
        //}
        public bool CanSave(TM_Education_History TM_Education_History)
        {
            bool sCan = false;

            if (TM_Education_History.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => w.TM_Candidates.Id == TM_Education_History.TM_Candidates.Id && w.TM_Universitys_Major.Id == TM_Education_History.TM_Universitys_Major.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => w.TM_Candidates.Id == TM_Education_History.TM_Candidates.Id && w.TM_Universitys_Major.Id == TM_Education_History.TM_Universitys_Major.Id && w.Id != TM_Education_History.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNewAndEdit(TM_Education_History s)
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
                    _getData.Degree = s.Degree;
                    _getData.update_date = s.update_date;
                    _getData.update_user = s.update_user;
                    _getData.grade = s.grade;
                    _getData.start_date = s.start_date;
                    _getData.end_date = s.end_date;
                    _getData.education_history_description = s.education_history_description;
                    _getData.TM_Education_Degree = s.TM_Education_Degree;
                    _getData.TM_Universitys_Major = s.TM_Universitys_Major;
                    _getData.Ref_Cert_ID = s.Ref_Cert_ID;
                }
            }
            sResult = SaveChanges();

            return sResult;
        }
        public int InActive(TM_Education_History s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;

                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
