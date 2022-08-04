using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models._360Feedback;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.Feedbacks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.FeedbacksController
{
    public class FeedbacksController : BaseController
    {
        private FeedbackService _FeedbackService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        New_HRISEntities dbHr = new New_HRISEntities();
        public FeedbacksController(
            FeedbackService feedbackservice
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService)
        {
            _FeedbackService = feedbackservice;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
        }


        // GET: Feedbacks
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Feedbacks(string qryStr)
        {
            vFeedback_Return result = new vFeedback_Return();

            if (!string.IsNullOrEmpty(qryStr))
            {
                var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
                vObjectMail objMaila = HCMFunc.DecryptUrl(qryStr);
                string winloginnamea = this.User.Identity.Name;
                string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                if (!string.IsNullOrEmpty(EmpUserIDa))
                {
                    if (CGlobal.IsUserExpired())
                    {
                        DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserIDa);
                        // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                        if (staff != null)
                        {
                            var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMaila.emp_no + "").FirstOrDefault();
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
                }
                var sCheck = acCheckLoginAndPermission();
                if (sCheck != null)
                {
                    return sCheck;
                }
                vFeedback_obj_save objnew = new vFeedback_obj_save();


                objnew.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMaila.id + ""));
                objnew.Request_User = objMaila.emp_no;
                objnew.Given_User = CGlobal.UserInfo.EmployeeNo;
                var getfeedback = _FeedbackService.Find(objnew.Id);
                if (getfeedback != null)
                {
                    objnew.Positive = getfeedback.Positive;
                    objnew.Strength = getfeedback.Strength;
                    objnew.Need_Improvement = getfeedback.Need_Improvement;
                    objnew.Recommendations = getfeedback.Recommendations;
                    objnew.Rate = getfeedback.Rate.ToString();
                    objnew.Status = getfeedback.Status;

                    objnew.Mode = "View";
                }
                result.maindata = objnew;


            }




            //var objMail = HCMFunc.DecrypFeedback(qryStr);

            //if (objMail.id != null)
            //{
            //    result.maindata.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMail.id + ""));
            //    result.maindata.Request_User = objMail.emp_no;
            //    result.maindata.Given_User = objMail.emp_to;


            //}

            //List<vFeedback_obj_save> feedback = new List<vFeedback_obj_save>();
            //New_HRISEntities dbHr = new New_HRISEntities();
            //IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

            //var getrequest = _FeedbackService.GetDataForSelectByRequest(CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.Create_Date).ToList();

            //foreach (var ex in getrequest)
            //{
            //    var get_mgr = sQuery.Where(w => w.Employeeno == ex.Given_User).FirstOrDefault();

            //    vFeedback_obj_save feedbacksub = new vFeedback_obj_save();
            //    feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy HH:mm");
            //    feedbacksub.Given_User = get_mgr.Employeename + " " + get_mgr.Employeesurname;
            //    feedbacksub.Status = ex.Status;

            //    feedback.Add(feedbacksub);
            //}
            //result.lstData = feedback;


            return View(result);
        }

        public ActionResult FeedbackRequestHistory(string qryStr)
        {
            vFeedback_Return result = new vFeedback_Return();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            if (!string.IsNullOrEmpty(qryStr))
            {
                var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
                vObjectMail objMaila = HCMFunc.DecryptUrl(qryStr);
                string winloginnamea = this.User.Identity.Name;
                string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                if (!string.IsNullOrEmpty(EmpUserIDa))
                {
                    if (CGlobal.IsUserExpired())
                    {
                        DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserIDa);
                        // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                        if (staff != null)
                        {
                            var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMaila.emp_no + "").FirstOrDefault();
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
                }

                vFeedback_obj_save objnew = new vFeedback_obj_save();


                objnew.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMaila.id + ""));
                objnew.Request_User = objMaila.emp_no;
                objnew.Given_User = CGlobal.UserInfo.EmployeeNo;

                result.maindata = objnew;
            }




            //var objMail = HCMFunc.DecrypFeedback(qryStr);

            //if (objMail.id != null)
            //{
            //    result.maindata.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMail.id + ""));
            //    result.maindata.Request_User = objMail.emp_no;
            //    result.maindata.Given_User = objMail.emp_to;


            //}

            List<vFeedback_obj_save> feedback = new List<vFeedback_obj_save>();
            List<vFeedback_obj_save> feedbackRe = new List<vFeedback_obj_save>();
            New_HRISEntities dbHr = new New_HRISEntities();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

            var getrequest = _FeedbackService.GetDataForSelectByRequest(CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.Create_Date).ToList();
            var getrequestRe = _FeedbackService.GetDataForSelectByGiven(CGlobal.UserInfo.EmployeeNo).Where(w => w.Type == "R" || w.Type == "B").OrderByDescending(o => o.Create_Date).ToList();

            foreach (var ex in getrequest)
            {
                var get_mgr = sQuery.Where(w => w.Employeeno == ex.Given_User).FirstOrDefault();

                vFeedback_obj_save feedbacksub = new vFeedback_obj_save();
                feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy HH:mm");
                feedbacksub.Given_User = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                feedbacksub.Status = ex.Status;

                feedback.Add(feedbacksub);
            }


            foreach (var ex in getrequestRe)
            {
                var get_mgr = sQuery.Where(w => w.Employeeno == ex.Request_User).FirstOrDefault();
                string pathUrl = Url.Action("Feedbacks", "Feedbacks", null, Request.Url.Scheme);
                //production

                pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");
                string genlink = HCMFunc.GetUrl("submit", ex.Id.ToString(), pathUrl, ex.Request_User);

                vFeedback_obj_save feedbacksub = new vFeedback_obj_save();
                if (ex.Type == "R")
                    feedbacksub.Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""window.location='" + genlink + @"';return false;"">Give <i class=""fa fa-comment""></i></a>";

                feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy HH:mm");
                feedbacksub.Request_User = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                feedbacksub.Status = ex.Status;
                feedbacksub.Type = ex.Type;

                feedbackRe.Add(feedbacksub);
            }
            result.lstData = feedback;


            result.lstDataRe = feedbackRe;


            return View(result);
        }

        public ActionResult FeedbackGiveHistory(string qryStr)
        {
            vFeedback_Return result = new vFeedback_Return();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            if (!string.IsNullOrEmpty(qryStr))
            {
                var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
                vObjectMail objMaila = HCMFunc.DecryptUrl(qryStr);
                string winloginnamea = this.User.Identity.Name;
                string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                if (!string.IsNullOrEmpty(EmpUserIDa))
                {
                    if (CGlobal.IsUserExpired())
                    {
                        DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserIDa);
                        // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                        if (staff != null)
                        {
                            var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMaila.emp_no + "").FirstOrDefault();
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
                }

                vFeedback_obj_save objnew = new vFeedback_obj_save();


                objnew.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMaila.id + ""));
                objnew.Request_User = objMaila.emp_no;
                objnew.Given_User = CGlobal.UserInfo.EmployeeNo;

                result.maindata = objnew;
            }




            //var objMail = HCMFunc.DecrypFeedback(qryStr);

            //if (objMail.id != null)
            //{
            //    result.maindata.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMail.id + ""));
            //    result.maindata.Request_User = objMail.emp_no;
            //    result.maindata.Given_User = objMail.emp_to;


            //}

            List<vFeedback_obj_save> feedback = new List<vFeedback_obj_save>();
            New_HRISEntities dbHr = new New_HRISEntities();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

            //var getrequest = _FeedbackService.GetDataForSelectByRequest(CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.Create_Date).ToList();
            var getrequest = _FeedbackService.GetDataForSelectByRequest(CGlobal.UserInfo.EmployeeNo).Where(w => w.Status == "S").OrderByDescending(o => o.Create_Date).ToList();

            foreach (var ex in getrequest)
            {
                var get_mgr = sQuery.Where(w => w.Employeeno == ex.Request_User).FirstOrDefault();
                string pathUrl = Url.Action("Feedbacks", "Feedbacks", null, Request.Url.Scheme);
                //production

                pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");
                string genlink = HCMFunc.GetUrl("submit", ex.Id.ToString(), pathUrl, ex.Request_User);

                var get_give = sQuery.Where(w => w.Employeeno == ex.Given_User).FirstOrDefault();
                var get_request = sQuery.Where(w => w.Employeeno == ex.Request_User).FirstOrDefault();
                //var get_mgr = sQuery.Where(w => w.Employeeno == ex.Given_User).FirstOrDefault();

                vFeedback_obj_save feedbacksub = new vFeedback_obj_save();
                //feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy HH:mm");
                //feedbacksub.Given_User = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                //feedbacksub.Status = ex.Status;
                var dte = (DateTime.Now.Date - ex.Approve_Date.Value.Date).TotalDays < 3 ? "<span class='fa fa-angle-up text-success'></span>" : "";
                feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy");
                feedbacksub.Given_User = get_give.Employeename + " " + get_give.Employeesurname;
                feedbacksub.Request_User = get_request.Employeename + " " + get_request.Employeesurname;
                feedbacksub.Positive = ex.Positive;
                feedbacksub.Strength = ex.Strength;
                feedbacksub.Need_Improvement = ex.Need_Improvement;
                feedbacksub.Recommendations = ex.Recommendations;
                feedbacksub.Rate = ex.Rate.ToString();
                feedbacksub.Status = ex.Status;
                feedbacksub.Icon = ex.Rate.ToString();
                feedbacksub.Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""window.open('" + genlink + @"', '_blank');return false;"">View <i class=""fa fa-comment""></i></a>";
                feedback.Add(feedbacksub);
            }
            result.lstData = feedback;


            return View(result);
        }

        public ActionResult Feedback_List(string qryStr)
        {
            vFeedback_Return result = new vFeedback_Return();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            if (!string.IsNullOrEmpty(qryStr))
            {
                var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
                vObjectMail objMaila = HCMFunc.DecryptUrl(qryStr);
                string winloginnamea = this.User.Identity.Name;
                string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                if (!string.IsNullOrEmpty(EmpUserIDa))
                {
                    if (CGlobal.IsUserExpired())
                    {
                        DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserIDa);
                        // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                        if (staff != null)
                        {
                            var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMaila.emp_no + "").FirstOrDefault();
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
                }

                vFeedback_obj_save objnew = new vFeedback_obj_save();


                objnew.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMaila.id + ""));
                objnew.Request_User = objMaila.emp_no;
                objnew.Given_User = objMaila.emp_no;

                result.maindata = objnew;
            }





            return View(result);
        }
        public ActionResult Request_List(string qryStr)
        {
            vFeedback_Return result = new vFeedback_Return();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            if (!string.IsNullOrEmpty(qryStr))
            {
                var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
                vObjectMail objMaila = HCMFunc.DecryptUrl(qryStr);
                string winloginnamea = this.User.Identity.Name;
                string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                if (!string.IsNullOrEmpty(EmpUserIDa))
                {
                    if (CGlobal.IsUserExpired())
                    {
                        DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserIDa);
                        // DataTable staff = wsHRis.getActiveStaffByUserID("zjaffer");
                        if (staff != null)
                        {
                            var CheckUser = staff.AsEnumerable().Where(w => w.Field<string>("EmpNo") == objMaila.emp_no + "").FirstOrDefault();
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
                }

                vFeedback_obj_save objnew = new vFeedback_obj_save();


                objnew.Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(objMaila.id + ""));
                objnew.Request_User = objMaila.emp_no;
                objnew.Given_User = objMaila.emp_no;

                result.maindata = objnew;
            }





            return View(result);
        }
        public class image_obj
        {
            public string path { get; set; }
        }
        [HttpPost]
        public ActionResult getImageEmp(string empNo)
        {

            image_obj result = new image_obj();

            New_HRISEntities dbHr = new New_HRISEntities();
            var GetPic = dbHr.vw_StaffPhoto_FileName.Where(w => w.Employeeno == empNo).FirstOrDefault();
            try
            {

                string sPicturePath = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];
                var absolutePath = HttpContext.Server.MapPath("~/Image/ic_noimage.png");
                var patha = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];
                if (GetPic != null)
                {
                    sPicturePath = (sPicturePath + GetPic.PhotoName + ".jpg").Replace("/", "\\");
                }
                else
                {
                    sPicturePath = absolutePath;
                }

                result.path = sPicturePath;

            }
            catch (Exception ex)
            {
            }


            return Json(new { result });
        }

        [HttpPost]
        public ActionResult Search_Feedback(string start, string end, string status, string empno, string type)
        {
            vFeedback_Return result = new vFeedback_Return();
            try
            {

                List<vFeedback_obj_save> feedback = new List<vFeedback_obj_save>();
                New_HRISEntities dbHr = new New_HRISEntities();
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

                var getrequest = _FeedbackService.GetDataForSelect().OrderByDescending(o => o.Create_Date).ToList();
                if (type != "All" && !String.IsNullOrEmpty(type))
                {
                    getrequest = getrequest.Where(w => w.Status == type).ToList();
                }


                if (!string.IsNullOrEmpty(start))
                {
                    getrequest = getrequest.Where(w => w.Create_Date.Value.Date >= SystemFunction.ConvertStringToDateTime(start, "", "dd MMMM yyyy").Date).ToList();
                }
                if (!string.IsNullOrEmpty(end))
                {
                    getrequest = getrequest.Where(w => w.Create_Date.Value.Date <= SystemFunction.ConvertStringToDateTime(end, "", "dd MMMM yyyy").Date).ToList();
                }
                if (!string.IsNullOrEmpty(status))
                {
                    if (status != "All")
                    {
                        getrequest = getrequest.Where(w => w.Type == status || w.Type == "B").ToList();
                    }
                }

                if (!string.IsNullOrEmpty(empno))
                {
                    if (status == "G")
                    {
                        getrequest = getrequest.Where(w => w.Given_User == empno).ToList();
                    }
                    else if (status == "R")
                    {
                        getrequest = getrequest.Where(w => w.Request_User == empno).ToList();
                    }
                }



                foreach (var ex in getrequest)
                {
                    var get_give = sQuery.Where(w => w.Employeeno == ex.Given_User).FirstOrDefault();
                    var get_request = sQuery.Where(w => w.Employeeno == ex.Request_User).FirstOrDefault();

                    vFeedback_obj_save feedbacksub = new vFeedback_obj_save();
                    feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy HH:mm");
                    feedbacksub.Given_User = get_give.Employeename + " " + get_give.Employeesurname;
                    feedbacksub.Request_User = get_request.Employeename + " " + get_request.Employeesurname;
                    feedbacksub.Positive = ex.Positive;
                    feedbacksub.Strength = ex.Strength;
                    feedbacksub.Need_Improvement = ex.Need_Improvement;
                    feedbacksub.Recommendations = ex.Recommendations;
                    feedbacksub.Rate = ex.Rate.ToString();



                    feedbacksub.Status = ex.Status;
                    if (status == "R")
                    {
                        if (ex.Type == "R")
                        {
                            feedbacksub.Status = "Request";
                        }
                        else if (ex.Type == "G")
                        {
                            feedbacksub.Status = "Give";
                        }
                        else if (ex.Type == "B")
                        {
                            feedbacksub.Status = "Waiting For Review";
                        }
                    }
                    feedback.Add(feedbacksub);
                    var groupget = "";
                    if (status == "G")
                    {
                        var get = wsHRis.getEmployeeInfoByEmpNo(ex.Given_User);
                        var CheckUser = get.AsEnumerable().FirstOrDefault();
                        groupget = CheckUser.Field<string>("UnitGroup");
                    }
                    else if (status == "R")
                    {
                        var get = wsHRis.getEmployeeInfoByEmpNo(ex.Request_User);
                        var CheckUser = get.AsEnumerable().FirstOrDefault();
                        groupget = CheckUser.Field<string>("UnitGroup");
                    }
                    feedbacksub.Group = groupget;

                    if (ex.Status == "Y")
                    {
                        feedbacksub.Id = ex.Id;
                    }

                }

                result.lstData = feedback;
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }


        [HttpPost]
        public ActionResult Save_Feedback(vFeedback_obj_save itemdata)
        {
            vFeedback_obj_save result = new vFeedback_obj_save();
            try
            {
                var getmain = itemdata;

                Feedback feedback = new Feedback();

                feedback.Id = itemdata.Id;
                feedback.Positive = itemdata.Positive;
                feedback.Strength = itemdata.Strength;
                feedback.Need_Improvement = itemdata.Need_Improvement;
                feedback.Recommendations = itemdata.Recommendations;
                feedback.Rate = Convert.ToInt16(itemdata.Rate);

                feedback.Create_Date = DateTime.Now;
                feedback.Create_User = CGlobal.UserInfo.EmployeeNo;
                feedback.Update_Date = DateTime.Now;
                feedback.Update_User = CGlobal.UserInfo.EmployeeNo;

                feedback.Given_User = CGlobal.UserInfo.EmployeeNo;
                feedback.Given_Date = DateTime.Now;
                feedback.Request_User = itemdata.Request_User;

                feedback.Active_Status = "Y";
                feedback.Type = "G";
                if (getmain.Id != 0)
                    feedback.Type = "B";
                feedback.Status = "Y";


                //if (getmain.Id != 0)
                //{
                //    feedback.Type = "RG";
                //    var add = _FeedbackService.CreateNew(ref feedback);
                //    if (add > 0)
                //    {
                //        result.Status = SystemFunction.process_Success;
                //        result.Msg = "Success ";
                //    }
                //    else
                //    {
                //        result.Status = SystemFunction.process_Failed;
                //        result.Msg = "Error ";

                //    }
                //}
                //else
                //{
                var add = _FeedbackService.CreateNewOrUpdate(feedback);
                if (add > 0)
                {
                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Success ";
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error ";

                }
                //}
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Request_Feedback(string staffNo)
        {
            vFeedback_obj_save result = new vFeedback_obj_save();
            try
            {
                Feedback feedback = new Feedback();

                feedback.Create_Date = DateTime.Now;
                feedback.Create_User = CGlobal.UserInfo.EmployeeNo;
                feedback.Update_Date = DateTime.Now;
                feedback.Update_User = CGlobal.UserInfo.EmployeeNo;

                feedback.Given_User = staffNo;
                feedback.Request_User = CGlobal.UserInfo.EmployeeNo;
                feedback.Request_Date = DateTime.Now;

                feedback.Active_Status = "Y";
                feedback.Type = "R";
                feedback.Status = "N";

                var add = _FeedbackService.CreateNew(ref feedback);
                if (add > 0)
                {
                    result.Status = SystemFunction.process_Success;
                    result.Msg = "Success ";

                    //var cand = _TM_CandidatesService.GetDataForSelect().Select(x=> x.TM_PR_Candidate_Mapping.Select(s=> s).Where(w=> w.s) == staffNo);
                    string msg = "";
                    IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                    var getbyno = sQuery.Where(w => w.Employeeno == staffNo).FirstOrDefault();
                    string pathUrl = Url.Action("Feedbacks", "Feedbacks", null, Request.Url.Scheme);
                    //production
                    pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");


                    var MailRequest = _MailContentService.GetMailContent("Request Feedback", "Y").FirstOrDefault();
                    string genlink = HCMFunc.GetUrl("submit", feedback.Id.ToString(), pathUrl, CGlobal.UserInfo.EmployeeNo);

                    string sContent = MailRequest.content;
                    string from = MailRequest.sender_name;
                    string subject = MailRequest.mail_header;

                    sContent = (sContent + "").Replace("$linkto", genlink);
                    sContent = (sContent + "").Replace("$sender", CGlobal.UserInfo.FullName);
                    sContent = (sContent + "").Replace("$recever", getbyno.Employeename + " " + getbyno.Employeesurname);

                    var objMail = new vObjectMail_Send();
                    objMail.mail_from = "hcmthailand@kpmg.co.th";
                    objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                    objMail.mail_to = getbyno.Email;
                    objMail.mail_cc = CGlobal.UserInfo.EMail;
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

            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult RequestFeedbackHistory(string staffNo)
        {
            vFeedback_Return result = new vFeedback_Return();
            try
            {
                List<vFeedback_obj_save> feedback = new List<vFeedback_obj_save>();
                New_HRISEntities dbHr = new New_HRISEntities();
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

                var getrequest = _FeedbackService.GetDataForSelectByRequest(CGlobal.UserInfo.EmployeeNo).ToList();

                foreach (var ex in getrequest)
                {
                    var get_mgr = sQuery.Where(w => w.Employeeno == ex.Given_User).FirstOrDefault();

                    vFeedback_obj_save feedbacksub = new vFeedback_obj_save();
                    feedbacksub.Create_Date = ex.Create_Date.Value.ToString("dd MMM yyyy");
                    feedbacksub.Given_User = get_mgr.Employeename + " " + get_mgr.Employeesurname;
                    feedbacksub.Status = ex.Status;

                    feedback.Add(feedbacksub);
                }
                result.lstData = feedback.OrderByDescending(o => o.Create_Date).ToList();
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult Approve_Feedback(string lstid)
        {
            vFeedback_obj_save result = new vFeedback_obj_save();
            try
            {
                result.Msg = "Save Data Success";
                bool check = false;
                var idfeedback = lstid.Remove(lstid.Length - 1).Trim().Split(',').ToList();

                foreach (var exid in idfeedback)
                {
                    int ID = SystemFunction.GetIntNullToZero(exid + "");

                    var updaetA = _FeedbackService.UpdateStatus(ID, CGlobal.UserInfo.EmployeeNo, "A");
                    var getfeedback = _FeedbackService.Find(ID);



                }
                var getforselect = _FeedbackService.GetDataForSelect().Where(w => w.Status == "A").ToList();
                var firstgroup = getforselect.GroupBy(g => g.Request_User);

                foreach (var exfirst in firstgroup)
                {
                    var getfirst = exfirst.Select(s => s).OrderBy(o => o.Given_User).ThenBy(t => t.Create_Date).ToList();
                    var countfirst = getfirst.GroupBy(g => g.Given_User).Count();
                    if (countfirst >= 3)
                    {
                        foreach (var forfirst in getfirst)
                        {
                            var updaetS = _FeedbackService.UpdateStatus(forfirst.Id, CGlobal.UserInfo.EmployeeNo, "S");
                            if (updaetS > 0)
                            {
                                check = true;
                            }
                        }

                        if (check)
                        {
                            //var cand = _TM_CandidatesService.GetDataForSelect().Select(x=> x.TM_PR_Candidate_Mapping.Select(s=> s).Where(w=> w.s) == staffNo);
                            string msg = "";
                            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                            var getbyno = sQuery.Where(w => w.Employeeno == exfirst.Key).FirstOrDefault();
                            string pathUrl = Url.Action("FeedbackGiveHistory", "Feedbacks", null, Request.Url.Scheme);
                            //production
                            pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");


                            var MailRequest = _MailContentService.GetMailContent("Give Feedback", "Y").FirstOrDefault();
                            string genlink = HCMFunc.GetUrl(MailRequest.mail_type, getbyno.Employeeno, pathUrl, CGlobal.UserInfo.EmployeeNo);

                            string sContent = MailRequest.content;
                            string from = MailRequest.sender_name;
                            string subject = MailRequest.mail_header;

                            sContent = (sContent + "").Replace("$linkto", genlink);
                            sContent = (sContent + "").Replace("$sender", CGlobal.UserInfo.FullName);
                            sContent = (sContent + "").Replace("$recever", getbyno.Employeename + " " + getbyno.Employeesurname);

                            var objMail = new vObjectMail_Send();
                            objMail.mail_from = "hcmthailand@kpmg.co.th";
                            objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                            objMail.mail_to = getbyno.Email;
                            objMail.mail_cc = CGlobal.UserInfo.EMail;
                            objMail.mail_subject = subject;
                            objMail.mail_content = sContent;

                            var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                            if (sSendMail)
                            {
                                result.Status = SystemFunction.process_Success;
                                result.Msg += "<br> <span class='fa fa-check text-success'></span> (sent) " + getbyno.Employeename + " " + getbyno.Employeesurname;

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Submit Success. But Error in Email : " + msg;
                                return Json(new
                                {
                                    result
                                });
                            }

                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Success;
                        result.Msg += "";
                    }


                    //var getbyemployeno = _FeedbackService.GetDataForSelectByRequest(getfeedback.Request_User).Where(w => w.Status == "A").ToList();
                    //var getbyemployeno_sub_ = getbyemployeno.GroupBy(g => g.Given_User).ToList();
                    //if (getbyemployeno_sub_.Count() == 3)
                    //{


                    //    foreach (var fors in getbyemployeno)
                    //    {
                    //        var updaetS = _FeedbackService.UpdateStatus(fors.Id, CGlobal.UserInfo.EmployeeNo, "S");
                    //        if (updaetS > 0)
                    //        {
                    //            check = true;
                    //        }
                    //    }

                    //    if (check)
                    //    {


                    //        //var cand = _TM_CandidatesService.GetDataForSelect().Select(x=> x.TM_PR_Candidate_Mapping.Select(s=> s).Where(w=> w.s) == staffNo);
                    //        string msg = "";
                    //        IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                    //        var getbyno = sQuery.Where(w => w.Employeeno == getfeedback.Request_User).FirstOrDefault();
                    //        string pathUrl = Url.Action("Feedbacks", "Feedbacks", null, Request.Url.Scheme);
                    //        //production
                    //        pathUrl = pathUrl.Replace("TraineeEvaluation", "application/HR/HumanCapitalManagement");


                    //        var MailRequest = _MailContentService.GetMailContent("Give Feedback", "Y").FirstOrDefault();
                    //        string genlink = HCMFunc.GetUrl(MailRequest.mail_type, getbyno.Employeeno, pathUrl, CGlobal.UserInfo.EmployeeNo);

                    //        string sContent = MailRequest.content;
                    //        string from = MailRequest.sender_name;
                    //        string subject = MailRequest.mail_header;

                    //        sContent = (sContent + "").Replace("$linkto", genlink);
                    //        sContent = (sContent + "").Replace("$sender", CGlobal.UserInfo.FullName);
                    //        sContent = (sContent + "").Replace("$recever", getbyno.Employeename + " " + getbyno.Employeesurname);

                    //        var objMail = new vObjectMail_Send();
                    //        objMail.mail_from = "hcmthailand@kpmg.co.th";
                    //        objMail.title_mail_from = "HCM System";//GetContent.sender_name;
                    //        objMail.mail_to = getbyno.Email;
                    //        objMail.mail_cc = CGlobal.UserInfo.EMail;
                    //        objMail.mail_subject = subject;
                    //        objMail.mail_content = sContent;

                    //        var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                    //        if (sSendMail)
                    //        {
                    //            result.Status = SystemFunction.process_Success;
                    //            result.Msg += "<br> - " + getbyno.Employeename + " " + getbyno.Employeesurname;

                    //        }
                    //        else
                    //        {
                    //            result.Status = SystemFunction.process_Failed;
                    //            result.Msg = "Error, Submit Success. But Error in Email : " + msg;
                    //            return Json(new
                    //            {
                    //                result
                    //            });
                    //        }

                    //    }

                }




            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }



            return Json(new
            {
                result
            });
        }
    }
}