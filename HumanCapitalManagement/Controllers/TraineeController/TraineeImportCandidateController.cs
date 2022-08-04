using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TraineeSite;
using HumanCapitalManagement.ViewModel.MainVM;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineeImportCandidateController : Controller
    {
        private TM_CandidatesService _TM_CandidatesService;

        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        private TimeSheet_FormService _TimeSheet_FormService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TimeSheet_DetailService _TimeSheet_DetailService;
        private TM_Time_TypeService _TM_Time_TypeService;
        private DivisionService _DivisionService;

        public TraineeImportCandidateController(TM_CandidatesService TM_CandidatesService
            , TimeSheet_FormService TimeSheet_FormService
            , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TimeSheet_DetailService TimeSheet_DetailService
            , TM_Time_TypeService TM_Time_TypeService
            , DivisionService DivisionService
            )
        {
            _TM_CandidatesService = TM_CandidatesService;
            _TimeSheet_FormService = TimeSheet_FormService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TimeSheet_DetailService = TimeSheet_DetailService;
            _TM_Time_TypeService = TM_Time_TypeService;
            _DivisionService = DivisionService;

        }
        public class _import_candidate_more_detail : CResutlWebMethod
        {
            public List<import_candidate_more_detail> lstNewData { get; set; }
            public string IdEncrypt { get; set; }
        }
        public class import_candidate_more_detail
        {
            public string no { get; set; }
            public string ref_no { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string id_crad { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string trainee_no { get; set; }
            public string email { get; set; }
            public string daily_wage { get; set; }
            public string pm_no { get; set; }
            public string bank_number { get; set; }
            public string bank_account_number { get; set; }
            public string status { get; set; }
            public string cost_no { get; set; }
        }

        // GET: TraineeImportCandidate
        public ActionResult TraineeImportCandidate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFileBypass()
        {

            _import_candidate_more_detail result = new _import_candidate_more_detail();
            File_Upload_Can objFile = new File_Upload_Can();
            List<import_candidate_more_detail> lstFile = new List<import_candidate_more_detail>();

            var count_can_map = 0;
            List<TM_PR_Candidate_Mapping> get_candidate_map = new List<TM_PR_Candidate_Mapping>();
            List<TM_PR_Candidate_Mapping> new_lst_candidate_map = new List<TM_PR_Candidate_Mapping>();
            List<TimeSheet_Form> new_lst_timesheet_form = new List<TimeSheet_Form>();

            Session["sstimpform"] = null;
             Session["ssmaping"] = null;

            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }

            string IdEncrypt = Request.Form["IdEncrypt"];

            if (Request != null)
            {
                string sTSForm = Request.Form["sTSForm"];
                string sCDMap = Request.Form["sCDMap"];
                objFile = Session[sCDMap] as File_Upload_Can;
                if (objFile == null)
                {
                    objFile = new File_Upload_Can();
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
                                    //var ws = package.Workbook.Worksheets.First();
                                    var ws = package.Workbook.Worksheets.First();
                                    // objFile.sfile64 = package.GetAsByteArray();
                                    objFile.sfile_name = fileName;
                                    objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";

                                    DataTable tbl = new DataTable();
                                    // List<string> lstHead = new List<string>();
                                    var testget = ws.Cells[1, 1, 1, ws.Dimension.End.Column - 1].Value;
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

                                            if (row[cell.Start.Column - 1] == null)
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                row[cell.Start.Column - 1] = cell.Text;
                                            }
                                        }
                                    }
                                    if (tbl != null)
                                    {
                                        if (tbl.Columns.Contains("firstname")
                                            && tbl.Columns.Contains("lastname")
                                            && tbl.Columns.Contains("start_date")
                                            && tbl.Columns.Contains("end_date")
                                            && tbl.Columns.Contains("daily_wage")
                                            )
                                        {

                                            //var getcan = _TM_CandidatesService.GetDataForSelect().Where(w => w.candidate_TraineeNumber == tbl.Columns.Contains("Trainee Code") + "").ToList();

                                            lstFile = (from row in tbl.AsEnumerable().Where(w =>
                                                        !String.IsNullOrEmpty((w.Field<string>("firstname") + ""))
                                                       && !String.IsNullOrEmpty((w.Field<string>("lastname") + ""))
                                                       && !String.IsNullOrEmpty((w.Field<string>("start_date") + "")))
                                                       group row by new
                                                       {
                                                           ref_no = tbl.Columns.Contains("ref_no") ? row.Field<string>("ref_no") : "",
                                                           firstname = tbl.Columns.Contains("firstname") ? row.Field<string>("firstname") : "",
                                                           lastname = tbl.Columns.Contains("lastname") ? row.Field<string>("lastname") : "",
                                                           id_crad = tbl.Columns.Contains("id_crad") ? row.Field<string>("id_crad") : "",
                                                           start_date = tbl.Columns.Contains("start_date") ? row.Field<string>("start_date") : "",
                                                           end_date = tbl.Columns.Contains("end_date") ? row.Field<string>("end_date") : "",
                                                           trainee_no = tbl.Columns.Contains("trainee_no") ? row.Field<string>("trainee_no") : "",
                                                           email = tbl.Columns.Contains("email") ? row.Field<string>("email") : "",
                                                           daily_wage = tbl.Columns.Contains("daily_wage") ? row.Field<string>("daily_wage") : "",
                                                           pm_no = tbl.Columns.Contains("pm_no") ? row.Field<string>("pm_no") : "",
                                                           bank_account_number = tbl.Columns.Contains("bank_account_number") ? row.Field<string>("bank_account_number") : "",
                                                           bank_number = tbl.Columns.Contains("bank_number") ? row.Field<string>("bank_number") : "",
                                                           cost_no = tbl.Columns.Contains("cost_no") ? row.Field<string>("cost_no") : ""


                                                       } into grp
                                                       select new import_candidate_more_detail
                                                       {
                                                           ref_no = grp.Key.ref_no,
                                                           firstname = grp.Key.firstname,
                                                           lastname = grp.Key.lastname,
                                                           id_crad = grp.Key.id_crad,
                                                           start_date = grp.Key.start_date,
                                                           end_date = grp.Key.end_date,
                                                           trainee_no = grp.Key.trainee_no,
                                                           email = grp.Key.email,
                                                           daily_wage = grp.Key.daily_wage,
                                                           pm_no = grp.Key.pm_no,
                                                           bank_account_number = grp.Key.bank_account_number,
                                                           bank_number = grp.Key.bank_number,
                                                           cost_no = grp.Key.cost_no

                                                       }
                                            ).ToList();



                                            result.Status = SystemFunction.process_Success;

                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Incorrect template, please try again.";
                                        }
                                        //end of test import file

                                    }
                                }

                                foreach (var ext in lstFile)
                                {
                                    TM_PR_Candidate_Mapping get_candidate_map_one = new TM_PR_Candidate_Mapping();

                                    get_candidate_map_one = _TM_PR_Candidate_MappingService.GetDataForSelect().Where(w =>
                                     w.TM_Candidates.first_name_en.ToLower().Trim().Contains(ext.firstname.ToLower().Trim())
                                    && w.TM_Candidates.last_name_en.ToLower().Trim().Contains(ext.lastname.ToLower().Trim())
                                    && w.PersonnelRequest.RefNo.Trim().Contains(ext.ref_no)
                                     ).OrderByDescending(o => o.update_date).ToList().FirstOrDefault();

                                  
                                    //foreach (var sub in get_candidate_map)
                                    //{
                                    if (get_candidate_map_one != null)
                                    {
                                        var get_form_timesheet = _TimeSheet_FormService.GetDataForSelect().Where(w => w.TM_PR_Candidate_Mapping.Id == get_candidate_map_one.Id).ToList();
                                        if (get_form_timesheet.Count() <= 0)
                                        {
                                            TimeSheet_Form timeSheet_Form = new TimeSheet_Form();
                                            timeSheet_Form.Id = 0;
                                            timeSheet_Form.submit_status = "Y";
                                            timeSheet_Form.active_status = "Y";
                                            timeSheet_Form.create_user = CGlobal.UserInfo.FullName;
                                            timeSheet_Form.create_date = DateTime.Now;
                                            timeSheet_Form.update_user = CGlobal.UserInfo.FullName;
                                            timeSheet_Form.update_date = DateTime.Now;
                                            timeSheet_Form.Approve_user = ext.pm_no;
                                            timeSheet_Form.Approve_status = "N";
                                            timeSheet_Form.trainee_create_user = get_candidate_map_one.TM_Candidates.Id;
                                            timeSheet_Form.trainee_create_date = DateTime.Now;
                                            timeSheet_Form.trainee_update_user = get_candidate_map_one.TM_Candidates.Id;
                                            timeSheet_Form.trainee_update_date = DateTime.Now;
                                            timeSheet_Form.TM_PR_Candidate_Mapping_Id = get_candidate_map_one.Id;

                                            new_lst_timesheet_form.Add(timeSheet_Form);


                                        }
                                        else
                                        {
                                            get_form_timesheet.OrderByDescending(o => o.create_date).FirstOrDefault();
                                            TimeSheet_Form timeSheet_Form = new TimeSheet_Form();
                                            timeSheet_Form.Id = get_form_timesheet.OrderByDescending(o => o.create_date).FirstOrDefault().Id;

                                            timeSheet_Form.update_user = CGlobal.UserInfo.FullName;
                                            timeSheet_Form.update_date = DateTime.Now;
                                            timeSheet_Form.Approve_user = ext.pm_no;

                                            timeSheet_Form.trainee_update_user = get_candidate_map_one.TM_Candidates.Id;
                                            timeSheet_Form.trainee_update_date = DateTime.Now;
                                            timeSheet_Form.TM_PR_Candidate_Mapping_Id = get_candidate_map_one.Id;

                                            new_lst_timesheet_form.Add(timeSheet_Form);

                                        }

                                        get_candidate_map_one.TM_Candidates.id_card = ext.id_crad;

                                        if (!String.IsNullOrEmpty(ext.start_date))
                                            get_candidate_map_one.trainee_start = DateTime.ParseExact(ext.start_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                        if (!String.IsNullOrEmpty(ext.end_date))
                                            get_candidate_map_one.trainee_end = DateTime.ParseExact(ext.end_date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                        if (!String.IsNullOrEmpty(ext.email))
                                            get_candidate_map_one.TM_Candidates.trainee_email = ext.email;
                                        if (!String.IsNullOrEmpty(ext.trainee_no))
                                            get_candidate_map_one.TM_Candidates.candidate_TraineeNumber = ext.trainee_no;
                                        if (!String.IsNullOrEmpty(ext.daily_wage))
                                            get_candidate_map_one.daily_wage = SystemFunction.GetNumberNullToZero(ext.daily_wage);

                                        if (!String.IsNullOrEmpty(ext.cost_no))
                                        {
                                            var getdivision = _DivisionService.FindByCode(ext.cost_no);
                                            get_candidate_map_one.TM_Divisions = getdivision; 
                                        }

                                        if (!String.IsNullOrEmpty(ext.bank_account_number))
                                            get_candidate_map_one.TM_Candidates.candidate_BankAccountNumber = ext.bank_account_number;

                                        if (!String.IsNullOrEmpty(ext.bank_number))
                                            get_candidate_map_one.TM_Candidates.candidate_BankAccountBranchNumber = ext.bank_number;

                                        get_candidate_map_one.PersonnelRequest.RefNo = ext.ref_no;

                                        get_candidate_map_one.update_user = CGlobal.UserInfo.UserId;
                                        get_candidate_map_one.update_date = DateTime.Now;

                                        new_lst_candidate_map.Add(get_candidate_map_one);

                                        count_can_map = get_candidate_map.Count();
                                    }
                                }
                                foreach (var x in lstFile)
                                {
                                    var where_map = new_lst_candidate_map.Where(w =>
                                    w.TM_Candidates.first_name_en.ToLower().Trim() == x.firstname.ToLower().Trim()
                                    && w.TM_Candidates.last_name_en.ToLower().Trim() == x.lastname.ToLower().Trim()
                                    && w.PersonnelRequest.RefNo.ToLower().Trim() == x.ref_no.ToLower().Trim()
                                   ).ToList();
                                    if (where_map.Count() <= 0)
                                    {
                                        x.status = "N";
                                    }
                                    else { x.status = "Y"; }
                                }

                                result.lstNewData = lstFile;
                            }
                            catch (Exception e)
                            {
                                result.Msg = e.Message + "";
                                result.Status = SystemFunction.process_Failed;
                            }
                        }
                    }

                    //Session[sCDMap] = lstFile;
                    Session["sstimpform"] = new_lst_timesheet_form.Where(w=> w.active_status != "N").ToList();
                    Session["ssmaping"] = new_lst_candidate_map.Where(w => w.active_status != "N").ToList();
                }
                else
                {
                    Session[sCDMap] = lstFile;
                    result.Status = "Clear";
                }
            }
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult UploadFileSave()
        {
            _TimeSheet_Detail result = new _TimeSheet_Detail();
            var count_ = 0;
            try
            {
                var setform = (List<TimeSheet_Form>)Session["sstimpform"];
                var setmaping = (List<TM_PR_Candidate_Mapping>)Session["ssmaping"];

                foreach (var tsForm in setform.Where(w=> w.active_status != "N"))
                {
                    //tsForm.TM_PR_Candidate_Mapping = _TM_PR_Candidate_MappingService.GetDataForTimeSheet_CDD;

                    TimeSheet_Form new_form = new TimeSheet_Form();
                    new_form = tsForm;

                    var change_form = _TimeSheet_FormService.CreateNewOrUpdate(new_form);
                    if (change_form <= 0)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't Save " + new_form.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en;
                        return Json(new
                        {
                            result
                        });
                    }
                    else
                    {

                    }
                }


                foreach (var tsmap in setmaping)
                {
                    //TM_PR_Candidate_Mapping new_map = new TM_PR_Candidate_Mapping();
                    //new_map = tsmap;
                    var fine_new_map = _TM_PR_Candidate_MappingService.Find(tsmap.Id);
                    if (tsmap.trainee_start != null)
                        fine_new_map.trainee_start = tsmap.trainee_start;
                    if (tsmap.trainee_end != null)
                        fine_new_map.trainee_end = tsmap.trainee_end;
                    if (!String.IsNullOrEmpty(tsmap.TM_Candidates.trainee_email))
                        fine_new_map.TM_Candidates.trainee_email = tsmap.TM_Candidates.trainee_email;
                    if (!String.IsNullOrEmpty(tsmap.TM_Candidates.candidate_TraineeNumber))
                        fine_new_map.TM_Candidates.candidate_TraineeNumber = tsmap.TM_Candidates.candidate_TraineeNumber;
                    if (!String.IsNullOrEmpty(tsmap.TM_Candidates.trainee_email))
                        fine_new_map.daily_wage = tsmap.daily_wage;

                    fine_new_map.update_date = DateTime.Now;
                    fine_new_map.update_user = CGlobal.UserInfo.UserId;

                    if (!String.IsNullOrEmpty(tsmap.TM_Candidates.id_card))
                        fine_new_map.TM_Candidates.id_card = tsmap.TM_Candidates.id_card;
                    fine_new_map.TM_Candidates.verify_code = "00000000";
                    fine_new_map.TM_Candidates.is_verify = "Y";
                    if (!String.IsNullOrEmpty(tsmap.TM_Candidates.candidate_BankAccountNumber))
                        fine_new_map.TM_Candidates.candidate_BankAccountNumber = tsmap.TM_Candidates.candidate_BankAccountNumber;

                    if (!String.IsNullOrEmpty(tsmap.TM_Candidates.candidate_BankAccountBranchNumber))
                        fine_new_map.TM_Candidates.candidate_BankAccountBranchNumber = tsmap.TM_Candidates.candidate_BankAccountBranchNumber;
                    
                    if (tsmap.TM_Divisions != null)
                    {
                        var getdivision = _DivisionService.FindByCode(tsmap.TM_Divisions.division_code);
                        fine_new_map.TM_Divisions = getdivision;
                    }

                    var change_map = _TM_PR_Candidate_MappingService.Update(fine_new_map);
                    //if (change_map <= 0)
                    //{
                    //    result.Status = SystemFunction.process_Failed;
                    //    result.Msg = "Error, Can't Save " + new_map.TM_Candidates.first_name_en;
                    //    return Json(new
                    //    {
                    //        result
                    //    });
                    //}
                    //else
                    //{

                    //}
                    count_ += 1;
                }



            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }
            if (true)
            {
                result.Status = SystemFunction.process_Success;
                result.Msg = "Success " + count_ + " Items.";
            }

            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, File Not Found.";
            }


            return Json(new
            {
                result
            });
        }
    }
}