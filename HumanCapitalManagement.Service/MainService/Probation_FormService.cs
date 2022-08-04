using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.MainService
{
    public class Probation_FormService : ServiceBase<Probation_Form>
    {
        public Probation_FormService(IRepository<Probation_Form> repo) : base(repo)
        {
            //
        }
        public Probation_Form Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.Active_Status == "Y");
        }

        public Probation_Form FindExtendForm(int id)
        {
            return Query().SingleOrDefault(s => s.Extend_Form == id.ToString() && s.Active_Status == "Y");
        }

        public IEnumerable<Probation_Form> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<Probation_Form> GetDataExtendForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y" && w.Extend_Status == "Y");
            return sQuery.ToList();
        }

        public IEnumerable<Probation_Form> GetDataWithOutExtendForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y" && w.Extend_Status != "Y");
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref Probation_Form s)
        {
            //s.Id = SelectMax();


            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdate(Probation_Form s)
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
                if (!String.IsNullOrEmpty(s.Assessment))
                    _getData.Assessment = s.Assessment;
                if (!String.IsNullOrEmpty(s.Remark))
                    _getData.Remark = s.Remark;

                if (!String.IsNullOrEmpty(s.GroupHead_No))
                    _getData.GroupHead_No = s.GroupHead_No;

                if (s.PM_Submit_Date != null)
                {
                    _getData.PM_Submit_Date = s.PM_Submit_Date;
                    if (!String.IsNullOrEmpty(s.PM_Action))
                        _getData.PM_Action = s.PM_Action;
                }


                //if (s.Status == "R")
                //{
                //    _getData.PM_Submit_Date = s.PM_Submit_Date;
                //    _getData.Staff_Acknowledge_Date = s.Staff_Acknowledge_Date;
                //}

                if (s.Staff_Acknowledge_Date != null)
                {
                    _getData.Staff_Acknowledge_Date = s.Staff_Acknowledge_Date;

                }
                if (!String.IsNullOrEmpty(s.Staff_Action))
                    _getData.Staff_Action = s.Staff_Action;

                if (s.GroupHead_Submit_Date != null)
                {
                    _getData.GroupHead_Submit_Date = s.GroupHead_Submit_Date;
                    if (!String.IsNullOrEmpty(s.GroupHead_Action))
                        _getData.GroupHead_Action = s.GroupHead_Action;
                }

                _getData.HOP_No = s.HOP_No;
                if (s.HOP_Submit_Date != null)
                {
                    _getData.HOP_Submit_Date = s.HOP_Submit_Date;
                    if (!String.IsNullOrEmpty(s.HOP_Action))
                        _getData.HOP_Action = s.HOP_Action;
                }

                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = s.Update_User;
                _getData.Status = s.Status;

                if (!String.IsNullOrEmpty(s.Mail_Send))
                    _getData.Mail_Send = s.Mail_Send;

                if (s.Probation_Details != null && s.Probation_Details.Count > 0)
                    _getData.Probation_Details = s.Probation_Details;

                if (!String.IsNullOrEmpty(s.Remark_Revise))
                    _getData.Remark_Revise = s.Remark_Revise;

                if (!String.IsNullOrEmpty(s.Extend_Period))
                    _getData.Extend_Period = s.Extend_Period;

                if (s.Status == "PM")
                {
                    if (s.Send_Mail_Date != null)
                        _getData.Send_Mail_Date = s.Send_Mail_Date;
                }
                else
                {
                    _getData.Send_Mail_Date = null;
                }
                //foreach (var ex in _getData.Probation_Details)
                //{
                //    s.Probation_Details.Where(w=> w.Id == ex.Id);
                //}

                sResult = SaveChanges();


            }
            //sResult = SaveChanges();
            return sResult;
        }

        public int Reset(Probation_Form s)
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

                _getData.Status = s.Status;
                _getData.Staff_Acknowledge_Date = null;
                _getData.PM_Submit_Date = null;
                _getData.GroupHead_Submit_Date = null;
                _getData.HOP_Submit_Date = null;
                _getData.Staff_Action = null;
                _getData.PM_Action = null;
                _getData.GroupHead_Action = null;
                _getData.HOP_Action = null;

                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = s.Update_User;

                _getData.Assessment = null;


                sResult = SaveChanges();


            }
            //sResult = SaveChanges();
            return sResult;
        }

        public int UpdateStatus(int id, string update_user, string status)
        {
            var sResult = 0;

            var _getData = Query().Where(w => w.Id == id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.Status = status;
                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = update_user;

                sResult = SaveChanges();
            }

            return sResult;
        }

        public int SetInActive(int id, string update_user, string status)
        {
            var sResult = 0;

            var _getData = Query().Where(w => w.Id == id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.Active_Status = status;
                _getData.Update_Date = DateTime.Now;
                _getData.Update_User = update_user;

                sResult = SaveChanges();
            }

            return sResult;
        }

        public int AdminUpdate(Probation_Form s)
        {
            var sResult = 0;

            var _getDataAD = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataAD != null)
            {
                //_getDataAD.update_user = s.update_user;
                //_getDataAD.update_date = s.update_date;
                //_getDataAD.job_descriptions = s.job_descriptions;
                //_getDataAD.qualification_experience = s.qualification_experience;
                //_getDataAD.remark = s.remark;
                //_getDataAD.active_status = s.active_status;
                ////    _getDataAD.TM_SubGroup_Id = s.TM_SubGroup_Id;
                //_getDataAD.TM_Pool_Rank = s.TM_Pool_Rank;
                //_getDataAD.TM_Position = s.TM_Position;
                //_getDataAD.TM_PR_Status = s.TM_PR_Status;
                //_getDataAD.no_of_headcount = s.no_of_headcount;

                sResult = SaveChanges();
            }
            return sResult;

        }
        #endregion
    }
}
