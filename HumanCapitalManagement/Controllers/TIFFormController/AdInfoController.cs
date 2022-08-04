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
    public class AdInfoController : BaseController
    {
        private TM_Additional_InformationService _TM_Additional_InformationService;
        private TM_Additional_QuestionsService _TM_Additional_QuestionsService;
        private TM_Additional_AnswersService _TM_Additional_AnswersService;
        public AdInfoController(TM_Additional_InformationService TM_Additional_InformationService
            , TM_Additional_QuestionsService TM_Additional_QuestionsService
            , TM_Additional_AnswersService TM_Additional_AnswersService)
        {
            _TM_Additional_InformationService = TM_Additional_InformationService;
            _TM_Additional_QuestionsService = TM_Additional_QuestionsService;
            _TM_Additional_AnswersService = TM_Additional_AnswersService;
        }
        // GET: AdInfo
        public ActionResult AdInfoList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vAdInfo result = new vAdInfo();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchAdInfo SearchItem = (CSearchAdInfo)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchAdInfo)));
                var lstData = _TM_Additional_InformationService.GetTIF_Form(
               SearchItem.name,
          "");
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vAdInfo_obj
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
        public ActionResult AdInfoCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vAdInfo_obj_save result = new vAdInfo_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.action_date = DateTime.Now.DateTimebyCulture();
            return View(result);
        }
        public ActionResult AdInfoView(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vAdInfo_obj_save result = new vAdInfo_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Additional_InformationService.Find(nId);
                    if (_getData != null)
                    {
                        result.active_status = _getData.active_status;
                        result.action_date = _getData.action_date.HasValue ? _getData.action_date.Value.DateTimebyCulture() : "";
                        result.description = _getData.description;
                        result.code = _getData.Id + "";
                        result.IdEncrypt = qryStr;
                        if (_getData.TM_Additional_Questions != null && _getData.TM_Additional_Questions.Any())
                        {
                            result.lstQuestion = _getData.TM_Additional_Questions.Select(s => new vAdInfo_Question
                            {
                                header = s.header + "",
                                question = s.question + "",
                                nID = s.seq + "",
                                multi_text = s.multi_answer + "" == "Y" ? "Yes" : "No",
                                // multi_answer = s.TM_Additional_Answers != null ? s.TM_Additional_Answers.OrderBy(o => o.seq).Select(s2 => s2.answers + "<br/>"). : "",
                                //lstAnswers = s.TM_Additional_Answers != null ? s.TM_Additional_Answers.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s2 => new vAdInfo_Answers { answer = s2.answers }).ToList() : new List<vAdInfo_Answers>(),
                                lstAnswers = new List<vAdInfo_Answers>(),
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
        public ActionResult LoadAdInfoList(CSearchPool SearchItem)
        {
            vAdInfo_Return result = new vAdInfo_Return();
            List<vAdInfo_obj> lstData_resutl = new List<vAdInfo_obj>();
            var lstData = _TM_Additional_InformationService.GetTIF_Form(
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
                                  select new vAdInfo_obj
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
        public ActionResult CreateAdInfo(vAdInfo_obj_save ItemData)
        {
            vAdInfo_Return result = new vAdInfo_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstQuestion != null && ItemData.lstQuestion.Any())
                {

                    List<TM_Additional_Questions> lstObjQuestion = new List<TM_Additional_Questions>();
                    lstObjQuestion = ItemData.lstQuestion.Select(s => new TM_Additional_Questions
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        question = (s.question + "").Trim(),
                        multi_answer = s.multi_answer,
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),

                    }).ToList();

                    TM_Additional_Information objSave = new TM_Additional_Information()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        TM_Additional_Questions = lstObjQuestion.ToList(),
                        description = (ItemData.description + "").Trim(),
                        action_date = dNow,
                    };

                    var sComplect = _TM_Additional_InformationService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_Additional_InformationService.UpdateInactive(objSave);
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
        public ActionResult EditAdInfo(vAdInfo_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vAdInfo_Return result = new vAdInfo_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Additional_InformationService.Find(nId);
                    if (_getData != null)
                    {
                        if (ItemData.lstAnswers != null && ItemData.lstAnswers.Any())
                        {
                            int[] nQuestionID = ItemData.lstAnswers.Select(s => SystemFunction.GetIntNullToZero(s.question_id)).ToArray();
                            nQuestionID = nQuestionID.Distinct().ToArray();
                            if (nQuestionID.Length > 0)
                            {
                                foreach (var item in nQuestionID)
                                {
                                    var GetQuestion = _TM_Additional_QuestionsService.Find(item);
                                    if (GetQuestion != null)
                                    {
                                        int nSeq = 1;
                                        foreach (var ans in ItemData.lstAnswers.Where(w => w.question_id == item + "").OrderBy(o => SystemFunction.GetIntNullToZero(o.nID + "")))
                                        {
                                            TM_Additional_Answers objSave = new TM_Additional_Answers()
                                            {
                                                seq = nSeq++,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                active_status = "Y",
                                                is_other = ans.other_answer,
                                                answers = ans.answer,
                                                TM_Additional_Questions = GetQuestion,
                                            };

                                            var sComplect = _TM_Additional_AnswersService.CreateNew(ref objSave);
                                        }
                                    }


                                }
                                result.Status = SystemFunction.process_Success;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Additional Information Not Found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Additional Information Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Additional Information Not Found.";
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