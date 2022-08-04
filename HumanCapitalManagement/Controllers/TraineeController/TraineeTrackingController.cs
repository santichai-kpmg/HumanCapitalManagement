using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TraineeSite;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static HumanCapitalManagement.Controllers.TraineeController.TraineePayMentController;
using static HumanCapitalManagement.Controllers.TraineeController.TraineeTSheetController;
using static HumanCapitalManagement.ViewModel.Trainee.Engagement;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineeTrackingController : Controller
    {
        #region
        // GET: TraineePayMent
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        private Perdiem_TransportService _Perdiem_TransportService;
        public TraineeTrackingController(TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
            , Perdiem_TransportService Perdiem_TransportService
                   )
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
            _Perdiem_TransportService = Perdiem_TransportService;
        }
        #endregion


        public class vPerdiem_Transport
        {
            public string Edit { get; set; }
            public int Id { get; set; }
            public int? seq { get; set; }
            public string date_start { get; set; }
            public string date_end { get; set; }
            public string daterange { get; set; }
            public string Engagement_Code { get; set; }
            public string Engagement_Name { get; set; }
            public string remark { get; set; }
            public string Type_of_withdrawal { get; set; }
            public string Company { get; set; }
            public string Reimbursable { get; set; }
            public string Business_Purpose { get; set; }
            public string Description { get; set; }
            public decimal? Amount { get; set; }
            public string active_status { get; set; }
            public DateTime? trainee_create_date { get; set; }
            public int? trainee_create_user { get; set; }
            public DateTime? trainee_update_date { get; set; }
            public int? trainee_update_user { get; set; }
            public string submit_status { get; set; }
            public string approve_status { get; set; }
            public string Approve_user { get; set; }
            public string mgr_user_id { get; set; }
            public string mgr_user_no { get; set; }
            public string mgr_user_name { get; set; }
            public string mgr_user_rank { get; set; }
            public string mgr_unit_name { get; set; }
            public string status { get; set; }
            public string Cost_Center { get; set; }
            public string Trainee_Code { get; set; }
            public string Name { get; set; }
            public string first_name_en { get; set; }
            public string last_name_en { get; set; }
            public string submit_date { get; set; }
            public string approve_date { get; set; }
            public string client_name { get; set; }
            public string review_date { get; set; }
            public string review_user { get; set; }
            public string paid_date { get; set; }
            public string paid_user { get; set; }

        }

        public class vdata_sorce_Return : CResutlWebMethod
        {
            public List<vPerdiem_Transport> lstData { get; set; }
        }


        public List<Engagement_PR> lst_Engagement = new List<Engagement_PR>();
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        // GET: TraineePerdiemTransport
        public ActionResult TraineePerdiemTransportList(string qryStr)
        {
            vdata_sorce_Return result = new vdata_sorce_Return();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            //if (!string.IsNullOrEmpty(qryStr))
            //{
            var getlinkid = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr));
            vObjectMail objMail = HCMFunc.DecryptUrl(qryStr);
            string winloginname = this.User.Identity.Name;
            string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);

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

            if (objMail.type + "" == "Trainee Perdeim/Transport Submit" || getlinkid >= 0)
            {

                var approve_user = "";
                if (!String.IsNullOrEmpty(CGlobal.UserInfo.EmployeeNo))
                {

                    approve_user = CGlobal.UserInfo.EmployeeNo;
                }


                int nId = SystemFunction.GetIntNullToZero(getlinkid == 0 ? HCMFunc.Decrypt(objMail.id + "") : getlinkid.ToString());
                var getlist = bindlist(CGlobal.UserInfo.EmployeeNo, "", nId.ToString());
                result.lstData = getlist;


            }



            //}
            //else
            //{
            //    return RedirectToAction("ErrorNopermission", "MasterPage");
            //}
            return View(
                result
            );
        }
        public ActionResult TraineeTracking()
        {
            return View();
        }



        [HttpPost]
        public ActionResult LoadPerdiemTransportList(string txtname, string txtid)
        {
            vdata_sorce_Return result = new vdata_sorce_Return();
            try
            {
                var approve_user = CGlobal.UserInfo.EmployeeNo;
                var approve_users = CGlobal.UserInfo.UserId;
                var getlist = bindlist(approve_user, txtname, txtid);
                result.lstData = getlist;

            }
            catch (Exception ex)
            {
            }
            return Json(new
            {
                result
            });

        }



        public List<vPerdiem_Transport> bindlist(string approve_user, string traineename, string traineeid)
        {


            List<vPerdiem_Transport> lstData = new List<vPerdiem_Transport>();
            GetEngagement("");
            var _getList = _Perdiem_TransportService.GetDataForSelect().Where(w => w.Approve_user == approve_user && w.active_status == "Y" && w.approve_status == "N").ToList();
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

                    vPerdiem_Transport sublst = new vPerdiem_Transport();
                    sublst.Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""View_PT('" + a.Id + "" + @"');return false;"">View <i class=""fa fa-eye""></i></a>";

                    sublst.Id = a.Id;
                    sublst.Type_of_withdrawal = a.Type_of_withdrawal;
                    sublst.Company = a.Company;
                    sublst.Reimbursable = a.Reimbursable;
                    sublst.Business_Purpose = a.Business_Purpose;
                    sublst.date_start = a.date_start.Value.ToString("dd MMM yyyy");
                    sublst.date_end = a.date_end.Value.ToString("dd MMM yyyy");

                    var getname = _TM_CandidatesService.Find(a.trainee_create_user.Value);
                    if (getname != null)
                    {
                        sublst.Name = getname.first_name_en + " " + getname.last_name_en;
                    }
                    sublst.daterange = a.date_start.Value.ToString("dd MMM yyyy");
                    sublst.Amount = a.Amount;
                    var get = lst_Engagement.Where(w => w.id == a.Engagement_Code).FirstOrDefault();
                    if (get != null)
                    {
                        sublst.Engagement_Code = get.id + "|" + get.text;
                    }
                    sublst.Approve_user = a.Approve_user;
                    sublst.trainee_create_user = a.trainee_create_user;

                    sublst.approve_status = a.approve_status;

                    lstData.Add(sublst);
                }
            return lstData;
        }

        [HttpPost]
        public ActionResult SavePerdiem_Transport(vPerdiem_Transport ItemData, string type,string remark)
        {

            vdata_sorce_Return result = new vdata_sorce_Return();
            var save = 0;
            var getUserid = CGlobal.UserInfo.EmployeeNo;

            if (ItemData != null)
            {
                Perdiem_Transport setdata = new Perdiem_Transport();
                setdata.Id = ItemData.Id;
                setdata.approve_status = type;
                setdata.approve_date = DateTime.Now;
                setdata.remark = remark;
                
                save = _Perdiem_TransportService.UpdateApprove_Status(setdata);

            }

            
           
            


            if (save > 0)
            {
                var newlst = bindlist(CGlobal.UserInfo.EmployeeNo, "", "");
                result.lstData = newlst;

                result.Status = SystemFunction.process_Success;
                result.Msg = "Success, Submit Success.";


                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));

                var get_can = _TM_CandidatesService.GetDataForSelect().Where(w => w.Id == ItemData.trainee_create_user).FirstOrDefault();
                string msg = "";

                var Mail1 = _MailContentService.GetMailContent("Trainee Perdeim/Transport Approved", "Y").FirstOrDefault();
                if (type == "R")
                {
                    Mail1 = _MailContentService.GetMailContent("Trainee Perdeim/Transport Rejected", "Y").FirstOrDefault();
                }

                if (Mail1 != null)
                {
                    string sContent = Mail1.content;
                    sContent = (sContent + "").Replace("$emailto", get_can.trainee_email);
                    sContent = (sContent + "").Replace("$pm", CGlobal.UserInfo.FullName);
                    sContent = (sContent + "").Replace("$trainee", get_can.first_name_en + " " + get_can.last_name_en);
                    sContent = (sContent + "").Replace("$remark", remark);
                    var objMail = new vObjectMail_Send();

                    objMail.mail_from = "hcmthailand@kpmg.co.th";
                    objMail.title_mail_from = "HCM System";
                    objMail.mail_to = get_can.trainee_email;
                    objMail.mail_cc = "";
                    objMail.mail_subject = Mail1.mail_header;
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
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public JsonResult GetEngagement(string university_id)
        {
            Viztopia.ViztopiaSoapClient wsViztp = new Viztopia.ViztopiaSoapClient();

            string ChargeCode = "";
            string CompName = "";

            var data = wsViztp.GetAllEngagementFromSSA("", "", "2", "", "", "", "", "", "", "", (DateTime.Now.Year - 3).ToString(), "");

            lst_Engagement = new List<Engagement_PR>();
            lst_Engagement = data.AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("EngagementNo"), text = dataRow.Field<string>("EngagementName"), value = dataRow.Field<string>("EngagementManagerNo") }).ToList();


            var indata = wsViztp.Get_Internal_Chargecode("", "");
            var a = new List<Engagement_PR>();
            a = indata.Tables[0].AsEnumerable().Select(dataRow => new Engagement_PR { id = dataRow.Field<string>("Code"), text = dataRow.Field<string>("Description") }).ToList();


            lst_Engagement.AddRange(a);


            return Json(lst_Engagement, JsonRequestBehavior.AllowGet);
        }

        public ActionResult acCheckLoginAndPermission(bool isReport = false)
        {
            try
            {
                if (CGlobal.IsUserExpired())
                {
                    return RedirectToAction("Login", "Login");
                }
                else if (1 == 1)
                {
                    var ControllerName = (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString() + "").Trim();
                    var ActionName = (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() + "").Trim();
                    StoreDb db = new StoreDb();
                    string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
                    if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
                    {
                        var _CheckMenuActive = db.MenuAction.Where(w => (w.Controller + "").Trim() == ControllerName
                        && (w.Action + "").Trim() == ActionName
                        && w.Menu.ACTIVE_FLAG + "" == "Y"
                        && w.ACTIVE_FLAG + "" == "Y").FirstOrDefault();
                        if (_CheckMenuActive != null || isReport)
                        {
                            return null;
                        }
                        else
                        {
                            return RedirectToAction("ErrorNopermission", "MasterPage");
                        }
                    }
                    var _getActionMenu = db.MenuAction.Where(w => (w.Controller + "").Trim() == ControllerName && (w.Action + "").Trim() == ActionName && w.Menu.MENU_Permission + "" == "N" && w.Menu.ACTIVE_FLAG + "" == "Y").FirstOrDefault();
                    if (_getActionMenu == null)
                    {
                        var _getPermission = db.UserPermission.Where(w => w.user_id == CGlobal.UserInfo.UserId && w.active_status == "Y").FirstOrDefault();
                        if (_getPermission != null)
                        {
                            if (!_getPermission.UserListPermission.Any(w => w.MenuAction.Menu.ACTIVE_FLAG + "" == "Y"
                            && w.MenuAction.ACTIVE_FLAG + "" == "Y"
                            && w.active_status == "Y"
                            && (w.MenuAction.Controller + "").Trim() == ControllerName
                            && (w.MenuAction.Action + "").Trim() == ActionName))
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

                        return null;
                    }

                }
                //else if (!CGlobal.UserIsAdmin())
                //{
                //    return RedirectToAction("ErrorNopermission", "MasterPage");
                //}
                return null;
            }
            catch (Exception e)
            {

                return RedirectToAction("Maintenance", "MasterPage");
            }

        }
    }
}