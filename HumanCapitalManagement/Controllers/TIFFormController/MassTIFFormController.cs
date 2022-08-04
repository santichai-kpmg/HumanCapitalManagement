using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.TIFForm;
using HumanCapitalManagement.Service.TIFForm;
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

    public class MassTIFFormController : BaseController
    {
        private TM_Mass_TIF_FormService _TM_Mass_TIF_FormService;
        private TM_Mass_Question_TypeService _TM_Mass_Question_TypeService;
        public MassTIFFormController(TM_Mass_TIF_FormService TM_Mass_TIF_FormService,
            TM_Mass_Question_TypeService TM_Mass_Question_TypeService)
        {
            _TM_Mass_TIF_FormService = TM_Mass_TIF_FormService;
            _TM_Mass_Question_TypeService = TM_Mass_Question_TypeService;
        }
        // GET: MassTIFForm

        public ActionResult MassTIFFormList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMassTIFForm result = new vMassTIFForm();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchMassTIFForm SearchItem = (CSearchMassTIFForm)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchMassTIFForm)));
                var lstData = _TM_Mass_TIF_FormService.GetTIF_Form(
               SearchItem.name,
          "");
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vMassTIFForm_obj
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
        public ActionResult MassTIFFormCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMassTIFForm_obj_save result = new vMassTIFForm_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.action_date = DateTime.Now.DateTimebyCulture();
            return View(result);
        }
        public ActionResult MassTIFFormView(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vMassTIFForm_obj_save result = new vMassTIFForm_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Mass_TIF_FormService.Find(nId);
                    if (_getData != null)
                    {

                        result.active_status = _getData.active_status;
                        result.action_date = _getData.action_date.HasValue ? _getData.action_date.Value.DateTimebyCulture() : "";
                        result.description = _getData.description;
                        result.code = _getData.Id + "";
                        if (_getData.TM_MassTIF_Form_Question != null && _getData.TM_MassTIF_Form_Question.Any())
                        {
                            result.lstQuestion = _getData.TM_MassTIF_Form_Question.Select(s => new vMassTIFForm_Question
                            {
                                header = s.header + "",
                                aanswer = s.a_answer + "",
                                banswer = s.b_answer + "",
                                canswer = s.c_answer + "",
                                nID = s.seq + "",
                            }).ToList();
                        }

                        if (_getData.TM_Mass_Auditing_Question != null && _getData.TM_Mass_Auditing_Question.Any())
                        {
                            result.lstQuestion = _getData.TM_MassTIF_Form_Question.Select(s => new vMassTIFForm_Question
                            {
                                header = s.header + "",
                                aanswer = s.a_answer + "",
                                banswer = s.b_answer + "",
                                canswer = s.c_answer + "",
                                nID = s.seq + "",
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
        public ActionResult LoadMassTIFFormList(CSearchPool SearchItem)
        {
            vMassTIFForm_Return result = new vMassTIFForm_Return();
            List<vMassTIFForm_obj> lstData_resutl = new List<vMassTIFForm_obj>();
            var lstData = _TM_Mass_TIF_FormService.GetTIF_Form(
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
                                  select new vMassTIFForm_obj
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
        public ActionResult CreateMassTIFForm(vMassTIFForm_obj_save ItemData)
        {
            vMassTIFForm_Return result = new vMassTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstQuestion != null && ItemData.lstQuestion.Any() && ItemData.lstAuditing_Questions != null && ItemData.lstAuditing_Questions.Any())
                {

                    List<TM_MassTIF_Form_Question> lstObjQuestion = new List<TM_MassTIF_Form_Question>();
                    lstObjQuestion = ItemData.lstQuestion.Select(s => new TM_MassTIF_Form_Question
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        //question = (s.question + "").Trim(),
                        a_answer = (s.aanswer + "").Trim(),
                        b_answer = (s.banswer + "").Trim(),
                        c_answer = (s.canswer + "").Trim(),
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),

                    }).ToList();

                    List<TM_Mass_Auditing_Question> lstAudQuestion = new List<TM_Mass_Auditing_Question>();
                    int SetA = 1;
                    int SetB = 1;
                    int SetC = 1;
                    ItemData.lstAuditing_Questions.ForEach(ed =>
                    {
                        if (ed.set_id + "" == "1")
                        {
                            ed.nSeq = SetA++;
                        }
                        else if (ed.set_id + "" == "2")
                        {
                            ed.nSeq = SetB++;
                        }
                        else if (ed.set_id + "" == "3")
                        {
                            ed.nSeq = SetC++;
                        }
                    });

                    lstAudQuestion = ItemData.lstAuditing_Questions.Select(s => new TM_Mass_Auditing_Question
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        answer_guideline = (s.guideline + "").Trim(),
                        seq = s.nSeq,
                        TM_Mass_Question_Type = _TM_Mass_Question_TypeService.Find(SystemFunction.GetIntNullToZero(s.set_id + "")),
                    }).ToList();


                    TM_Mass_TIF_Form objSave = new TM_Mass_TIF_Form()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        TM_MassTIF_Form_Question = lstObjQuestion.ToList(),
                        TM_Mass_Auditing_Question = lstAudQuestion.ToList(),
                        description = (ItemData.description + "").Trim(),
                        action_date = dNow,
                    };

                    var sComplect = _TM_Mass_TIF_FormService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_Mass_TIF_FormService.UpdateInactive(objSave);
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
        public ActionResult EditMassTIFForm(vMassTIFForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vMassTIFForm_Return result = new vMassTIFForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Mass_TIF_FormService.Find(nId);
                    //if (_getData != null)
                    //{
                    //    if (!string.IsNullOrEmpty(ItemData.name_en))
                    //    {

                    //        _getData.update_user = CGlobal.UserInfo.UserId;
                    //        _getData.update_date = dNow;
                    //        _getData.active_status = ItemData.active_status;

                    //            var sComplect = _TM_Mass_TIF_FormService.Update(_getData);
                    //            if (sComplect > 0)
                    //            {
                    //                result.Status = SystemFunction.process_Success;
                    //            }
                    //            else
                    //            {
                    //                result.Status = SystemFunction.process_Failed;
                    //                result.Msg = "Error, please try again.";
                    //            }


                    //    }
                    //    else
                    //    {
                    //        result.Status = SystemFunction.process_Failed;
                    //        result.Msg = "Error, please enter name";
                    //    }
                    //}
                    //else
                    //{
                    //    result.Status = SystemFunction.process_Failed;
                    //    result.Msg = "Error, Request Type Not Found.";
                    //}
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