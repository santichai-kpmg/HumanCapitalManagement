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
    public class UniversityMajorController : BaseController
    {
        private TM_UniversitysService _TM_UniversitysService;
        private TM_Universitys_FacultyService _TM_Universitys_FacultyService;
        private TM_FacultyService _TM_FacultyService;
        private TM_Universitys_MajorService _TM_Universitys_MajorService;
        TM_MajorService _TM_MajorService;
        public UniversityMajorController(TM_UniversitysService TM_UniversitysService
            , TM_Universitys_FacultyService TM_Universitys_FacultyService
            , TM_FacultyService TM_FacultyService
            , TM_Universitys_MajorService TM_Universitys_MajorService
            , TM_MajorService TM_MajorService)
        {
            _TM_UniversitysService = TM_UniversitysService;
            _TM_Universitys_FacultyService = TM_Universitys_FacultyService;
            _TM_FacultyService = TM_FacultyService;
            _TM_Universitys_MajorService = TM_Universitys_MajorService;
            _TM_MajorService = TM_MajorService;
        }
        // GET: UniversityMajor
        public ActionResult UniversityMajorList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUniversityMajor result = new vUniversityMajor();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchUniversityMajor SearchItem = (CSearchUniversityMajor)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchUniversityMajor)));
                var lstData = _TM_UniversitysService.GetUniversity(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vUniversityMajor_obj
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
        public ActionResult UniversityMajorEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vUniversityMajor_obj_save result = new vUniversityMajor_obj_save();
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
                        var _getEditList = _TM_Universitys_MajorService.FindListByUniver(nId);
                        if (_getEditList != null && _getEditList.Any())
                        {

                            result.lstMajor = (from lstAD in _getEditList
                                               select new vUniversityMajor_Faculty
                                               {
                                                   ma_display_name = lstAD.universitys_major_name_en,
                                                   ma_name = lstAD.TM_Major.major_name_en,
                                                   fa_display_name = lstAD.TM_Universitys_Faculty.universitys_faculty_name_en,
                                                   active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                   create_user = lstAD.create_user,
                                                   create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                                   update_user = lstAD.update_user,
                                                   update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                   Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMajor('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                               }).ToList();
                        }
                        else
                        {
                            result.lstMajor = new List<vUniversityMajor_Faculty>();
                        }
                    }
                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadUniversityMajorList(CSearchPool SearchItem)
        {
            vUniversity_Return result = new vUniversity_Return();
            List<vUniversity_obj> lstData_resutl = new List<vUniversity_obj>();
            var lstData = _TM_UniversitysService.GetUniversity(
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
        public ActionResult CreateUniversityMajor(vUniversityMajor_save ItemData)
        {
            objUniversityMajor_Return result = new objUniversityMajor_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.fa_name_id))
                {
                    int nFaId = SystemFunction.GetIntNullToZero(ItemData.fa_name_id);
                    int nMaId = SystemFunction.GetIntNullToZero(ItemData.ma_name_id);
                    int nUniUD = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    if (nFaId != 0 && nMaId != 0 && nUniUD != 0)
                    {
                        var _GetFaculty = _TM_Universitys_FacultyService.Find(nFaId);
                        var _GetMajor = _TM_MajorService.Find(nMaId);
                        if (_GetFaculty != null && _GetMajor != null)
                        {
                            TM_Universitys_Major objSave = new TM_Universitys_Major()
                            {
                                Id = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.id + "")),
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = ItemData.active_status + "",
                                universitys_major_name_en = (ItemData.ma_display_name + "").Trim(),
                                universitys_major_description = (ItemData.decs + "").Trim(),
                                TM_Universitys_Faculty = _GetFaculty,
                                TM_Major = _GetMajor,
                            };
                            if (_TM_Universitys_MajorService.CanSave(objSave))
                            {
                                var sComplect = _TM_Universitys_MajorService.CreateNew(objSave);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _TM_Universitys_MajorService.FindListByUniver(nUniUD);
                                    if (_getEditList != null && _getEditList.Any())
                                    {

                                        result.lstData = (from lstAD in _getEditList
                                                          select new vUniversityMajor_Faculty
                                                          {
                                                              ma_display_name = lstAD.universitys_major_name_en,
                                                              ma_name = lstAD.TM_Major.major_name_en,
                                                              fa_display_name = lstAD.TM_Universitys_Faculty.universitys_faculty_name_en,
                                                              active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                              create_user = lstAD.create_user,
                                                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                                              update_user = lstAD.update_user,
                                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMajor('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                          }).ToList();
                                    }
                                    else
                                    {
                                        result.lstData = new List<vUniversityMajor_Faculty>();
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
                                result.Msg = "Duplicate Major name.";
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
        public ActionResult LoadUniversityMajor(vUniversity_Faculty_edit ItemData)
        {
            objUniversityMajor_Return result = new objUniversityMajor_Return();
            result.objData = new vUniversityMajor_save();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(ItemData.IdEncrypt_Uni_Id))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    int nUniUD = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt_Uni_Id + ""));
                    if (nId != 0 && nUniUD != 0)
                    {
                        var _GetData = _TM_Universitys_MajorService.FindForEdit(nId, nUniUD);
                        if (_GetData != null)
                        {
                            result.objData.id = ItemData.IdEncrypt;
                            result.objData.ma_display_name = _GetData.universitys_major_name_en + "";
                            result.objData.decs = _GetData.universitys_major_description + "";
                            result.objData.fa_name_id = _GetData.TM_Universitys_Faculty != null ? _GetData.TM_Universitys_Faculty.Id + "" : "";
                            result.objData.ma_name_id = _GetData.TM_Major.Id + "";
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