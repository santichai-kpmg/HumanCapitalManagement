using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class BaseController : Controller
    {
        // GET: Base
        public BaseController()
        {

            if (System.Web.HttpContext.Current.Session == null)
            {
                ViewBag.Menu = "";
            }
            else
            {
                string status = System.Web.HttpContext.Current.Session["MenuOpen"] + "";
                if (status == "true")
                {
                    ViewBag.Menu = "sidebar-collapse";
                }
                else
                {
                    ViewBag.Menu = "";

                }
            }
            if (CGlobal.UserInfo != null)
            {
                ViewBag.UserName = CGlobal.UserInfo.FullName;
                if (CGlobal.UserInfo.Picture != null)
                {
                    //ViewBag.UserPic = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(CGlobal.UserInfo.aPicture));
                    ViewBag.UserPic = CGlobal.UserInfo.Picture;
                }
                else
                {
                    // ViewBag.UserPic = "Image/noimgaaaa.png";
                }
                ViewBag.Group = CGlobal.UserInfo.UnitGroup;
                ViewBag.Rank = CGlobal.UserInfo.Rank;
                ViewBag.IsAdmin = CGlobal.UserIsAdmin();
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
        public ActionResult acCheckLoginAndPermissionExport(string action, string contoller)
        {
            try
            {
                if (CGlobal.IsUserExpired())
                {
                    return RedirectToAction("Login", "Login");
                }
                else if (1 == 1)
                {
                    StoreDb db = new StoreDb();
                    if (CGlobal.UserIsAdmin())
                    {
                        return null;
                    }
                    else
                    {
                        var _getActionMenu = db.MenuAction.Where(w => (w.Controller + "").Trim() == contoller && (w.Action + "").Trim() == action && w.Menu.MENU_Permission + "" == "N" && w.Menu.ACTIVE_FLAG + "" == "Y").FirstOrDefault();
                        if (_getActionMenu == null)
                        {
                            var _getPermission = db.UserPermission.Where(w => w.user_id == CGlobal.UserInfo.UserId && w.active_status == "Y").FirstOrDefault();
                            if (_getPermission != null)
                            {
                                if (!_getPermission.UserListPermission.Any(w => w.MenuAction.Menu.ACTIVE_FLAG + "" == "Y"
                                && w.MenuAction.ACTIVE_FLAG + "" == "Y"
                                && w.active_status == "Y"
                                && (w.MenuAction.Controller + "").Trim() == contoller
                                && (w.MenuAction.Action + "").Trim() == action))
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
        public ActionResult pesCheckLoginAndPermission(bool isReport = false)
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
                    string[] UserAdmin = WebConfigurationManager.AppSettings["PESAdmin"].Split(';');
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
                    if (_getActionMenu == null && !isReport)
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
        public ActionResult pesCheckLoginAndPermissionExport(string action, string contoller)
        {
            try
            {
                if (CGlobal.IsUserExpired())
                {
                    return RedirectToAction("Login", "Login");
                }
                else if (1 == 1)
                {
                    StoreDb db = new StoreDb();
                    if (CGlobal.UserIsAdminPES())
                    {
                        return null;
                    }
                    else
                    {
                        var _getActionMenu = db.MenuAction.Where(w => (w.Controller + "").Trim() == contoller && (w.Action + "").Trim() == action && w.Menu.MENU_Permission + "" == "N" && w.Menu.ACTIVE_FLAG + "" == "Y").FirstOrDefault();
                        if (_getActionMenu == null)
                        {
                            var _getPermission = db.UserPermission.Where(w => w.user_id == CGlobal.UserInfo.UserId && w.active_status == "Y").FirstOrDefault();
                            if (_getPermission != null)
                            {
                                if (!_getPermission.UserListPermission.Any(w => w.MenuAction.Menu.ACTIVE_FLAG + "" == "Y"
                                && w.MenuAction.ACTIVE_FLAG + "" == "Y"
                                && w.active_status == "Y"
                                && (w.MenuAction.Controller + "").Trim() == contoller
                                && (w.MenuAction.Action + "").Trim() == action))
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
        public ActionResult acCheckLogin()
        {
            if (CheckLogin())
            {
                return RedirectToAction("LoginError", "MasterPage");
            }
            return null;
        }
        protected bool CheckLogin()
        {
            bool bReturn = false;
            try
            {
                string winloginname = this.User.Identity.Name;
                string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);
                if (Session["UserInfo"] != null && CGlobal.UserInfo.UserId == EmpUserID)
                {
                    //User have login 
                }
                else
                {
                    User newUser = new User(EmpUserID);
                    if (!string.IsNullOrEmpty(EmpUserID))
                    {
                        wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();

                        DataTable staff = wsHRis.getActiveStaffByUserID(EmpUserID);
                        if (staff != null)
                        {
                            if (staff.Rows.Count > 0)
                            {
                                StoreDb db = new StoreDb();
                                New_HRISEntities dbHr = new New_HRISEntities();
                                //string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');

                                newUser.UserId = EmpUserID;
                                newUser.EmployeeNo = staff.Rows[0]["EmpNo"].ToString();
                                newUser.FirstName = staff.Rows[0]["EmpFirstName"].ToString();
                                newUser.LastName = staff.Rows[0]["EmpSurname"].ToString();
                                newUser.FullName = staff.Rows[0]["EmpName"].ToString();
                                newUser.EMail = staff.Rows[0]["Email"].ToString();
                                newUser.OfficePhone = staff.Rows[0]["OfficePhone"].ToString();
                                newUser.Company = staff.Rows[0]["CompanyCode"].ToString();
                                newUser.PI = staff.Rows[0]["PI"].ToString();
                                newUser.Pool = staff.Rows[0]["Pool"].ToString();
                                newUser.Division = staff.Rows[0]["Division"].ToString();
                                newUser.UnitGroup = staff.Rows[0]["UnitGroup"].ToString();
                                newUser.Rank = staff.Rows[0]["RankCode"].ToString();
                                newUser.lstDivision = new List<lstDivision>();
                                var CheckDivi = dbHr.vw_Unit.Where(w => w.UnitID + "" == newUser.Division + "").FirstOrDefault();
                                if (CheckDivi != null)
                                {
                                    newUser.lstDivision.Add(new lstDivision
                                    {
                                        sID = CheckDivi.UnitGroupID + "",// dbHr.vw_Unit.Where(w => w.UnitID == newUser.Division).Select(s => s.UnitGroupID + "").FirstOrDefault(),
                                        sName = newUser.UnitGroup,
                                        from_role = "Y",

                                    });
                                }


                                var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
                                var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
                                var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == newUser.EmployeeNo && w.RankID == 0).ToList();
                                if (CheckCEO.Any())
                                {
                                    newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckCEO.Select(s => s.CompanyID).ToArray()).Contains(w.CompanyID)).Select(s => new lstDivision
                                    {
                                        sID = s.UnitGroupID + "",
                                        sName = s.UnitName,
                                        from_role = "Y",
                                    }).ToList());
                                }
                                if (CheckPool.Any())
                                {
                                    newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckPool.Select(s => s.PoolID).ToArray()).Contains(w.PoolID)).Select(s => new lstDivision
                                    {
                                        sID = s.UnitGroupID + "",
                                        sName = s.UnitName,
                                        from_role = "Y",
                                    }).ToList());
                                }
                                if (CheckGroupH.Any())
                                {
                                    newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckGroupH.Select(s => s.UnitGroupID).ToArray()).Contains(w.UnitGroupID)).Select(s => new lstDivision
                                    {
                                        sID = s.UnitGroupID + "",
                                        sName = s.UnitName,
                                        from_role = "Y",
                                    }).ToList());
                                }


                                //newUser.lstDivision = db.Divisions.OrderBy(o => o.Sort).Select(s => new lstDivision
                                //{
                                //    sID = s.devision_code,
                                //    sName = s.DivisionName,
                                //}).ToList();

                                //if (UserAdmin.Contains(EmpUserID))
                                //{
                                //    //var xxx = dbHr.tbMaster_UnitGroup.Where(w => w.IsActive == true).ToList();
                                //    // newUser.Pool_Role = dbHr.tbMaster_UnitGroup.Where(w => w.IsActive == true).Select(s => s.ID).ToArray();
                                //}
                                //else
                                //{
                                //    newUser.Pool_Role = new int[] { };
                                //}

                                //asdasd  Context.User = newUser;
                                Session["UserInfo"] = newUser;
                                ViewBag.UserName = newUser.FullName;

                                ViewBag.IsAdmin = CGlobal.UserIsAdmin();
                            }
                            else
                            {
                                bReturn = true;
                            }
                        }
                        else
                        {
                            bReturn = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bReturn = true;
                // throw;
                return bReturn;
            }
            if (!string.IsNullOrEmpty(Session["UserInfo"] as string))
            {
                bReturn = true;
            }
            return bReturn;
        }
        [HttpPost]
        public ActionResult SetOpenMenu(string sVal)
        {
            sVal = (sVal + "").ToLower();
            if (sVal == "true")
            {
                ViewBag.Menu = "sidebar-collapse";
                System.Web.HttpContext.Current.Session["MenuOpen"] = "true";
            }
            else
            {
                ViewBag.Menu = "";
                System.Web.HttpContext.Current.Session["MenuOpen"] = "false";

            }
            return Json(new { status = "true" });
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


        #region Mail PR Form
        public bool SendRequestMail(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PersonnelRequest != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PersonnelRequest.request_user);
                    lstUserApp.Add(PersonnelRequest.Req_BUApprove_user);
                    lstUserApp.Add(PersonnelRequest.Req_HeadApprove_user);
                    if (PersonnelRequest.need_ceo_approve + "" == "Y")
                    {
                        lstUserApp.Add(PersonnelRequest.Req_CeoApprove_user);
                    }

                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();

                        if (PersonnelRequest.BUApprove_status + "" != "Y")
                        {
                            _getReceive = _getEmp.Where(w => w.EmpNo == PersonnelRequest.Req_BUApprove_user).FirstOrDefault();
                        }
                        else if (PersonnelRequest.HeadApprove_status + "" != "Y")
                        {
                            _getReceive = _getEmp.Where(w => w.EmpNo == PersonnelRequest.Req_HeadApprove_user).FirstOrDefault();
                        }
                        else if (PersonnelRequest.CeoApprove_status + "" != "Y" && PersonnelRequest.need_ceo_approve + "" == "Y")
                        {
                            _getReceive = _getEmp.Where(w => w.EmpNo == PersonnelRequest.Req_CeoApprove_user).FirstOrDefault();
                        }

                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrl(GetContent.mail_type, PersonnelRequest.Id + "", pathUrl, _getReceive.EmpNo);// 
                            var sTable = HCMFunc.CreateApproveTable(PersonnelRequest);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = _getReceive.Email;
                            mail_to_log = _getReceive.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMail(objMail, ref msg);
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
        public bool SendApproveMail(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 2).FirstOrDefault();
                if (PersonnelRequest != null && GetContent != null && CGlobal.UserInfo != null)
                {

                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PersonnelRequest.request_user);
                    lstUserApp.Add(PersonnelRequest.Req_BUApprove_user);
                    lstUserApp.Add(PersonnelRequest.Req_HeadApprove_user);
                    List<string> PRCC = new List<string>();

                    if (PersonnelRequest.need_ceo_approve + "" == "Y")
                    {
                        lstUserApp.Add(PersonnelRequest.Req_CeoApprove_user);
                    }
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);
                    // PRCC.Add(CGlobal.UserInfo.EmployeeNo);
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
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();
                        AllInfo_WS _getApproved = new AllInfo_WS();
                        if (PersonnelRequest.CeoApprove_status + "" == "Y" && PersonnelRequest.need_ceo_approve + "" == "Y")
                        {
                            _getApproved = _getEmp.Where(w => w.EmpNo == PersonnelRequest.Req_CeoApprove_user).FirstOrDefault();
                            PRCC.Add(PersonnelRequest.Req_CeoApprove_user);
                            PRCC.Add(PersonnelRequest.Req_HeadApprove_user);
                            PRCC.Add(PersonnelRequest.Req_BUApprove_user);

                        }
                        else if (PersonnelRequest.HeadApprove_status + "" == "Y")
                        {
                            _getApproved = _getEmp.Where(w => w.EmpNo == PersonnelRequest.Req_HeadApprove_user).FirstOrDefault();
                            PRCC.Add(PersonnelRequest.Req_HeadApprove_user);
                            PRCC.Add(PersonnelRequest.Req_BUApprove_user);
                        }
                        else if (PersonnelRequest.BUApprove_status + "" == "Y")
                        {
                            _getApproved = _getEmp.Where(w => w.EmpNo == PersonnelRequest.Req_BUApprove_user).FirstOrDefault();
                            PRCC.Add(PersonnelRequest.Req_BUApprove_user);
                        }



                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        if (PRCC.Any())
                        {
                            foreach (var lstCCbm in PRCC)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCCbm).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                        }

                        string pathUrl = Url.Action("MailBoxReceive", "Mail", null, Request.Url.Scheme);

                        if (_getRequest != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrl(GetContent.mail_type, PersonnelRequest.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreateApproveTable(PersonnelRequest);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$approved", _getApproved.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = _getRequest.Email;
                            mail_to_log = _getRequest.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMail(objMail, ref msg);
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
        public bool SendRejectMail(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 4).FirstOrDefault();
                if (PersonnelRequest != null && GetContent != null && CGlobal.UserInfo != null)
                {

                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PersonnelRequest.request_user);
                    lstUserApp.Add(PersonnelRequest.Req_BUApprove_user);
                    lstUserApp.Add(PersonnelRequest.Req_HeadApprove_user);
                    if (PersonnelRequest.need_ceo_approve + "" == "Y")
                    {
                        lstUserApp.Add(PersonnelRequest.Req_CeoApprove_user);
                    }
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();
                        var _getReject = _getEmp.Where(w => w.EmpNo == PersonnelRequest.reject_user).FirstOrDefault();
                        //var _getReceive = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxReceive", "Mail", null, Request.Url.Scheme);

                        if (_getRequest != null && _getReject != null)
                        {
                            string sUrl = HCMFunc.GetUrl(GetContent.mail_type, PersonnelRequest.Id + "", pathUrl, _getRequest.EmpNo);// 
                            var sTable = HCMFunc.CreateApproveTable(PersonnelRequest);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$areject", _getReject.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", PersonnelRequest.reject_reason);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = _getRequest.Email;
                            mail_to_log = _getRequest.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMail(objMail, ref msg);
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
        public bool SendSuccessfullyMail(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 5).FirstOrDefault();
                if (PersonnelRequest != null && GetContent != null && CGlobal.UserInfo != null)
                {

                    List<string> lstUserApp = new List<string>();
                   // List<string> PRCC = new List<string>();
                    lstUserApp.Add(PersonnelRequest.request_user);
                    lstUserApp.Add(PersonnelRequest.Req_BUApprove_user);
                    lstUserApp.Add(PersonnelRequest.Req_HeadApprove_user);

                    //PRCC.Add(PersonnelRequest.Req_HeadApprove_user);
                    //PRCC.Add(PersonnelRequest.Req_BUApprove_user);
                    if (PersonnelRequest.need_ceo_approve + "" == "Y")
                    {
                        lstUserApp.Add(PersonnelRequest.Req_CeoApprove_user);
                        //PRCC.Add(PersonnelRequest.Req_CeoApprove_user);
                    }
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);
                    //PRCC.Add(CGlobal.UserInfo.EmployeeNo);
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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();
                        //var _getReceive = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        //if (PRCC.Any())
                        //{
                        //    foreach (var lstCCbm in PRCC)
                        //    {
                        //        cc += _getEmp.Where(w => w.EmpNo == lstCCbm).Select(s => s.Email).FirstOrDefault() + ",";
                        //    }
                        //}
                        string pathUrl = Url.Action("MailBoxReceive", "Mail", null, Request.Url.Scheme);

                        if (_getRequest != null)
                        {
                            string sUrl = HCMFunc.GetUrlTracking(GetContent.mail_type, PersonnelRequest.Id + "", pathUrl, _getRequest.EmpNo);// 
                            var sTable = HCMFunc.CreateApproveTable(PersonnelRequest);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = _getRequest.Email;
                            mail_to_log = _getRequest.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMail(objMail, ref msg);
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
        public bool SendCancelMail(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 2).FirstOrDefault();
                if (PersonnelRequest != null && GetContent != null && CGlobal.UserInfo != null)
                {

                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PersonnelRequest.request_user);
                    lstUserApp.Add(PersonnelRequest.Req_BUApprove_user);
                    lstUserApp.Add(PersonnelRequest.Req_HeadApprove_user);
                    if (PersonnelRequest.need_ceo_approve + "" == "Y")
                    {
                        lstUserApp.Add(PersonnelRequest.Req_CeoApprove_user);
                    }
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    string cc = "";
                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();
                        var _getCancel = _getEmp.Where(w => w.EmpNo == PersonnelRequest.cancel_user).FirstOrDefault();
                        //var _getReceive = _getEmp.Where(w => w.EmpNo == PersonnelRequest.request_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxReceive", "Mail", null, Request.Url.Scheme);
                        List<string> lstMailto = new List<string>();
                        lstMailto.Add(PersonnelRequest.request_user);
                        if (PersonnelRequest.BUApprove_status == "Y")
                        {
                            lstMailto.Add(PersonnelRequest.Req_BUApprove_user);
                        }
                        if (PersonnelRequest.HeadApprove_status == "Y")
                        {
                            lstMailto.Add(PersonnelRequest.Req_HeadApprove_user);
                        }

                        if (PersonnelRequest.need_ceo_approve + "" == "Y")
                        {
                            if (PersonnelRequest.CeoApprove_status == "Y")
                            {
                                lstMailto.Add(PersonnelRequest.Req_CeoApprove_user);
                            }

                        }

                        string[] aMailto = lstMailto.Select(s => s).Distinct().ToArray();
                        foreach (var aMT in aMailto)
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT).Select(s => s.Email).FirstOrDefault() + ",";
                        }

                        if (_getCancel != null)
                        {
                            string sUrl = HCMFunc.GetUrl(GetContent.mail_type, PersonnelRequest.Id + "", pathUrl, _getRequest.EmpNo);// 
                            var sTable = HCMFunc.CreateApproveTable(PersonnelRequest);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$request", _getCancel.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$cancelremark", PersonnelRequest.cancel_reason);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = mailto;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMail(objMail, ref msg);
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
        #endregion
        #region Mail Partner
        public bool SendPTRSubmit(Models.PartnerEvaluation.PESMain.PTR_Evaluation_Approve PTR_Evaluation_Approve,string comment, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation_Approve != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PTR_Evaluation_Approve.Req_Approve_user);
                    lstUserApp.Add(PTR_Evaluation_Approve.PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.Req_Approve_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation_Approve.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);

                            sContent = (sContent + "").Replace("$sender", _getSender.EmpFullName);

                            sContent = (sContent + "").Replace("$rejectremark", comment);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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
        public bool SendPTRSubmitNewVersion(Models.PartnerEvaluation.PESMain.PTR_Evaluation_Approve PTR_Evaluation_Approve,  Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation_Approve != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PTR_Evaluation_Approve.Req_Approve_user);
                    lstUserApp.Add(PTR_Evaluation_Approve.PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.Req_Approve_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation_Approve.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);

                            sContent = (sContent + "").Replace("$sender", _getSender.EmpFullName);
                            
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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
        public bool SendPTRReviseNewVersion(Models.PartnerEvaluation.PESMain.PTR_Evaluation_Approve PTR_Evaluation_Approve, string comment, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation_Approve != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PTR_Evaluation_Approve.Req_Approve_user);
                    lstUserApp.Add(PTR_Evaluation_Approve.PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.Req_Approve_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation_Approve.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);

                            sContent = (sContent + "").Replace("$sender", _getSender.EmpFullName);

                            sContent = (sContent + "").Replace("$rejectremark", comment);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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

        public bool SendPTRSubmitPlan(Models.PartnerEvaluation.PESMain.PTR_Evaluation_Approve PTR_Evaluation_Approve, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation_Approve != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PTR_Evaluation_Approve.Req_Approve_user);
                    lstUserApp.Add(PTR_Evaluation_Approve.PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.Req_Approve_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation_Approve.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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

        public bool SendPTRSuccessfully(Models.PartnerEvaluation.PESMain.PTR_Evaluation PTR_Evaluation, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    if (PTR_Evaluation.PTR_Evaluation_Approve != null && PTR_Evaluation.PTR_Evaluation_Approve.Any(a => a.active_status == "Y"))
                    {
                        lstUserApp.AddRange(PTR_Evaluation.PTR_Evaluation_Approve.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    }
                    lstUserApp.Add(PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation.user_no).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        if (PTR_Evaluation.PTR_Evaluation_Approve != null && PTR_Evaluation.PTR_Evaluation_Approve.Any(a => a.active_status == "Y"))
                        {
                            foreach (var lstAppCC in PTR_Evaluation.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.type_of_step == "G" && w.active_status == "Y"))
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstAppCC.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                        }

                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            //sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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
        public bool SendPTRRevise(Models.PartnerEvaluation.PESMain.PTR_Evaluation PTR_Evaluation, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);



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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        // var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation.user_no).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$rejectremark", PTR_Evaluation.comments);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
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
       public bool SendPTRReviseNewVersion(Models.PartnerEvaluation.PESMain.PTR_Evaluation PTR_Evaluation, Models.PartnerEvaluation.PESMain.PTR_Evaluation_Approve PTR_Evaluation_Approve, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PTR_Evaluation != null && GetContent != null && CGlobal.UserInfo != null)
                {
                 
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PTR_Evaluation_Approve.Req_Approve_user);
                    lstUserApp.Add(PTR_Evaluation_Approve.PTR_Evaluation.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.Req_Approve_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PTR_Evaluation.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$rejectremark", PTR_Evaluation.comments);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
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
        #endregion
        #region Mail Trainee

        public bool SendEvaApprove(TM_Trainee_Eva TM_Trainee_Eva, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
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
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);



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
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        //var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();

                        //if (TM_Trainee_Eva.approve_type != "M")
                        //{
                        //    _getReceive = _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_mgr_Approve_user).FirstOrDefault();
                        //}
                        //else
                        //{
                        //    _getReceive = _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_incharge_Approve_user).FirstOrDefault();
                        //}

                        _getReceive = _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_mgr_Approve_user).FirstOrDefault();
                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                       // cc += _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_incharge_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                       // cc += _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_mgr_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        string pathUrl = Url.Action("MailBoxTRNReceive", "Mail", null, Request.Url.Scheme);
                        //production

                        //testserver
                        //pathUrl = pathUrl.Replace("TraineeManagement", "application/HR/HumanCapitalManagement");
                        if (_getReceive != null)
                        {
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Trainee_Eva.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", nameReq);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$approved", _getReceive.EmpFullName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = "hcmthailand@kpmg.co.th";// _getReceive.Email;
                            mail_to_log = _getReceive.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMailEva(objMail, ref msg);
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

        public bool SendEvaRollback(TM_Trainee_Eva TM_Trainee_Eva, MailContent MailContent, ref string msg, ref string mail_to_log, string sreject)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
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
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);



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
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
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
                       // cc += _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_incharge_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                       // cc += _getEmp.Where(w => w.EmpNo == TM_Trainee_Eva.req_mgr_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        string pathUrl = Url.Action("MailBoxTRNReceive", "Mail", null, Request.Url.Scheme);
                        //production

                        //testserver
                        //pathUrl = pathUrl.Replace("TraineeManagement", "application/HR/HumanCapitalManagement");
                        if (_getReceive != null)
                        {
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Trainee_Eva.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", nameReq);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$approved", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$rejectremark", sreject);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
                            objMail.mail_to = _getReceive.Email;
                            mail_to_log = _getReceive.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendMailEva(objMail, ref msg);
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
        #endregion

        #region Mail TIF Form
        public bool SendFirstTIFFormSubmit(TM_Candidate_TIF TM_Candidate_TIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_TIF != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status != "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    var GetApproval = TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status != "Y").OrderBy(o => o.seq).FirstOrDefault();
                    if (GetApproval != null)
                    {
                        List<string> lstUserApp = new List<string>();
                        lstUserApp.Add(GetApproval.Req_Approve_user);
                        lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                        var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                        var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                        if (_TM_MailContent_Cc.Any())
                        {
                            lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                        }

                        string[] aUser = lstUserApp.ToArray();
                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                        if (_getEmp.Any())
                        {
                            var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                            AllInfo_WS _getReceive = new AllInfo_WS();

                            _getReceive = _getEmp.Where(w => w.EmpNo == GetApproval.Req_Approve_user).FirstOrDefault();

                            string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                            string candidateName = TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                            if (_getReceive != null && _getSender != null)
                            {
                                string cc = "";// _getSender.Email + ",";
                                foreach (var lstCC in _TM_MailContent_Cc)
                                {
                                    cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                                }
                                foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                                {
                                    cc += lstCCbm.e_mail + ",";
                                }
                                string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, _getReceive.EmpNo);// 
                                var sTable = HCMFunc.CreatTIFFormTable(TM_Candidate_TIF);
                                string sContent = GetContent.content;
                                sContent = (sContent + "").Replace("$request", _getSender.EmpFullName);
                                sContent = (sContent + "").Replace("$slink", sUrl);
                                sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                                sContent = (sContent + "").Replace("$table", sTable);
                                sContent = (sContent + "").Replace("$candidate", candidateName);
                                objMail.mail_from = _getSender.Email;
                                objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                                objMail.mail_to = _getReceive.Email;
                                mail_to_log = _getReceive.Email;
                                objMail.mail_cc = cc;
                                objMail.mail_subject = GetContent.mail_header;
                                objMail.mail_content = sContent;
                                return HCMFunc.SendTIFForm(objMail, ref msg);
                            }
                            else
                            {
                                msg = "Sender or Receive not found";
                                return false;
                            }
                        }
                        else
                        {
                            msg = "Approval Not Found.";
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
        public bool SendTIFFormComplected(TM_Candidate_TIF TM_Candidate_TIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_TIF != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();


                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatTIFFormTable(TM_Candidate_TIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendTIFFormToHR(TM_Candidate_TIF TM_Candidate_TIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_TIF != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                        string candidateName = TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_TIF.TM_PR_Candidate_Mapping.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatTIFFormTable(TM_Candidate_TIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = "hcmthailand@kpmg.co.th";// _getReceive.Email;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendBuRollback(TM_Candidate_TIF TM_Candidate_TIF, MailContent MailContent, string reject, int nSeq, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_TIF != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y" && a.seq < nSeq) && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq).Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();




                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";//_getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatTIFFormTable(TM_Candidate_TIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendHRRollback(TM_Candidate_TIF TM_Candidate_TIF, MailContent MailContent, string reject, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_TIF != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatTIFFormTable(TM_Candidate_TIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendHRAck(TM_Candidate_TIF TM_Candidate_TIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_TIF != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv != null && TM_Candidate_TIF.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();


                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatTIFFormTable(TM_Candidate_TIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        #endregion

        #region Maill Pre Intern
        public bool SendFirstPreInternFormSubmit(TM_Candidate_PIntern TM_Candidate_PIntern, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y" && a.Approve_status != "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    var GetApproval = TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Approve_status != "Y").OrderBy(o => o.seq).FirstOrDefault();
                    if (GetApproval != null)
                    {
                        List<string> lstUserApp = new List<string>();
                        lstUserApp.Add(GetApproval.Req_Approve_user);
                        lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                        var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                        var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                        if (_TM_MailContent_Cc.Any())
                        {
                            lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                        }

                        string[] aUser = lstUserApp.ToArray();
                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                        if (_getEmp.Any())
                        {
                            var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                            AllInfo_WS _getReceive = new AllInfo_WS();

                            _getReceive = _getEmp.Where(w => w.EmpNo == GetApproval.Req_Approve_user).FirstOrDefault();

                            string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                            string candidateName = TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                            if (_getReceive != null && _getSender != null)
                            {
                                string cc = "";// _getSender.Email + ",";
                                foreach (var lstCC in _TM_MailContent_Cc)
                                {
                                    cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                                }
                                foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                                {
                                    cc += lstCCbm.e_mail + ",";
                                }
                                string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, _getReceive.EmpNo);// 
                                var sTable = HCMFunc.CreatPreInternFormTable(TM_Candidate_PIntern);
                                string sContent = GetContent.content;
                                sContent = (sContent + "").Replace("$request", _getSender.EmpFullName);
                                sContent = (sContent + "").Replace("$slink", sUrl);
                                sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                                sContent = (sContent + "").Replace("$table", sTable);
                                sContent = (sContent + "").Replace("$candidate", candidateName);
                                objMail.mail_from = _getSender.Email;
                                objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                                objMail.mail_to = _getReceive.Email;
                                mail_to_log = _getReceive.Email;
                                objMail.mail_cc = cc;
                                objMail.mail_subject = GetContent.mail_header;
                                objMail.mail_content = sContent;
                                return HCMFunc.SendTIFForm(objMail, ref msg);
                            }
                            else
                            {
                                msg = "Sender or Receive not found";
                                return false;
                            }
                        }
                        else
                        {
                            msg = "Approval Not Found.";
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
        public bool SendPreInternFormComplected(TM_Candidate_PIntern TM_Candidate_PIntern, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();


                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreInternFormTable(TM_Candidate_PIntern);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendPreInternFormToHR(TM_Candidate_PIntern TM_Candidate_PIntern, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                        string candidateName = TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern.TM_PR_Candidate_Mapping.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreInternFormTable(TM_Candidate_PIntern);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = "hcmthailand@kpmg.co.th";// _getReceive.Email;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendPreInternRollback(TM_Candidate_PIntern TM_Candidate_PIntern, MailContent MailContent, string reject, int nSeq, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y" && a.seq < nSeq) && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq).Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();




                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";//_getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreInternFormTable(TM_Candidate_PIntern);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendHRPreInternAck(TM_Candidate_PIntern TM_Candidate_PIntern, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();


                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreInternFormTable(TM_Candidate_PIntern);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendHRPReInternRollback(TM_Candidate_PIntern TM_Candidate_PIntern, MailContent MailContent, string reject, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv != null && TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_PIntern.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreInternFormTable(TM_Candidate_PIntern);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        #endregion

        #region Mass Pre Intern
        public bool SendFirstPreIntern_Mass_FormSubmit(TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern_Mass != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y" && a.Approve_status != "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    var GetApproval = TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Approve_status != "Y").OrderBy(o => o.seq).FirstOrDefault();
                    if (GetApproval != null)
                    {
                        List<string> lstUserApp = new List<string>();
                        lstUserApp.Add(GetApproval.Req_Approve_user);
                        lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                        var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                        var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                        if (_TM_MailContent_Cc.Any())
                        {
                            lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                        }

                        string[] aUser = lstUserApp.ToArray();
                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                        if (_getEmp.Any())
                        {
                            var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                            AllInfo_WS _getReceive = new AllInfo_WS();

                            _getReceive = _getEmp.Where(w => w.EmpNo == GetApproval.Req_Approve_user).FirstOrDefault();

                            string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                            string candidateName = TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                            if (_getReceive != null && _getSender != null)
                            {
                                string cc = "";// _getSender.Email + ",";
                                foreach (var lstCC in _TM_MailContent_Cc)
                                {
                                    cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                                }
                                foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                                {
                                    cc += lstCCbm.e_mail + ",";
                                }
                                string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, _getReceive.EmpNo);// 
                                var sTable = HCMFunc.CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass);
                                string sContent = GetContent.content;
                                sContent = (sContent + "").Replace("$request", _getSender.EmpFullName);
                                sContent = (sContent + "").Replace("$slink", sUrl);
                                sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                                sContent = (sContent + "").Replace("$table", sTable);
                                sContent = (sContent + "").Replace("$candidate", candidateName);
                                objMail.mail_from = _getSender.Email;
                                objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                                objMail.mail_to = _getReceive.Email;
                                mail_to_log = _getReceive.Email;
                                objMail.mail_cc = cc;
                                objMail.mail_subject = GetContent.mail_header;
                                objMail.mail_content = sContent;
                                return HCMFunc.SendTIFForm(objMail, ref msg);
                            }
                            else
                            {
                                msg = "Sender or Receive not found";
                                return false;
                            }
                        }
                        else
                        {
                            msg = "Approval Not Found.";
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
        public bool SendPreIntern_Mass_FormComplected(TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern_Mass != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();


                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendPreIntern_Mass_FormToHR(TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern_Mass != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                        string candidateName = TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = "hcmthailand@kpmg.co.th";// _getReceive.Email;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendPreIntern_Mass_Rollback(TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass, MailContent MailContent, string reject, int nSeq, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern_Mass != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y" && a.seq < nSeq) && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq).Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();




                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";//_getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendHRPreIntern_Mass_Ack(TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern_Mass != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();


                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendHRPReIntern_Mass_Rollback(TM_Candidate_PIntern_Mass TM_Candidate_PIntern_Mass, MailContent MailContent, string reject, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_PIntern_Mass != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv != null && TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxPreInternForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_PIntern_Mass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        #endregion
        #region Mail Mass TIF Form
        public bool SendFirstMassTIFFormSubmit(TM_Candidate_MassTIF TM_Candidate_MassTIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_MassTIF != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status != "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    var GetApproval = TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status != "Y").OrderBy(o => o.seq).FirstOrDefault();
                    if (GetApproval != null)
                    {
                        List<string> lstUserApp = new List<string>();
                        lstUserApp.Add(GetApproval.Req_Approve_user);
                        lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                        var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                        var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                        if (_TM_MailContent_Cc.Any())
                        {
                            lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                        }

                        string[] aUser = lstUserApp.ToArray();
                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                        if (_getEmp.Any())
                        {
                            var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                            AllInfo_WS _getReceive = new AllInfo_WS();

                            _getReceive = _getEmp.Where(w => w.EmpNo == GetApproval.Req_Approve_user).FirstOrDefault();

                            string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                            string candidateName = TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                            if (_getReceive != null && _getSender != null)
                            {
                                string cc = "";//= _getSender.Email + ",";
                                foreach (var lstCC in _TM_MailContent_Cc)
                                {
                                    cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                                }
                                foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                                {
                                    cc += lstCCbm.e_mail + ",";
                                }

                                string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, _getReceive.EmpNo);// 
                                var sTable = HCMFunc.CreatMassTIFFormTable(TM_Candidate_MassTIF);
                                string sContent = GetContent.content;
                                sContent = (sContent + "").Replace("$request", _getSender.EmpFullName);
                                sContent = (sContent + "").Replace("$slink", sUrl);
                                sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                                sContent = (sContent + "").Replace("$table", sTable);
                                sContent = (sContent + "").Replace("$candidate", candidateName);
                                objMail.mail_from = _getSender.Email;
                                objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                                objMail.mail_to = _getReceive.Email;
                                mail_to_log = _getReceive.Email;
                                objMail.mail_cc = cc;
                                objMail.mail_subject = GetContent.mail_header;
                                objMail.mail_content = sContent;
                                return HCMFunc.SendTIFForm(objMail, ref msg);
                            }
                            else
                            {
                                msg = "Sender or Receive not found";
                                return false;
                            }
                        }
                        else
                        {
                            msg = "Approval Not Found.";
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
        public bool SendMassTIFFormComplected(TM_Candidate_MassTIF TM_Candidate_MassTIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_MassTIF != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatMassTIFFormTable(TM_Candidate_MassTIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendMassTIFFormToHR(TM_Candidate_MassTIF TM_Candidate_MassTIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_MassTIF != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                        string candidateName = TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatMassTIFFormTable(TM_Candidate_MassTIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = "hcmthailand@kpmg.co.th";// _getReceive.Email;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendMassBuRollback(TM_Candidate_MassTIF TM_Candidate_MassTIF, MailContent MailContent, string reject, int nSeq, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_MassTIF != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y" && a.seq < nSeq) && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq).Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y" && a.seq < nSeq))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {

                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatMassTIFFormTable(TM_Candidate_MassTIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendMassHRRollback(TM_Candidate_MassTIF TM_Candidate_MassTIF, MailContent MailContent, string reject, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_MassTIF != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);
                        string sRecivename = "";
                        foreach (var aMT in TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            sRecivename = "Khun " + _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.EmpFullName).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                        if (_getSender != null)
                        {
                            string cc = "";//_getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }
                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatMassTIFFormTable(TM_Candidate_MassTIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", sRecivename);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$rejectremark", reject);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        public bool SendMassHRAck(TM_Candidate_MassTIF TM_Candidate_MassTIF, MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (TM_Candidate_MassTIF != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv != null && TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y" && a.Approve_status == "Y") && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.AddRange(TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);

                    var _TM_MailContent_Cc = GetContent.TM_MailContent_Cc.Where(w => w.active_status == "Y").ToList();
                    var _TM_MailContent_Cc_bymail = GetContent.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y").ToList();
                    if (_TM_MailContent_Cc.Any())
                    {
                        lstUserApp.AddRange(_TM_MailContent_Cc.Select(s => s.user_no).ToArray());
                    }
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();

                    string mailto = "";
                    if (_getEmp.Any())
                    {
                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();

                        string pathUrl = Url.Action("MailBoxTIFForm", "Mail", null, Request.Url.Scheme);

                        foreach (var aMT in TM_Candidate_MassTIF.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y" && a.Approve_status == "Y"))
                        {
                            mailto += _getEmp.Where(w => w.EmpNo == aMT.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string candidateName = TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                        if (_getSender != null)
                        {
                            string cc = "";// _getSender.Email + ",";
                            foreach (var lstCC in _TM_MailContent_Cc)
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                            foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                            {
                                cc += lstCCbm.e_mail + ",";
                            }

                            string sUrl = HCMFunc.GetUrlTrainee(GetContent.mail_type, TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.Id + "", pathUrl, "");// 
                            var sTable = HCMFunc.CreatMassTIFFormTable(TM_Candidate_MassTIF);
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$table", sTable);
                            sContent = (sContent + "").Replace("$candidate", candidateName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
                            objMail.mail_to = mailto;
                            mail_to_log = _getSender.Email;
                            objMail.mail_cc = cc;
                            objMail.mail_subject = GetContent.mail_header;
                            objMail.mail_content = sContent;
                            return HCMFunc.SendTIFForm(objMail, ref msg);
                        }
                        else
                        {
                            msg = "Sender or Receive not found";
                            return false;
                        }
                    }
                    else
                    {
                        msg = "Approval Not Found.";
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
        #endregion

        #region Nomination Mail
        public bool SendNominationSubmit(Models.PartnerEvaluation.NominationMain.PES_Nomination_Signatures PES_Nomination_Signatures, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PES_Nomination_Signatures != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PES_Nomination_Signatures.Req_Approve_user);
                    lstUserApp.Add(PES_Nomination_Signatures.PES_Nomination.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PES_Nomination_Signatures.PES_Nomination.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PES_Nomination_Signatures.Req_Approve_user).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PES_Nomination_Signatures.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName + (PES_Nomination_Signatures.TM_PES_NMN_SignatureStep != null ? "(" + PES_Nomination_Signatures.TM_PES_NMN_SignatureStep.Step_name_en + ")" : ""));
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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

        public bool SendNominationSuccessfully(Models.PartnerEvaluation.NominationMain.PES_Nomination PES_Nomination, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PES_Nomination != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    if (PES_Nomination.PES_Nomination_Signatures != null && PES_Nomination.PES_Nomination_Signatures.Any(a => a.active_status == "Y"))
                    {
                        lstUserApp.AddRange(PES_Nomination.PES_Nomination_Signatures.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToList());
                    }
                    lstUserApp.Add(PES_Nomination.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        var _getRequest = _getEmp.Where(w => w.EmpNo == PES_Nomination.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PES_Nomination.user_no).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        if (PES_Nomination.PES_Nomination_Signatures != null && PES_Nomination.PES_Nomination_Signatures.Any(a => a.active_status == "Y"))
                        {
                            foreach (var lstAppCC in PES_Nomination.PES_Nomination_Signatures.Where(w => w.active_status == "Y"))
                            {
                                cc += _getEmp.Where(w => w.EmpNo == lstAppCC.Req_Approve_user).Select(s => s.Email).FirstOrDefault() + ",";
                            }
                        }

                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PES_Nomination.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getRequest.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            //sContent = (sContent + "").Replace("$spprove", _getReceive.EmpFullName);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFirstName + ", " + _getSender.EmpSurname;//GetContent.sender_name;
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

        public bool SendNominationRevise(Models.PartnerEvaluation.NominationMain.PES_Nomination PES_Nomination, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PES_Nomination != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PES_Nomination.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);


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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        // var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PES_Nomination.user_no).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PES_Nomination.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$rejectremark", PES_Nomination.comments);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
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

        public bool SendNominationReviseNewVersion(Models.PartnerEvaluation.NominationMain.PES_Nomination PES_Nomination,List<string> MaillCC, Models.Common.MailContent MailContent, ref string msg, ref string mail_to_log)
        {

            ViewModel.CommonVM.vObjectMail_Send objMail = new ViewModel.CommonVM.vObjectMail_Send();
            try
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                var GetContent = MailContent;//db.MailContent.Where(w => w.Id == 1).FirstOrDefault();
                if (PES_Nomination != null && GetContent != null && CGlobal.UserInfo != null)
                {
                    List<string> lstUserApp = new List<string>();
                    lstUserApp.Add(PES_Nomination.user_no);
                    lstUserApp.Add(CGlobal.UserInfo.EmployeeNo);
                    foreach (var lstCCset in MaillCC)
                    {
                        lstUserApp.Add(lstCCset);
                    }

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

                        var _getSender = _getEmp.Where(w => w.EmpNo == CGlobal.UserInfo.EmployeeNo).FirstOrDefault();
                        // var _getRequest = _getEmp.Where(w => w.EmpNo == PTR_Evaluation_Approve.PTR_Evaluation.user_no).FirstOrDefault();
                        AllInfo_WS _getReceive = new AllInfo_WS();


                        _getReceive = _getEmp.Where(w => w.EmpNo == PES_Nomination.user_no).FirstOrDefault();


                        foreach (var lstCC in _TM_MailContent_Cc)
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCC.user_no).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        foreach (var lstCCbm in _TM_MailContent_Cc_bymail)
                        {
                            cc += lstCCbm.e_mail + ",";
                        }
                        foreach (var lstCCset in MaillCC.Where(w => w != PES_Nomination.user_no))
                        {
                            cc += _getEmp.Where(w => w.EmpNo == lstCCset).Select(s => s.Email).FirstOrDefault() + ",";
                        }
                        string pathUrl = Url.Action("MailBoxPTRReceive", "Mail", null, Request.Url.Scheme);

                        if (_getReceive != null && _getSender != null)
                        {
                            string sUrl = HCMFunc.GetUrlPTR(GetContent.mail_type, PES_Nomination.Id + "", pathUrl, _getReceive.EmpNo);// 
                            string sContent = GetContent.content;
                            sContent = (sContent + "").Replace("$request", _getReceive.EmpFullName);
                            sContent = (sContent + "").Replace("$slink", sUrl);
                            sContent = (sContent + "").Replace("$spprove", _getSender.EmpFullName);
                            sContent = (sContent + "").Replace("$rejectremark", PES_Nomination.comments);
                            objMail.mail_from = _getSender.Email;
                            objMail.title_mail_from = _getSender.EmpFullName;//GetContent.sender_name;
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
        #endregion
    }

}