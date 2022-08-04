using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace HumanCapitalManagement.Controllers.Common
{
    public class UserPermissionController : BaseController
    {
        private MPService _MPRepository;
        private MenuActionService _MenuActionService;
        private GroupPermissionService _GroupPermissionService;
        private UserPermissionService _UserPermissionService;
        private UserListPermissionService _UserListPermissionService;
        private DivisionService _DivisionService;
        private UserUnitGroupService _UserUnitGroupService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        public UserPermissionController(MPService MPService, MenuActionService MenuActionService,
            GroupPermissionService GroupPermissionService,
            UserPermissionService UserPermissionService,
            UserListPermissionService UserListPermissionService,
            DivisionService DivisionService,
            UserUnitGroupService UserUnitGroupService)
        {
            _MPRepository = MPService;
            _MenuActionService = MenuActionService;
            _GroupPermissionService = GroupPermissionService;
            _UserPermissionService = UserPermissionService;
            _UserListPermissionService = UserListPermissionService;
            _DivisionService = DivisionService;
            _UserUnitGroupService = UserUnitGroupService;
        }
        // GET: UserPermission
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserPermissionList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUserPermission result = new vUserPermission();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchUser_Permission SearchItem = (CSearchUser_Permission)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchUser_Permission)));

                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
                string[] UserNo = null;
                if (!string.IsNullOrEmpty(SearchItem.name))
                {
                    UserNo = sQuery.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.UserID + "").Trim()).ToLower()).ToArray();
                }
                int nGID = SystemFunction.GetIntNullToZero(SearchItem.group_permiss_id + "");
                var lstData = _UserPermissionService.GetUserPermissionList(
           UserNo,
            nGID,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                result.group_code = SearchItem.group_code;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    UserNo = lstData.Select(s => s.user_id).ToArray();
                    //string aCode = "";
                    //DataTable lstEmp = new DataTable();
                    //foreach (var code in lstData)
                    //{
                    //    aCode += code.user_no + ",";
                    //}
                    //lstEmp = wsHRis.getEmployeeInfoByEmpNoMultiple(aCode);
                    List<vClassEmployeeFromWS> lstEmplist = dbHr.AllInfo_WS.Where(w => UserNo.Contains(w.UserID)).Select(s => new vClassEmployeeFromWS
                    {
                        unitgroup_code = s.UnitGroupID + "",
                        rank = s.Rank,
                        unitgroup = s.UnitGroupName,
                        emp_no = s.EmpNo,
                        emp_full_name = s.EmpFullName,
                    }).ToList();
                    //List<vClassEmployeeFromWS> lstEmplist = lstEmp.AsEnumerable().Select(e => new vClassEmployeeFromWS
                    //{
                    //    unitgroup_code = e["Division"].ToString(),
                    //    rank = e["Rank"].ToString(),
                    //    unitgroup = e["UnitGroup"].ToString(),
                    //    emp_no = e["EmpNo"].ToString(),
                    //    emp_full_name = e.Field<string>("EmpFullName"),
                    //}).ToList();
                    result.lstData = (from lstAD in lstData
                                      from lstE in lstEmplist.Where(w => w.emp_no == lstAD.user_no).DefaultIfEmpty(new vClassEmployeeFromWS())
                                      select new vUserPermiss_obj
                                      {
                                          user_no = lstAD.user_no,
                                          user_name = lstE.emp_full_name,
                                          unit_group = lstE.unitgroup,
                                          rank = lstE.rank,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          permis_group = lstAD.GroupPermission.group_name_en.StringRemark(40),
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          unit_permis = "",
                                          lstGroup_Per = lstAD.UserUnitGroup != null ? lstAD.UserUnitGroup.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Divisions.division_name_en).Take(2).Select(s2 => new vListGroup_lstData
                                          {
                                              group_name = s2.TM_Divisions.division_name_en
                                          }).ToList() : new List<vListGroup_lstData>(),
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          user_group_code = lstE.unitgroup_code,
                                      }).ToList();
                    if (!string.IsNullOrEmpty(SearchItem.group_code))
                    {
                        result.lstData = result.lstData.Where(w => w.user_group_code == SearchItem.group_code).ToList();
                    }
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult UserPermissionCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUserPermiss_obj result = new vUserPermiss_obj();

            var _getMenuActive = _MPRepository.GetlstMenu();
            List<Menu> lstMenuWithOrder = new List<Menu>();

            foreach (var lsM in _getMenuActive.Where(w => w.MENU_LEVEL == 1).OrderBy(o => o.MENU_SEQ).ToList())
            {
                lstMenuWithOrder.Add(lsM);
                if (lsM.MENU_SUB + "" == "Y")
                {
                    lstMenuWithOrder = lstMenuWithOrder.Concat(HCMFunc.CreatListMenu(_getMenuActive.ToList(), lsM.MENU_ID)).ToList();
                }
            }
            int nSeq = 1;
            result.lstData = lstMenuWithOrder.Select(s => new vUserListPermission
            {
                menu_id = s.MENU_ID,
                menu_name = (s.MENU_SUB + "" == "Y" ? @"<i class=""fa fa-fw fa-plus""></i>" : @"<i class=""fa fa-fw fa-minus""></i>") + s.MENU_NAME_EN,
                n_seq = nSeq++,
                menu_level = s.MENU_LEVEL,
                view_action = s.MenuAction.Any(w => w.Action_Type == 1 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                add_action = s.MenuAction.Any(w => w.Action_Type == 2 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                edit_action = s.MenuAction.Any(w => w.Action_Type == 3 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                approve_action = s.MenuAction.Any(w => w.Action_Type == 4 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                detail_action = s.MenuAction.Any(w => w.Action_Type == 5 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
            }).ToList();
            result.lstDataSave = new List<vUserListPermission>();
            return View(result);
        }

        public ActionResult UserPermissionEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code
            var bReturn = false;
            vUserPermiss_obj result = new vUserPermiss_obj();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _UserPermissionService.Find(nId);
                    if (_getData != null)
                    {

                        var _Getuser = dbHr.Employee.Where(w => w.Employeeno == _getData.user_no).FirstOrDefault();
                        var _getUnit = dbHr.JobInfo.Where(w => w.Employeeno == _getData.user_no).FirstOrDefault();
                        result = new vUserPermiss_obj
                        {
                            user_no = _getData.user_no,
                            active_status = _getData.active_status,
                            IdEncrypt = HCMFunc.Encrypt(_getData.Id + ""),
                            user_name = _Getuser != null ? _Getuser.Employeename + " " + _Getuser.Employeesurname : "",
                            user_last_name = _Getuser != null ? _Getuser.Employeesurname : "",
                            unit_name = _getUnit != null ? _getUnit.UnitGroup : "",
                            description = _getData.User_description,
                            group_permiss_id = _getData.GroupPermission.Id + "",
                            user_id = _getData.user_id,
                            aUnitCode = _getData.UserUnitGroup != null ? _getData.UserUnitGroup.Where(w => w.active_status == "Y").Select(s => s.TM_Divisions.division_code).ToArray() : new string[] { },
                        };
                        var _getMenuActive = _MPRepository.GetlstMenu();
                        List<Menu> lstMenuWithOrder = new List<Menu>();

                        foreach (var lsM in _getMenuActive.Where(w => w.MENU_LEVEL == 1).OrderBy(o => o.MENU_SEQ).ToList())
                        {
                            lstMenuWithOrder.Add(lsM);
                            if (lsM.MENU_SUB + "" == "Y")
                            {
                                lstMenuWithOrder = lstMenuWithOrder.Concat(HCMFunc.CreatListMenu(_getMenuActive.ToList(), lsM.MENU_ID)).ToList();
                            }
                        }
                        int nSeq = 1;
                        result.lstData = (from lstM in lstMenuWithOrder
                                          from lstView in _getData.UserListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 1 && w.active_status == "Y").DefaultIfEmpty(new UserListPermission()).Take(1)
                                          from lstAdd in _getData.UserListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 2 && w.active_status == "Y").DefaultIfEmpty(new UserListPermission()).Take(1)
                                          from lstEdit in _getData.UserListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 3 && w.active_status == "Y").DefaultIfEmpty(new UserListPermission()).Take(1)
                                          from lstApprove in _getData.UserListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 4 && w.active_status == "Y").DefaultIfEmpty(new UserListPermission()).Take(1)
                                          from lstDetail in _getData.UserListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 5 && w.active_status == "Y").DefaultIfEmpty(new UserListPermission()).Take(1)
                                          select new vUserListPermission
                                          {
                                              menu_id = lstM.MENU_ID,
                                              menu_name = (lstM.MENU_SUB + "" == "Y" ? @"<i class=""fa fa-fw fa-plus""></i>" : @"<i class=""fa fa-fw fa-minus""></i>") + lstM.MENU_NAME_EN,
                                              n_seq = nSeq++,
                                              menu_level = lstM.MENU_LEVEL,
                                              view_action = lstM.MenuAction.Any(w => w.Action_Type == 1 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                                              add_action = lstM.MenuAction.Any(w => w.Action_Type == 2 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                                              edit_action = lstM.MenuAction.Any(w => w.Action_Type == 3 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                                              approve_action = lstM.MenuAction.Any(w => w.Action_Type == 4 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                                              detail_action = lstM.MenuAction.Any(w => w.Action_Type == 5 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                                              view_value = lstView.active_status + "" == "Y" ? "Y" : "N",
                                              add_value = lstAdd.active_status + "" == "Y" ? "Y" : "N",
                                              edit_value = lstEdit.active_status + "" == "Y" ? "Y" : "N",
                                              approve_value = lstApprove.active_status + "" == "Y" ? "Y" : "N",
                                              detail_value = lstDetail.active_status + "" == "Y" ? "Y" : "N",
                                          }).ToList();
                        bReturn = true;
                    }
                    result.lstDataSave = new List<vUserListPermission>();
                }
            }

            //return
            if (bReturn)
            {
                return View(result);
            }
            else
            {
                return RedirectToAction("ListPermission", "Permission");
            }

            #endregion

        }

        public ActionResult GroupDetails(vListGroup_popup_detail ItemData)
        {
            vListGroup_popup_detail result = new vListGroup_popup_detail();
            if (ItemData != null && !string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nID = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nID != 0)
                {
                    List<vListGroup_lstData> lstData = new List<vListGroup_lstData>();
                    var _getData = _UserPermissionService.Find(nID);
                    if (_getData != null)
                    {
                        if (_getData.UserUnitGroup != null && _getData.UserUnitGroup.Any(w => w.active_status == "Y"))
                        {
                            result.lstGroup = (from lstAD in _getData.UserUnitGroup.Where(w => w.active_status == "Y")
                                               select new vListGroup_lstData
                                               {
                                                   group_name = lstAD.TM_Divisions != null ? lstAD.TM_Divisions.division_name_en : "",
                                               }).ToList();

                        }
                    }

                }
                else
                {
                    result.IdEncrypt = "";
                }

            }
            else
            {
                result.IdEncrypt = "";
            }
            return PartialView("_GroupDetails", result);
        }

        #region Ajax Function
        [HttpPost]
        public JsonResult UserAutoComplete(string SearchItem)
        {
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            string[] aAllUser = _UserPermissionService.GetAllUserPermissions().Select(s => s.user_id).ToArray();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && !aAllUser.Contains(w.UserID));

            var _getData = sQuery.Where(w =>
            (
            (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
            ).Contains((SearchItem + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
            ).Take(15).ToList();
            if (_getData.Any())
            {
                string[] aNo = _getData.Select(s => s.Employeeno).ToArray();
                var _getUnit = dbHr.JobInfo.Where(w => aNo.Contains(w.Employeeno)).ToList();
                result = (from lstGt in _getData
                          from lstUnit in _getUnit.Where(w => w.Employeeno == lstGt.Employeeno).DefaultIfEmpty(new JobInfo())
                          select new C_USERS_RETURN
                          {
                              id = lstGt.Employeeno,
                              user_id = lstGt.UserID,
                              user_name = lstGt.Employeename + " " + lstGt.Employeesurname,
                              unit_name = lstUnit.UnitGroup
                          }
                    ).ToList();
                //_getData.Select(s => new C_USERS_RETURN
                //    {
                //        id = s.Employeeno,
                //        user_id = s.UserID,
                //        user_name = s.Employeename,
                //        user_last_name = s.Employeesurname
                //    }).ToList();
            }
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadListofUser(vSearchPermisMuntl SearchItem)
        {
            vUserPermisMuntl_Return result = new vUserPermisMuntl_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            #region แก้ไขข้อมูลในการค้นหา
            string[] aAllUser = _UserPermissionService.GetAllUserPermissions().Select(s => s.user_id).ToArray();
            IQueryable<AllInfo_WS> sQuery = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && !aAllUser.Contains(w.UserID));



            if (!string.IsNullOrEmpty(SearchItem.group_id))
            {
                sQuery = sQuery.Where(w => w.UnitGroupID + "" == SearchItem.group_id);
            }
            if (!string.IsNullOrEmpty(SearchItem.rank_id))
            {
                sQuery = sQuery.Where(w => w.RankID + "" == SearchItem.rank_id);
            }
            var _getData = sQuery.ToList();
            if (_getData.Any())
            {
                int nID = 1;
                result.lstData = (from lstGt in _getData
                                  select new C_USERS_RETURN
                                  {
                                      id = "" + nID++,
                                      user_id = lstGt.UserID,
                                      user_name = lstGt.EmpFullName,
                                      unit_name = lstGt.UnitGroupName,
                                      user_rank = lstGt.Rank,
                                      user_no = lstGt.EmpNo,
                                  }
                    ).ToList();
               
            }
            result.Status = SystemFunction.process_Success;
            #endregion
            //return result;
            return Json(new { result });
        }
        [HttpPost]
        public JsonResult LoadGroupPermis(string SearchItem)
        {
            vLoadPermission_Return result = new vLoadPermission_Return();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            var _getData = _GroupPermissionService.Find(SystemFunction.GetIntNullToZero(SearchItem));
            if (_getData != null)
            {
                if (_getData.GroupListPermission != null && _getData.GroupListPermission.Any())
                    result.lstData = _getData.GroupListPermission.Select(s => new vListPermission
                    {
                        menu_id = s.MenuAction.Menu.MENU_ID,
                        Action_Type = s.MenuAction.Action_Type + "",
                    }).ToList();
            }
            result.Status = SystemFunction.process_Success;
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateUserPermission(vUserPermiss_obj ItemData)
        {
            vUserPermission_Return result = new vUserPermission_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;

                if (!string.IsNullOrEmpty(ItemData.user_no))
                {

                    var _getEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.user_no).FirstOrDefault();
                    if (_getEmp == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Employee not found.";
                        return Json(new
                        {
                            result
                        });
                    }
                    List<MenuAction> lstForSaave = new List<MenuAction>();
                    List<UserListPermission> Permiss = new List<UserListPermission>();
                    List<UserUnitGroup> lstDivision = new List<UserUnitGroup>();

                    #region List Menu Permission
                    if (ItemData.lstDataSave != null && ItemData.lstDataSave.Any())
                    {

                        foreach (var lsA in ItemData.lstDataSave)
                        {
                            if (lsA.view_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 1);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }

                            }

                            if (lsA.add_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 2);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }
                            if (lsA.edit_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 3);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }
                            if (lsA.approve_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 4);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }
                            if (lsA.detail_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 5);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }

                        }
                    }
                    #endregion

                    #region List Division Permission
                    var _GetDivision = _DivisionService.GetDivisionForSave(ItemData.aUnitCode);
                    if (_GetDivision.Any())
                    {

                        lstDivision = _GetDivision.Select(s => new UserUnitGroup
                        {
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            TM_Divisions = s,
                            unit_code = s.division_code,
                        }).ToList();


                    }

                    #endregion


                    UserPermission objSave = new UserPermission()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        User_description = ItemData.description,
                        UserListPermission = Permiss.ToList(),
                        GroupPermission = _GroupPermissionService.Find(SystemFunction.GetIntNullToZero(ItemData.group_permiss_id)),
                        user_id = _getEmp.UserID,
                        user_no = _getEmp.Employeeno,
                        UserUnitGroup = lstDivision.ToList(),
                    };
                    if (_UserPermissionService.CanSave(objSave))
                    {
                        var sComplect = _UserPermissionService.CreateNew(ref objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Duplicate;
                        result.Msg = "Staff already exist, please try again.";
                    }


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, โปรดระบุพนักงาน";
                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult CreateMultiUserPermission(vUserPermiss_obj ItemData)
        {
            vUserPermission_Return result = new vUserPermission_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;

                if (ItemData.lstUser != null && ItemData.lstUser.Any())
                {

                    List<MenuAction> lstForSaave = new List<MenuAction>();
                    List<UserListPermission> Permiss = new List<UserListPermission>();
                    List<UserUnitGroup> lstDivision = new List<UserUnitGroup>();

                    #region List Menu Permission
                    if (ItemData.lstDataSave != null && ItemData.lstDataSave.Any())
                    {

                        foreach (var lsA in ItemData.lstDataSave)
                        {
                            if (lsA.view_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 1);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }

                            }

                            if (lsA.add_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 2);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }
                            if (lsA.edit_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 3);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }
                            if (lsA.approve_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 4);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }
                            if (lsA.detail_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 5);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        //Permiss.Add(new UserListPermission
                                        //{
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    MenuAction = item,
                                        //});
                                        if (item.Menu.menu_type != "PES")
                                        {
                                            Permiss.Add(new UserListPermission
                                            {
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                MenuAction = item,
                                            });
                                        }
                                        else
                                        {
                                            if (CGlobal.UserIsAdminPES())
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                });
                                            }
                                        }
                                    }

                                }
                            }

                        }
                    }
                    #endregion

                    #region List Division Permission
                    var _GetDivision = _DivisionService.GetDivisionForSave(ItemData.aUnitCode);
                    if (_GetDivision.Any())
                    {

                        lstDivision = _GetDivision.Select(s => new UserUnitGroup
                        {
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            TM_Divisions = s,
                            unit_code = s.division_code,
                        }).ToList();


                    }

                    #endregion

                    foreach (var item in ItemData.lstUser)
                    {

                        var _getEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == item.user_no).FirstOrDefault();
                        if (_getEmp != null)
                        {
                            UserPermission objSave = new UserPermission()
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                User_description = ItemData.description,
                                GroupPermission = _GroupPermissionService.Find(SystemFunction.GetIntNullToZero(ItemData.group_permiss_id)),
                                user_id = _getEmp.UserID,
                                user_no = _getEmp.Employeeno,
                            };

                            if (_UserPermissionService.CanSave(objSave))
                            {
                                var sComplect = _UserPermissionService.CreateNew(ref objSave);
                                if (sComplect > 0)
                                {
                                    List<UserListPermission> PermissSave = new List<UserListPermission>();
                                    List<UserUnitGroup> lstDivisionSave = new List<UserUnitGroup>();

                                    PermissSave = Permiss.Select(s => new UserListPermission
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        MenuAction = s.MenuAction,
                                        UserPermission = objSave,
                                    }).ToList();

                                    lstDivisionSave = lstDivision.Select(s=> new UserUnitGroup
                                    {
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        active_status = "Y",
                                        TM_Divisions = s.TM_Divisions,
                                        unit_code = s.unit_code,
                                        UserPermission = objSave,
                                    }).ToList();
                                   
                                    _UserListPermissionService.UpdateUserListPermission(PermissSave, objSave.Id, CGlobal.UserInfo.UserId, dNow);
                                    _UserUnitGroupService.UpdateUserUnitGroupService(lstDivisionSave, objSave.Id, CGlobal.UserInfo.UserId, dNow);
                                  

                                }
                            }
                        }

                    }
                    result.Status = SystemFunction.process_Success;
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, โปรดระบุพนักงาน";
                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditUserPermission(vUserPermiss_obj ItemData)
        {
            vUserPermission_Return result = new vUserPermission_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _UserPermissionService.Find(nId);
                    if (_getData != null)
                    {
                        List<MenuAction> lstForSaave = new List<MenuAction>();
                        List<UserListPermission> Permiss = new List<UserListPermission>();
                        List<UserUnitGroup> lstDivision = new List<UserUnitGroup>();

                        #region List Menu Permission
                        if (ItemData.lstDataSave != null && ItemData.lstDataSave.Any())
                        {
                            foreach (var lsA in ItemData.lstDataSave)
                            {
                                if (lsA.view_value + "" == "Y")
                                {
                                    var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 1);

                                    if (GetMAC != null && GetMAC.Any())
                                    {
                                        foreach (var item in GetMAC)
                                        {

                                            if (item.Menu.menu_type != "PES")
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    UserPermission = _getData,
                                                });

                                            }
                                            else
                                            {
                                                if (CGlobal.UserIsAdminPES())
                                                {
                                                    Permiss.Add(new UserListPermission
                                                    {
                                                        active_status = "Y",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        MenuAction = item,
                                                        UserPermission = _getData,
                                                    });

                                                }
                                            }
                                        }

                                    }
                                }

                                if (lsA.add_value + "" == "Y")
                                {
                                    var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 2);
                                    if (GetMAC != null && GetMAC.Any())
                                    {
                                        foreach (var item in GetMAC)
                                        {
                                            //Permiss.Add(new UserListPermission
                                            //{
                                            //    active_status = "Y",
                                            //    update_user = CGlobal.UserInfo.UserId,
                                            //    update_date = dNow,
                                            //    create_user = CGlobal.UserInfo.UserId,
                                            //    create_date = dNow,
                                            //    MenuAction = item,
                                            //    UserPermission = _getData,
                                            //});
                                            if (item.Menu.menu_type != "PES")
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    UserPermission = _getData,
                                                });

                                            }
                                            else
                                            {
                                                if (CGlobal.UserIsAdminPES())
                                                {
                                                    Permiss.Add(new UserListPermission
                                                    {
                                                        active_status = "Y",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        MenuAction = item,
                                                        UserPermission = _getData,
                                                    });

                                                }
                                            }
                                        }

                                    }
                                }
                                if (lsA.edit_value + "" == "Y")
                                {
                                    var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 3);
                                    if (GetMAC != null && GetMAC.Any())
                                    {
                                        foreach (var item in GetMAC)
                                        {
                                            //Permiss.Add(new UserListPermission
                                            //{
                                            //    active_status = "Y",
                                            //    update_user = CGlobal.UserInfo.UserId,
                                            //    update_date = dNow,
                                            //    create_user = CGlobal.UserInfo.UserId,
                                            //    create_date = dNow,
                                            //    MenuAction = item,
                                            //    UserPermission = _getData,
                                            //});
                                            if (item.Menu.menu_type != "PES")
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    UserPermission = _getData,
                                                });

                                            }
                                            else
                                            {
                                                if (CGlobal.UserIsAdminPES())
                                                {
                                                    Permiss.Add(new UserListPermission
                                                    {
                                                        active_status = "Y",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        MenuAction = item,
                                                        UserPermission = _getData,
                                                    });

                                                }
                                            }
                                        }

                                    }
                                }
                                if (lsA.approve_value + "" == "Y")
                                {
                                    var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 4);
                                    if (GetMAC != null && GetMAC.Any())
                                    {
                                        foreach (var item in GetMAC)
                                        {
                                            //Permiss.Add(new UserListPermission
                                            //{
                                            //    active_status = "Y",
                                            //    update_user = CGlobal.UserInfo.UserId,
                                            //    update_date = dNow,
                                            //    create_user = CGlobal.UserInfo.UserId,
                                            //    create_date = dNow,
                                            //    MenuAction = item,
                                            //    UserPermission = _getData,
                                            //});
                                            if (item.Menu.menu_type != "PES")
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    UserPermission = _getData,
                                                });

                                            }
                                            else
                                            {
                                                if (CGlobal.UserIsAdminPES())
                                                {
                                                    Permiss.Add(new UserListPermission
                                                    {
                                                        active_status = "Y",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        MenuAction = item,
                                                        UserPermission = _getData,
                                                    });

                                                }
                                            }
                                        }

                                    }
                                }
                                if (lsA.detail_value + "" == "Y")
                                {
                                    var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 5);
                                    if (GetMAC != null && GetMAC.Any())
                                    {
                                        foreach (var item in GetMAC)
                                        {
                                            //Permiss.Add(new UserListPermission
                                            //{
                                            //    active_status = "Y",
                                            //    update_user = CGlobal.UserInfo.UserId,
                                            //    update_date = dNow,
                                            //    create_user = CGlobal.UserInfo.UserId,
                                            //    create_date = dNow,
                                            //    MenuAction = item,
                                            //    UserPermission = _getData,
                                            //});
                                            if (item.Menu.menu_type != "PES")
                                            {
                                                Permiss.Add(new UserListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    UserPermission = _getData,
                                                });

                                            }
                                            else
                                            {
                                                if (CGlobal.UserIsAdminPES())
                                                {
                                                    Permiss.Add(new UserListPermission
                                                    {
                                                        active_status = "Y",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        MenuAction = item,
                                                        UserPermission = _getData,
                                                    });

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        #endregion

                        #region List Division Permission
                        var _GetDivision = _DivisionService.GetDivisionForSave(ItemData.aUnitCode);
                        if (_GetDivision.Any())
                        {

                            lstDivision = _GetDivision.Select(s => new UserUnitGroup
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Divisions = s,
                                unit_code = s.division_code,
                                UserPermission = _getData,
                            }).ToList();


                        }
                        #endregion

                        UserPermission objSave = new UserPermission()
                        {
                            Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + "")),
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            active_status = ItemData.active_status,
                            User_description = ItemData.description,
                            GroupPermission = _GroupPermissionService.Find(SystemFunction.GetIntNullToZero(ItemData.group_permiss_id)),
                            user_id = ItemData.user_id,
                            user_no = ItemData.user_no,

                        };
                        if (_UserPermissionService.CanSave(objSave))
                        {
                            var sComplect = _UserPermissionService.Update(objSave);
                            if (sComplect > 0)
                            {
                                if (_UserListPermissionService.UpdateUserListPermission(Permiss, objSave.Id, CGlobal.UserInfo.UserId, dNow) > 0 || sComplect > 0)
                                {

                                }
                                if (_UserUnitGroupService.UpdateUserUnitGroupService(lstDivision, objSave.Id, CGlobal.UserInfo.UserId, dNow) > 0 || sComplect > 0)
                                {

                                }

                                result.Status = SystemFunction.process_Success;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Duplicate;
                            result.Msg = "Staff already exist, please try again.";
                        }


                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Group Permission Not Found.";
                    }
                }

            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult LoadUserPermissionList(CSearchUser_Permission SearchItem)
        {
            vUserPermission_Return result = new vUserPermission_Return();
            List<vUserPermiss_obj> lstData_resutl = new List<vUserPermiss_obj>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);

            string[] UserNo = null;
            if (!string.IsNullOrEmpty(SearchItem.name))
            {
                UserNo = sQuery.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.UserID + "").Trim()).ToLower()).ToArray();
            }
            int nGID = SystemFunction.GetIntNullToZero(SearchItem.group_permiss_id + "");
            var lstData = _UserPermissionService.GetUserPermissionList(
            UserNo,
            nGID,
            SearchItem.active_status);
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {
                UserNo = lstData.Select(s => s.user_id).ToArray();
                //string aCode = "";
                //DataTable lstEmp = new DataTable();
                //foreach (var code in lstData)
                //{
                //    aCode += code.user_no + ",";
                //}
                //lstEmp = wsHRis.getEmployeeInfoByEmpNoMultiple(aCode);
                List<vClassEmployeeFromWS> lstEmplist = dbHr.AllInfo_WS.Where(w => UserNo.Contains(w.UserID)).Select(s => new vClassEmployeeFromWS
                {
                    unitgroup_code = s.UnitGroupID + "",
                    rank = s.Rank,
                    unitgroup = s.UnitGroupName,
                    emp_no = s.EmpNo,
                    emp_full_name = s.EmpFullName,
                }).ToList();
                result.lstData = (from lstAD in lstData
                                  from lstE in lstEmplist.Where(w => w.emp_no == lstAD.user_no).DefaultIfEmpty(new vClassEmployeeFromWS())
                                  select new vUserPermiss_obj
                                  {
                                      user_no = lstAD.user_no,
                                      user_name = lstE.emp_full_name,
                                      unit_group = lstE.unitgroup,
                                      rank = lstE.rank,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      permis_group = lstAD.GroupPermission.group_name_en.StringRemark(40),
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      unit_permis = "",
                                      lstGroup_Per = lstAD.UserUnitGroup != null ? lstAD.UserUnitGroup.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Divisions.division_name_en).Take(2).Select(s2 => new vListGroup_lstData
                                      {
                                          group_name = s2.TM_Divisions.division_name_en
                                      }).ToList() : new List<vListGroup_lstData>(),
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      user_group_code = lstE.unitgroup_code,
                                  }).ToList();
                if (!string.IsNullOrEmpty(SearchItem.group_code))
                {
                    result.lstData = result.lstData.Where(w => w.user_group_code == SearchItem.group_code).ToList();
                }
                //if (lstEmp != null)
                //{
                //    result.lstData.ForEach(ed =>
                //    {
                //        var _empData = lstEmp.AsEnumerable().Where(w => w.Field<string>("EmpNo") == ed.user_no).FirstOrDefault();
                //        if (_empData != null)
                //        {
                //            ed.user_name = _empData.Field<string>("EmpNo") + " : " + _empData.Field<string>("EmpFullName");
                //            ed.user_group = _empData.Field<string>("UnitGroup");
                //        }
                //    });
                //}

            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        #endregion
    }
}