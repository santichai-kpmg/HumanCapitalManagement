using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanCapitalManagement.Models.PreInternAssessment;

namespace HumanCapitalManagement.Controllers.PreInternFormController
{
    public class ActivityFormController : BaseController
    {
        private TM_PInternAssessment_ActivitiesService _TM_PInternAssessment_ActivitiesService;
        public ActivityFormController(TM_PInternAssessment_ActivitiesService TM_PInternAssessment_ActivitiesService)
        {
            _TM_PInternAssessment_ActivitiesService = TM_PInternAssessment_ActivitiesService;
        }
        // GET: PreInternForm
        public ActionResult ActivityFormList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTIFForm result = new vTIFForm();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchTIFForm SearchItem = (CSearchTIFForm)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchTIFForm)));
                var lstData = _TM_PInternAssessment_ActivitiesService.GetDataAllActive();
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstDataActivity = (from lstAD in lstData
                                              select new vActivity_obj
                                              {
                                                  Id = lstAD.Id + "",
                                                  Activities_name_en = lstAD.Activities_name_en + "",
                                                  activestatus = lstAD.active_status + "",
                                                  create_user = lstAD.create_user,
                                                  create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                                  update_user = lstAD.update_user,
                                                  update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                  View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              }).ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult ActivityFormCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vActivity_obj_save result = new vActivity_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.action_date = DateTime.Now.DateTimebyCulture();
            return View(result);
        }
        public ActionResult ActivityFormEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vActivity_obj_save result = new vActivity_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PInternAssessment_ActivitiesService.FindById(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.activities_name_en = _getData.Activities_name_en + "";
                        result.activities_description = _getData.Activities_descriptions + "";
                        result.active_status = _getData.active_status;

                    }
                }
            }
            return View(result);

            #endregion

        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadActivityFormList(CSearchPool SearchItem)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            List<vActivity_obj> lstData_resutl = new List<vActivity_obj>();
            var lstData = _TM_PInternAssessment_ActivitiesService.GetDataAllActive();
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
                                  select new vActivity_obj
                                  {
                                      Id = lstAD.Id + "",
                                      Activities_name_en = lstAD.Activities_name_en + "",
                                      activestatus = lstAD.active_status + "",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstDataActivity = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreateActivityForm(vActivity_obj_save ItemData)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.activities_name_en))
                {
                    TM_PInternAssessment_Activities objSave = new TM_PInternAssessment_Activities()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        Activities_name_en = (ItemData.activities_name_en + "").Trim(),
                        Activities_name_th = (ItemData.activities_name_en + "").Trim(),
                        Activities_descriptions = (ItemData.activities_description+"").Trim(),
                        

                    };
                    if (_TM_PInternAssessment_ActivitiesService.CanSave(objSave))
                    {
                        var sComplect = _TM_PInternAssessment_ActivitiesService.CreateNew(objSave);
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
                
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditActivityForm(vActivity_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PInternAssessment_ActivitiesService.FindById(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.activities_name_en))
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = ItemData.active_status;
                            _getData.Activities_name_en = ItemData.activities_name_en;
                            _getData.Activities_name_th = ItemData.activities_name_en;
                            _getData.Activities_descriptions = ItemData.activities_description;
                            if (_TM_PInternAssessment_ActivitiesService.CanSave(_getData))
                            {
                                var sComplect = _TM_PInternAssessment_ActivitiesService.Update(_getData);
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