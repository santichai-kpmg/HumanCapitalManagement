using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HumanCapitalManagement.Service.TraineeSite
{
    public class TimeSheet_DetailService : ServiceBase<TimeSheet_Detail>
    {
        public TimeSheet_DetailService(IRepository<TimeSheet_Detail> repo) : base(repo)
        {
            //
        }
        public TimeSheet_Detail Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TimeSheet_Detail GetActiveTimeSheetDetail()
        {
            return Query().Where(s => s.active_status == "Y").OrderByDescending(o => o.trainee_update_date).FirstOrDefault();
        }
        public IEnumerable<TimeSheet_Detail> GetDataForSelect()
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public IEnumerable<TimeSheet_Detail> GetDataByTrainee(int ID)
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y" && w.trainee_create_user == ID);
            return sQuery.ToList();
        }

        public IEnumerable<TimeSheet_Detail> GetDataForSelectbyTimeSheetForm(TimeSheet_Form timsheetform)
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public IEnumerable<TimeSheet_Detail> Get_By_TimeSheetFrom_ID(int timesheetformID)
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TimeSheet_Form.Id == timesheetformID);
            return sQuery.ToList();
        }

        public IEnumerable<TimeSheet_Detail> Get_By_Approve_ID(string Approve_ID)
        {
            //var sQuery = Query();
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Approve_user == Approve_ID);
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref TimeSheet_Detail s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TimeSheet_Detail s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TimeSheet_Detail s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id && w.TimeSheet_Form.Id == s.TimeSheet_Form.Id && w.active_status != "N").ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "N";
                    ed.trainee_update_date = DateTime.Now;
                    ed.trainee_update_user = s.trainee_update_user;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }

        public int UpdateApproveDetail(TimeSheet_Detail s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "Y";
                    ed.approve_status = s.approve_status;


                    if (s.approve_status == "A")
                    {
                        ed.review_date = DateTime.Now;
                        ed.review_date = s.review_date;
                    }
                    else if (s.approve_status == "P")
                    {
                        ed.paid_date = DateTime.Now;
                        ed.paid_user = s.paid_user;
                    }
                    else
                    {
                        ed.Approve_user = s.Approve_user;
                        ed.approve_date = DateTime.Now;
                    }

                    
                   
                });
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(TimeSheet_Detail s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData == null)
            {
                Add(s);

                sResult = SaveChanges();
            }
            else
            {

                if (_getData.approve_status != "Y")
                {
                    _getData.date_start = s.date_start;
                    _getData.date_end = s.date_end;
                    _getData.TM_Time_Type_Id = s.TM_Time_Type_Id;
                    _getData.submit_status = s.submit_status;
                    _getData.Approve_user = s.Approve_user;
                    _getData.trainee_update_date = DateTime.Now;
                    _getData.trainee_update_user = s.trainee_update_user;
                    _getData.TimeSheet_Form = s.TimeSheet_Form;

                    sResult = SaveChanges();
                }
                else
                {
                    sResult = 1;
                }

            }
            //sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdateCheck(TimeSheet_Detail s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData == null)
            {
                Add(s);

                sResult = SaveChanges();
            }
            else
            {

                if (_getData.approve_status == "N" || _getData.approve_status == "R")
                {
                    _getData.submit_status = s.submit_status;
                    if (s.submit_status == "S")
                        _getData.submit_date = DateTime.Now;

                    _getData.Approve_user = s.Approve_user;
                    if (_getData.approve_status == "R")
                    {
                        _getData.approve_status = "N";
                    }
                    sResult = SaveChanges();
                }


            }
            //sResult = SaveChanges();
            return sResult;
        }
        #endregion
    }
}
