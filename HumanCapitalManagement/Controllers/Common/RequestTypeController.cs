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
    public class RequestTypeController : BaseController
    {
        private RequestTypeService _RequestTypeService;
        public RequestTypeController(RequestTypeService RequestTypeService)
        {
            _RequestTypeService = RequestTypeService;
        }
        // GET: RequestType
        public ActionResult RequestTypeList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRequestType result = new vRequestType();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchRequestType SearchItem = (CSearchRequestType)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchRequestType)));
                var lstData = _RequestTypeService.GetRequestType(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vRequestType_obj
                                      {
                                          name_en = lstAD.request_type_name_en.StringRemark(500),
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.request_type_description.StringRemark(),
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

        public ActionResult RequestTypeCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRequestType_obj_save result = new vRequestType_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.replaced_user = "N";
            return View(result);
        }

        public ActionResult RequestTypeEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vRequestType_obj_save result = new vRequestType_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _RequestTypeService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.name_en = _getData.request_type_name_en + "";
                        result.description = _getData.request_type_description + "";
                        result.active_status = _getData.active_status;
                        result.replaced_user = _getData.replaced_user;

                    }
                }
            }
            return View(result);

            #endregion

        }


        #region Ajax Function
        [HttpPost]
        public ActionResult LoadRequestTypeList(CSearchPool SearchItem)
        {
            vRequestType_Return result = new vRequestType_Return();
            List<vRequestType_obj> lstData_resutl = new List<vRequestType_obj>();
            var lstData = _RequestTypeService.GetRequestType(
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
                                  select new vRequestType_obj
                                  {
                                      name_en = lstAD.request_type_name_en.StringRemark(500),
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.request_type_description.StringRemark(),
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
        public ActionResult CreateRequestType(vRequestType_obj_save ItemData)
        {
            vRequestType_Return result = new vRequestType_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    TM_Request_Type objSave = new TM_Request_Type()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        request_type_name_en = (ItemData.name_en + "").Trim(),
                        request_type_description = ItemData.description,
                        replaced_user = ItemData.replaced_user + "",

                    };
                    if (_RequestTypeService.CanSave(objSave))
                    {
                        var sComplect = _RequestTypeService.CreateNew(objSave);
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
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditRequestType(vRequestType_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vRequestType_Return result = new vRequestType_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _RequestTypeService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = ItemData.active_status;
                            _getData.request_type_name_en = ItemData.name_en;
                            _getData.request_type_description = ItemData.description;
                            _getData.replaced_user = ItemData.replaced_user + "";
                            if (_RequestTypeService.CanSave(_getData))
                            {
                                var sComplect = _RequestTypeService.Update(_getData);
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