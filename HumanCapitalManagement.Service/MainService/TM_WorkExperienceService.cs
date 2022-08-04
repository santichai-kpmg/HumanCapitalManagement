using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_WorkExperienceService : ServiceBase<TM_WorkExperience>
    {
        public TM_WorkExperienceService(IRepository<TM_WorkExperience> repo) : base(repo)
        {
            //
        }

        public TM_WorkExperience Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_WorkExperience> GetDataForSelect()
        {
            //var sQuery = Query().Where(w => w.active_status == "Y");
            var sQuery = Query();
            return sQuery.ToList();
        }
        public IEnumerable<TM_WorkExperience> GetDataForSelect_AllStatus()
        {
            var sQuery = Query();
            return sQuery.ToList();
        }
        public IEnumerable<TM_WorkExperience> GetFacultyForSave(int[] aID)//,bool isAdmin
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
        //public IEnumerable<TM_WorkExperience> GetFaculty(string name, string status)//,bool isAdmin
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
        public bool CanSave(TM_WorkExperience TM_WorkExperience)
        {
            bool sCan = false;

            //if (TM_WorkExperience.Id == 0)
            //{
            //    var sCheck = Query().FirstOrDefault(w => w.TM_Candidates.Id == TM_WorkExperience.TM_Candidates.Id && w.TM_Universitys_Major.Id == TM_WorkExperience.TM_Universitys_Major.Id);
            //    if (sCheck == null)
            //    {
            //        sCan = true;
            //    }
            //}
            //else
            //{
            //    var sCheck = Query().FirstOrDefault(w => w.TM_Candidates.Id == TM_WorkExperience.TM_Candidates.Id && w.TM_Universitys_Major.Id == TM_WorkExperience.TM_Universitys_Major.Id && w.Id != TM_WorkExperience.Id);
            //    if (sCheck == null)
            //    {
            //        sCan = true;
            //    }
            //}
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNewAndEdit(TM_WorkExperience s)
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
                    _getData.CompanyName = s.CompanyName;
                    _getData.JobPosition = s.JobPosition;
                    if (!string.IsNullOrEmpty(s.active_status))
                    {
                        _getData.active_status = s.active_status;
                    }
                    _getData.TypeOfRelatedToJob = s.TypeOfRelatedToJob;
                    _getData.StartDate = s.StartDate;
                    _getData.EndDate = s.EndDate;
                    _getData.base_salary = s.base_salary;
                    _getData.transportation = s.transportation;
                    _getData.mobile_allowance = s.mobile_allowance;
                    _getData.position_allowance = s.position_allowance;
                    _getData.other_allowance = s.other_allowance;
                    _getData.annual_leave = s.annual_leave;
                    _getData.variable_bonus = s.variable_bonus;
                    _getData.expected_salary = s.expected_salary;
                    _getData.update_date = s.update_date;
                    _getData.update_user = s.update_user;
                }
            }
            sResult = SaveChanges();

            return sResult;
        }
        public int InActive(TM_WorkExperience s)
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
