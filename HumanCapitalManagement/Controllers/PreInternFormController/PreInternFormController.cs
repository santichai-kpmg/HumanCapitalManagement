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
    public class PreInternFormController : BaseController
    {
        private TM_PIntern_FormService _TM_PIntern_FormService;
        private TM_PIntern_Mass_Form_QuestionService _TM_PIntern_Mass_Form_QuestionService;
        public PreInternFormController(TM_PIntern_FormService TM_PIntern_FormService, 
            TM_PIntern_Mass_Form_QuestionService TM_PIntern_Mass_Form_QuestionService)
        {
            _TM_PIntern_FormService = TM_PIntern_FormService;
            _TM_PIntern_Mass_Form_QuestionService = TM_PIntern_Mass_Form_QuestionService;
        }
        // GET: PreInternForm Non Mass
        public ActionResult PreInternFormList(string qryStr)
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
                var lstData = _TM_PIntern_FormService.GetPIntern_Form(
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
            return View(result);
        }
        public ActionResult PreInternFormCreate()
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
            return View(result);
        }
        public ActionResult PreInternFormView(string id)
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
                    var _getData = _TM_PIntern_FormService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.active_status = _getData.active_status;
                        result.action_date = _getData.action_date.HasValue ? _getData.action_date.Value.DateTimebyCulture() : "";
                        result.description = _getData.description;
                        result.code = _getData.Id + "";
                        if (_getData.TM_PIntern_Form_Question != null && _getData.TM_PIntern_Form_Question.Any())
                        {
                            result.lstQuestion = _getData.TM_PIntern_Form_Question.Select(s => new vTIFForm_Question
                            {
                                header = s.header + "",
                                question = s.question + "",
                                nID = s.seq + "",
                                topic = s.topic+"",
                            }).ToList();
                        }
                    }
                }
            }
            return View(result);

            #endregion

        }

        //GET: PreInternForm Mass 
        public ActionResult PreIntern_Mass_FormList(string qryStr)
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
                var lstData = _TM_PIntern_Mass_Form_QuestionService.GetPIntern_Form(
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
            return View(result);
        }
        public ActionResult PreIntern_Mass_FormCreate()
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
            return View(result);
        }
        public ActionResult PreIntern_Mass_FormView(string id)
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
                    var _getData = _TM_PIntern_Mass_Form_QuestionService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.active_status = _getData.active_status;
                        result.action_date = _getData.action_date.HasValue ? _getData.action_date.Value.DateTimebyCulture() : "";
                        result.description = _getData.description;
                        result.code = _getData.Id + "";
                        if (_getData.TM_PIntern_Mass_Question != null && _getData.TM_PIntern_Mass_Question.Any())
                        {
                            result.lstQuestion = _getData.TM_PIntern_Mass_Question.Select(s => new vTIFForm_Question
                            {
                                header = s.header + "",
                                question = s.question + "",
                                nID = s.seq + "",
                                topic = s.topic + "",
                            }).ToList();
                        }
                    }
                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function
        //Non Mass Question Ajax
        [HttpPost]
        public ActionResult LoadPreInternFormList(CSearchPool SearchItem)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            List<vTIFForm_obj> lstData_resutl = new List<vTIFForm_obj>();
            var lstData = _TM_PIntern_FormService.GetPIntern_Form(
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
        public ActionResult CreatePreInternForm(vTIFForm_obj_save ItemData)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstQuestion != null && ItemData.lstQuestion.Any())
                {

                    List<TM_PIntern_Form_Question> lstObjQuestion = new List<TM_PIntern_Form_Question>();
                    lstObjQuestion = ItemData.lstQuestion.Select(s => new TM_PIntern_Form_Question
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        question = (s.question + "").Trim(),
                        topic = (s.topic+"").Trim(),
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),

                    }).ToList();

                    TM_PIntern_Form objSave = new TM_PIntern_Form()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        TM_PIntern_Form_Question = lstObjQuestion.ToList(),
                        description = (ItemData.description + "").Trim(),
                        action_date = dNow,
                    };

                    var sComplect = _TM_PIntern_FormService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_PIntern_FormService.UpdateInactive(objSave);
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
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditPreInternForm(vTIFForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PIntern_FormService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = "Y";
                        var sComplect = _TM_PIntern_FormService.Update(_getData);
                        if (sComplect > 0)
                        {
                            _TM_PIntern_FormService.UpdateInactive(_getData);
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

        //Mass Question Ajax
        [HttpPost]
        public ActionResult LoadPreIntern_Mass_FormList(CSearchPool SearchItem)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            List<vTIFForm_obj> lstData_resutl = new List<vTIFForm_obj>();
            var lstData = _TM_PIntern_Mass_Form_QuestionService.GetPIntern_Form(
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
        public ActionResult CreatePreIntern_Mass_Form(vTIFForm_obj_save ItemData)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstQuestion != null && ItemData.lstQuestion.Any())
                {

                    List<TM_PIntern_Mass_Question> lstObjQuestion = new List<TM_PIntern_Mass_Question>();
                    lstObjQuestion = ItemData.lstQuestion.Select(s => new TM_PIntern_Mass_Question
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        question = (s.question + "").Trim(),
                        topic = (s.topic + "").Trim(),
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),

                    }).ToList();

                    TM_PIntern_Mass_Form_Question objSave = new TM_PIntern_Mass_Form_Question()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        TM_PIntern_Mass_Question = lstObjQuestion.ToList(),
                        description = (ItemData.description + "").Trim(),
                        action_date = dNow,
                    };

                    var sComplect = _TM_PIntern_Mass_Form_QuestionService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_PIntern_Mass_Form_QuestionService.UpdateInactive(objSave);
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
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditPreIntern_Mass_Form(vTIFForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PIntern_Mass_Form_QuestionService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = "Y";
                        var sComplect = _TM_PIntern_Mass_Form_QuestionService.Update(_getData);
                        if (sComplect > 0)
                        {
                            _TM_PIntern_Mass_Form_QuestionService.UpdateInactive(_getData);
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
                        result.Msg = "Error, Pre Intern Form Not Found.";
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