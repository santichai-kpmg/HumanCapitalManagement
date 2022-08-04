using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;

namespace HumanCapitalManagement.Controllers.Common
{
    public class GroupPermissionController : BaseController
    {
        private MPService _MPRepository;
        private MenuActionService _MenuActionService;
        private GroupPermissionService _GroupPermissionService;
        private GroupListPermissionService _GroupListPermissionService;
        public GroupPermissionController(MPService MPService, MenuActionService MenuActionService, GroupPermissionService GroupPermissionService, GroupListPermissionService GroupListPermissionService)
        {
            _MPRepository = MPService;
            _MenuActionService = MenuActionService;
            _GroupPermissionService = GroupPermissionService;
            _GroupListPermissionService = GroupListPermissionService;
        }
        // GET: GroupPermission

        public ActionResult GroupPermissionList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vGroupPermission result = new vGroupPermission();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchGroup_Permission SearchItem = (CSearchGroup_Permission)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchGroup_Permission)));
                var lstData = _GroupPermissionService.GetGroupPermissionList(
           SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vGroupPermiss_obj
                                      {
                                          name_en = lstAD.group_name_en.StringRemark(20),
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.group_description.StringRemark(),
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult GroupPermissionCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vGroupPermiss_obj result = new vGroupPermiss_obj();

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
            result.lstData = lstMenuWithOrder.Select(s => new vGroupListPermission
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
            result.lstDataSave = new List<vGroupListPermission>();
            return View(result);
        }
        public ActionResult GroupPermissionEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code
            var bReturn = false;
            vGroupPermiss_obj result = new vGroupPermiss_obj();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _GroupPermissionService.Find(nId);
                    if (_getData != null)
                    {
                        result = new vGroupPermiss_obj
                        {
                            active_status = _getData.active_status,
                            IdEncrypt = HCMFunc.Encrypt(_getData.Id + ""),
                            name_en = _getData.group_name_en + "",
                            name_th = _getData.group_name_th + "",
                            description = _getData.group_description,

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
                      
                        //result.lstData = (from lstM in lstMenuWithOrder
                        //                  from lstView in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 1 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission())
                        //                  from lstAdd in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 2 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission())
                        //                  from lstEdit in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 3 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission())
                        //                  from lstApprove in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 4 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission())
                        //                  from lstDetail in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 5 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission())
                        //                  select new vGroupListPermission
                        //                  {
                        //                      menu_id = lstM.MENU_ID,
                        //                      menu_name = (lstM.MENU_SUB + "" == "Y" ? @"<i class=""fa fa-fw fa-plus""></i>" : @"<i class=""fa fa-fw fa-minus""></i>") + lstM.MENU_NAME_EN,
                        //                      n_seq = nSeq++,
                        //                      menu_level = lstM.MENU_LEVEL,
                        //                      view_action = lstM.MenuAction.Any(w => w.Action_Type == 1 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                        //                      add_action = lstM.MenuAction.Any(w => w.Action_Type == 2 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                        //                      edit_action = lstM.MenuAction.Any(w => w.Action_Type == 3 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                        //                      approve_action = lstM.MenuAction.Any(w => w.Action_Type == 4 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                        //                      detail_action = lstM.MenuAction.Any(w => w.Action_Type == 5 && w.Menu.MENU_Permission == "Y") ? "Y" : "N",
                        //                      view_value = lstView.active_status + "" == "Y" ? "Y" : "N",
                        //                      add_value = lstAdd.active_status + "" == "Y" ? "Y" : "N",
                        //                      edit_value = lstEdit.active_status + "" == "Y" ? "Y" : "N",
                        //                      approve_value = lstApprove.active_status + "" == "Y" ? "Y" : "N",
                        //                      detail_value = lstDetail.active_status + "" == "Y" ? "Y" : "N",
                        //                  }).ToList();
                        result.lstData = (from lstM in lstMenuWithOrder
                                          from lstView in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 1 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission()).Take(1)
                                          from lstAdd in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 2 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission()).Take(1)
                                          from lstEdit in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 3 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission()).Take(1)
                                          from lstApprove in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 4 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission()).Take(1)
                                          from lstDetail in _getData.GroupListPermission.Where(w => w.MenuAction.Menu.Id == lstM.Id && w.MenuAction.Action_Type == 5 && w.active_status == "Y").DefaultIfEmpty(new GroupListPermission()).Take(1)
                                          select new vGroupListPermission
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

                    result.lstDataSave = new List<vGroupListPermission>();


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

        #region Ajax Function
        [HttpPost]
        public ActionResult CreateGroupPermission(vGroupPermiss_obj ItemData)
        {
            vGroupPermission_Return result = new vGroupPermission_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;

                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    List<MenuAction> lstForSaave = new List<MenuAction>();
                    List<GroupListPermission> Permiss = new List<GroupListPermission>();
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
                                        Permiss.Add(new GroupListPermission
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

                            if (lsA.add_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 2);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        Permiss.Add(new GroupListPermission
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
                            if (lsA.edit_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 3);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        Permiss.Add(new GroupListPermission
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
                            if (lsA.approve_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 4);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        Permiss.Add(new GroupListPermission
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
                            if (lsA.detail_value + "" == "Y")
                            {
                                var GetMAC = _MenuActionService.GetMenuActionByMenuAndType(SystemFunction.GetIntNull(lsA.menu_id), 5);
                                if (GetMAC != null && GetMAC.Any())
                                {
                                    foreach (var item in GetMAC)
                                    {
                                        Permiss.Add(new GroupListPermission
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

                    //  int[] a = new int[] { 1, 2 };
                    //lstForSaave = _MenuActionService.SelectMenuActionByaID(a, a).ToList();
                    GroupPermission objSave = new GroupPermission()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        group_name_th = (ItemData.name_th + "").Trim(),
                        group_name_en = (ItemData.name_en + "").Trim(),
                        group_description = ItemData.description,
                        GroupListPermission = Permiss.ToList(),
                    };

                    if (_GroupPermissionService.CanSave(objSave))
                    {
                        var sComplect = _GroupPermissionService.CreateNew(objSave);
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
                        result.Msg = "Duplicate group name.";
                    }



                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, โปรดระบุประเภทการส่งจดหมาย";
                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditGroupPermission(vGroupPermiss_obj ItemData)
        {
            vGroupPermission_Return result = new vGroupPermission_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _GroupPermissionService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {
                            List<MenuAction> lstForSaave = new List<MenuAction>();
                            List<GroupListPermission> Permiss = new List<GroupListPermission>();
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
                                                Permiss.Add(new GroupListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    GroupPermission = _getData,
                                                });
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
                                                Permiss.Add(new GroupListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    GroupPermission = _getData,
                                                });
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
                                                Permiss.Add(new GroupListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    GroupPermission = _getData,
                                                });
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
                                                Permiss.Add(new GroupListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    GroupPermission = _getData,
                                                });
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
                                                Permiss.Add(new GroupListPermission
                                                {
                                                    active_status = "Y",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    MenuAction = item,
                                                    GroupPermission = _getData,
                                                });
                                            }

                                        }

                                    }

                                }
                            }

                            //  int[] a = new int[] { 1, 2 };
                            //lstForSaave = _MenuActionService.SelectMenuActionByaID(a, a).ToList();
                            GroupPermission objSave = new GroupPermission()
                            {
                                Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + "")),
                                active_status = ItemData.active_status,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                group_name_th = (ItemData.name_th + "").Trim(),
                                group_name_en = (ItemData.name_en + "").Trim(),
                                group_description = ItemData.description,
                                //GroupListPermission = Permiss.ToList(),

                            };

                            if (_GroupPermissionService.CanSave(objSave))
                            {
                                var sComplect = _GroupPermissionService.Update(objSave);
                                if (sComplect > 0)
                                {
                                    if (_GroupListPermissionService.UpdateGroupListPermission(Permiss, objSave.Id, CGlobal.UserInfo.UserId, dNow) > 0 || sComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                    }
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
                                result.Msg = "Duplicate group name.";
                            }



                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, โปรดระบุประเภทการส่งจดหมาย";
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
        public ActionResult LoadGroupPermissionList(CSearchGroup_Permission SearchItem)
        {
            vGroupPermission_Return result = new vGroupPermission_Return();
            List<vGroupPermiss_obj> lstData_resutl = new List<vGroupPermiss_obj>();
            var lstData = _GroupPermissionService.GetGroupPermissionList(
            SearchItem.name,
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

                lstData_resutl = (from lstAD in lstData
                                  select new vGroupPermiss_obj
                                  {
                                      name_en = lstAD.group_name_en.StringRemark(20),
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.group_description.StringRemark(),
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        #endregion
    }
}
