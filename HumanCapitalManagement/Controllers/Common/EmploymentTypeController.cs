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
    public class EmploymentTypeController : BaseController
    {
        private EmploymentTypeService _EmploymentTypeService;
        private RequestTypeService _RequestTypeService;
        private TM_Employment_RequestService _TM_Employment_RequestService;
        public EmploymentTypeController(EmploymentTypeService EmploymentTypeService, RequestTypeService RequestTypeService, TM_Employment_RequestService TM_Employment_RequestService)
        {
            _RequestTypeService = RequestTypeService;
            _EmploymentTypeService = EmploymentTypeService;
            _TM_Employment_RequestService = TM_Employment_RequestService;
        }
        // GET: EmploymentType
        public ActionResult EmploymentTypeList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEmploymentType result = new vEmploymentType();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchEmploymentType SearchItem = (CSearchEmploymentType)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchEmploymentType)));
                var lstData = _EmploymentTypeService.GetEmploymentType(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vEmploymentType_obj
                                      {
                                          name_en = lstAD.employee_type_name_en.StringRemark(500),
                                      
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.employee_type_description.StringRemark(),
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

        public ActionResult EmploymentTypeCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEmploymentType_obj_save result = new vEmploymentType_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.target_period = "N";
            result.personnel_type = "S";
            return View(result);
        }

        public ActionResult EmploymentTypeEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vEmploymentType_obj_save result = new vEmploymentType_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _EmploymentTypeService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.name_en = _getData.employee_type_name_en + "";
                        result.description = _getData.employee_type_description + "";
                        result.active_status = _getData.active_status;
                        result.aReqeust = _getData.TM_Employment_Request != null ? _getData.TM_Employment_Request.Where(w => w.active_status == "Y").Select(s => s.TM_Request_Type.Id + "").ToArray() : new string[] { };
                        result.target_period = _getData.target_period_validate + "";
                        result.personnel_type = _getData.personnel_type + "";
                    }
                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function 
        [HttpPost]
        public ActionResult LoadEmploymentTypeList(CSearchPool SearchItem)
        {
            vEmploymentType_Return result = new vEmploymentType_Return();
            List<vEmploymentType_obj> lstData_resutl = new List<vEmploymentType_obj>();
            var lstData = _EmploymentTypeService.GetEmploymentType(
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
                                  select new vEmploymentType_obj
                                  {
                                      name_en = lstAD.employee_type_name_en.StringRemark(500),
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.employee_type_description.StringRemark(),
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
        public ActionResult CreateEmploymentType(vEmploymentType_obj_save ItemData)
        {
            vUserPermission_Return result = new vUserPermission_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;

                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    List<TM_Employment_Request> lstEmployment_Request = new List<TM_Employment_Request>();

                    #region List RequestType Permission
                    if (ItemData.aReqeust != null && ItemData.aReqeust.Length > 0)
                    {
                        int[] aID = ItemData.aReqeust.Select(s => SystemFunction.GetIntNullToZero(s)).ToArray();
                        if (aID != null && aID.Length > 0)
                        {
                            var _GetRequestType = _RequestTypeService.GetRequestTypeForSave(aID);
                            if (_GetRequestType.Any())
                            {

                                lstEmployment_Request = _GetRequestType.Select(s => new TM_Employment_Request
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    TM_Request_Type = s,
                                }).ToList();
                            }
                        }

                    }

                    #endregion

                    TM_Employment_Type objSave = new TM_Employment_Type()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        employee_type_name_en = (ItemData.name_en + "").Trim(),
                        employee_type_description = ItemData.description,
                        TM_Employment_Request = lstEmployment_Request.Any() ? lstEmployment_Request.ToList() : null,
                        target_period_validate = ItemData.target_period + "",
                        personnel_type = ItemData.personnel_type + "",
                    };
                    if (_EmploymentTypeService.CanSave(objSave))
                    {
                        var sComplect = _EmploymentTypeService.CreateNew(objSave);
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
        public ActionResult EditEmploymentType(vEmploymentType_obj_save ItemData)
        {
            vUserPermission_Return result = new vUserPermission_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _EmploymentTypeService.Find(nId);
                    if (_getData != null)
                    {
                        List<TM_Employment_Request> lstEmployment_Request = new List<TM_Employment_Request>();

                        #region List RequestType Permission
                        if (ItemData.aReqeust != null && ItemData.aReqeust.Length > 0)
                        {
                            int[] aID = ItemData.aReqeust.Select(s => SystemFunction.GetIntNullToZero(s)).ToArray();
                            var _GetRequestType = _RequestTypeService.GetRequestTypeForSave(aID);
                            if (_GetRequestType.Any())
                            {

                                lstEmployment_Request = _GetRequestType.Select(s => new TM_Employment_Request
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    TM_Request_Type = s,
                                    TM_Employment_Type = _getData,
                                }).ToList();
                            }
                        }

                        #endregion


                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = ItemData.active_status;
                        _getData.employee_type_name_en = ItemData.name_en;
                        _getData.employee_type_description = ItemData.description;
                        _getData.target_period_validate = ItemData.target_period + "";
                        _getData.personnel_type = ItemData.personnel_type + "";
                        // _getData.TM_Employment_Request = lstEmployment_Request.ToList();


                        if (_EmploymentTypeService.CanSave(_getData))
                        {
                            var sComplect = _EmploymentTypeService.Update(_getData);
                            if (sComplect > 0)
                            {
                                if (_TM_Employment_RequestService.UpdateEmployment_Request(lstEmployment_Request, _getData.Id, CGlobal.UserInfo.UserId, dNow) > 0 || sComplect > 0)
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
        #endregion
    }
}