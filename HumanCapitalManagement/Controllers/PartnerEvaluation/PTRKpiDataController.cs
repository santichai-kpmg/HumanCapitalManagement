using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.PESVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class PTRKpiDataController : BaseController
    {
        private PTR_Evaluation_YearService _PTR_Evaluation_YearService;
        private PTR_EvaluationService _PTR_EvaluationService;
        private TM_PTR_Eva_ApproveStepService _TM_PTR_Eva_ApproveStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PTR_Evaluation_KPIsService _PTR_Evaluation_KPIsService;
        private TM_PTR_Eva_StatusService _TM_PTR_Eva_StatusService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public PTRKpiDataController(PTR_Evaluation_YearService PTR_Evaluation_YearService
    , PTR_EvaluationService PTR_EvaluationService
    , TM_PTR_Eva_ApproveStepService TM_PTR_Eva_ApproveStepService
    , TM_KPIs_BaseService TM_KPIs_BaseService
    , PTR_Evaluation_KPIsService PTR_Evaluation_KPIsService
    , TM_PTR_Eva_StatusService TM_PTR_Eva_StatusService)
        {
            _PTR_Evaluation_YearService = PTR_Evaluation_YearService;
            _PTR_EvaluationService = PTR_EvaluationService;
            _TM_PTR_Eva_ApproveStepService = TM_PTR_Eva_ApproveStepService;
            _TM_KPIs_BaseService = TM_KPIs_BaseService;
            _PTR_Evaluation_KPIsService = PTR_Evaluation_KPIsService;
            _TM_PTR_Eva_StatusService = TM_PTR_Eva_StatusService;
        }
        // GET: PTRKpiData
        public ActionResult PTRKpiDataList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTRMailBox result = new vPTRMailBox();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPTRMailBox SearchItem = (CSearchPTRMailBox)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPTRMailBox)));
                var lstData = _PTR_Evaluation_YearService.GetCRank(
               SearchItem.pr_status,SearchItem.pr_status);
                result.pr_status = SearchItem.pr_status;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vPTRMailBox_obj
                                      {
                                          name_en = lstAD.evaluation_year.HasValue ? lstAD.evaluation_year.Value.DateTimeWithTimebyCulture() : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult PTRKpiDataCreate()
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            return View();
        }
        public ActionResult PTRKpiDataEdit(string id)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            return View();
        }
    }
}