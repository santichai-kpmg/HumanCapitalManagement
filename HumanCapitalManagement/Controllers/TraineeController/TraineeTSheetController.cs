using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TraineeSite;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.ViewModel.Trainee.Engagement;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineeTSheetController : BaseController
    {
        private TimeSheet_FormService _TimeSheet_FormService;
        private TimeSheet_DetailService _TimeSheet_DetailService;
        private TM_Time_TypeService _TM_Time_TypeService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public TraineeTSheetController(
             TimeSheet_FormService TimeSheet_FormService
            , TimeSheet_DetailService TimeSheet_DetailService
            , TM_Time_TypeService TM_Time_TypeService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
           )
        {

            _TimeSheet_FormService = TimeSheet_FormService;
            _TimeSheet_DetailService = TimeSheet_DetailService;
            _TM_Time_TypeService = TM_Time_TypeService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;

        }

        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();


        public ActionResult TraineeTSheetList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            //qryStr = CGlobal.UserInfo.UserId;
            vTimeSheet_obj_View result = new vTimeSheet_obj_View();
            if (!string.IsNullOrEmpty(qryStr))
            {
                //    result.IdEncrypt = qryStr;
                //    int nId = Convert.ToInt32(qryStr);
                //    //int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                //    if (nId != 0)
                //    {
                //        var _getData = _TM_CandidatesService.Find(nId);
                //        if (_getData != null)
                //        {
                //            if (_getData.Id + "" != CGlobal.UserInfo.UserId)
                //            {
                //                return RedirectToAction("TimeSheetView", "Timesheet");
                //            }

                //            int nID = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);

                //            var _getList = _TimeSheet_FormService.FindByCandidateID(nID);
                //            if (_getList != null && _getList.Any(a => a.active_status + "" == "Y"))
                //            {
                //                List<vTimeSheet_obj> mainlstdata = new List<vTimeSheet_obj>();
                //                vTimeSheet_obj lstdata = new vTimeSheet_obj();
                //                foreach (var s in _getList)
                //                {
                //                    lstdata = new vTimeSheet_obj();
                //                    lstdata.acknowledge_user = s.acknowledge_user;
                //                    lstdata.active_status = s.active_status;
                //                    lstdata.create_date = s.create_date.HasValue ? s.create_date.Value.DateTimebyCulture() : "";
                //                    lstdata.Approve_user = s.Approve_user;
                //                    lstdata.Edit_timesheet = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit_timesheet('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Timesheet <i class=""fa fa-calendar""></i></a>";
                //                    lstdata.Edit = @"<a href=""#"" class=""btn btn-animated btn-  btn-sm"" href=""#"" onclick=""Edit('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Edit <i class=""fa fa-edit""></i></a>";
                //                    mainlstdata.Add(lstdata);
                //                }
                //                result.lstData = mainlstdata;
                //                //result.lstData = _getList.Where(w => w.active_status + "" == "Y").Select(s => new vTimeSheet_obj
                //                //{
                //                //    acknowledge_user = s.acknowledge_user,
                //                //    create_date = s.create_date.HasValue ? s.create_date.Value.DateTimebyCulture() : "",
                //                //    active_status = s.active_status,

                //                //    Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Edit <i class=""fa fa-arrow-right""></i></a>"

                //                //}).ToList();
                //            }
                //        }
                //        else
                //        {
                //            return RedirectToAction("TimeSheetView", "Timesheet");
                //        }
                //    }

            }
            else
            {
                //return RedirectToAction("TimeSheetForm", "Timesheet");
            }

            return View(result);
        }


        public ActionResult TimeSheetApprove(string qryStr)
        {

            try
            {
                vTimeSheet_Detail_obj_Save result = new vTimeSheet_Detail_obj_Save();
                if (!string.IsNullOrEmpty(qryStr))
                {
                    var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
                    vObjectMail objMail = HCMFunc.DecryptUrl(qryStr);
                    string winloginname = this.User.Identity.Name;
                    string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);
                    if (!string.IsNullOrEmpty(EmpUserID))
                    {
                        if (CGlobal.IsUserExpired())
                        {
                            DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserID);
                            // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                            if (staff != null)
                            {
                                var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMail.emp_no + "").FirstOrDefault();
                                if (CheckUser != null)
                                {
                                    var Login = HCMFunc.funcLogin(CheckUser.Field<string>("EmpNo") + "");
                                }
                                else
                                {
                                    var Login = HCMFunc.funcLogin(staff.Rows[0].Field<string>("EmpNo") + "");
                                }
                            }
                        }

                        if (objMail.type + "" == "Time Sheet Submit" || getlinkid >= 0)
                        {
                            var sCheck = acCheckLoginAndPermission();
                            if (sCheck != null)
                            {
                                return sCheck;
                            }
                            var approve_user = "";
                            if (!String.IsNullOrEmpty(CGlobal.UserInfo.EmployeeNo))
                            {

                                approve_user = CGlobal.UserInfo.EmployeeNo;
                            }

                            List<Calandar_Even> mainlstdata = new List<Calandar_Even>();
                            List<vTimeSheet_Detail_obj> maindata = new List<vTimeSheet_Detail_obj>();
                            int nId = SystemFunction.GetIntNullToZero(getlinkid == 0 ? HCMFunc.Decrypt(objMail.id + "") : getlinkid.ToString());
                            var _getList = _TimeSheet_FormService.GetDataForSelect().Where(w => w.Id == nId).FirstOrDefault();
                            if (_getList != null)
                            {
                                result = new vTimeSheet_Detail_obj_Save();

                                result.Id = _getList.Id.ToString();
                                result.IdEncrypt = objMail.id;
                                result.trainee_create_user = _getList.trainee_create_user.ToString();
                                result.trainee_update_user = _getList.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getList.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                                result.lstData_cld = mainlstdata;
                                result.lstData = maindata;
                                var _getlistde = _TimeSheet_DetailService.Get_By_TimeSheetFrom_ID(nId).Where(w => w.active_status == "Y" && w.submit_status == "S" && w.Approve_user == CGlobal.UserInfo.EmployeeNo).ToList();

                                Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();

                                List<Engagement_PR> lst_Engagement = new List<Engagement_PR>();
                                var exdata = wsViztp.GetAllEngagementFromSSA("", "", "2,3", "", "", "", "", "", "", "", "", "");

                                lst_Engagement = new List<Engagement_PR>();
                                lst_Engagement = exdata.AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("EngagementNo"), text = dataRow.Field<string>("EngagementName") }).ToList();


                                var indata = wsViztp.Get_Internal_Chargecode("", "");
                                var _int = new List<Engagement_PR>();
                                _int = indata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("Code"), text = dataRow.Field<string>("Description") }).ToList();


                                lst_Engagement.AddRange(_int);

                                foreach (var i in _getlistde.OrderBy(o => o.date_start).ToList())
                                {
                                    string colors = "";
                                    if (i.submit_status == "S")
                                    {
                                        colors = "#a3ada3";
                                    }
                                    else
                                    {
                                        colors = _TM_Time_TypeService.Find(Convert.ToInt32(i.TM_Time_Type_Id)).colors;
                                    }

                                    Calandar_Even lstdata = new Calandar_Even();

                                    lstdata.id = i.Id.ToString();
                                    lstdata.title = i.title;
                                    lstdata.start = Convert.ToDateTime(i.date_start).ToString("yyyy-MM-dd HH:mm");
                                    lstdata.end = Convert.ToDateTime(i.date_end).ToString("yyyy-MM-dd HH:mm");
                                    //lstdata.allDay = "false";
                                    lstdata.backgroundColor = _TM_Time_TypeService.Find(Convert.ToInt32(i.TM_Time_Type_Id)).colors;
                                    mainlstdata.Add(lstdata);

                                }
                                foreach (var a in _getlistde.Where(w => w.approve_status == "N").OrderBy(o => o.date_start).ToList())
                                {
                                    vTimeSheet_Detail_obj lstmain = new vTimeSheet_Detail_obj();
                                    lstmain.Id = a.Id.ToString();
                                    lstmain.title = a.title;
                                    if (!String.IsNullOrEmpty(a.Engagement_Code))
                                    {
                                        if (a.Engagement_Code == "150000000001")
                                        {
                                            lstmain.Engagement_Code = "Sick Leave : 150000000001";
                                        }
                                        else if (a.Engagement_Code == "150000000000")
                                        {
                                            lstmain.Engagement_Code = "Vacation Leave : 150000000000";
                                        }
                                        else
                                        {
                                            lstmain.Engagement_Code = lst_Engagement.Where(w => w.id == a.Engagement_Code).Select(s => s.text).First() + " - " + a.Engagement_Code;
                                        }
                                    }
                                    lstmain.TM_Time_Type_Id = _TM_Time_TypeService.Find2((int)a.TM_Time_Type_Id).type_name_en;
                                    lstmain.start = Convert.ToDateTime(a.date_start).ToString("yyyy-MM-dd HH:mm");
                                    lstmain.end = Convert.ToDateTime(a.date_end).ToString("yyyy-MM-dd HH:mm");
                                    lstmain.hour = a.hours;
                                    lstmain.remark = a.remark;
                                    lstmain.approve_status = a.approve_status;
                                    //lstmain.approve_date = DateTime.Now;
                                    maindata.Add(lstmain);
                                }

                                result.lstData_cld = mainlstdata.ToList();
                                if (maindata != null)
                                    result.lstData = maindata.ToList();

                            }
                        }


                    }
                    else
                    {
                        return RedirectToAction("ErrorNopermission", "MasterPage");
                    }
                }
                else
                {
                    return RedirectToAction("ErrorNopermission", "MasterPage");
                }
                return View(
                result
            );
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");

            }


        }


        [HttpPost]
        public ActionResult LoadTraineeTSheetList(string txtname, string txtid)
        {
            List<vTimeSheet_obj_View> lstData = new List<vTimeSheet_obj_View>();
            try
            {
                var approve_user = CGlobal.UserInfo.EmployeeNo;
                var approve_users = CGlobal.UserInfo.UserId;
                //var approve_user = "00011935";

                lstData = bindlist(approve_user, txtname, txtid);
            }
            catch (Exception ex)
            {
            }
            return Json(new
            {
                lstData
            });

        }

        public List<vTimeSheet_obj_View> bindlist(string approve_user, string traineename, string traineeid)
        {

            List<vTimeSheet_obj_View> lstData = new List<vTimeSheet_obj_View>();

            var _getFirstList = _TimeSheet_DetailService.Get_By_Approve_ID(approve_user).Where(w => w.submit_status == "S" && w.active_status == "Y" && w.approve_status == "N").ToList();
            //var _group_detail = _getFirstList.GroupBy(g => g.TimeSheet_Form).ToList();
            var _group_detail_id = (from p in _getFirstList
                                    group p by p.TimeSheet_Form into g
                                    select new
                                    {
                                        id = g.Key.Id,
                                    }).ToList();

            List<TimeSheet_Form> lst_timesheet_form = new List<TimeSheet_Form>();
            foreach (var a in _group_detail_id)
            {
                var dser = a.id;
                lst_timesheet_form.Add(_TimeSheet_FormService.Find(a.id));
            }

            var _getList = lst_timesheet_form.Where(w => w.active_status == "Y" && w.Approve_status == "N").ToList();
            if (!string.IsNullOrEmpty(traineename))
            {
                _getList = _getList.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en).Trim().ToLower()).Contains((traineename.Trim().ToLower()))).ToList();
            }
            if (!string.IsNullOrEmpty(traineeid))
            {
                _getList = _getList.Where(w => w.TM_PR_Candidate_Mapping.TM_Candidates.Id.ToString().Contains(traineeid)).ToList();
            }

            if (_getList != null)
                foreach (var a in _getList)
                {

                    vTimeSheet_obj_View sublst = new vTimeSheet_obj_View();
                    sublst.Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit_timesheet('" + HCMFunc.Encrypt(a.Id + "") + @"');return false;"">Timesheet <i class=""fa fa-calendar""></i></a>";

                    sublst.Id = a.Id.ToString();
                    sublst.View = a.active_status;
                    sublst.Approve_date = a.Approve_date.ToString();
                    sublst.create_date = a.create_date.ToString();
                    sublst.update_date = a.update_date.ToString();
                    sublst.create_user = a.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + a.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en.ToString();
                    sublst.Approve_status = a.Approve_status;

                    lstData.Add(sublst);
                }
            return lstData.OrderBy(o => o.create_date).ToList();
        }



        [HttpPost]
        public ActionResult TraineeTSheetSave(vTimeSheet_Detail_obj_Save ItemData, string Type)
        {
            vTimeSheet_Return result = new vTimeSheet_Return();
            try
            {

                TimeSheet_Detail timeSheet_Detail = new TimeSheet_Detail();
                string gettype = Type;
                var mainid = SystemFunction.GetIntNullToZero(ItemData.Id);



                foreach (var xp in ItemData.lstData)
                {
                    timeSheet_Detail = new TimeSheet_Detail();
                    timeSheet_Detail.Id = SystemFunction.GetIntNullToZero(xp.Id + "");

                    timeSheet_Detail.approve_status = Type;
                    timeSheet_Detail.Approve_user = CGlobal.UserInfo.EmployeeNo;

                    var approves = _TimeSheet_DetailService.UpdateApproveDetail(timeSheet_Detail);
                    if (approves > 0)
                    {
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Save Time Sheet Detail Failed.";
                        return Json(new
                        {
                            result
                        });
                    }

                }
                var getforcheck = _TimeSheet_DetailService.Get_By_Approve_ID(CGlobal.UserInfo.EmployeeNo).Where(w => w.approve_status != "Y" && w.active_status == "Y").ToList();
                var approveMain = new TimeSheet_Form() { Id = mainid, Approve_status = getforcheck.Count() == 0 ? Type : "N", Approve_remark = ItemData.remark };

                var approvesmain = _TimeSheet_FormService.UpdateApproveFrom(approveMain);
                if (approvesmain > 0)
                {
                    result.Status = SystemFunction.process_Success;
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error,Save Time Sheet From Failed.";
                    return Json(new
                    {
                        result
                    });
                }


                //send mail
                //IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                //var get_mgr = sQuery.Where(w => w.Employeeno == ItemData.mgr_user_no).FirstOrDefault();
                var cand = _TM_CandidatesService.Find(SystemFunction.GetIntNullToZero(ItemData.trainee_create_user));
                string msg = "";

                var MailApprove = _MailContentService.GetMailContent("Time Sheet Approved", "Y").FirstOrDefault();
                var MailRevise = _MailContentService.GetMailContent("Time Sheet Revise", "Y").FirstOrDefault();

                string sContent = MailApprove.content;
                string from = MailApprove.sender_name;
                string subject = MailApprove.mail_header;
                if (Type == "R")
                {
                    sContent = MailRevise.content;
                    from = MailRevise.sender_name;
                    subject = MailRevise.mail_header;
                }

                sContent = (sContent + "").Replace("$emailto", cand.trainee_email);
                sContent = (sContent + "").Replace("$pm", CGlobal.UserInfo.FullName);
                sContent = (sContent + "").Replace("$trainee", cand.first_name_en + " " + cand.last_name_en);
                sContent = (sContent + "").Replace("$remark", ItemData.remark);

                var objMail = new vObjectMail_Send();
                objMail.mail_from = "hcmthailand@kpmg.co.th";
                objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                objMail.mail_to = cand.trainee_email;
                objMail.mail_cc = "";
                objMail.mail_subject = subject;
                objMail.mail_content = sContent;

                var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                if (!sSendMail)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Submit Success. But Error in Email : " + msg;
                    return Json(new
                    {
                        result
                    });
                }

            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error," + ex.Message;
                return Json(new
                {
                    result
                });
            }


            return Json(new
            {
                result
            });

        }
    }
}