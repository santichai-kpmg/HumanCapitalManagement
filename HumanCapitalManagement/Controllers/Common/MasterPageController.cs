using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.HCMFunc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class MasterPageController : Controller
    {
        private MPService _MPRepository;
        private MenuActionService _MenuActionService;
        private UserPermissionService _UserPermissionService;
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        public MasterPageController(MPService MPService, MenuActionService MenuActionService
            , UserPermissionService UserPermissionService
            )
        {
            _MPRepository = MPService;
            _MenuActionService = MenuActionService;
            _UserPermissionService = UserPermissionService;
        }

        public ActionResult Error500(vErrorObject obj)
        {
            return View(obj);
        }
        public ActionResult Error404(vErrorObject obj)
        {
            vErrorObject result = new vErrorObject();

            return View(obj);
        }
        public ActionResult ErrorNopermission(vErrorObject obj)
        {
            vErrorObject result = new vErrorObject();

            return PartialView("_noPermission");
        }
        //public ActionResult LoginError(vErrorObject obj)
        //{
        //    vErrorObject result = new vErrorObject();

        //    return PartialView("_noPermission");
        //}
        public ActionResult LoginError(vErrorObject obj)
        {
            vErrorObject result = new vErrorObject();

            return View(obj);
        }
        public ActionResult Maintenance(vErrorObject obj)
        {
            vErrorObject result = new vErrorObject();

            return View(obj);
        }
        public ActionResult CheckCurrentSession(vErrorObject obj)
        {
            vErrorObject result = new vErrorObject();
            result.msg = "";
            if (Session != null)
            {
                foreach (var crntSession in Session)
                {
                    result.msg += string.Concat(crntSession, "=", Session[crntSession.ToString()]) + "<br />";
                }
            }

            return View(result);
        }
        #region Ajax Function

        // GET: MasterPage
        public ActionResult CreMenu()
        {
            List<Menu> lstMenu = new List<Menu>();
            if (_MPRepository != null)
            {
                int[] aNotAdmin = new int[] { };
                var ControllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                var ActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                lstMenu = _MPRepository.GetlstMenu().OrderBy(o => o.MENU_SEQ).ToList();
                var _GetPESMenu = _MPRepository.GetPESMenu();
                string winloginnamea = this.User.Identity.Name;
                var c = this.User.Identity;
                string EmpUserIDa = winloginnamea.Substring(winloginnamea.IndexOf("\\") + 1);
                if (CGlobal.IsUserExpired())
                {
                    DataTable staff = wsHRis.getEmployeeInfoByUserID(EmpUserIDa);
                    //staff = wsHRis.getEmployeeInfoByUserID("ojiyaamorndej");
                    if (staff != null)
                    {
                        var CheckUser = staff.AsEnumerable().FirstOrDefault();
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
                if (!CGlobal.UserIsAdmin() && !CGlobal.UserIsAdminPES())
                {
                    List<Menu> lstUserMenu = new List<Menu>();
                    lstUserMenu = lstMenu.Where(w => w.MENU_Permission + "" == "N").ToList();
                    List<int> lstMID = new List<int>();
                    var _getUser = _UserPermissionService.FindByEmpNo(CGlobal.UserInfo.EmployeeNo);
                    if (_getUser != null && _getUser.UserListPermission != null && _getUser.UserListPermission.Any(a => a.active_status + "" == "Y"))
                    {
                        lstUserMenu.AddRange(_getUser.UserListPermission.Where(w => w.active_status + "" == "Y").Select(s => s.MenuAction.Menu).ToList());
                    }


                    if ((CGlobal.UserInfo.RankID != "12" && CGlobal.UserInfo.RankID != "20" && CGlobal.UserInfo.RankID != "32" && CGlobal.UserInfo.EmployeeNo != "00019318") && !CGlobal.UserIsAdminPES())
                    {
                        foreach (var nID in Enum.GetValues(typeof(PESClass.PRT_Menu)))
                        {
                            int[] nPESid = _GetPESMenu.Select(s => s.Id).ToArray();
                            int xID = (int)nID;
                            lstUserMenu = lstUserMenu.Where(w => !nPESid.Contains(w.Id)).ToList();
                        }
                        //lstUserMenu = lstUserMenu.Where(w => w.Id != (int)PESClass.PRT_Menu.Evaluation_Form).ToList();
                        // lstUserMenu = lstUserMenu.Where(w => w.Id != (int)PESClass.PRT_Menu.ApproveEvaluation_Form).ToList();
                    }


                    foreach (var lst in lstUserMenu)
                    {
                        lstMID.Add(lst.Id);
                        if (!string.IsNullOrEmpty(lst.MENU_PARENT))
                        {
                            lstMID = lstMID.Concat(GetParent(lstMenu, lst.MENU_PARENT)).ToList();
                        }
                    }

                    aNotAdmin = lstMID.ToArray();
                    lstMenu = lstMenu.Where(w => aNotAdmin.Contains(w.Id)).ToList();
                }
                else if (CGlobal.UserIsAdmin() && !CGlobal.UserIsAdminPES())
                {
                    // lstMenu = lstMenu.Where(w => w.menu_type == "HCM" || w.MENU_Permission + "" == "N").ToList();
                    List<Menu> lstUserMenu = new List<Menu>();
                    lstUserMenu = lstMenu.Where(w => w.menu_type == "HCM" || w.MENU_Permission + "" == "N").ToList();
                    List<int> lstMID = new List<int>();
                    var _getUser = _UserPermissionService.FindByEmpNo(CGlobal.UserInfo.EmployeeNo);
                    if (_getUser != null && _getUser.UserListPermission != null && _getUser.UserListPermission.Any(a => a.active_status + "" == "Y"))
                    {
                        lstUserMenu.AddRange(_getUser.UserListPermission.Where(w => w.active_status + "" == "Y").Select(s => s.MenuAction.Menu).ToList());
                    }

                    if ((CGlobal.UserInfo.RankID != "12" && CGlobal.UserInfo.RankID != "20" && CGlobal.UserInfo.RankID != "32") && !CGlobal.UserIsAdminPES())
                    {
                        foreach (var nID in Enum.GetValues(typeof(PESClass.PRT_Menu)))
                        {
                            int[] nPESid = _GetPESMenu.Select(s => s.Id).ToArray();
                            int xID = (int)nID;
                            lstUserMenu = lstUserMenu.Where(w => !nPESid.Contains(w.Id)).ToList();
                        }

                        //    lstUserMenu = lstUserMenu.Where(w => w.Id != (int)PESClass.PRT_Menu.Evaluation_Form).ToList();
                        //lstUserMenu = lstUserMenu.Where(w => w.Id != (int)PESClass.PRT_Menu.ApproveEvaluation_Form).ToList();
                    }

                    foreach (var lst in lstUserMenu)
                    {
                        lstMID.Add(lst.Id);
                        if (!string.IsNullOrEmpty(lst.MENU_PARENT) && !lst.MENU_PARENT.Contains(lstMID + ""))
                        {
                            lstMID = lstMID.Concat(GetParent(lstMenu, lst.MENU_PARENT)).ToList();
                        }
                    }

                    aNotAdmin = lstMID.ToArray();
                    lstMenu = lstMenu.Where(w => aNotAdmin.Contains(w.Id)).ToList();
                }
                else if (!CGlobal.UserIsAdmin() && CGlobal.UserIsAdminPES())
                {
                    //lstMenu = lstMenu.Where(w => w.menu_type == "PES" || w.MENU_Permission + "" == "N").ToList();
                    List<Menu> lstUserMenu = new List<Menu>();
                    lstUserMenu = lstMenu.Where(w => w.menu_type == "PES" || w.MENU_Permission + "" == "N").ToList();
                    List<int> lstMID = new List<int>();
                    var _getUser = _UserPermissionService.FindByEmpNo(CGlobal.UserInfo.EmployeeNo);
                    if (_getUser != null && _getUser.UserListPermission != null && _getUser.UserListPermission.Any(a => a.active_status + "" == "Y"))
                    {
                        lstUserMenu.AddRange(_getUser.UserListPermission.Where(w => w.active_status + "" == "Y").Select(s => s.MenuAction.Menu).ToList());
                    }

                    foreach (var lst in lstUserMenu)
                    {
                        lstMID.Add(lst.Id);
                        if (!string.IsNullOrEmpty(lst.MENU_PARENT) && !lst.MENU_PARENT.Contains(lstMID + ""))
                        {
                            lstMID = lstMID.Concat(GetParent(lstMenu, lst.MENU_PARENT)).ToList();
                        }
                    }

                    aNotAdmin = lstMID.ToArray();
                    lstMenu = lstMenu.Where(w => aNotAdmin.Contains(w.Id)).ToList();
                }


                lstMenu.ForEach(ed =>
                {
                    (ed.LINK + "").Replace("\r\n", string.Empty);
                    (ed.MENU_NAME_TH + "").Replace("\r\n", string.Empty);
                    ed.Controller = ed.MenuAction.Any() ? ed.MenuAction.OrderBy(f => f.Action_Type).FirstOrDefault().Controller : "";
                    ed.Action = ed.MenuAction.Any() ? ed.MenuAction.OrderBy(f => f.Action_Type).FirstOrDefault().Action : "";
                });
                var _Ac = lstMenu.Where(w =>
                (w.Controller + "").Trim().ToLower() == (ControllerName + "").Trim().ToLower() &&
                (w.Action + "").Trim().ToLower() == (ActionName + "").Trim().ToLower()
                ).FirstOrDefault();


                if (_Ac != null)
                {
                    lstMenu.Where(w => w.Id == _Ac.Id).ToList().ForEach(ed => { ed.active_menu = "Y"; });
                    //if (!string.IsNullOrEmpty(_Ac.MENU_PARENT))
                    //{
                    //    lstMenu = SetActiveMenu(lstMenu, SystemFunction.GetIntNullToZero(_Ac.MENU_PARENT));
                    //}
                }
                else
                {
                    var nIDAc = _MenuActionService.GetActionMenu(ControllerName, ActionName);
                    if (nIDAc != 0)
                    {
                        lstMenu.Where(w => w.Id == nIDAc).ToList().ForEach(ed => { ed.active_menu = "Y"; });
                    }
                }
            }
            return PartialView("_LoMenu", lstMenu);
        }
        private List<Menu> SetActiveMenu(List<Menu> oldlst, int nActive)
        {
            List<Menu> lstNew = new List<Menu>();
            var _Ac = oldlst.Where(w => w.Id == nActive).FirstOrDefault();
            if (_Ac != null)
            {
                oldlst.Where(w => w.Id == _Ac.Id).ToList().ForEach(ed => { ed.active_menu = "Y"; });
                if (!string.IsNullOrEmpty(_Ac.MENU_PARENT))
                {
                    oldlst = SetActiveMenu(oldlst, SystemFunction.GetIntNullToZero(_Ac.MENU_PARENT));
                }
            }


            return oldlst;
        }
        private List<int> GetParent(List<Menu> lstMenu, string sParentID)
        {
            List<int> lstNewID = new List<int>();
            var _Ac = lstMenu.Where(w => w.Id + "" == sParentID).FirstOrDefault();
            if (_Ac != null)
            {
                lstNewID.Add(_Ac.Id);
                if (!string.IsNullOrEmpty(_Ac.MENU_PARENT))
                {
                    lstNewID = lstNewID.Concat(GetParent(lstMenu, _Ac.MENU_PARENT)).ToList();
                }
            }


            return lstNewID;
        }
        //function for ddl advance type
        #endregion

    }
}