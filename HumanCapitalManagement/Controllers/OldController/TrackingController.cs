using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.OldTable;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.OldController
{
    public class TrackingController : BaseController
    {

        private StoreDb db = new StoreDb();
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        // GET: Tracking
        public ActionResult Index()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            vTracking result = new vTracking();
            return View(result);
        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadTracking(CSearchTracking SearchItem)
        {
            vTracking_Result result = new vTracking_Result();
            List<vTrackingData> lstData_resutl = new List<vTrackingData>();
            DateTime? dStart = null, dTo = null;
            //if (!string.IsNullOrEmpty(SearchItem.date_start))
            //{
            //    dStart = SystemFunction.ConvertStringToDateTime(SearchItem.date_start, "", "MM/dd/yyyy");
            //}
            //if (!string.IsNullOrEmpty(SearchItem.date_to))
            //{
            //    dTo = SystemFunction.ConvertStringToDateTime(SearchItem.date_to, "", "MM/dd/yyyy");
            //}

            IQueryable<PRForm> _getPrForm = db.PRForms.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(SearchItem.pr_status))
            {
                _getPrForm = _getPrForm.Where(w => w.Status.Id == SystemFunction.GetIntNullToZero(SearchItem.pr_status));
            }
            if (!string.IsNullOrEmpty(SearchItem.division))
            {
                _getPrForm = _getPrForm.Where(w => w.Division == SearchItem.division);
            }
            var lstData = _getPrForm.ToList();
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
                                  select new vTrackingData
                                  {
                                      View = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + lstAD.Id + "" + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      //  request_type = lstAD.RequestType.RequestTypeDesc
                                      position = lstAD.Positions.Select(s => s.PositionTitle).FirstOrDefault(),
                                      request_type = lstAD.RequestType.RequestTypeDesc,
                                      rank = lstAD.Positions.Select(s => s.Rank).FirstOrDefault(),
                                      pr_status = lstAD.Status.StatusDesc,
                                      hc = lstAD.Positions.Select(s => s.Headcount + "").FirstOrDefault(),
                                      remark = lstAD.Remark.StringRemark(35),
                                  }
                          ).ToList();
                //lstATS = lstData.Select(s => new vListExportxlxs
                //{
                //    Id = s.Id + "",
                //    IdEncrypt = StaffPersonnalAdvanceFunc.Encrypt(s.Id + ""),
                //    amount = s.amount + "",
                //    code = s.ListTempTransaction.code + "",
                //    company = dbHr.tbMaster_Company.FirstOrDefault(w => w.ID == s.AdvanceTransaction.TempTransaction.id_company) != null ? dbHr.tbMaster_Company.FirstOrDefault(w => w.ID == s.AdvanceTransaction.TempTransaction.id_company).LocalCompCode : "",
                //    posting_date = s.ListTempTransaction.posting_date.HasValue ? s.ListTempTransaction.posting_date.Value.ToString("MM/dd/yyyy") : "",

                //}).ToList();
                result.lstData = lstData_resutl.ToList();
                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Success;
            }

            return Json(new { result });
        }

        #endregion
    }
}