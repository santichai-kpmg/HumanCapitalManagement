using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PreInternAssessment;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.PreInternFormController
{
    public class RatingPIFormController : BaseController
    {
        private TM_PIntern_RatingFormService _TM_PIntern_RatingFormService;
        public RatingPIFormController(TM_PIntern_RatingFormService TM_PIntern_RatingFormService)
        {
            _TM_PIntern_RatingFormService = TM_PIntern_RatingFormService;
        }
        // GET: rating pre intern
        public ActionResult RatingPIFormList(string qryStr)
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
                var lstData = _TM_PIntern_RatingFormService.GetPIntern_Form(
               SearchItem.name,
          "");
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vTIFForm_obj
                                      {
                                          name_en = lstAD.action_date.HasValue ? lstAD.action_date.Value.DateTimeWithTimebyCulture() : "",
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                }
            }

            #endregion
            return View("RatingPIFormList", result);
        }
        public ActionResult RatingPIFormCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTIFForm_obj_save result = new vTIFForm_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.action_date = DateTime.Now.DateTimebyCulture();
            return View("RatingPIFormCreate", result);
        }
        public ActionResult RatingPIFormView(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vTIFForm_obj_save result = new vTIFForm_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PIntern_RatingFormService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.active_status = _getData.active_status;
                        result.action_date = _getData.action_date.HasValue ? _getData.action_date.Value.DateTimebyCulture() : "";
                        result.description = _getData.description;
                        result.code = _getData.Id + "";
                        if (_getData.TM_PIntern_Rating != null && _getData.TM_PIntern_Rating.Any())
                        {
                            result.lstRating = _getData.TM_PIntern_Rating.Select(s => new vRatingForm_PreIntern
                            {
                                nID = s.seq + "",
                                rating_name = s.rating_name_en + " ",
                                rating_des = s.rating_description + " ",
                                
                            }).ToList();
                        }
                    }
                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadRatingPIFormList(CSearchPool SearchItem)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            List<vTIFForm_obj> lstData_resutl = new List<vTIFForm_obj>();
            var lstData = _TM_PIntern_RatingFormService.GetPIntern_Form(
            SearchItem.name,
            "");
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
                                  select new vTIFForm_obj
                                  {
                                      name_en = lstAD.action_date.HasValue ? lstAD.action_date.Value.DateTimeWithTimebyCulture() : "",
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreateRatingPIForm(vTIFForm_obj_save ItemData)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstRating != null && ItemData.lstRating.Any())
                {

                    List<TM_PIntern_Rating> lstObjRating = new List<TM_PIntern_Rating>();
                    lstObjRating = ItemData.lstRating.Select(s => new TM_PIntern_Rating
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        rating_name_th = (s.rating_name + "").Trim(),
                        rating_name_en = (s.rating_name + "").Trim(),
                        point = SystemFunction.GetIntNullToZero(s.point + ""),
                        rating_description = (s.rating_des + "").Trim(),
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),

                    }).ToList();

                    TM_PIntern_RatingForm objSave = new TM_PIntern_RatingForm()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        TM_PIntern_Rating = lstObjRating.ToList(),
                        description = (ItemData.description + "").Trim(),
                        action_date = dNow,
                    };

                    var sComplect = _TM_PIntern_RatingFormService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_PIntern_RatingFormService.UpdateInactive(objSave);
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
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please enter rating name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditRatingPIForm(vTIFForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PIntern_RatingFormService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = "Y";
                        var sComplect = _TM_PIntern_RatingFormService.Update(_getData);
                        if (sComplect > 0)
                        {
                            _TM_PIntern_RatingFormService.UpdateInactive(_getData);
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
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, TIF Form Not Found.";
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