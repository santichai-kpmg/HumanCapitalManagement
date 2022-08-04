using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.PESVM;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.PESClass;

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class PTRMailBoxController : BaseController
    {
        private PTR_Evaluation_YearService _PTR_Evaluation_YearService;
        private PTR_EvaluationService _PTR_EvaluationService;
        private TM_PTR_Eva_ApproveStepService _TM_PTR_Eva_ApproveStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PTR_Evaluation_KPIsService _PTR_Evaluation_KPIsService;
        private TM_PTR_Eva_StatusService _TM_PTR_Eva_StatusService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public PTRMailBoxController(PTR_Evaluation_YearService PTR_Evaluation_YearService
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

        // GET: PTRMailBox
        public ActionResult PTRMailBoxList(string qryStr)
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
               SearchItem.pr_status,
           SearchItem.pr_status);
                result.pr_status = SearchItem.pr_status;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vPTRMailBox_obj
                                      {
                                          name_en = lstAD.evaluation_year.HasValue ? lstAD.evaluation_year.Value.DateTimeWithTimebyCulture("yyyy") : "",
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
        public ActionResult PTRMailBoxCreate()
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTRMailBox_obj_save result = new vPTRMailBox_obj_save();
            result.Id = 0;
            return View(result);
        }
        public ActionResult PTRMailBoxEdit(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vPTRMailBox_obj_save result = new vPTRMailBox_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = qryStr;
                        result.file_name = _getData.sfile_oldname + "";
                        result.actual_file_name = _getData.actual_sfile_oldname + "";
                        int[] aStatus = new int[] { 1, 3 };
                        string[] aRank = new string[] { "12", "20", "32" };
                        string[] empNO = new string[] { };

                        if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                        {
                            empNO = _getData.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();

                            result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                 from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                 select new vPartnerKPIs_obj
                                                 {
                                                     lstApproval = PESFunc.GetApprovalPESName(lstAD.PTR_Evaluation_Approve.Where(w => w.active_status == "Y").ToList()),
                                                     codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                     Edit = "",
                                                     emp_name = lstEmpReq.EmpFullName,
                                                     emp_comp = lstEmpReq.CompanyCode,
                                                     emp_group = lstEmpReq.UnitGroupName,
                                                     emp_code = lstEmpReq.EmpNo,
                                                     emp_rank = lstEmpReq.RankCode,
                                                     //        Billing = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                     //        (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                     //        (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        Collections = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                     //        (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                     //       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        Fee_Managed = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                     //        (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        Lock_up = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                     //       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                     //       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //        //----- Group

                                                     //        Billing_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                     //(s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     //        Chargeability_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                     //        (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     //        Collections_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                     //       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     //        Contribution_margin_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                     //        (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     //        Fee_Managed_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                     //       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     //        Lock_up_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                     //      (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     //        Recovery_rate_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                     //       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",


                                                 }).ToList();

                            result.lstActualData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                    from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPartnerKPIs_obj
                                                    {

                                                        codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                        Edit = "",
                                                        emp_name = lstEmpReq.EmpFullName,
                                                        emp_comp = lstEmpReq.CompanyCode,
                                                        emp_group = lstEmpReq.UnitGroupName,
                                                        emp_code = lstEmpReq.EmpNo,
                                                        emp_rank = lstEmpReq.RankCode,
                                                        //                        Billing = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                        //                         HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                        //                        HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        Collections = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                        //                        HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                        //                       HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        Fee_Managed = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                        //                        HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        Lock_up = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                        //                       HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                        //                       HCMFunc.DataDecryptPES(s.actual)).FirstOrDefault() + "" : "",

                                                        //                        //group

                                                        //                        Billing_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                        //HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",

                                                        //                        Chargeability_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                        //                        HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",

                                                        //                        Collections_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                        //                        HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",

                                                        //                        Contribution_margin_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                        //                       HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",

                                                        //                        Fee_Managed_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                        //                        HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",

                                                        //                        Lock_up_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                        //                       HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",

                                                        //                        Recovery_rate_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                        //                       HCMFunc.DataDecryptPES(s.group_actual)).FirstOrDefault() + "" : "",






                                                    }).ToList();
                        }
                        result.action_date = _getData.evaluation_year.HasValue ? _getData.evaluation_year.Value.DateTimeWithTimebyCulture() : "";
                        string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        string sSession = "ImportFileKPI" + unixTimestamps;
                        result.Session = sSession;

                        File_Upload_PTRMailBox objFile = new File_Upload_PTRMailBox();
                        if (_getData.sfile64 != null)
                        {
                            objFile.sfile64 = _getData.sfile64;
                            objFile.sfile_name = _getData.sfile_oldname;
                            objFile.sfileType = _getData.sfileType;
                            List<vPartnerKPIs_FileTemp> lstFile = new List<vPartnerKPIs_FileTemp>();
                            Stream stream = new MemoryStream(_getData.sfile64);
                            try
                            {
                                using (var package = new ExcelPackage(stream))
                                {
                                    var ws = package.Workbook.Worksheets.First();
                                    DataTable tbl = new DataTable();
                                    // List<string> lstHead = new List<string>();
                                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                    {
                                        tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                        //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                    }
                                    var startRow = true ? 2 : 1;
                                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                        DataRow row = tbl.Rows.Add();
                                        foreach (var cell in wsRow)
                                        {
                                            row[cell.Start.Column - 1] = cell.Text;
                                        }
                                    }
                                    if (tbl != null)
                                    {

                                        if (tbl.Columns.Contains("employee no") && tbl.Columns.Contains("fee managed")
                                           && tbl.Columns.Contains("contribution margin%") && tbl.Columns.Contains("recovery %")
                                           && tbl.Columns.Contains("lockup days") && tbl.Columns.Contains("chargeability %")
                                           && tbl.Columns.Contains("billing") && tbl.Columns.Contains("collection")
                                           )
                                        {
                                            int i = 1;
                                            lstFile = tbl.AsEnumerable().Where(w => (w.Field<string>("employee no") + "") != "").Select(s =>
                                            new vPartnerKPIs_FileTemp
                                            {
                                                emp_code = s.Field<string>("employee no"),
                                                Billing = s.Field<string>("billing"),
                                                Chargeability = s.Field<string>("chargeability %"),
                                                Collections = s.Field<string>("collection"),
                                                Contribution_margin = s.Field<string>("contribution margin%"),
                                                Fee_Managed = s.Field<string>("fee managed"),
                                                Lock_up = s.Field<string>("lockup days"),
                                                Recovery_rate = s.Field<string>("recovery %"),
                                            }).ToList();

                                            objFile.lstPartnerKPIs = lstFile;

                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                            }

                        }
                        if (_getData.actual_sfile64 != null)
                        {
                            objFile.Actualsfile64 = _getData.actual_sfile64;
                            objFile.Actualsfile_name = _getData.actual_sfile_oldname;
                            objFile.ActualsfileType = _getData.actual_sfileType;
                            List<vPartnerKPIs_FileTemp> lstFile = new List<vPartnerKPIs_FileTemp>();
                            Stream stream = new MemoryStream(_getData.actual_sfile64);
                            try
                            {
                                using (var package = new ExcelPackage(stream))
                                {
                                    var ws = package.Workbook.Worksheets.First();
                                    DataTable tbl = new DataTable();
                                    // List<string> lstHead = new List<string>();
                                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                    {
                                        tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                        //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                    }
                                    var startRow = true ? 2 : 1;
                                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                        DataRow row = tbl.Rows.Add();
                                        foreach (var cell in wsRow)
                                        {
                                            row[cell.Start.Column - 1] = cell.Text;
                                        }
                                    }
                                    if (tbl != null)
                                    {

                                        if (tbl.Columns.Contains("employee no") && tbl.Columns.Contains("fee managed")
                                           && tbl.Columns.Contains("contribution margin%") && tbl.Columns.Contains("recovery %")
                                           && tbl.Columns.Contains("lockup days") && tbl.Columns.Contains("chargeability %")
                                           && tbl.Columns.Contains("billing") && tbl.Columns.Contains("collection")
                                           )
                                        {
                                            int i = 1;
                                            lstFile = tbl.AsEnumerable().Where(w => (w.Field<string>("employee no") + "") != "").Select(s =>
                                            new vPartnerKPIs_FileTemp
                                            {
                                                emp_code = s.Field<string>("employee no"),
                                                Billing = (SystemFunction.GetNumberNullToZero(s.Field<string>("billing")) * 1000000) + "",
                                                Chargeability = s.Field<string>("chargeability %"),
                                                Collections = (SystemFunction.GetNumberNullToZero(s.Field<string>("collection")) * 1000000) + "",
                                                Contribution_margin = s.Field<string>("contribution margin%"),
                                                Fee_Managed = (SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed")) * 1000000) + "",
                                                Lock_up = s.Field<string>("lockup days"),
                                                Recovery_rate = s.Field<string>("recovery %"),
                                            }).ToList();

                                            objFile.lstActualPTR = lstFile;

                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                            }

                        }
                        Session[sSession] = objFile;
                        result.lstOldData = new List<vPartner_obj>();
                        var _getPartner = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status) && !empNO.Contains(w.EmpNo)).ToList();
                        if (_getPartner.Any())
                        {
                            result.lstOldData = _getPartner.Select(s => new vPartner_obj
                            {
                                codeEncrypt = HCMFunc.EncryptPES(s.EmpNo + ""),
                                Edit = "",
                                emp_name = s.EmpFullName,
                                emp_comp = s.CompanyCode,
                                emp_group = s.UnitGroupName,
                                emp_code = s.EmpNo,
                                emp_rank = s.RankCode,
                            }).ToList();

                            //result.lstOldData = (from lstAD in _getPartner
                            //                     select new vPartner_obj
                            //                     {
                            //                         //codeEncrypt = HCMFunc.EncryptPES(lstAD.EmpNo + ""),
                            //                         Edit = "",
                            //                         emp_name = lstAD.EmpFullName,
                            //                         emp_comp = lstAD.CompanyCode,
                            //                         emp_group = lstAD.UnitGroupName,
                            //                         emp_code = lstAD.EmpNo,
                            //                         emp_rank = lstAD.RankCode,
                            //                     }).ToList();
                        }
                    }
                }
            }
            return View(result);

            #endregion
        }
        public ActionResult CreatExcelPTRMailBox(string id)
        {
            var sCheck = pesCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            Stream stream = new MemoryStream();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(id + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null)
                    {
                        using (var xlPackage = new ExcelPackage())
                        {
                            var ws = xlPackage.Workbook.Worksheets.Add("Sheet1");
                            ws.SetValue(1, 1, "Updated this cell");
                            return File(_getData.sfile64, "application/excel", _getData.sfile_oldname + ".xlsx");
                        }
                        //stream = new MemoryStream(_getData.sfile64);
                        //// Get content of your Excel file
                        ////var stream = wBook.WriteXLSX(); // Return a MemoryStream (Using DTG.Spreadsheet)

                        //stream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            return JavaScript("CloseTab();");
        }
        public ActionResult CreatExcelActualPTRMailBox(string id)
        {
            var sCheck = pesCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            Stream stream = new MemoryStream();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(id + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null)
                    {
                        using (var xlPackage = new ExcelPackage())
                        {
                            var ws = xlPackage.Workbook.Worksheets.Add("Sheet1");
                            ws.SetValue(1, 1, "Updated this cell");
                            return File(_getData.actual_sfile64, "application/excel", _getData.actual_sfile_oldname + ".xlsx");
                        }
                        //stream = new MemoryStream(_getData.sfile64);
                        //// Get content of your Excel file
                        ////var stream = wBook.WriteXLSX(); // Return a MemoryStream (Using DTG.Spreadsheet)

                        //stream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            return JavaScript("CloseTab();");
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPTRMailBoxList(CSearchPTRMailBox SearchItem)
        {
            vPTRMailBox_Return result = new vPTRMailBox_Return();
            List<vPTRMailBox_obj> lstData_resutl = new List<vPTRMailBox_obj>();
            var lstData = _PTR_Evaluation_YearService.GetCRank(
            "",
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
                                  select new vPTRMailBox_obj
                                  {
                                      name_en = lstAD.evaluation_year.HasValue ? lstAD.evaluation_year.Value.DateTimeWithTimebyCulture("yyyy") : "",
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();

                string[] UserAdmin = WebConfigurationManager.AppSettings["PESSubAdmin"].Split(';');
                if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
                { lstData_resutl = lstData_resutl.Where(w => w.name_en == "2020").ToList(); }

                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreatePlan(vPTRMailBox_obj_save ItemData)
        {
            vPTRMailBox_Return result = new vPTRMailBox_Return();

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
                if (!string.IsNullOrEmpty(ItemData.action_date))
                {
                    DateTime? dAction = null;

                    if (!string.IsNullOrEmpty(ItemData.action_date))
                    {
                        dAction = SystemFunction.ConvertStringToDateTime("01-Jan-" + (ItemData.action_date).Trim(), "");
                    }
                    PTR_Evaluation_Year objSave = new PTR_Evaluation_Year()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        evaluation_year = dAction,

                    };
                    if (_PTR_Evaluation_YearService.CanSave(objSave))
                    {
                        var sComplect = _PTR_Evaluation_YearService.CreateNew(ref objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            result.IdEncrypt = HCMFunc.EncryptPES(objSave.Id + "");

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Duplicate;
                        result.Msg = "Duplicate Type name.";
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
        public ActionResult AddEvaluationPlan(vPTRMailBox_Return ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vAddEvaluationPlan_Return result = new vAddEvaluationPlan_Return();
            List<vPartner_obj> lstNewData = new List<vPartner_obj>();
            if (ItemData != null && ItemData.lstData != null && ItemData.lstData.Any())
            {
                int[] aStatus = new int[] { 1, 3 };
                string[] aRank = new string[] { "12", "20", "32" };
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                DateTime dNow = DateTime.Now;



                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Draft_Plan);
                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }
                    var _GetFY = _PTR_Evaluation_YearService.Find(nId);
                    var _GetApproveStep = _TM_PTR_Eva_ApproveStepService.GetDataForSelect();
                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                    if (_GetFY != null && _GetApproveStep.Any())
                    {
                        List<int> lstPlan = new List<int>();
                        //lstPlan.Add((int)StepApprovePlan.Self);
                        lstPlan.Add((int)StepApprovePlan.pHead);
                        lstPlan.Add((int)StepApprovePlan.gHead);
                        lstPlan.Add((int)StepApprovePlan.Ceo);
                        //_GetApproveStep = _GetApproveStep.Where(w => lstPlan.Contains(w.Id)).ToList();
                        foreach (var item in ItemData.lstData)
                        {
                            string user_no = HCMFunc.DecryptPES(item.IdEncrypt + "");
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status) && w.EmpNo == user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {

                                List<PTR_Evaluation_Approve> lstApprove = new List<PTR_Evaluation_Approve>();
                                lstApprove.Add(new PTR_Evaluation_Approve
                                {
                                    active_status = "Y",
                                    Req_Approve_user = _checkActiv.EmpNo + "",
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApprovePlan.Self).FirstOrDefault(),

                                });
                                #region Add Approve Step 
                                var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == _checkActiv.EmpNo && w.RankID == 0).FirstOrDefault();
                                var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == _checkActiv.EmpNo).FirstOrDefault();
                                var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == _checkActiv.EmpNo).FirstOrDefault();
                                foreach (var app in _GetApproveStep.Where(w => lstPlan.Contains(w.Id)).OrderBy(o => o.seq))
                                {
                                    if (app.Id == (int)StepApprovePlan.gHead)
                                    {

                                        if (CheckGroupH == null && CheckPool == null && CheckCEO == null)
                                        {
                                            var _getGroupHead = dbHr.tbMaster_UnitGroupHead.Where(w => w.UnitGroupID == _checkActiv.UnitGroupID && w.RankID == 0).FirstOrDefault();
                                            if (_getGroupHead != null)
                                            {
                                                if (_checkActiv.PoolID != 2)
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "Y",
                                                        Req_Approve_user = _getGroupHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApprovePlan.gHead).FirstOrDefault(),
                                                    });
                                                }
                                                else
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "N",
                                                        Req_Approve_user = _getGroupHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApprovePlan.gHead).FirstOrDefault(),
                                                    });
                                                }



                                            }

                                        }
                                    }
                                    else if (app.Id == (int)StepApprovePlan.pHead)
                                    {

                                        if (CheckPool == null && CheckCEO == null)
                                        {
                                            var _getPoolHead = dbHr.tbMaster_PoolHead.Where(w => w.PoolID == _checkActiv.PoolID).FirstOrDefault();
                                            if (_getPoolHead != null)
                                            {
                                                if (_checkActiv.PoolID != 3)
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "Y",
                                                        Req_Approve_user = _getPoolHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApprovePlan.pHead).FirstOrDefault(),
                                                    });
                                                }
                                                else
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "Y",
                                                        Req_Approve_user = PESClass.KhunSukit + "",// _getPoolHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApprovePlan.pHead).FirstOrDefault(),
                                                    });
                                                }

                                            }

                                        }
                                    }
                                    else if (app.Id == (int)StepApprovePlan.Ceo)
                                    {

                                        if (CheckCEO == null)
                                        {
                                            var _getCEOHead = dbHr.tbMaster_CompanyHead.Where(w => w.tbMaster_Company.LocalCompCode == _checkActiv.CompanyCode).FirstOrDefault();
                                            if (_getCEOHead != null)
                                            {
                                                lstApprove.Add(new PTR_Evaluation_Approve
                                                {
                                                    active_status = "Y",
                                                    Req_Approve_user = PESClass.Deputy_Ceo + "",// _getCEOHead.EmployeeNo + "",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApprovePlan.Ceo).FirstOrDefault(),
                                                });
                                            }
                                        }
                                    }

                                }
                                #endregion
                                List<PTR_Evaluation_KPIs> lstKpis = new List<PTR_Evaluation_KPIs>();

                                if (_GetKPIs.Any())
                                {
                                    File_Upload_PTRMailBox objFile = new File_Upload_PTRMailBox();
                                    if (Session[ItemData.Session] != null)
                                    {
                                        objFile = Session[ItemData.Session] as File_Upload_PTRMailBox;
                                    }

                                    foreach (var kpi in _GetKPIs.OrderBy(o => o.seq))
                                    {
                                        vPartnerKPIs_FileTemp _detExcel = null;
                                        if (objFile != null && objFile.lstPartnerKPIs != null && objFile.lstPartnerKPIs.Any())
                                        {
                                            _detExcel = objFile.lstPartnerKPIs.Where(w => w.emp_code == _checkActiv.EmpNo).FirstOrDefault();
                                        }

                                        if (kpi.Id == (int)PESClass.KPIsBase.Fee_Managed && _detExcel != null)
                                        {
                                            lstKpis.Add(new PTR_Evaluation_KPIs
                                            {
                                                active_status = "Y",
                                                seq = kpi.seq,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                target_max = HCMFunc.DataEncryptPES(_detExcel.Fee_Managed),
                                                //   target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",
                                                TM_KPIs_Base = kpi,
                                                group_target_max = HCMFunc.DataEncryptPES(_detExcel.Fee_Managed),

                                            });
                                            //  _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Fee_Managed);
                                        }
                                        //else if (kpi.Id == (int)PESClass.KPIsBase.Lock_up && _detExcel != null)
                                        //{
                                        //    lstKpis.Add(new PTR_Evaluation_KPIs
                                        //    {
                                        //        active_status = "Y",
                                        //        seq = kpi.seq,
                                        //        update_user = CGlobal.UserInfo.UserId,
                                        //        update_date = dNow,
                                        //        create_user = CGlobal.UserInfo.UserId,
                                        //        create_date = dNow,
                                        //        //target_max = HCMFunc.DataEncryptPES(_detExcel.Lock_up),
                                        //        target_max = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",
                                        //        TM_KPIs_Base = kpi,

                                        //    });
                                        //    //  _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Lock_up);
                                        //}
                                        else if (kpi.Id == (int)PESClass.KPIsBase.Billing && _detExcel != null)
                                        {
                                            lstKpis.Add(new PTR_Evaluation_KPIs
                                            {
                                                active_status = "Y",
                                                seq = kpi.seq,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                target_max = HCMFunc.DataEncryptPES(_detExcel.Billing),
                                                target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",
                                                TM_KPIs_Base = kpi,
                                                group_target_max = HCMFunc.DataEncryptPES(_detExcel.Billing),
                                                group_target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",

                                            });
                                            // _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Billing);
                                        }
                                        else if (kpi.Id == (int)PESClass.KPIsBase.Collections && _detExcel != null)
                                        {
                                            lstKpis.Add(new PTR_Evaluation_KPIs
                                            {
                                                active_status = "Y",
                                                seq = kpi.seq,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                target_max = HCMFunc.DataEncryptPES(_detExcel.Collections),
                                                target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",
                                                TM_KPIs_Base = kpi,
                                                group_target_max = HCMFunc.DataEncryptPES(_detExcel.Collections),
                                                group_target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",

                                            });
                                            //  _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Collections);
                                        }
                                        else
                                        {
                                            lstKpis.Add(new PTR_Evaluation_KPIs
                                            {
                                                active_status = "Y",
                                                seq = kpi.seq,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                target_max = HCMFunc.DataEncryptPES(kpi.base_max + ""),
                                                target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",
                                                TM_KPIs_Base = kpi,
                                                group_target_max = HCMFunc.DataEncryptPES(kpi.base_max + ""),
                                                group_target = kpi.base_min.HasValue ? HCMFunc.DataEncryptPES(kpi.base_min + "") : "",
                                            });
                                        }
                                    }
                                }

                                PTR_Evaluation objSave = new PTR_Evaluation()
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    PTR_Evaluation_Year = _GetFY,
                                    user_id = _checkActiv.UserID,
                                    user_no = _checkActiv.EmpNo,
                                    PTR_Evaluation_Approve = lstApprove.ToList(),
                                    PTR_Evaluation_KPIs = lstKpis.Any() ? lstKpis.ToList() : null,
                                    TM_PTR_Eva_Status = _GetStatus
                                };
                                if (_PTR_EvaluationService.CanSave(objSave))
                                {
                                    var sComplect = _PTR_EvaluationService.CreateNewOrUpdate(objSave);
                                    if (sComplect > 0)
                                    {
                                        var _getUpdateMail = _PTR_Evaluation_YearService.Find(nId);
                                        string[] empNO = new string[] { };
                                        if (_getUpdateMail != null && _getUpdateMail.PTR_Evaluation.Any(a => a.active_status == "Y"))
                                        {
                                            empNO = _getUpdateMail.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();

                                            result.lstNewData = (from lstAD in _getUpdateMail.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                                 from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                 select new vPartnerKPIs_obj
                                                                 {
                                                                     codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                                     Edit = "",
                                                                     emp_name = lstEmpReq.EmpFullName,
                                                                     emp_comp = lstEmpReq.CompanyCode,
                                                                     emp_group = lstEmpReq.UnitGroupName,
                                                                     emp_code = lstEmpReq.EmpNo,
                                                                     emp_rank = lstEmpReq.RankCode,
                                                                     Billing = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     Collections = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     Fee_Managed = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     Lock_up = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                                   (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                     //----- Group

                                                                     Billing_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                             (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                     Chargeability_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                     Collections_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                     Contribution_margin_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                     Fee_Managed_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                     Lock_up_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                                   (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                     Recovery_rate_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",

                                                                 }).ToList();

                                            result.lstOldData = new List<vPartner_obj>();
                                            var _getPartner = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status) && !empNO.Contains(w.EmpNo)).ToList();
                                            if (_getPartner.Any())
                                            {
                                                result.lstOldData = _getPartner.Select(s => new vPartner_obj
                                                {
                                                    codeEncrypt = HCMFunc.EncryptPES(s.EmpNo + ""),
                                                    Edit = "",
                                                    emp_name = s.EmpFullName,
                                                    emp_comp = s.CompanyCode,
                                                    emp_group = s.UnitGroupName,
                                                    emp_code = s.EmpNo,
                                                    emp_rank = s.RankCode,
                                                }).ToList();
                                            }
                                        }

                                    }
                                    else
                                    {
                                        //result.Status = SystemFunction.process_Failed;
                                        //result.Msg = "Error, please try again.";
                                    }

                                }
                                else
                                {
                                    //result.Status = SystemFunction.process_Duplicate;
                                    //result.Msg = "Duplicate Type name.";
                                }
                                result.Status = SystemFunction.process_Success;
                                result.IdEncrypt = HCMFunc.EncryptPES(objSave.Id + "");


                            }
                        }
                    }
                }

                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select employee.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditPTRMailBox(vPTRMailBox_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTRMailBox_Return result = new vPTRMailBox_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null && Session[ItemData.Session] != null)
                    {

                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        //   _getData.active_status = ItemData.active_status;
                        File_Upload_PTRMailBox objFile = new File_Upload_PTRMailBox();
                        objFile = Session[ItemData.Session] as File_Upload_PTRMailBox;
                        _getData.sfile64 = objFile.sfile64;
                        _getData.sfileType = objFile.sfileType;
                        _getData.sfile_oldname = objFile.sfile_name;

                        _getData.actual_sfile64 = objFile.Actualsfile64;
                        _getData.actual_sfileType = objFile.ActualsfileType;
                        _getData.actual_sfile_oldname = objFile.Actualsfile_name;

                        if (_PTR_Evaluation_YearService.CanSave(_getData))
                        {
                            var sComplect = _PTR_Evaluation_YearService.Update(_getData);
                            if (sComplect > 0)
                            {
                                if (_getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                                {
                                    List<int> lstKpi = new List<int>();
                                    lstKpi.Add((int)PESClass.KPIsBase.Fee_Managed);
                                    lstKpi.Add((int)PESClass.KPIsBase.Lock_up);
                                    lstKpi.Add((int)PESClass.KPIsBase.Billing);
                                    lstKpi.Add((int)PESClass.KPIsBase.Collections);
                                    foreach (var item in _getData.PTR_Evaluation.Where(a => a.active_status == "Y"))
                                    {
                                        if (item.PTR_Evaluation_KPIs != null && item.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y" && lstKpi.Contains(a.TM_KPIs_Base.Id)))
                                        {
                                            foreach (var kpi in item.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y"))
                                            {
                                                var _updateKPI = kpi;
                                                vPartnerKPIs_FileTemp _detExcel = null;
                                                if (objFile != null && objFile.lstPartnerKPIs != null && objFile.lstPartnerKPIs.Any())
                                                {
                                                    _detExcel = objFile.lstPartnerKPIs.Where(w => w.emp_code == kpi.PTR_Evaluation.user_no).FirstOrDefault();
                                                }
                                                vPartnerKPIs_FileTemp _ActualExcel = null;
                                                if (objFile != null && objFile.lstActualPTR != null && objFile.lstActualPTR.Any())
                                                {
                                                    _ActualExcel = objFile.lstActualPTR.Where(w => w.emp_code == kpi.PTR_Evaluation.user_no).FirstOrDefault();
                                                }

                                                if (lstKpi.Contains(kpi.TM_KPIs_Base.Id))
                                                {
                                                    if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed && _detExcel != null)
                                                    {
                                                        _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Fee_Managed + "");
                                                        _updateKPI.group_target_max = HCMFunc.DataEncryptPES(_detExcel.Fee_Managed_Group + "");
                                                    }
                                                    //else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up && _detExcel != null)
                                                    //{
                                                    //    _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Lock_up);
                                                    //}
                                                    else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing && _detExcel != null)
                                                    {
                                                        _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Billing + "");
                                                        _updateKPI.group_target_max = HCMFunc.DataEncryptPES(_detExcel.Billing_Group + "");
                                                    }
                                                    else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections && _detExcel != null)
                                                    {
                                                        _updateKPI.target_max = HCMFunc.DataEncryptPES(_detExcel.Collections + "");
                                                        _updateKPI.group_target_max = HCMFunc.DataEncryptPES(_detExcel.Collections_Group + "");
                                                    }
                                                    else
                                                    {
                                                        //_updateKPI.target_max = "";
                                                    }
                                                }

                                                // Actual
                                                if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed && _ActualExcel != null)
                                                {
                                                    //_updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Fee_Managed) * 1000000) + "");
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Fee_Managed + "")) + "");

                                                    //  if (!string.IsNullOrEmpty(_ActualExcel.Fee_Managed_Group))
                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Fee_Managed_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Fee_Managed_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Fee_Managed_su + "")) + "");
                                                }
                                                else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up && _ActualExcel != null)
                                                {
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Lock_up + "")) + "");
                                                    // if (!string.IsNullOrEmpty(_ActualExcel.Lock_up_Group))
                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Lock_up_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Lock_up_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Lock_up_su + "")) + "");
                                                }
                                                else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing && _ActualExcel != null)
                                                {
                                                    // _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Billing) * 1000000) + "");
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Billing + "")) + "");
                                                    // if (!string.IsNullOrEmpty(_ActualExcel.Billing_Group))
                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Billing_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Billing_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Billing_su + "")) + "");
                                                }
                                                else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections && _ActualExcel != null)
                                                {
                                                    // _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Collections) * 1000000) + "");
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Collections + "")) + "");
                                                    // if (!string.IsNullOrEmpty(_ActualExcel.Collections_Group))
                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Collections_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Collections_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Collections_su + "")) + "");
                                                }
                                                else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin && _ActualExcel != null)
                                                {
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Contribution_margin + "")) + "");
                                                    //if (!string.IsNullOrEmpty(_ActualExcel.Contribution_margin_Group))
                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Contribution_margin_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Contribution_margin_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Contribution_margin_su + "")) + "");
                                                }
                                                else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate && _ActualExcel != null)
                                                {
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Recovery_rate + "")) + "");
                                                    //   if (!string.IsNullOrEmpty(_ActualExcel.Recovery_rate_Group))
                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Recovery_rate_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Recovery_rate_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Recovery_rate_su + "")) + "");
                                                }
                                                else if (_updateKPI.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability && _ActualExcel != null)
                                                {
                                                    _updateKPI.actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Chargeability + "")) + "");

                                                    _updateKPI.group_actual = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Chargeability_Group + "")) + "");
                                                    _updateKPI.sbu = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Chargeability_sbu + "")) + "");
                                                    _updateKPI.su = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(_ActualExcel.Chargeability_su + "")) + "");
                                                }
                                                else
                                                {

                                                }

                                                _updateKPI.update_user = CGlobal.UserInfo.UserId;
                                                _updateKPI.update_date = dNow;
                                                _updateKPI.PTR_Evaluation = item;
                                                _PTR_Evaluation_KPIsService.Update(_updateKPI);
                                            }
                                        }
                                    }
                                }

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
                            result.Status = SystemFunction.process_Duplicate;
                            result.Msg = "Duplicate Type name.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
                }
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            vPTRMailBoReturn_UploadFile result = new vPTRMailBoReturn_UploadFile();
            File_Upload_PTRMailBox objFile = new File_Upload_PTRMailBox();
            List<vPartnerKPIs_FileTemp> lstFile = new List<vPartnerKPIs_FileTemp>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            string IdEncrypt = Request.Form["IdEncrypt"];
            if (!string.IsNullOrEmpty(IdEncrypt))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null)
                    {
                        if (Request != null)
                        {
                            string sSess = Request.Form["sSess"];
                            objFile = Session[sSess] as File_Upload_PTRMailBox;
                            if (objFile == null)
                            {
                                objFile = new File_Upload_PTRMailBox();
                            }
                            if (Request.Files.Count > 0)
                            {

                                foreach (string file in Request.Files)
                                {
                                    var fileContent = Request.Files[file];
                                    if (fileContent != null && fileContent.ContentLength > 0)
                                    {
                                        // get a stream
                                        var stream = fileContent.InputStream;
                                        // and optionally write the file to disk
                                        var fileName = Path.GetFileName(file);
                                        try
                                        {
                                            using (var package = new ExcelPackage(stream))
                                            {
                                                var ws = package.Workbook.Worksheets.First();
                                                objFile.sfile64 = package.GetAsByteArray();
                                                objFile.sfile_name = fileName;
                                                objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";



                                                DataTable tbl = new DataTable();
                                                // List<string> lstHead = new List<string>();
                                                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                                {
                                                    tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                                    //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                                }
                                                var startRow = true ? 2 : 1;
                                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                                {
                                                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                                    DataRow row = tbl.Rows.Add();
                                                    foreach (var cell in wsRow)
                                                    {
                                                        row[cell.Start.Column - 1] = cell.Text;
                                                    }
                                                }
                                                if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                                                {
                                                    string[] aPartnerNO = _getData.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aPartnerNO.Contains(w.EmpNo)).ToList();
                                                    result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                         from lFile in lstFile.Where(w => w.emp_code == lstAD.user_no).DefaultIfEmpty(new vPartnerKPIs_FileTemp())
                                                                         select new vPartnerKPIs_obj
                                                                         {
                                                                             codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                                             Edit = "",
                                                                             emp_name = lstEmpReq.EmpFullName,
                                                                             emp_comp = lstEmpReq.CompanyCode,
                                                                             emp_group = lstEmpReq.UnitGroupName,
                                                                             emp_code = lstEmpReq.EmpNo,
                                                                             emp_rank = lstEmpReq.RankCode,
                                                                             Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                                             (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                             Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                             Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                             //Billing = lFile.Billing,
                                                                             //Chargeability = lFile.Chargeability,
                                                                             //Collections = lFile.Collections,
                                                                             //Contribution_margin = lFile.Contribution_margin,
                                                                             //Fee_Managed = lFile.Fee_Managed,
                                                                             //Lock_up = lFile.Lock_up,
                                                                             //Recovery_rate = lFile.Recovery_rate,

                                                                         }).ToList();
                                                    if (tbl != null)
                                                    {

                                                        if (tbl.Columns.Contains("employee no") && tbl.Columns.Contains("fee managed")
                                                           && tbl.Columns.Contains("contribution margin%") && tbl.Columns.Contains("recovery %")
                                                           && tbl.Columns.Contains("lockup days") && tbl.Columns.Contains("chargeability %")
                                                           && tbl.Columns.Contains("billing") && tbl.Columns.Contains("collection")
                                                           )
                                                        {
                                                            int i = 1;
                                                            lstFile = tbl.AsEnumerable().Where(w => (w.Field<string>("employee no") + "") != "").Select(s =>
                                                            new vPartnerKPIs_FileTemp
                                                            {
                                                                emp_code = s.Field<string>("employee no"),
                                                                Billing = (SystemFunction.GetNumberNullToZero(s.Field<string>("billing")) * 1000000) + "",
                                                                Chargeability = s.Field<string>("chargeability %"),
                                                                Collections = (SystemFunction.GetNumberNullToZero(s.Field<string>("collection")) * 1000000) + "",
                                                                Contribution_margin = s.Field<string>("contribution margin%"),
                                                                Fee_Managed = (SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed")) * 1000000) + "",
                                                                Lock_up = s.Field<string>("lockup days"),
                                                                Recovery_rate = s.Field<string>("recovery %"),
                                                                Billing_Group = tbl.Columns.Contains("billing") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("billing(group)")) * 1000000) + "") : "",
                                                                Chargeability_Group = tbl.Columns.Contains("chargeability %(group)") ? (s.Field<string>("chargeability %(group)")) : "",
                                                                Collections_Group = tbl.Columns.Contains("collection(group)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("collection(group)")) * 1000000) + "") : "",
                                                                Contribution_margin_Group = tbl.Columns.Contains("contribution margin%(group)") ? (s.Field<string>("contribution margin%(group)")) : "",
                                                                Fee_Managed_Group = tbl.Columns.Contains("fee managed(group)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed(group)")) * 1000000) + "") : "",
                                                                Lock_up_Group = tbl.Columns.Contains("lockup days(group)") ? (s.Field<string>("lockup days(group)")) : "",
                                                                Recovery_rate_Group = tbl.Columns.Contains("recovery %(group)") ? (s.Field<string>("recovery %(group)")) : "",
                                                            }).ToList();

                                                            if (lstFile.Any())
                                                            {
                                                                result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                                     from lFile in lstFile.Where(w => w.emp_code == lstAD.user_no).DefaultIfEmpty(new vPartnerKPIs_FileTemp())
                                                                                     select new vPartnerKPIs_obj
                                                                                     {
                                                                                         codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                                                         Edit = "",
                                                                                         emp_name = lstEmpReq.EmpFullName,
                                                                                         emp_comp = lstEmpReq.CompanyCode,
                                                                                         emp_group = lstEmpReq.UnitGroupName,
                                                                                         emp_code = lstEmpReq.EmpNo,
                                                                                         emp_rank = lstEmpReq.RankCode,
                                                                                         Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                                                         (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                                         Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                                                         (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                                         Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                                                       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                                         Billing = lFile.Billing,
                                                                                         Collections = lFile.Collections,
                                                                                         Fee_Managed = lFile.Fee_Managed,
                                                                                         Lock_up = lFile.Lock_up,
                                                                                         Contribution_margin_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                                                       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                                         Chargeability_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                                                         (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                                         Recovery_rate_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                                                       (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                                                         Billing_group = lFile.Billing_Group,
                                                                                         Collections_group = lFile.Collections_Group,
                                                                                         Fee_Managed_group = lFile.Fee_Managed_Group,
                                                                                         Lock_up_group = lFile.Lock_up_Group,
                                                                                     }).ToList();



                                                                objFile.lstPartnerKPIs = lstFile;

                                                            }
                                                            result.Status = SystemFunction.process_Success;

                                                        }
                                                        else
                                                        {
                                                            result.Status = SystemFunction.process_Failed;
                                                            result.Msg = "Incorrect template, please try again.";
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Incorrect template, please try again.";
                                        }
                                    }
                                }

                                Session[sSess] = objFile;
                              

                            }
                            else
                            {
                                Session[sSess] = objFile;
                                result.Status = "Clear";
                                if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                                {
                                    string[] aPartnerNO = _getData.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aPartnerNO.Contains(w.EmpNo)).ToList();
                                    result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                         from lFile in lstFile.Where(w => w.emp_code == lstAD.user_no).DefaultIfEmpty(new vPartnerKPIs_FileTemp())
                                                         select new vPartnerKPIs_obj
                                                         {
                                                             codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                             Edit = "",
                                                             emp_name = lstEmpReq.EmpFullName,
                                                             emp_comp = lstEmpReq.CompanyCode,
                                                             emp_group = lstEmpReq.UnitGroupName,
                                                             emp_code = lstEmpReq.EmpNo,
                                                             emp_rank = lstEmpReq.RankCode,
                                                             Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                             (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                             Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                             Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                             Contribution_margin_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                             Chargeability_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                             Recovery_rate_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                            (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                         }).ToList();
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Session!";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Session!";
            }

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult UploadFileActual()
        {
            vPTRMailBoReturn_UploadFile result = new vPTRMailBoReturn_UploadFile();
            File_Upload_PTRMailBox objFile = new File_Upload_PTRMailBox();
            List<vPartnerKPIs_FileTemp> lstFile = new List<vPartnerKPIs_FileTemp>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            string IdEncrypt = Request.Form["IdEncrypt"];
            if (!string.IsNullOrEmpty(IdEncrypt))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null)
                    {
                        if (Request != null)
                        {
                            string sSess = Request.Form["sSess"];
                            objFile = Session[sSess] as File_Upload_PTRMailBox;
                            if (objFile == null)
                            {
                                objFile = new File_Upload_PTRMailBox();
                            }
                            if (Request.Files.Count > 0)
                            {

                                foreach (string file in Request.Files)
                                {
                                    var fileContent = Request.Files[file];
                                    if (fileContent != null && fileContent.ContentLength > 0)
                                    {
                                        // get a stream
                                        var stream = fileContent.InputStream;
                                        // and optionally write the file to disk
                                        var fileName = Path.GetFileName(file);
                                        try
                                        {
                                            using (var package = new ExcelPackage(stream))
                                            {
                                                var ws = package.Workbook.Worksheets.First();
                                                objFile.Actualsfile64 = package.GetAsByteArray();

                                                objFile.Actualsfile_name = fileName;
                                                objFile.ActualsfileType = Path.GetExtension(fileName).ToLower() + "";
                                                DataTable tbl = new DataTable();
                                                // List<string> lstHead = new List<string>();
                                                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                                {
                                                    tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                                    //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                                }
                                                var startRow = true ? 2 : 1;
                                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                                {
                                                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                                    DataRow row = tbl.Rows.Add();
                                                    foreach (var cell in wsRow)
                                                    {
                                                        row[cell.Start.Column - 1] = cell.Text;
                                                    }
                                                }
                                                if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                                                {
                                                    string[] aPartnerNO = _getData.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aPartnerNO.Contains(w.EmpNo)).ToList();
                                                    result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                         from lFile in lstFile.Where(w => w.emp_code == lstAD.user_no).DefaultIfEmpty(new vPartnerKPIs_FileTemp())
                                                                         select new vPartnerKPIs_obj
                                                                         {
                                                                             codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                                             Edit = "",
                                                                             emp_name = lstEmpReq.EmpFullName,
                                                                             emp_comp = lstEmpReq.CompanyCode,
                                                                             emp_group = lstEmpReq.UnitGroupName,
                                                                             emp_code = lstEmpReq.EmpNo,
                                                                             emp_rank = lstEmpReq.RankCode,
                                                                             // Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                                             // (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                             // Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                                             //(s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                             // Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                                             //(s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                                             //Billing = lFile.Billing,
                                                                             //Chargeability = lFile.Chargeability,
                                                                             //Collections = lFile.Collections,
                                                                             //Contribution_margin = lFile.Contribution_margin,
                                                                             //Fee_Managed = lFile.Fee_Managed,
                                                                             //Lock_up = lFile.Lock_up,
                                                                             //Recovery_rate = lFile.Recovery_rate,
                                                                         }).ToList();
                                                    if (tbl != null)
                                                    {

                                                        if (tbl.Columns.Contains("employee no") && tbl.Columns.Contains("fee managed")
                                                           && tbl.Columns.Contains("contribution margin%") && tbl.Columns.Contains("recovery %")
                                                           && tbl.Columns.Contains("lockup days") && tbl.Columns.Contains("chargeability %")
                                                           && tbl.Columns.Contains("billing") && tbl.Columns.Contains("collection")
                                                           )
                                                        {
                                                            int i = 1;
                                                            lstFile = tbl.AsEnumerable().Where(w => (w.Field<string>("employee no") + "") != "").Select(s =>
                                                            new vPartnerKPIs_FileTemp
                                                            {
                                                                emp_code = s.Field<string>("employee no"),
                                                                Billing = (SystemFunction.GetNumberNullToZero(s.Field<string>("billing")) * 1000000) + "",
                                                                Chargeability = s.Field<string>("chargeability %"),
                                                                Collections = (SystemFunction.GetNumberNullToZero(s.Field<string>("collection")) * 1000000) + "",
                                                                Contribution_margin = s.Field<string>("contribution margin%"),
                                                                Fee_Managed = (SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed")) * 1000000) + "",
                                                                Lock_up = s.Field<string>("lockup days"),
                                                                Recovery_rate = s.Field<string>("recovery %"),
                                                                Billing_Group = tbl.Columns.Contains("billing(group)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("billing(group)")) * 1000000) + "") : "",
                                                                Chargeability_Group = tbl.Columns.Contains("chargeability %(group)") ? (s.Field<string>("chargeability %(group)")) : "",
                                                                Collections_Group = tbl.Columns.Contains("collection(group)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("collection(group)")) * 1000000) + "") : "",
                                                                Contribution_margin_Group = tbl.Columns.Contains("contribution margin%(group)") ? (s.Field<string>("contribution margin%(group)")) : "",
                                                                Fee_Managed_Group = tbl.Columns.Contains("fee managed(group)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed(group)")) * 1000000) + "") : "",
                                                                Lock_up_Group = tbl.Columns.Contains("lockup days(group)") ? (s.Field<string>("lockup days(group)")) : "",
                                                                Recovery_rate_Group = tbl.Columns.Contains("recovery %(group)") ? (s.Field<string>("recovery %(group)")) : "",
                                                                Billing_sbu = tbl.Columns.Contains("billing(sbu)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("billing(sbu)")) * 1000000) + "") : "",
                                                                Chargeability_sbu = tbl.Columns.Contains("chargeability %(sbu)") ? (s.Field<string>("chargeability %(sbu)")) : "",
                                                                Collections_sbu = tbl.Columns.Contains("collection(sbu)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("collection(sbu)")) * 1000000) + "") : "",
                                                                Contribution_margin_sbu = tbl.Columns.Contains("contribution margin%(sbu)") ? (s.Field<string>("contribution margin%(sbu)")) : "",
                                                                Fee_Managed_sbu = tbl.Columns.Contains("fee managed(sbu)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed(sbu)")) * 1000000) + "") : "",
                                                                Lock_up_sbu = tbl.Columns.Contains("lockup days(sbu)") ? (s.Field<string>("lockup days(sbu)")) : "",
                                                                Recovery_rate_sbu = tbl.Columns.Contains("recovery %(sbu)") ? (s.Field<string>("recovery %(sbu)")) : "",
                                                                Billing_su = tbl.Columns.Contains("billing(su)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("billing(su)")) * 1000000) + "") : "",
                                                                Chargeability_su = tbl.Columns.Contains("chargeability %(su)") ? (s.Field<string>("chargeability %(su)")) : "",
                                                                Collections_su = tbl.Columns.Contains("collection(su)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("collection(su)")) * 1000000) + "") : "",
                                                                Contribution_margin_su = tbl.Columns.Contains("contribution margin%(su)") ? (s.Field<string>("contribution margin%(su)")) : "",
                                                                Fee_Managed_su = tbl.Columns.Contains("fee managed(su)") ? ((SystemFunction.GetNumberNullToZero(s.Field<string>("fee managed(su)")) * 1000000) + "") : "",
                                                                Lock_up_su = tbl.Columns.Contains("lockup days(su)") ? (s.Field<string>("lockup days(su)")) : "",
                                                                Recovery_rate_su = tbl.Columns.Contains("recovery %(su)") ? (s.Field<string>("recovery %(su)")) : "",

                                                            }).ToList();

                                                            if (lstFile.Any())
                                                            {
                                                                result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                                     from lFile in lstFile.Where(w => w.emp_code == lstAD.user_no).DefaultIfEmpty(new vPartnerKPIs_FileTemp())
                                                                                     select new vPartnerKPIs_obj
                                                                                     {
                                                                                         codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                                                         Edit = "",
                                                                                         emp_name = lstEmpReq.EmpFullName,
                                                                                         emp_comp = lstEmpReq.CompanyCode,
                                                                                         emp_group = lstEmpReq.UnitGroupName,
                                                                                         emp_code = lstEmpReq.EmpNo,
                                                                                         emp_rank = lstEmpReq.RankCode,
                                                                                         Billing = lFile.Billing,
                                                                                         Chargeability = lFile.Chargeability,
                                                                                         Collections = lFile.Collections,
                                                                                         Contribution_margin = lFile.Contribution_margin,
                                                                                         Fee_Managed = lFile.Fee_Managed,
                                                                                         Lock_up = lFile.Lock_up,
                                                                                         Recovery_rate = lFile.Recovery_rate,
                                                                                         Billing_group = lFile.Billing_Group,
                                                                                         Collections_group = lFile.Collections_Group,
                                                                                         Fee_Managed_group = lFile.Fee_Managed_Group,
                                                                                         Lock_up_group = lFile.Lock_up_Group,
                                                                                         Chargeability_group = lFile.Chargeability_Group,
                                                                                         Contribution_margin_group = lFile.Contribution_margin_Group,
                                                                                         Recovery_rate_group = lFile.Recovery_rate_Group,
                                                                                         Billing_sbu = lFile.Billing_sbu,
                                                                                         Collections_sbu = lFile.Collections_sbu,
                                                                                         Fee_Managed_sbu = lFile.Fee_Managed_sbu,
                                                                                         Lock_up_sbu = lFile.Lock_up_sbu,
                                                                                         Chargeability_sbu = lFile.Chargeability_sbu,
                                                                                         Contribution_margin_sbu = lFile.Contribution_margin_sbu,
                                                                                         Recovery_rate_sbu = lFile.Recovery_rate_sbu,
                                                                                         Billing_su = lFile.Billing_su,
                                                                                         Collections_su = lFile.Collections_su,
                                                                                         Fee_Managed_su = lFile.Fee_Managed_su,
                                                                                         Lock_up_su = lFile.Lock_up_su,
                                                                                         Chargeability_su = lFile.Chargeability_su,
                                                                                         Contribution_margin_su = lFile.Contribution_margin_su,
                                                                                         Recovery_rate_su = lFile.Recovery_rate_su,
                                                                                         //sbu = lFile.sbu,
                                                                                         //su = lFile.su,
                                                                                     }).ToList();



                                                                objFile.lstActualPTR = lstFile;

                                                            }
                                                            result.Status = SystemFunction.process_Success;

                                                        }
                                                        else
                                                        {
                                                            result.Status = SystemFunction.process_Failed;
                                                            result.Msg = "Incorrect template, please try again.";
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Incorrect template, please try again.";
                                        }
                                    }
                                }

                                Session[sSess] = objFile;


                            }
                            else
                            {
                                Session[sSess] = objFile;
                                result.Status = "Clear";
                                if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                                {
                                    string[] aPartnerNO = _getData.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aPartnerNO.Contains(w.EmpNo)).ToList();
                                    result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                         from lFile in lstFile.Where(w => w.emp_code == lstAD.user_no).DefaultIfEmpty(new vPartnerKPIs_FileTemp())
                                                         select new vPartnerKPIs_obj
                                                         {
                                                             codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                             Edit = "",
                                                             emp_name = lstEmpReq.EmpFullName,
                                                             emp_comp = lstEmpReq.CompanyCode,
                                                             emp_group = lstEmpReq.UnitGroupName,
                                                             emp_code = lstEmpReq.EmpNo,
                                                             emp_rank = lstEmpReq.RankCode,
                                                             // Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                             // (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                             // Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                             //(s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",
                                                             // Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                             //(s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                         }).ToList();
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Session!";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Session!";
            }

            return Json(new { result });
        }


        [HttpPost]
        public JsonResult LoadKPIsData(string qryStr)
        {
            vPTRMailBoReturn_UploadFile result = new vPTRMailBoReturn_UploadFile();
            File_Upload_PTRMailBox objFile = new File_Upload_PTRMailBox();
            List<vPartnerKPIs_FileTemp> lstFile = new List<vPartnerKPIs_FileTemp>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }

            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_YearService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.Any(a => a.active_status == "Y"))
                        {
                            string[] empNO = new string[] { };
                            empNO = _getData.PTR_Evaluation.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                            result.lstNewData = (from lstAD in _getData.PTR_Evaluation.Where(a => a.active_status == "Y")
                                                 from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                 select new vPartnerKPIs_obj
                                                 {
                                                     lstApproval = PESFunc.GetApprovalPESName(lstAD.PTR_Evaluation_Approve.Where(w => w.active_status == "Y").ToList()),
                                                     codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                     Edit = "",
                                                     emp_name = lstEmpReq.EmpFullName,
                                                     emp_comp = lstEmpReq.CompanyCode,
                                                     emp_group = lstEmpReq.UnitGroupName,
                                                     emp_code = lstEmpReq.EmpNo,
                                                     emp_rank = lstEmpReq.RankCode,
                                                     Billing = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     Chargeability = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     Collections = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     Contribution_margin = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     Fee_Managed = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     Lock_up = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     Recovery_rate = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : HCMFunc.DataDecryptPES(s.target_max))).FirstOrDefault() + "" : "",

                                                     //----- Group

                                                     Billing_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Billing).Select(s =>
                                             (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     Chargeability_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Chargeability).Select(s =>
                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     Collections_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Collections).Select(s =>
                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     Contribution_margin_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Contribution_margin).Select(s =>
                                                     (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     Fee_Managed_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Fee_Managed).Select(s =>
                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     Lock_up_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Lock_up).Select(s =>
                                                   (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",
                                                     Recovery_rate_group = lstAD.PTR_Evaluation_KPIs.Any(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate) ? lstAD.PTR_Evaluation_KPIs.Where(a => a.TM_KPIs_Base.Id == (int)PESClass.KPIsBase.Recovery_rate).Select(s =>
                                                    (s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : HCMFunc.DataDecryptPES(s.group_target_max))).FirstOrDefault() + "" : "",


                                                 }).ToList();
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Session!";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Session!";
            }

            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            //   return Json(new { result, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue });
        }
        #endregion
    }
}