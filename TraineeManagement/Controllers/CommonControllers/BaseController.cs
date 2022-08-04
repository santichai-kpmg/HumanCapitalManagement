using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraineeManagement.App_Start;
using TraineeManagement.ViewModels.CommonVM;

namespace TraineeManagement.Controllers.CommonControllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public BaseController()
        {

            //if (System.Web.HttpContext.Current.Session == null)
            //{
            //    ViewBag.Menu = "";
            //}
            //else
            //{
            //    string status = System.Web.HttpContext.Current.Session["MenuOpen"] + "";
            //    if (status == "true")
            //    {
            //        ViewBag.Menu = "sidebar-collapse";
            //    }
            //    else
            //    {
            //        ViewBag.Menu = "";

            //    }
            //}
            ViewBag.Title = "KPMG INTERN MANAGEMENT";
            if (CGlobal.UserInfo != null)
            {
                
                ViewBag.UserName = CGlobal.UserInfo.FullName;
                ViewBag.Group = CGlobal.UserInfo.UnitGroup;
                ViewBag.Rank = CGlobal.UserInfo.Rank;
                ViewBag.IsAdmin = CGlobal.UserIsAdmin();
                ViewBag.EMail = CGlobal.UserInfo.EMail;
                ViewBag.IsVerify = CGlobal.UserInfo.IsVerify;
            }
            else
            {

            }
        }
        public ActionResult acCheckLoginAndPermission(bool isReport = false)
        {
            try
            {
                if (CGlobal.IsUserExpired())
                {
                    return RedirectToAction("Login", "Login");
                }
                return null;
            }
            catch (Exception e)
            {

                return RedirectToAction("Maintenance", "MasterPage");
            }

        }
        [HttpPost]
        public ActionResult ClearSession(string sVal)
        {
            List<string> aSession = sVal.Split(',').ToList();
            foreach (var sSessionName in aSession)
            {
                if (System.Web.HttpContext.Current.Session[sSessionName] != null)
                {
                    System.Web.HttpContext.Current.Session[sSessionName] = null;
                }
            }
            System.Web.HttpContext.Current.Response.End();
            return Json(new { });
        }

        public bool SendSubmit(TM_Trainee_Eva TM_Trainee_Eva, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModels.CommonVM.vObjectMail_Send objMail = new ViewModels.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Trainee_Eva != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(TM_Trainee_Eva.req_incharge_Approve_user);
                    lstUserApp.Add(TM_Trainee_Eva.req_mgr_Approve_user);



                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }

                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    string cc = "";
                    if (_getEmp.Any())
                    {
                        string nameReq = TM_Trainee_Eva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Trainee_Eva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        //var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();

                        if (TM_Trainee_Eva.approve_type == "M")
                        {
                            _getReceive = _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_mgr_Approve_user).FirstOrDefault();
                        }
                        else
                        {
                            _getReceive = _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_incharge_Approve_user).FirstOrDefault();
                        }


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        cc += _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_incharge_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        cc += _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_mgr_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        string pathUrl = Url.Action("MailBoxTRNReceive", "Mail", null, Request.Url.Scheme);
                        //production
                        pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");

                        //testserver
                        //pathUrl = pathUrl.Replace("TraineeManagement", "application/HR/HumanCapitalManagement");
                        if (_getReceive != null)
                        {
                            string sUrl = HCMFunc.GetUrl(GetContent.mail_type, TM_Trainee_Eva.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", nameReq);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$approved", _getReceive.EmpFullName);
                            objMail.mail_from = "hcmthailand@kpmg.co.th";
                            objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                            objMail.mail_to = _getReceive.Email;
                            mail_to_log = _getReceive.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMailPES(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }

                }

                return false;
            }
            catch (Exception e)
            {
                msg = e.Message;
                return false;
            }

        }
    }
}