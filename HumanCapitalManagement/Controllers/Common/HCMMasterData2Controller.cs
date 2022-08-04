using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.eGreetingsService;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class HCMMasterData2Controller : Controller
    {
        private TM_Mass_Question_TypeService _TM_Mass_Question_TypeService;
        private TM_MassTIF_StatusService _TM_MassTIF_StatusService;
        private TM_PR_StatusService _TM_PR_StatusService;
        private TM_TraineeEva_StatusService _TM_TraineeEva_StatusService;
        private TM_Additional_InformationService _TM_Additional_InformationService;
        private TM_MiniHeart_PeroidService _TM_MiniHeart_PeroidService;
        private TM_MiniHeart_Peroid2021Service _TM_MiniHeart_Peroid2021Service;
        private TM_eGreetings_PeroidService _TM_eGreetings_PeroidService;
        private TM_Trainee_HiringRatingService _TM_Trainee_HiringRatingService;
        public HCMMasterData2Controller(TM_Mass_Question_TypeService TM_Mass_Question_TypeService
            , TM_MassTIF_StatusService TM_MassTIF_StatusService
            , TM_PR_StatusService TM_PR_StatusService
            , TM_TraineeEva_StatusService TM_TraineeEva_StatusService
            , TM_Additional_InformationService TM_Additional_InformationService
            , TM_MiniHeart_PeroidService TM_MiniHeart_PeroidService
            , TM_MiniHeart_Peroid2021Service TM_MiniHeart_Peroid2021Service
            , TM_eGreetings_PeroidService TM_eGreetings_PeroidService
            , TM_Trainee_HiringRatingService TM_Trainee_HiringRatingService
       )
        {
            _TM_Mass_Question_TypeService = TM_Mass_Question_TypeService;
            _TM_MassTIF_StatusService = TM_MassTIF_StatusService;
            _TM_PR_StatusService = TM_PR_StatusService;
            _TM_TraineeEva_StatusService = TM_TraineeEva_StatusService;
            _TM_Additional_InformationService = TM_Additional_InformationService;
            _TM_MiniHeart_PeroidService = TM_MiniHeart_PeroidService;
            _TM_MiniHeart_Peroid2021Service = TM_MiniHeart_Peroid2021Service;
            _TM_eGreetings_PeroidService = TM_eGreetings_PeroidService;
            _TM_Trainee_HiringRatingService = TM_Trainee_HiringRatingService;
        }
        // GET: HCMMasterData2
        public ActionResult CreateddlMassType(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Mass_Question_TypeService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.question_type_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlMassTIFStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_MassTIF_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.masstif_status_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult CreateddlPRStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_PR_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.status_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlTraineeEvaStatus(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_TraineeEva_StatusService.GetDataForSelect().ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.eva_status_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlAdInfo(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Additional_InformationService.GetActiveTIFForm();
            if (lst != null && lst.TM_Additional_Questions != null && lst.TM_Additional_Questions.Any(a => a.active_status == "Y"))
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.TM_Additional_Questions.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.question }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult CreateddlRankFromHRIS(vSelect item)
        {
            New_HRISEntities dbHr = new New_HRISEntities();
            vSelect lstData = new vSelect();
            var lst = dbHr.tbMaster_Rank.ToList();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Piority).Select(s => new lstDataSelect { value = s.ID + "", text = s.Name }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult GetPeroidMiniHeartStart(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_MiniHeart_PeroidService.GetDataForSelectAll();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Start_Peroid).Select(s => new lstDataSelect { value = s.Id + "", text = s.Start_Peroid.Value.ToString("dd MMM yyyy") }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult GetPeroidMiniHeartEnd(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_MiniHeart_PeroidService.GetDataForSelectAll();
            List<vMiniHeart_Peroid_obj> obj = new List<vMiniHeart_Peroid_obj>();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Start_Peroid).Select(s => new lstDataSelect { value = s.Id + "", text = s.End_Peroid.Value.ToString("dd MMM yyyy") }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult GetPeroidMiniHeartStart2021(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_MiniHeart_Peroid2021Service.GetDataForSelectAll();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Start_Peroid).Select(s => new lstDataSelect { value = s.Id + "", text = s.Start_Peroid.Value.ToString("dd MMM yyyy") }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult GetPeroidMiniHeartEnd2021(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_MiniHeart_Peroid2021Service.GetDataForSelectAll();
            List<vMiniHeart_Peroid_obj> obj = new List<vMiniHeart_Peroid_obj>();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Start_Peroid).Select(s => new lstDataSelect { value = s.Id + "", text = s.End_Peroid.Value.ToString("dd MMM yyyy") }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult GetPeroideGreetingsStart(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_eGreetings_PeroidService.GetDataForSelectAll();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Start_Peroid).Select(s => new lstDataSelect { value = s.Id + "", text = s.Start_Peroid.Value.ToString("dd MMM yyyy") }).ToList();
            }
            return PartialView("_select", lstData);
        }
        public ActionResult GetPeroideGreetingsEnd(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_eGreetings_PeroidService.GetDataForSelectAll();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.Start_Peroid).Select(s => new lstDataSelect { value = s.Id + "", text = s.End_Peroid.Value.ToString("dd MMM yyyy") }).ToList();
            }
            return PartialView("_select", lstData);
        }

        public ActionResult HiringRating(vSelect item)
        {

            vSelect lstData = new vSelect();
            var lst = _TM_Trainee_HiringRatingService.GetDataForall();
            List<TM_Trainee_HiringRating> obj = new List<TM_Trainee_HiringRating>();
            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderBy(o => o.piority).Select(s => new lstDataSelect { value = s.Id + "", text = s.Trainee_HiringRating_name_en }).ToList();
            }
            return PartialView("_select", lstData);
        }
    }

}