using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class SubGroupController : BaseController
    {
        private TM_SubGroupService _TM_SubGroupService;
        private DivisionService _DivisionService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public SubGroupController(TM_SubGroupService TM_SubGroupService, DivisionService DivisionService)
        {
            _TM_SubGroupService = TM_SubGroupService;
            _DivisionService = DivisionService;
        }
        // GET: SubGroup
        public ActionResult SubGroupList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vSubGroup result = new vSubGroup();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchSubGroup SearchItem = (CSearchSubGroup)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchSubGroup)));
                var lstData = _TM_SubGroupService.GetSubGroup(
               SearchItem.name,
               SearchItem.group_id,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                result.group_id = SearchItem.group_id;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vSubGroup_obj
                                      {
                                          name_en = lstAD.sub_group_name_en.StringRemark(70),
                                          group_name = (lstAD.TM_Divisions.division_name_en + "").StringRemark(70),
                                          short_name_en = lstAD.sub_group_short_name_en+"",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.sub_group_description.StringRemark(),
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
        public ActionResult SubGroupCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vSubGroup_obj_save result = new vSubGroup_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            return View(result);
        }
        public ActionResult SubGroupEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vSubGroup_obj_save result = new vSubGroup_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_SubGroupService.Find(nId);
                    if (_getData != null)
                    {
                        var _GetHead = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.head_user_no).FirstOrDefault();

                        result.IdEncrypt = id;
                        if (_GetHead != null)
                        {
                            result.user_id = _getData.head_user_id + "";
                            result.user_no = _getData.head_user_no + "";
                            result.user_name = _GetHead.EmpFullName + "";
                            result.unit_name = _GetHead.UnitGroup;
                            result.user_position = _GetHead.Rank;
                        }
                        result.active_status = _getData.active_status;
                        result.name_en = _getData.sub_group_name_en;
                        result.short_name_en = _getData.sub_group_short_name_en + "";
                        result.description = _getData.sub_group_description + "";
                        result.group_code = _getData.TM_Divisions.division_code + "";
                    }
                }
            }
            return View(result);

            #endregion

        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadSubGroupList(CSearchSubGroup SearchItem)
        {
            vSubGroup_Return result = new vSubGroup_Return();
            List<vSubGroup_obj> lstData_resutl = new List<vSubGroup_obj>();
            var lstData = _TM_SubGroupService.GetSubGroup(
            SearchItem.name,
               SearchItem.group_id,
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
                                  select new vSubGroup_obj
                                  {
                                      name_en = lstAD.sub_group_name_en.StringRemark(70),
                                      group_name = (lstAD.TM_Divisions.division_name_en + "").StringRemark(70),
                                      short_name_en = lstAD.sub_group_short_name_en + "",
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.sub_group_description.StringRemark(),
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
        [HttpPost]
        public ActionResult CreateSubGroup(vSubGroup_obj_save ItemData)
        {
            vSubGroup_Return result = new vSubGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en) && !string.IsNullOrEmpty(ItemData.short_name_en))
                {
                    var _GetGroup = _DivisionService.FindByCode(ItemData.group_code);
                    if (_GetGroup != null)
                    {
                        TM_SubGroup objSave = new TM_SubGroup()
                        {
                            Id = 0,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            head_user_id = ItemData.user_id,
                            head_user_no = ItemData.user_no,
                            TM_Divisions = _GetGroup,
                            sub_group_name_en = (ItemData.name_en + "").Trim(),
                            sub_group_short_name_en = (ItemData.short_name_en + "").Trim(),
                            sub_group_description = ItemData.description,
                        };
                        if (_TM_SubGroupService.CanSave(objSave))
                        {
                            var sComplect = _TM_SubGroupService.CreateNew(objSave);
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
                            result.Msg = "Duplicate Type name.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please enter Group.";
                    }


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please enter name and short name.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditSubGroup(vSubGroup_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vSubGroup_Return result = new vSubGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_SubGroupService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en) && !string.IsNullOrEmpty(ItemData.short_name_en))
                        {
                            var _GetGroup = _DivisionService.FindByCode(ItemData.group_code);
                            if (_GetGroup != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.active_status = ItemData.active_status;
                                _getData.TM_Divisions = _GetGroup;
                                _getData.sub_group_name_en = ItemData.name_en;
                                _getData.sub_group_short_name_en = ItemData.short_name_en;
                                _getData.sub_group_description = ItemData.description;
                                _getData.head_user_id = ItemData.user_id;
                                _getData.head_user_no = ItemData.user_no;


                                if (_TM_SubGroupService.CanSave(_getData))
                                {
                                    var sComplect = _TM_SubGroupService.Update(_getData);
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
                                    result.Msg = "Duplicate Type name.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please enter Group.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter name and short name.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
                }

            }

            return Json(new
            {
                result
            });
        }
        #endregion
    }
}