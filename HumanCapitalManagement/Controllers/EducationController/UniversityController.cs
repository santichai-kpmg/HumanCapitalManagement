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
    public class UniversityController : BaseController
    {
        private TM_UniversitysService _TM_UniversitysService;
        private TM_Universitys_FacultyService _TM_Universitys_FacultyService;
        private TM_FacultyService _TM_FacultyService;

        public UniversityController(TM_UniversitysService TM_UniversitysService
            , TM_Universitys_FacultyService TM_Universitys_FacultyService
            , TM_FacultyService TM_FacultyService)
        {
            _TM_UniversitysService = TM_UniversitysService;
            _TM_Universitys_FacultyService = TM_Universitys_FacultyService;
            _TM_FacultyService = TM_FacultyService;
        }
        // GET: University
        public ActionResult UniversityList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUniversity result = new vUniversity();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchUniversity SearchItem = (CSearchUniversity)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchUniversity)));
                var lstData = _TM_UniversitysService.GetUniversity(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vUniversity_obj
                                      {
                                          name_en = lstAD.universitys_name_en.StringRemark(500),
                                          short_name_en = lstAD.universitys_short_name_th,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.universitys_description.StringRemark(),
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

        public ActionResult UniversityCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUniversity_obj_save result = new vUniversity_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            return View(result);
        }

        public ActionResult UniversityEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vUniversity_obj_save result = new vUniversity_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_UniversitysService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.name_en = _getData.universitys_name_en + "";
                        result.description = _getData.universitys_description + "";
                        result.active_status = _getData.active_status;
                        result.name_th = _getData.universitys_name_th + "";
                        result.short_name_en = _getData.universitys_short_name_th + "";
                        result.name_aol = _getData.universitys_aol_name + "";
                        if (_getData.TM_Universitys_Faculty != null && _getData.TM_Universitys_Faculty.Any())
                        {

                            result.lstFaculty = (from lstAD in _getData.TM_Universitys_Faculty
                                                 select new vUniversity_Faculty
                                                 {
                                                     fa_display_name = lstAD.universitys_faculty_name_en,
                                                     fa_name = lstAD.TM_Faculty.faculty_name_en,
                                                     active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                     create_user = lstAD.create_user,
                                                     create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                                     update_user = lstAD.update_user,
                                                     update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                     Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditFaculty('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                 }).ToList();
                        }
                        else
                        {
                            result.lstFaculty = new List<vUniversity_Faculty>();
                        }
                    }
                }
            }
            return View(result);

            #endregion

        }


        #region Ajax Function
        [HttpPost]
        public ActionResult LoadUniversityList(CSearchPool SearchItem)
        {
            vUniversity_Return result = new vUniversity_Return();
            List<vUniversity_obj> lstData_resutl = new List<vUniversity_obj>();
            var lstData =new  List<TM_Universitys>();
            if (!string.IsNullOrEmpty(SearchItem.name) && !string.IsNullOrEmpty(SearchItem.active_status))
            {
                lstData = _TM_UniversitysService.GetUniversity(
               SearchItem.name,
               SearchItem.active_status).ToList();
            }
            else
            {

            }
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Count() != 0)
            {

                lstData_resutl = (from lstAD in lstData
                                  select new vUniversity_obj
                                  {
                                      name_en = lstAD.universitys_name_en.StringRemark(500),
                                      short_name_en = lstAD.universitys_short_name_th,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.universitys_description.StringRemark(),
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
        public ActionResult CreateUniversity(vUniversity_obj_save ItemData)
        {
            vUniversity_Return result = new vUniversity_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.name_en))
                {
                    TM_Universitys objSave = new TM_Universitys()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        universitys_name_en = (ItemData.name_en + "").Trim(),
                        universitys_aol_name = (ItemData.name_aol + "").Trim(),
                        universitys_name_th = (ItemData.name_th + "").Trim(),
                        universitys_short_name_th = (ItemData.short_name_en + "").Trim(),
                        universitys_description = ItemData.description,

                    };
                    if (_TM_UniversitysService.CanSave(objSave))
                    {
                        var sComplect = _TM_UniversitysService.CreateNew(objSave);
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
        public ActionResult EditUniversity(vUniversity_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vUniversity_Return result = new vUniversity_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_UniversitysService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.name_en))
                        {
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = ItemData.active_status;
                            _getData.universitys_description = ItemData.description;
                            _getData.universitys_name_en = (ItemData.name_en + "").Trim();
                            _getData.universitys_aol_name = (ItemData.name_aol + "").Trim();
                            _getData.universitys_name_th = (ItemData.name_th + "").Trim();
                            _getData.universitys_short_name_th = (ItemData.short_name_en + "").Trim();
                            if (_TM_UniversitysService.CanSave(_getData))
                            {
                                var sComplect = _TM_UniversitysService.Update(_getData);
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
                                result.Msg = "Duplicate name.";
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

        [HttpPost]
        public ActionResult CreateUniversityFaculty(vUniversity_Faculty_obj_save ItemData)
        {
            objUniversity_Faculty_Return result = new objUniversity_Faculty_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.fa_name_id))
                {
                    int nId = SystemFunction.GetIntNullToZero(ItemData.fa_name_id);
                    int nUniUD = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    if (nId != 0 && nUniUD != 0)
                    {
                        var _GetFaculty = _TM_FacultyService.Find(nId);
                        var _GetUniver = _TM_UniversitysService.Find(nUniUD);
                        TM_Universitys_Faculty objSave = new TM_Universitys_Faculty()
                        {
                            Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.id + "")),
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = ItemData.active_status + "",
                            universitys_faculty_name_en = (ItemData.fa_display_name + "").Trim(),
                            universitys_faculty_description = (ItemData.decs + "").Trim(),
                            TM_Faculty = _GetFaculty,
                            TM_Universitys = _GetUniver,
                        };
                        if (_TM_Universitys_FacultyService.CanSave(objSave))
                        {
                            var sComplect = _TM_Universitys_FacultyService.CreateNew(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _TM_UniversitysService.Find(nUniUD);
                                if (_getEditList != null && _getEditList.TM_Universitys_Faculty != null && _getEditList.TM_Universitys_Faculty.Any())
                                {

                                    result.lstData = (from lstAD in _getEditList.TM_Universitys_Faculty
                                                      select new vUniversity_Faculty
                                                      {
                                                          fa_display_name = lstAD.universitys_faculty_name_en,
                                                          fa_name = lstAD.TM_Faculty.faculty_name_en,
                                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                          create_user = lstAD.create_user,
                                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                                          update_user = lstAD.update_user,
                                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditFaculty('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                      }).ToList();
                                }
                                else
                                {
                                    result.lstData = new List<vUniversity_Faculty>();
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
                            result.Msg = "Duplicate Faculty name.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Faculty not found";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please Faculty";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult LoadUniversityFaculty(vUniversity_Faculty_edit ItemData)
        {
            objUniversity_Faculty_Return result = new objUniversity_Faculty_Return();
            result.objData = new vUniversity_Faculty_obj_save();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(ItemData.IdEncrypt_Uni_Id))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    int nUniUD = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt_Uni_Id + ""));
                    if (nId != 0 && nUniUD != 0)
                    {
                        var _GetData = _TM_Universitys_FacultyService.FindForEdit(nId, nUniUD);
                        if (_GetData != null)
                        {
                            result.objData.id = ItemData.IdEncrypt;
                            result.objData.fa_display_name = _GetData.universitys_faculty_name_en + "";
                            result.objData.decs = _GetData.universitys_faculty_description + "";
                            result.objData.fa_name_id = _GetData.TM_Faculty != null ? _GetData.TM_Faculty.Id + "" : "";
                            result.objData.active_status = _GetData.active_status + "";
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Faculty not found";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Faculty not found";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please Faculty";
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