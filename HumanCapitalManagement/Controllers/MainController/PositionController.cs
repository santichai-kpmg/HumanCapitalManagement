using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class PositionController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_PositionService _TM_PositionService;
        private PoolService _PoolService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public PositionController(TM_PositionService TM_PositionService, PoolService PoolService, DivisionService DivisionService)
        {
            _TM_PositionService = TM_PositionService;
            _PoolService = PoolService;
            _DivisionService = DivisionService;
        }
        // GET: Position
        public ActionResult PositionList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPosition result = new vPosition();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPosition SearchItem = (CSearchPosition)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPosition)));

                var lstData = _TM_PositionService.GetPosition(
              (SearchItem.name + "").Trim(),
               SearchItem.active_status, (SearchItem.group_id + "").Trim());
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vPosition_obj
                                      {
                                          name_en = lstAD.position_name_en.StringRemark(500),
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          group_name = lstAD.TM_Divisions.division_short_name_en + "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
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

        public ActionResult PositionCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPosition_obj_save result = new vPosition_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            return View(result);
        }

        public ActionResult PositionEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vPosition_obj_save result = new vPosition_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PositionService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.group_id = _getData.TM_Divisions.division_code + "";
                        result.active_status = _getData.active_status;
                        result.name_en = _getData.position_name_en;
                        result.short_name_en = _getData.position_short_name_en + "";
                        result.job_descriptions = _getData.job_descriptions + "";
                        result.qualification_experience = _getData.qualification_experience + "";

                    }
                }
            }
            return View(result);

            #endregion

        }


        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPositionList(CSearchPosition SearchItem)
        {
            vPosition_Return result = new vPosition_Return();
            List<vPosition_obj> lstData_resutl = new List<vPosition_obj>();

            var lstData = _TM_PositionService.GetPosition(
           (SearchItem.name + "").Trim(),
            SearchItem.active_status, (SearchItem.group_id + "").Trim());
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
                                  select new vPosition_obj
                                  {
                                      name_en = lstAD.position_name_en.StringRemark(500),
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      group_name = lstAD.TM_Divisions.division_short_name_en + "",
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
        public ActionResult CreatePosition(vPosition_obj_save ItemData)
        {
            vPosition_Return result = new vPosition_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    //int nPool = SystemFunction.GetIntNullToZero(ItemData.pool_id);
                    //var _getPool = _PoolService.Find(nPool);
                    var _getGrpup = _DivisionService.FindByCode(ItemData.group_id);
                    if (_getGrpup != null)
                    {
                        TM_Position objSave = new TM_Position()
                        {
                            Id = 0,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            position_name_en = (ItemData.name_en + "").Trim(),
                            position_short_name_en = (ItemData.short_name_en + "").Trim(),
                            TM_Divisions = _getGrpup,
                            job_descriptions = ItemData.job_descriptions + "",
                            qualification_experience = ItemData.qualification_experience + "",

                        };
                        if (_TM_PositionService.CanSave(objSave))
                        {
                            var sComplect = _TM_PositionService.CreateNew(objSave);
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
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditPosition(vPosition_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPosition_Return result = new vPosition_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PositionService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {
                            //int nPool = SystemFunction.GetIntNullToZero(ItemData.pool_id);
                            //var _getPool = _PoolService.Find(nPool);
                            var _getGrpup = _DivisionService.FindByCode(ItemData.group_id);
                            if (_getGrpup != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.active_status = ItemData.active_status;
                                _getData.position_name_en = (ItemData.name_en + "").Trim();
                                _getData.position_short_name_en = (ItemData.short_name_en + "").Trim();
                                //_getData.TM_Divisions = _getGrpup;
                                _getData.job_descriptions = ItemData.job_descriptions + "";
                                _getData.qualification_experience = ItemData.qualification_experience + "";

                                if (_TM_PositionService.CanSave(_getData))
                                {
                                    var sComplect = _TM_PositionService.Update(_getData);
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
                            result.Msg = "Error, please enter name";
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