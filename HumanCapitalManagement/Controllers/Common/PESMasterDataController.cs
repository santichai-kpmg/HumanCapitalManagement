using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class PESMasterDataController : Controller
    {
        // GET: PESMasterData
        private PTR_Evaluation_YearService _PTR_Evaluation_YearService;
        private TM_PTR_Eva_StatusService _TM_PTR_Eva_StatusService;
        private TM_Annual_RatingService _TM_Annual_RatingService;
        private TM_Feedback_RatingService _TM_Feedback_RatingService;
        private DivisionService _DivisionService;
        private PES_Nomination_YearService _PES_Nomination_YearService;
        private TM_PES_NMN_StatusService _TM_PES_NMN_StatusService;
        private TM_PES_NMN_SignatureStepService _TM_PES_NMN_SignatureStepService;


        public PESMasterDataController(PTR_Evaluation_YearService PTR_Evaluation_YearService
            , TM_PTR_Eva_StatusService TM_PTR_Eva_StatusService
            , TM_Annual_RatingService TM_Annual_RatingService
            , TM_Feedback_RatingService TM_Feedback_RatingService
            , DivisionService DivisionService
            , PES_Nomination_YearService PES_Nomination_YearService
            , TM_PES_NMN_StatusService TM_PES_NMN_StatusService
            , TM_PES_NMN_SignatureStepService TM_PES_NMN_SignatureStepService
       )
        {
            _PTR_Evaluation_YearService = PTR_Evaluation_YearService;
            _TM_PTR_Eva_StatusService = TM_PTR_Eva_StatusService;
            _TM_Annual_RatingService = TM_Annual_RatingService;
            _TM_Feedback_RatingService = TM_Feedback_RatingService;
            _DivisionService = DivisionService;
            _PES_Nomination_YearService = PES_Nomination_YearService;
            _TM_PES_NMN_StatusService = TM_PES_NMN_StatusService;
            _TM_PES_NMN_SignatureStepService = TM_PES_NMN_SignatureStepService;
        }

        public ActionResult CreateddlFYYear(vSelect item)
        {
            vSelect lstData = new vSelect();
            string[] UserAdmin = WebConfigurationManager.AppSettings["PESSubAdmin"].Split(';');
           
            var lst = _PTR_Evaluation_YearService.GetDataForSelect().ToList();
            if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
            {
                lst = lst.Where(w=> w.evaluation_year.Value.Year == 2020 ).ToList();
            }

            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.Where(w => w.evaluation_year.HasValue).OrderByDescending(o => o.evaluation_year).Select(s => new lstDataSelect { value = s.Id + "", text = s.evaluation_year.Value.Year + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlFYYearNMN(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _PES_Nomination_YearService.GetDataForSelect().ToList();
            string[] UserAdmin = WebConfigurationManager.AppSettings["PESSubAdmin"].Split(';');

           
            if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
            {
                lst = lst.Where(w => w.evaluation_year.Value.Year == 2020).ToList();
            }
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.Where(w => w.evaluation_year.HasValue).OrderByDescending(o => o.evaluation_year).Select(s => new lstDataSelect { value = s.Id + "", text = s.evaluation_year.Value.Year + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlFYYearActiveYear(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _PTR_Evaluation_YearService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                var dNow = DateTime.Now;
                var _getDefault = lst.Where(w => w.evaluation_year.Value.Year == dNow.Year).FirstOrDefault();
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";

                lstData.value = _getDefault != null ? _getDefault.Id + "" : item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.Where(w => w.evaluation_year.HasValue).OrderByDescending(o => o.evaluation_year).Select(s => new lstDataSelect { value = s.Id + "", text = s.evaluation_year.Value.Year + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreatePTREveStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PTR_Eva_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.status_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateNMNStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PES_NMN_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.status_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult CreateNMNSignatureStep(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PES_NMN_SignatureStepService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.Step_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult CreateddlEvaRating(vSelect item)
        {
            List<int> lstStatus = new List<int>();
            lstStatus.Add((int)PESClass.Annual_Rating.Expectations_high);
            lstStatus.Add((int)PESClass.Annual_Rating.Expectations_low);
            vSelect lstData = new vSelect();
            var lst = _TM_Annual_RatingService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lst = lst.Where(w => !lstStatus.Contains(w.Id)).ToList();
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.rating_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlEvaRating2019(vSelect item)
        {
            List<int> lstStatus = new List<int>();
            //lstStatus.Add((int)PESClass.Annual_Rating.Expectations);

            vSelect lstData = new vSelect();
            var lst = _TM_Annual_RatingService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lst = lst.Where(w => !lstStatus.Contains(w.Id)).ToList();
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.rating_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult CreateddlEvaRating2022(vSelect item)
        {
            List<int> lstStatus = new List<int>();
            //lstStatus.Add((int)PESClass.Annual_Rating.Expectations);

            vSelect lstData = new vSelect();
            var lst = _TM_Annual_RatingService.GetDataForSelect22().ToList();
            if (lst != null && lst.Any())
            {
                lst = lst.Where(w => !lstStatus.Contains(w.Id)).ToList();
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.rating_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlFeedbackRating(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Feedback_RatingService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.feedback_rating_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlGroupForFeedback(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _DivisionService.GetForFeedback().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.division_name_en).Select(s => new lstDataSelect { value = s.division_code + "", text = s.division_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult CreateUnitGroup(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _DivisionService.GetDivisionForPES().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.division_name_en).Select(s => new lstDataSelect { value = s.division_code + "", text = s.division_name_en + "" }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateListPtr(vSelect item)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            int[] aStatus = new int[] { 1, 3 };
            string[] aRank = new string[] { "12", "20" };
            vSelect lstData = new vSelect();
            var lst = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status)).ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.EmpFullName).Select(s => new lstDataSelect { value = s.EmpNo + "", text = s.EmpNo + " : " + s.EmpFullName + " : " + s.Rank }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateListPtrCom(vSelect item)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            int[] aStatus = new int[] { 1, 3 };
            string[] aRank = new string[] { "12" };
            vSelect lstData = new vSelect();
            var lst = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status)).ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.EmpFullName).Select(s => new lstDataSelect { value = s.EmpNo + "", text = s.EmpNo + " : " + s.EmpFullName + " : " + s.Rank }).ToList();
            }
            return PartialView("_select", lstData);
        }
    }
}