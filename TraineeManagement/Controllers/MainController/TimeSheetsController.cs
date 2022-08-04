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
    public class TimeSheetsController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        private TM_CandidatesService _TM_CandidatesService;

        public TimeSheetsController(TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
             , TM_CandidatesService TM_CandidatesService
                   )
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_CandidatesService = TM_CandidatesService;
        }
        public ActionResult TimeSheetView(string qryStr)
        {
            return View();
        }
        public class Calandar_Even
        {
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string allDay { get; set; }
            //public string url { get; set; }
            public string backgroundColor { get; set; }
            //public string borderColor { get; set; }
        }


        [HttpPost]
        public ActionResult GetEvenOfCalendar(string ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            List<Calandar_Even> returnresult = new List<Calandar_Even>();
            try
            {
                returnresult.Add(new Calandar_Even() {
                    title ="test sent",
                    //start = DateTime.Now.ToString("yyyy,MM,dd"),
                    start = "2019-03-16",
                    end = "2019-03-16",
                    allDay = "true",
                    backgroundColor = "#00c0ef", 
                    //borderColor = "#00c0ef"
                });

                }
            catch (Exception ex)
            {

            }
            return Json(new
            {
                returnresult
            });

        }


        [HttpPost]
        public ActionResult SaveEvenOfCalendar(string ItemData)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            result.Status = SystemFunction.process_Failed;
            result.Msg = "Error, Evaluation Not Found.";

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
    }
}