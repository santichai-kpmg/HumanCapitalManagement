
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class MailController : BaseController
    {
        // GET: Mail
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        public ActionResult MailBoxReceive(string qryStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(qryStr))
                {
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
                        // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                        if (objMail.type + "" != "submit")
                        {
                            return RedirectToAction("RCMTrackingView", "RCMTracking", new { qryStr = objMail.id });
                        }
                        else
                        {
                            return RedirectToAction("PRApproveEdit", "PRApprove", new { qryStr = objMail.id });
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
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");

            }
        }

        public ActionResult MailBoxPTRReceive(string qryStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(qryStr))
                {
                    vObjectMail objMail = HCMFunc.DecryptUrlPES(qryStr);
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
                        if (objMail.type + "" == "PTR Submit")
                        {
                            return RedirectToAction("PTRApproveEdit", "PTRApprove", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Submit New")
                        {
                            return RedirectToAction("PTRApproveEdit", "PTRApprove", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Approve")
                        {
                            return RedirectToAction("PTRApproveEdit", "PTRApprove", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Revise")
                        {
                            return RedirectToAction("PTREvaluationEdit", "PTREvaluation", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Revise New")
                        {
                            return RedirectToAction("PTREvaluationEdit", "PTREvaluation", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Successfully")
                        {
                            return RedirectToAction("PTRReportsEdit", "PTRReports", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Plan Submit")
                        {
                            return RedirectToAction("PTRPlanApproveEdit", "PTRApprove", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Plan Successfully")
                        {
                            return RedirectToAction("PTRPlanReportsEdit", "PTRReports", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "PTR Plan Revise")
                        {
                            return RedirectToAction("PTRPersonalPlanEdit", "PTREvaluation", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "Nomination_Submit")
                        {
                            return RedirectToAction("NominationApproveEdit", "NominationApprove", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "Nomination_Approve")
                        {
                            return RedirectToAction("NominationFormEdit", "NominationForm", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "Nomination_Revise")
                        {
                            return RedirectToAction("NominationFormEdit", "NominationForm", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "Nomination_Suscess")
                        {
                            return RedirectToAction("NominationReportEdit", "NominationReport", new { qryStr = objMail.id });
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
                }
                else
                {
                    return RedirectToAction("ErrorNopermission", "MasterPage");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");

            }
        }

        public ActionResult MailBoxTRNReceive(string qryStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(qryStr))
                {
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

                        if (objMail.type + "" == "EVA Trainee: Trainee to BU")
                        {
                            return RedirectToAction("TraineeEvaEdit", "TraineeEva", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "EVA Trainee: BU to HR")
                        {
                            return RedirectToAction("TraineeEvaEdit", "TraineeEva", new { qryStr = objMail.id });
                        }
                        else if (objMail.type + "" == "Eva Rollback")
                        {
                            return RedirectToAction("TraineeEvaEdit", "TraineeEva", new { qryStr = objMail.id });
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
                }
                else
                {
                    return RedirectToAction("ErrorNopermission", "MasterPage");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");

            }
        }
        public ActionResult MailBoxTIFForm(string qryStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(qryStr))
                {
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
                        string sName = "";
                        try
                        {
                            StoreDb db = new StoreDb();
                            int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMail.id + ""));
                            if (nId != 0)
                            {
                                var GetCandidate = db.TM_Candidates.Where(w => w.Id == nId).FirstOrDefault();
                                if (GetCandidate != null)
                                {
                                    //sName = GetCandidate.first_name_en + " " + GetCandidate.last_name_en;
                                    CSearchInterview SearchItem = new CSearchInterview();
                                    SearchItem.name = GetCandidate.first_name_en + " " + GetCandidate.last_name_en;
                                    string qryStrX = JsonConvert.SerializeObject(SearchItem,
                                                    Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        NullValueHandling = NullValueHandling.Ignore,
                                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                    });

                                    sName = qryStrX;
                                }
                            }
                        }
                        catch (Exception e)
                        {


                        }


                        if (objMail.type + "" == "HCM :1st Evaluator(Submit)")
                        {
                            return RedirectToAction("APTIFFormList", "Interview", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "HCM :2nd Evaluator(Submit no change)")
                        {
                            return RedirectToAction("APTIFFormList", "Interview", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "HCM :2nd Evaluator(Submit change)")
                        {
                            return RedirectToAction("APTIFFormList", "Interview", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "Rollback TIF")
                        {
                            return RedirectToAction("APTIFFormList", "Interview", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "HR Rollback TIF")
                        {
                            return RedirectToAction("APTIFFormList", "Interview", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "Last(Evaluator) Completion")
                        {
                            return RedirectToAction("AcKnowledgeEdit", "AcKnowledge", new { qryStr = objMail.id });
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
                }
                else
                {
                    return RedirectToAction("ErrorNopermission", "MasterPage");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");

            }
        }
        public ActionResult MailBoxPreInternForm(string qryStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(qryStr))
                {
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
                        string sName = "";
                        try
                        {
                            StoreDb db = new StoreDb();
                            int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMail.id + ""));
                            if (nId != 0)
                            {
                                var GetCandidate = db.TM_Candidates.Where(w => w.Id == nId).FirstOrDefault();
                                if (GetCandidate != null)
                                {
                                    //sName = GetCandidate.first_name_en + " " + GetCandidate.last_name_en;
                                    CSearchInterview SearchItem = new CSearchInterview();
                                    SearchItem.name = GetCandidate.first_name_en + " " + GetCandidate.last_name_en;
                                    string qryStrX = JsonConvert.SerializeObject(SearchItem,
                                                    Formatting.Indented,
                                                    new JsonSerializerSettings
                                                    {
                                                        NullValueHandling = NullValueHandling.Ignore,
                                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                                    });

                                    sName = qryStrX;
                                }
                            }
                        }
                        catch (Exception e)
                        {


                        }


                        if (objMail.type + "" == "HCM : Pre-Intern 1st Evaluator(Submit)")
                        {
                            return RedirectToAction("PreInternSecondList", "PreIntern", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "HCM : Pre-Intern 2nd Evaluator(Submit no change)")
                        {
                            return RedirectToAction("PreInternSecondList", "PreIntern", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "HCM : Pre-Intern 2nd Evaluator(Submit change)")
                        {
                            return RedirectToAction("PreInternSecondList", "PreIntern", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "Rollback Pre-Intern")
                        {
                            return RedirectToAction("PreInternSecondList", "PreIntern", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "HR Rollback Pre-Intern")
                        {
                            return RedirectToAction("PreInternSecondList", "PreIntern", new { qryStr = sName });
                        }
                        else if (objMail.type + "" == "Last(Evaluator) Completion Pre-Intern")
                        {
                            return RedirectToAction("AcKnowledgeEdit", "AcKnowledge", new { qryStr = objMail.id });
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
                }
                else
                {
                    return RedirectToAction("ErrorNopermission", "MasterPage");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Login");

            }
        }
    }
}