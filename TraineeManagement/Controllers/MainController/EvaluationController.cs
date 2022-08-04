using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraineeManagement.App_Start;
using TraineeManagement.Controllers.CommonControllers;
using TraineeManagement.ViewModels.CommonVM;
using TraineeManagement.ViewModels.MainVM;
using static TraineeManagement.App_Start.TraineeClass;

namespace TraineeManagement.Controllers.MainController
{
    public class EvaluationController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_Evaluation_FormService _TM_Evaluation_FormService;
        private TM_Eva_RatingService _TM_Eva_RatingService;
        private TM_Trainee_EvaService _TM_Trainee_EvaService;
        private TM_Trainee_Eva_AnswerService _TM_Trainee_Eva_AnswerService;
        private TM_TraineeEva_StatusService _TM_TraineeEva_StatusService;
        private TM_CandidatesService _TM_CandidatesService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public EvaluationController(TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Evaluation_FormService TM_Evaluation_FormService
            , TM_Eva_RatingService TM_Eva_RatingService
            , TM_Trainee_EvaService TM_Trainee_EvaService
            , TM_Trainee_Eva_AnswerService TM_Trainee_Eva_AnswerService
            , TM_TraineeEva_StatusService TM_TraineeEva_StatusService
            , TM_CandidatesService TM_CandidatesService
            , MailContentService MailContentService
                   )
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_Evaluation_FormService = TM_Evaluation_FormService;
            _TM_Eva_RatingService = TM_Eva_RatingService;
            _TM_Trainee_EvaService = TM_Trainee_EvaService;
            _TM_Trainee_Eva_AnswerService = TM_Trainee_Eva_AnswerService;
            _TM_TraineeEva_StatusService = TM_TraineeEva_StatusService;
            _TM_CandidatesService = TM_CandidatesService;
            _MailContentService = MailContentService;
        }
        // GET: Evaluation
        public ActionResult EvaluationList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEvaluation result = new vEvaluation();
            result.active_status = "Y";
            int nID = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);
            var lstData = _TM_PR_Candidate_MappingService.GetDataForEva(nID);

            if (lstData.Any())
            {
                result.lstData = (from lstAD in lstData
                                  select new vEvaluation_obj
                                  {
                                      Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + @"');return false;"">Click to Evaluate <i class=""fa fa-arrow-right""></i></a>",
                                      position = lstAD.PersonnelRequest.TM_Divisions.division_name_en + " (" + (lstAD.PersonnelRequest.target_period.HasValue ? lstAD.PersonnelRequest.target_period.Value.DateTimebyCulture("MMM-yyyy") : "") + ")",
                                      target_start = lstAD.PersonnelRequest.target_period.HasValue ? lstAD.PersonnelRequest.target_period.Value.DateTimebyCulture() : "",
                                      target_end = lstAD.PersonnelRequest.target_period_to.HasValue ? lstAD.PersonnelRequest.target_period_to.Value.DateTimebyCulture() : "",
                                  }).ToList();
            }
            return View(result);
        }
        public ActionResult EvaluationView(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEvaluation_obj_View result = new vEvaluation_obj_View();
            if (!string.IsNullOrEmpty(qryStr))
            { 
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.TM_Candidates.Id + "" != CGlobal.UserInfo.UserId)
                        {
                            return RedirectToAction("EvaluationList", "Evaluation");
                        }
                        result.group_name = _getData.PersonnelRequest.TM_Divisions.division_name_en + " (" + (_getData.PersonnelRequest.target_period.HasValue ? _getData.PersonnelRequest.target_period.Value.DateTimebyCulture("MMM-yyyy") : "") + ")";
                        result.trainee_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                        result.position = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                        result.no_eva = _getData.PersonnelRequest.no_of_eva + "";
                        result.nick_name = _getData.TM_Candidates.candidate_NickName + "";
                        result.trainee_no = _getData.TM_Candidates.candidate_TraineeNumber + "";
                        result.target_start = _getData.trainee_start.HasValue ? _getData.trainee_start.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.trainee_end.HasValue ? _getData.trainee_end.Value.DateTimebyCulture() : "";
                        int nID = SystemFunction.GetIntNullToZero(CGlobal.UserInfo.UserId);

                        var _getList = _TM_Trainee_EvaService.FindByMappingIDList(_getData.Id);
                        if (_getList != null && _getList.Any(a => a.active_status + "" == "Y"))
                        {
                            List<string> lstUser = new List<string>();
                            lstUser.AddRange(_getList.Select(s => s.req_incharge_Approve_user).ToList());
                            lstUser.AddRange(_getList.Select(s => s.req_mgr_Approve_user).ToList());

                            var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                            result.lstData = _getList.Where(w => w.active_status + "" == "Y").Select(s => new vEvaluation_obj
                            {
                                pm_name = _getUser.Where(w => w.EmpNo == s.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                incharge_name = _getUser.Where(w => w.EmpNo == s.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                key_type = s.approve_type + "" != "" ? (s.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                create_user = s.create_user,
                                create_date = s.create_date.HasValue ? s.create_date.Value.DateTimebyCulture() : "",
                                update_user = s.update_user,
                                update_date = s.update_date.HasValue ? s.update_date.Value.DateTimebyCulture() : "",
                                Edit = @"<a href=""#"" class=""btn btn-animated btn-info btn-sm"" href=""#"" onclick=""Edit('" + HCMFunc.Encrypt(s.Id + "") + @"');return false;"">Edit <i class=""fa fa-arrow-right""></i></a>",
                                eva_status = s.TM_TraineeEva_Status != null ? s.TM_TraineeEva_Status.eva_status_name_en : "",
                            }).ToList();
                        }
                    }
                    else
                    {
                        return RedirectToAction("EvaluationList", "Evaluation");
                    }
                }
                else
                {
                    return RedirectToAction("EvaluationList", "Evaluation");
                }
            }
            else
            {
                return RedirectToAction("EvaluationList", "Evaluation");
            }

            return View(result);
        }
        public ActionResult EvaluationCreate(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEvaluation_obj_Save result = new vEvaluation_obj_Save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.TM_Candidates.Id + "" != CGlobal.UserInfo.UserId)
                        {
                            return RedirectToAction("EvaluationList", "Evaluation");
                        }
                        var _GetEvaNotComplete = _TM_Trainee_EvaService.FindByMappingIDList(_getData.Id);
                        if (_GetEvaNotComplete != null && _GetEvaNotComplete.Any(a => a.hr_acknowledge + "" != "Y"))
                        {
                            result.can_create_eve = "N";
                        }
                        else
                        {
                            result.can_create_eve = "Y";
                            result.group_name = _getData.PersonnelRequest.TM_Divisions.division_name_en + " (" + (_getData.PersonnelRequest.target_period.HasValue ? _getData.PersonnelRequest.target_period.Value.DateTimebyCulture("MMM-yyyy") : "") + ")";
                            result.trainee_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                            result.position = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                            result.no_eva = _getData.PersonnelRequest.no_of_eva + "";
                            result.nick_name = _getData.TM_Candidates.candidate_NickName + "";
                            result.trainee_no = _getData.TM_Candidates.candidate_TraineeNumber + "";
                            result.target_start = _getData.trainee_start.HasValue ? _getData.trainee_start.Value.DateTimebyCulture() : "";
                            result.target_end = _getData.trainee_end.HasValue ? _getData.trainee_end.Value.DateTimebyCulture() : "";
                            var _getEva = _TM_Evaluation_FormService.GetActiveEvaForm();
                            if (_getEva != null && _getEva.TM_Evaluation_Question != null && _getEva.TM_Evaluation_Question.Any(a => a.active_status == "Y"))
                            {
                                result.objtifform = new vObject_of_tif();
                                result.objtifform.TIF_no = _getEva.Id + "";
                                result.objtifform.lstQuestion = (from lstQ in _getEva.TM_Evaluation_Question
                                                                 select new vEva_list_question
                                                                 {
                                                                     id = lstQ.Id + "",
                                                                     question = lstQ.question,
                                                                     sgroup = lstQ.header,
                                                                     nSeq = lstQ.seq,
                                                                     remark = "",
                                                                     rating = "",
                                                                 }).ToList();
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    point = s.point,

                                }).ToList();

                                result.objtifform.lstRatingActive = _TM_Eva_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description,
                                    point = s.point,

                                }).ToList();

                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - ", point = 0 });
                            }
                        }

                    }
                    else
                    {
                        return RedirectToAction("EvaluationList", "Evaluation");
                    }
                }
                else
                {
                    return RedirectToAction("EvaluationList", "Evaluation");
                }
            }
            else
            {
                return RedirectToAction("EvaluationList", "Evaluation");
            }

            return View(result);
        }
        public ActionResult EvaluationEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEvaluation_obj_Save result = new vEvaluation_obj_Save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getDataEva = _TM_Trainee_EvaService.Find(nId);
                    if (_getDataEva != null && _getDataEva.TM_PR_Candidate_Mapping != null)
                    {
                        if (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.Id + "" != CGlobal.UserInfo.UserId)
                        {
                            return RedirectToAction("EvaluationList", "Evaluation");
                        }
                        var _getData = _getDataEva.TM_PR_Candidate_Mapping;
                        result.group_name = _getData.PersonnelRequest.TM_Divisions.division_name_en + " (" + (_getData.PersonnelRequest.target_period.HasValue ? _getData.PersonnelRequest.target_period.Value.DateTimebyCulture("MMM-yyyy") : "") + ")";
                        result.trainee_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                        result.position = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                        result.no_eva = _getData.PersonnelRequest.no_of_eva + "";
                        result.nick_name = _getData.TM_Candidates.candidate_NickName + "";
                        result.trainee_no = _getData.TM_Candidates.candidate_TraineeNumber + "";
                        result.target_start = _getData.trainee_start.HasValue ? _getData.trainee_start.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.trainee_end.HasValue ? _getData.trainee_end.Value.DateTimebyCulture() : "";
                        result.status = _getDataEva.submit_status + "";

                        var getTrainee_Eva = _getDataEva.Id;
                        var getevaforsetrating = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(getTrainee_Eva).TM_Evaluation_Question.TM_Evaluation_Form.Id;

                        var _getEva = _TM_Evaluation_FormService.Find(getevaforsetrating);

                        if (_getEva != null && _getEva.TM_Evaluation_Question != null )
                        {
                            result.objtifform = new vObject_of_tif();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getEva.TM_Evaluation_Question
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                             }).ToList();
                            result.objtifform.lstRating = _TM_Eva_RatingService.GetActiveAndInactiveForSelect().Where(w => w.TM_Evaluation_Form.Id == getevaforsetrating).Select(s => new lstDataSelect
                            {
                                value = s.Id + "",
                                text = s.point + " : " + s.rating_name_en + "",
                                point = s.point,

                            }).ToList();

                            
                            result.objtifform.lstRatingActive = _TM_Eva_RatingService.GetActiveAndInactiveForSelect().Where(w => w.TM_Evaluation_Form.Id == getevaforsetrating).Select(s => new lstDataSelect
                            {
                                value = s.Id + "",
                                text = s.point + " : " + s.rating_name_en + "",
                                detail = s.rating_description,
                                point = s.point,

                            }).ToList();

                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - ", point = 0 });
                        }
                        result.IdEncryptBack = HCMFunc.Encrypt(_getDataEva.TM_PR_Candidate_Mapping.Id + "");
                        result.comment = _getDataEva.trainee_learned;
                        result.comment2 = _getDataEva.trainee_done_well;
                        result.comment3 = _getDataEva.trainee_developmental;
                        result.type_approve = _getDataEva.approve_type;
                        if (!string.IsNullOrEmpty(_getDataEva.req_mgr_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.mgr_user_no = _getDataEva.req_mgr_Approve_user;
                                result.mgr_user_name = Getmrg.EmpFullName;
                                result.mgr_unit_name = Getmrg.UnitGroup;
                                result.mgr_user_rank = Getmrg.Rank;
                            }
                        }
                        if (!string.IsNullOrEmpty(_getDataEva.req_incharge_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.ic_user_no = _getDataEva.req_incharge_Approve_user;
                                result.ic_user_name = Getmrg.EmpFullName;
                                result.ic_unit_name = Getmrg.UnitGroup;
                                result.ic_user_rank = Getmrg.Rank;
                            }
                        }

                        if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                        {
                            result.objtifform.lstQuestion.ForEach(ed =>
                            {
                                var GetAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.TM_Evaluation_Question.Id + "" == ed.id).FirstOrDefault();
                                if (GetAns != null)
                                {

                                    ed.rating = GetAns.trainee_rating != null ? GetAns.trainee_rating.Id + "" : "";
                                }
                            });
                        }

                    }
                    else
                    {
                        return RedirectToAction("EvaluationList", "Evaluation");
                    }
                }
                else
                {
                    return RedirectToAction("EvaluationList", "Evaluation");
                }
            }
            else
            {
                return RedirectToAction("EvaluationList", "Evaluation");
            }

            return View(result);
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult CreateEvaluatuinForm(vEvaluation_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vEvaluation_Return result = new vEvaluation_Return();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {

                    if (string.IsNullOrEmpty(ItemData.mgr_user_no))
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please select Performance Manager.";
                        return Json(new
                        {
                            result
                        });

                    }

                    if (string.IsNullOrEmpty(ItemData.ic_user_no))
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please select In-Charge / Engagement Manager.";
                        return Json(new
                        {
                            result
                        });

                    }
                    //if (ItemData.ic_user_no == ItemData.mgr_user_no)
                    //{
                    //    result.Status = SystemFunction.process_Failed;
                    //    result.Msg = "Error,Duplicate In-Charge.";
                    //    return Json(new
                    //    {
                    //        result
                    //    });
                    //}
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData != null)
                    {
                        var _GetEvaNotComplete = _TM_Trainee_EvaService.FindByMappingIDList(_getData.Id);
                        if (_GetEvaNotComplete != null && _GetEvaNotComplete.Any(a => a.hr_acknowledge + "" != "Y"))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Please wait until your latest evaluation form is approved.";
                            return Json(new
                            {
                                result
                            });
                        }
                        //id null = first time Save data
                        var _getEmpMrg = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.mgr_user_no).FirstOrDefault();
                        var _getEmpInc = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.ic_user_no).FirstOrDefault();
                        if (_getEmpMrg == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,Performance Manager Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        if (_getEmpInc == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,In-Charge / Engagement Manager Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                        var _getTifForm = _TM_Evaluation_FormService.Find(nTFId);
                        if (_getTifForm != null && _getTifForm.TM_Evaluation_Question != null && _getTifForm.TM_Evaluation_Question.Any())
                        {
                            string[] aTIFQID = _getTifForm.TM_Evaluation_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Trainee_Eva_Answer> lstAns = new List<TM_Trainee_Eva_Answer>();

                            if (ItemData.objtifform.lstQuestion != null)
                            {
                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                          from lstQ in _getTifForm.TM_Evaluation_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new HumanCapitalManagement.Models.TIFForm.TM_Evaluation_Question())
                                          select new TM_Trainee_Eva_Answer
                                          {
                                              update_user = CGlobal.UserInfo.UserId,
                                              update_date = dNow,
                                              create_user = CGlobal.UserInfo.UserId,
                                              create_date = dNow,
                                              inchage_comment = "",
                                              active_status = "Y",
                                              TM_Evaluation_Question = lstQ != null ? lstQ : null,
                                              trainee_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),

                                          }).ToList();
                            }
                            lstAns = lstAns.Where(w => w.TM_Evaluation_Question != null).ToList();
                            TM_Trainee_Eva objSave = new TM_Trainee_Eva()
                            {
                                TM_PR_Candidate_Mapping = _getData,
                                trainee_learned = ItemData.comment,
                                trainee_done_well = ItemData.comment2,
                                trainee_developmental = ItemData.comment3,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Trainee_Eva_Answer = lstAns,
                                submit_status = "Y",
                                approve_type = ItemData.type_approve,
                                req_mgr_Approve_user = ItemData.mgr_user_no,
                                req_incharge_Approve_user = ItemData.ic_user_no,
                                TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Bu_Approve),
                            };
                            var sComplect = _TM_Trainee_EvaService.CreateNew(ref objSave);
                            if (sComplect > 0)
                            {
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find(9);
                                if (objSave != null)
                                {
                                    var bSuss = SendSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                }
                                result.Status = SystemFunction.process_Success;
                                //if (_getData != null)
                                //{
                                //    if (_TM_Trainee_Eva_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
                                //    {
                                //        result.Status = SystemFunction.process_Success;
                                //    }
                                //    else
                                //    {
                                //        result.Status = SystemFunction.process_Failed;
                                //        result.Msg = "Error, please try again.";
                                //    }
                                //}
                                //else
                                //{
                                //    result.Status = SystemFunction.process_Success;
                                //}
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
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Candidate Not Found.";
                    }

                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveDraftEvaluation(vEvaluation_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vEvaluation_Return result = new vEvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData != null)
                    {
                        var _GetEvaNotComplete = _TM_Trainee_EvaService.FindByMappingIDList(_getData.Id);
                        if (_GetEvaNotComplete != null && _GetEvaNotComplete.Any(a => a.hr_acknowledge + "" != "Y"))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Please wait until your latest evaluation form is approved.";
                            return Json(new
                            {
                                result
                            });
                        }
                        //id null = first time Save data

                        int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                        var _getTifForm = _TM_Evaluation_FormService.Find(nTFId);
                        if (_getTifForm != null && _getTifForm.TM_Evaluation_Question != null && _getTifForm.TM_Evaluation_Question.Any())
                        {
                            string[] aTIFQID = _getTifForm.TM_Evaluation_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Trainee_Eva_Answer> lstAns = new List<TM_Trainee_Eva_Answer>();
                            if (ItemData.objtifform.lstQuestion != null)
                            {
                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                          from lstQ in _getTifForm.TM_Evaluation_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new HumanCapitalManagement.Models.TIFForm.TM_Evaluation_Question())
                                          select new TM_Trainee_Eva_Answer
                                          {
                                              update_user = CGlobal.UserInfo.UserId,
                                              update_date = dNow,
                                              create_user = CGlobal.UserInfo.UserId,
                                              create_date = dNow,
                                              inchage_comment = "",
                                              active_status = "Y",
                                              TM_Evaluation_Question = lstQ != null ? lstQ : null,
                                              trainee_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                          }).ToList();
                            }
                            lstAns = lstAns.Where(w => w.TM_Evaluation_Question != null).ToList();
                            TM_Trainee_Eva objSave = new TM_Trainee_Eva()
                            {
                                TM_PR_Candidate_Mapping = _getData,
                                trainee_learned = ItemData.comment,
                                trainee_done_well = ItemData.comment2,
                                trainee_developmental = ItemData.comment3,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Trainee_Eva_Answer = lstAns,
                                submit_status = "N",
                                TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Save_Draft),
                                req_mgr_Approve_user = ItemData.mgr_user_no,
                                req_incharge_Approve_user = ItemData.ic_user_no,
                                approve_type = ItemData.type_approve,
                            };
                            var sComplect = _TM_Trainee_EvaService.CreateNew(ref objSave);
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
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, TIF Form Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Candidate Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditEvaluatuinForm(vEvaluation_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vEvaluation_Return result = new vEvaluation_Return();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    if (string.IsNullOrEmpty(ItemData.mgr_user_no))
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please select Performance Manager.";
                        return Json(new
                        {
                            result
                        });

                    }

                    if (string.IsNullOrEmpty(ItemData.ic_user_no))
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please select In-Charge / Engagement Manager.";
                        return Json(new
                        {
                            result
                        });

                    }
                    //if (ItemData.ic_user_no == ItemData.mgr_user_no)
                    //{
                    //    result.Status = SystemFunction.process_Failed;
                    //    result.Msg = "Error,Duplicate In-Charge.";
                    //    return Json(new
                    //    {
                    //        result
                    //    });
                    //}
                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        //id null = first time Save data
                        var _getEmpMrg = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.mgr_user_no).FirstOrDefault();
                        var _getEmpInc = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.ic_user_no).FirstOrDefault();
                        if (_getEmpMrg == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,Performance Manager Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        if (_getEmpInc == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,In-Charge / Engagement Manager Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        var _getMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                        int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                        var _getTifForm = _TM_Evaluation_FormService.Find(nTFId);
                        if (_getTifForm != null && _getTifForm.TM_Evaluation_Question != null && _getTifForm.TM_Evaluation_Question.Any())
                        {
                            string[] aTIFQID = _getTifForm.TM_Evaluation_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Trainee_Eva_Answer> lstAns = new List<TM_Trainee_Eva_Answer>();
                            if (ItemData.objtifform.lstQuestion != null)
                            {
                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                          from lstQ in _getTifForm.TM_Evaluation_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new HumanCapitalManagement.Models.TIFForm.TM_Evaluation_Question())
                                          select new TM_Trainee_Eva_Answer
                                          {
                                              update_user = CGlobal.UserInfo.UserId,
                                              update_date = dNow,
                                              create_user = CGlobal.UserInfo.UserId,
                                              create_date = dNow,
                                              inchage_comment = "",
                                              active_status = "Y",
                                              TM_Evaluation_Question = lstQ != null ? lstQ : null,
                                              trainee_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                              TM_Trainee_Eva = _getData,
                                          }).ToList();
                            }
                            lstAns = lstAns.Where(w => w.TM_Evaluation_Question != null).ToList();

                            _getData.TM_PR_Candidate_Mapping = _getMap;
                            _getData.trainee_learned = ItemData.comment;
                            _getData.trainee_done_well = ItemData.comment2;
                            _getData.trainee_developmental = ItemData.comment3;
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = "Y";
                            _getData.submit_status = "Y";
                            _getData.approve_type = ItemData.type_approve;
                            _getData.req_mgr_Approve_user = ItemData.mgr_user_no;
                            _getData.req_incharge_Approve_user = ItemData.ic_user_no;
                            _getData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Bu_Approve);
                            var sComplect = _TM_Trainee_EvaService.UpdateEva(ref _getData);
                            if (sComplect > 0)
                            {
                                if (_getData != null)
                                {
                                    if (_TM_Trainee_Eva_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
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
                                    result.Status = SystemFunction.process_Success;
                                }
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find(9);
                                if (_getData != null)
                                {
                                    var bSuss = SendSubmit(_getData, Mail1, ref sError, ref mail_to_log);
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
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Evaluation Form Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }

                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveEditEvaluation(vEvaluation_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vEvaluation_Return result = new vEvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {


                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        //id null = first time Save data


                        var _getMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                        int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                        var _getTifForm = _TM_Evaluation_FormService.Find(nTFId);
                        if (_getTifForm != null && _getTifForm.TM_Evaluation_Question != null && _getTifForm.TM_Evaluation_Question.Any())
                        {
                            string[] aTIFQID = _getTifForm.TM_Evaluation_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Trainee_Eva_Answer> lstAns = new List<TM_Trainee_Eva_Answer>();
                            if (ItemData.objtifform.lstQuestion != null)
                            {
                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                          from lstQ in _getTifForm.TM_Evaluation_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new HumanCapitalManagement.Models.TIFForm.TM_Evaluation_Question())
                                          select new TM_Trainee_Eva_Answer
                                          {
                                              update_user = CGlobal.UserInfo.UserId,
                                              update_date = dNow,
                                              create_user = CGlobal.UserInfo.UserId,
                                              create_date = dNow,
                                              inchage_comment = "",
                                              active_status = "Y",
                                              TM_Evaluation_Question = lstQ != null ? lstQ : null,
                                              trainee_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                              TM_Trainee_Eva = _getData,
                                          }).ToList();
                            }
                            lstAns = lstAns.Where(w => w.TM_Evaluation_Question != null).ToList();

                            _getData.TM_PR_Candidate_Mapping = _getMap;
                            _getData.trainee_learned = ItemData.comment;
                            _getData.trainee_done_well = ItemData.comment2;
                            _getData.trainee_developmental = ItemData.comment3;
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = "Y";
                            _getData.submit_status = "N";
                            _getData.approve_type = ItemData.type_approve;
                            _getData.req_mgr_Approve_user = ItemData.mgr_user_no;
                            _getData.req_incharge_Approve_user = ItemData.ic_user_no;
                            _getData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Save_Draft);
                            var sComplect = _TM_Trainee_EvaService.UpdateEva(ref _getData);
                            if (sComplect > 0)
                            {
                                if (_getData != null)
                                {
                                    if (_TM_Trainee_Eva_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
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
                                    result.Status = SystemFunction.process_Success;
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
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Evaluation Form Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult UpdateCandidate(vEvaluation_obj_View ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vEvaluation_Return result = new vEvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    DateTime? dtarget_period = null, dtarget_period_to = null;

                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData != null)
                    {
                        var _getCandidate = _TM_CandidatesService.Find(_getData.TM_Candidates.Id);
                        if (_getCandidate != null)
                        {
                            _getCandidate.candidate_TraineeNumber = ItemData.trainee_no;
                            _getCandidate.candidate_NickName = ItemData.nick_name;
                            if (!string.IsNullOrEmpty(ItemData.target_end))
                            {
                                dtarget_period_to = SystemFunction.ConvertStringToDateTime(ItemData.target_end, "");
                            }
                            if (!string.IsNullOrEmpty(ItemData.target_start))
                            {
                                dtarget_period = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
                            }
                            _getData.trainee_start = dtarget_period;
                            _getData.trainee_end = dtarget_period_to;
                            //id null = first time Save data
                            var sComplect = _TM_PR_Candidate_MappingService.Update(_getData);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;

                            }
                            sComplect = _TM_CandidatesService.Update(_getCandidate);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                            }
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Trainee Not Found.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }
            return Json(new
            {
                result
            });
        }
        #endregion
    }
}