using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models._360Feedback.MiniHeart;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.FeedbacksController
{
    public class MiniHeart_QuestionController : BaseController
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        private MiniHeart_DetailService _MiniHeart_DetailService;
        private TM_MiniHeart_QuestionService _TM_MiniHeart_QuestionService;
        private TM_MiniHeart_Group_QuestionService _TM_MiniHeart_Group_QuestionService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public MiniHeart_QuestionController(
           TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
            , MiniHeart_DetailService MiniHeart_DetailService
            , TM_MiniHeart_QuestionService TM_MiniHeart_QuestionService
            , TM_MiniHeart_Group_QuestionService TM_MiniHeart_Group_QuestionService
            )
        {

            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
            _MiniHeart_DetailService = MiniHeart_DetailService;
            _TM_MiniHeart_QuestionService = TM_MiniHeart_QuestionService;
            _TM_MiniHeart_Group_QuestionService = TM_MiniHeart_Group_QuestionService;
        }
        // GET: MiniHeart_Question
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MiniHeart_QuestionList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMiniHeart_Group_Question result = new vMiniHeart_Group_Question();
        
            var getdata = _TM_MiniHeart_Group_QuestionService.GetDataAllStatus().ToList();
            string BackUrl = "";
            result.lstData = getdata.Select(s=> new vMiniHeart_Group_Question_obj() {
                Id = s.Id,
                View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(s.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                Action_date = s.Action_date.Value.ToString("dd MMM yyyy HH:mm"),
                Active_Status = s.Active_Status =="Y" ?"Active":"Inactive",
                Description = s.Description,
                Create_Date = s.Create_Date.Value.ToString("dd MMM yyyy HH:mm"),
                Create_User = s.Create_User,
                Update_Date = s.Update_Date.Value.ToString("dd MMM yyyy HH:mm"),
                Update_User = s.Update_User
            }).ToList();
            result.active_status = "Y";
            return View(result);
        }

        public ActionResult MiniHeart_QuestionCreate(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMiniHeart_All_Question_Save result = new vMiniHeart_All_Question_Save();


            result.Id = 0;
            result.Active_Status = "Y";
            result.Action_date = DateTime.Now.DateTimebyCulture();
            return View(result);
        }

        public ActionResult MiniHeart_QuestionView(string id)
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            #region main code

            vMiniHeart_All_Question_Save result = new vMiniHeart_All_Question_Save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    //var getqustion = _TM_MiniHeart_QuestionService.GetDataForSelect().Where(w => w.TM_MiniHeart_Group_Question.Id == nId).ToList();
                    ////var getgroup = _TM_MiniHeart_Group_QuestionService.GetDataForSelect().ToList().Select(s => s.TM_MiniHeart_Question).ToList();


                    //var setquestion = getqustion.Select(s => new vMiniHeart_Question_obj()
                    //{
                    //    Id = s.Id,
                    //    Seq = s.Seq,
                    //    Topic = s.Topic,
                    //    Content = s.Content,
                    //    icon = s.Icon,
                    //    remark = ""


                    //}).ToList();
                    //result.lstData = setquestion.OrderBy(o => o.Seq).ToList();


                    var _getData = _TM_MiniHeart_Group_QuestionService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.Active_Status = _getData.Active_Status;
                        result.Action_date = _getData.Action_date.HasValue ? _getData.Action_date.Value.DateTimebyCulture() : "";
                        result.Description = _getData.Description;
                        result.Id = _getData.Id;
                        if (_getData.TM_MiniHeart_Question != null && _getData.TM_MiniHeart_Question.Any())
                        {
                            result.lstData = _getData.TM_MiniHeart_Question.Select(s => new vMiniHeart_Question_obj()
                            {
                                Id = s.Id,
                                Seq = s.Seq,
                                Group = s.Group,
                                Topic = s.Topic,
                                Content = s.Content,
                                icon = s.Icon,
                                remark = ""
                            }).ToList();
                        }
                    }
                }
            }
            return View(result);

            #endregion

        }


        [HttpPost]
        public ActionResult CreateMiniHeart_Question_Group(vMiniHeart_All_Question_Save ItemData)
        {
            vMiniHeart_Group_Question_Return result = new vMiniHeart_Group_Question_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstData != null && ItemData.lstData.Any())
                {

                    List<TM_MiniHeart_Question> lstObjQuestion = new List<TM_MiniHeart_Question>();
                    lstObjQuestion = ItemData.lstData.Select(s => new TM_MiniHeart_Question
                    {
                        Update_User = CGlobal.UserInfo.EmployeeNo,
                        Update_Date = dNow,
                        Create_User = CGlobal.UserInfo.EmployeeNo,
                        Create_Date = dNow,
                        Active_Status = "Y",
                        Topic = (s.Topic + "").Trim(),
                        Content = (s.Content + "").Trim(),
                        Seq = SystemFunction.GetIntNullToZero(s.Id + ""),
                        Group = SystemFunction.GetIntNullToZero(s.Group + ""),

                    }).ToList();

                    TM_MiniHeart_Group_Question objSave = new TM_MiniHeart_Group_Question()
                    {
                        Id = 0,
                        Update_User = CGlobal.UserInfo.EmployeeNo,
                        Update_Date = dNow,
                        Create_User = CGlobal.UserInfo.EmployeeNo,
                        Create_Date = dNow,
                        Active_Status = "Y",
                        TM_MiniHeart_Question = lstObjQuestion.ToList(),
                        Description = (ItemData.Description + "").Trim(),
                        Action_date = dNow,
                    };

                    var sComplect = _TM_MiniHeart_Group_QuestionService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_MiniHeart_Group_QuestionService.UpdateInactive(objSave);
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
                    result.Msg = "Error, please add Question";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult EditMiniHeart_Question_Group(vMiniHeart_All_Question_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vMiniHeart_Group_Question_Return result = new vMiniHeart_Group_Question_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_MiniHeart_Group_QuestionService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.Update_User = CGlobal.UserInfo.EmployeeNo;
                        _getData.Update_Date = dNow;
                        _getData.Active_Status = "Y";
                        var sComplect = _TM_MiniHeart_Group_QuestionService.Update(_getData);
                        if (sComplect > 0)
                        {
                            _TM_MiniHeart_Group_QuestionService.UpdateInactive(_getData);
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

    }
}