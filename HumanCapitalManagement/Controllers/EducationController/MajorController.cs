using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.EducationModels;
using HumanCapitalManagement.Service.EduService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.EduVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.EducationController
{
    public class MajorController : BaseController
    {
        private TM_MajorService _TM_MajorService;
        public MajorController(TM_MajorService TM_MajorService)
        {
            _TM_MajorService = TM_MajorService;
        }
        // GET: Major
        public ActionResult MajorList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMajor result = new vMajor();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchMajor SearchItem = (CSearchMajor)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchMajor)));
                var lstData = _TM_MajorService.GetMajor(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vMajor_obj
                                      {
                                          name_en = lstAD.major_name_en.StringRemark(500),
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.major_description.StringRemark(),
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
        public ActionResult MajorCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMajor_obj_save result = new vMajor_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            return View(result);
        }
        public ActionResult MajorEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vMajor_obj_save result = new vMajor_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_MajorService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.name_en = _getData.major_name_en + "";
                        result.description = _getData.major_description + "";
                        result.active_status = _getData.active_status;

                    }
                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadMajorList(CSearchPool SearchItem)
        {
            vMajor_Return result = new vMajor_Return();
            List<vMajor_obj> lstData_resutl = new List<vMajor_obj>();
            var lstData = _TM_MajorService.GetMajor(
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
                                  select new vMajor_obj
                                  {
                                      name_en = lstAD.major_name_en.StringRemark(500),
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.major_description.StringRemark(),
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
        public ActionResult CreateMajor(vMajor_obj_save ItemData)
        {
            vMajor_Return result = new vMajor_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    TM_Major objSave = new TM_Major()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        major_name_en = (ItemData.name_en + "").Trim(),
                        major_description = ItemData.description,

                    };
                    if (_TM_MajorService.CanSave(objSave))
                    {
                        var sComplect = _TM_MajorService.CreateNew(objSave);
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
        public ActionResult EditMajor(vMajor_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vMajor_Return result = new vMajor_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_MajorService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = ItemData.active_status;
                            _getData.major_name_en = ItemData.name_en;
                            _getData.major_description = ItemData.description;
                            if (_TM_MajorService.CanSave(_getData))
                            {
                                var sComplect = _TM_MajorService.Update(_getData);
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