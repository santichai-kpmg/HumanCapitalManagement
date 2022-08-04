using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Controllers.MainController;
using HumanCapitalManagement.Models.TraineeSite;
using HumanCapitalManagement.Service.AddressService;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.EduService;
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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.ViewModel.MainVM.vTimeSheet;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineeImportRetainController : BaseController
    {
        private TM_CandidatesService _TM_CandidatesService;

        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        private TimeSheet_FormService _TimeSheet_FormService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TimeSheet_DetailService _TimeSheet_DetailService;
        private TM_Time_TypeService _TM_Time_TypeService;

        public TraineeImportRetainController(TM_CandidatesService TM_CandidatesService
            , TimeSheet_FormService TimeSheet_FormService
            , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TimeSheet_DetailService TimeSheet_DetailService
            , TM_Time_TypeService TM_Time_TypeService
            )
        {
            _TM_CandidatesService = TM_CandidatesService;
            _TimeSheet_FormService = TimeSheet_FormService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TimeSheet_DetailService = TimeSheet_DetailService;
            _TM_Time_TypeService = TM_Time_TypeService;

        }
        // GET: TraineeImportRetain
        public ActionResult TraineeImportRetain()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFileBypass()
        {

            _TimeSheet_Detail result = new _TimeSheet_Detail();
            File_Upload_Can objFile = new File_Upload_Can();
            List<vTimeSheet_Detail> lstFile = new List<vTimeSheet_Detail>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }

            string IdEncrypt = Request.Form["IdEncrypt"];

            if (Request != null)
            {
                string sSess = Request.Form["sSess"];
                objFile = Session[sSess] as File_Upload_Can;
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
                                    var ws = package.Workbook.Worksheets["Standard(Audit)"];
                                    // objFile.sfile64 = package.GetAsByteArray();
                                    objFile.sfile_name = fileName;
                                    objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";

                                    DataTable tbl = new DataTable();
                                    // List<string> lstHead = new List<string>();
                                    var testget = ws.Cells[4, 1, 4, ws.Dimension.End.Column - 1].Value;
                                    foreach (var firstRowCell in ws.Cells[4, 1, 4, ws.Dimension.End.Column - 1])
                                    {
                                        tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                        //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                    }
                                    var startRow = true ? 5 : 1;
                                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column - 1];


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
                                        if (tbl.Columns.Contains("Trainee Code") 
                                            && tbl.Columns.Contains("Engagement Code") 
                                            && tbl.Columns.Contains("Start date") 
                                            && tbl.Columns.Contains("End date") 
                                            && tbl.Columns.Contains("Hours"))
                                        {

                                            //var getcan = _TM_CandidatesService.GetDataForSelect().Where(w => w.candidate_TraineeNumber == tbl.Columns.Contains("Trainee Code") + "").ToList();

                                            lstFile = (from row in tbl.AsEnumerable().Where(w => (w.Field<string>("Name") + "") != ""
                                                       && !String.IsNullOrEmpty((w.Field<string>("Engagement Code") + ""))
                                                       && (w.Field<string>("Engagement Code") + "") != "HOL")
                                                       //&& Regex.IsMatch((w.Field<string>("Code") + ""), @"^\d+$"))
                                                       group row by new
                                                       {
                                                           NAME = tbl.Columns.Contains("Name") ? row.Field<string>("Name") : "",
                                                           Component = tbl.Columns.Contains("Engagement Code") ? row.Field<string>("Engagement Code") == "SICK" ? "150000000001" : row.Field<string>("Engagement Code") == "VAC" ? "150000000000" : row.Field<string>("Engagement Code") : "",
                                                           Start = tbl.Columns.Contains("Start date") ? row.Field<string>("Start date") : "",
                                                           End = tbl.Columns.Contains("End date") ? row.Field<string>("End date") : "",
                                                           Hrs = tbl.Columns.Contains("Hours") ? row.Field<string>("Hours") : "",
                                                           NOTES = tbl.Columns.Contains("Notes") ? row.Field<string>("Notes") : "",
                                                           TraineeCode = tbl.Columns.Contains("Trainee Code") ? row.Field<string>("Trainee Code") : ""



                                                       } into grp
                                                       select new vTimeSheet_Detail
                                                       {
                                                           name = grp.Key.NAME,
                                                           trainee_create_user = _TM_CandidatesService.GetDataForSelect().Where(w => w.candidate_TraineeNumber == grp.Key.TraineeCode).Select(s => s.Id).FirstOrDefault().ToString(),
                                                           Engagement_Code = grp.Key.Component,
                                                           date_start = grp.Key.Start,
                                                           date_end = grp.Key.End,
                                                           start = "08:00",
                                                           end = "17:00",
                                                           hour = grp.Key.Hrs,
                                                           remark = grp.Key.NOTES,
                                                           TM_Time_Type_Id = grp.Key.Component == "150000000000" ? _TM_Time_TypeService.FindByShortName("SL").Id.ToString() : grp.Key.Component == "150000000001" ? _TM_Time_TypeService.FindByShortName("VL").Id.ToString() : _TM_Time_TypeService.FindByShortName("NH").Id.ToString(),


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

                                List<vTimeSheet_Detail> newlist = new List<vTimeSheet_Detail>();
                                List<vTimeSheet_Detail> newlistEmp = new List<vTimeSheet_Detail>();
                                List<vTimeSheet_Detail> removes = new List<vTimeSheet_Detail>();
                                foreach (var s in lstFile)
                                {
                                    //vTimeSheet_Detail setnewEmp = new vTimeSheet_Detail();


                                    var _name = s.name;
                                    var _hour = s.hour;
                                    var _Engagement_Code = s.Engagement_Code;
                                    var _start = s.start;
                                    var _end = s.end;
                                    var _date_start = s.date_start;
                                    var _date_end = s.date_end;
                                    var _remark = s.remark;
                                    var _trainee_create_user = s.trainee_create_user;
                                    var _TM_Time_Type_Id = s.TM_Time_Type_Id;
                                    

                                    DateTime setdatestart = DateTime.ParseExact(_date_start, "d-MMM-yy", CultureInfo.InvariantCulture);
                                    DateTime setdateend = DateTime.ParseExact(_date_end, "d-MMM-yy", CultureInfo.InvariantCulture);

                                    //for (int r=1;r <= Convert.ToInt64(caldate); r++)
                                    //{ } 
                                    vTimeSheet_Detail setnew = new vTimeSheet_Detail();
                                    setnew.name = _name;
                                    setnew.Engagement_Code = _Engagement_Code;
                                    setnew.remark = _remark;
                                    setnew.trainee_create_user = _trainee_create_user;
                                    setnew.TM_Time_Type_Id = _TM_Time_Type_Id;

                                    decimal sethour = SystemFunction.GetNumberNullToZero(_hour + "");
                                    if (sethour > 8)
                                    {
                                        removes.Add(s);

                                        setdatestart = setdatestart.AddDays(-1);


                                        for (int i = 0; sethour > 0; i++)
                                        {
                                          

                                            setnew = new vTimeSheet_Detail();
                                            setnew.name = _name;
                                            setnew.Engagement_Code = _Engagement_Code;
                                            setnew.remark = _remark;
                                            setnew.trainee_create_user = _trainee_create_user;

                                            setdatestart = setdatestart.AddDays(1);

                                            setnew.date_start = setdatestart.ToString("d-MMM-yyyy");
                                            setnew.date_end = setdatestart.ToString("d-MMM-yyyy");
                                            if (sethour > 8)
                                            {
                                                setnew.hour = 8.ToString("00.00").Replace(".", ":");
                                                setnew.start = "08:00";
                                                setnew.end = (Convert.ToDecimal(setnew.hour.Replace(":", ".") + "") + 9).ToString("#0.00").Replace(".", ":");
                                                sethour = sethour - 8;
                                            }
                                            else
                                            {

                                                setnew.hour = sethour.ToString("00.00").Replace(".", ":");
                                                setnew.start = "08:00";
                                                if (sethour > 4)
                                                {
                                                    setnew.end = (Convert.ToDecimal(setnew.hour.Replace(":",".") + "") + 9).ToString("00.00").Replace(".", ":");
                                                }
                                                else
                                                {
                                                    setnew.end = (Convert.ToDecimal(setnew.hour.Replace(":", ".") + "") + 8).ToString("00.00").Replace(".", ":");
                                                }
                                                sethour = sethour - sethour;

                                            }
                                            newlist.Add(setnew);
                                        }
                                    }
                                    else
                                    {
                                        removes.Add(s);
                                        setnew.date_start = setdatestart.ToString("d-MMM-yyyy");
                                        setnew.date_end = setdatestart.ToString("d-MMM-yyyy");
                                        setnew.start = "08:00";
                                        setnew.hour = Convert.ToDecimal(_hour + "").ToString("00.00").Replace(".", ":");
                                        if (sethour > 4)
                                        {
                                            setnew.end = (Convert.ToDecimal(setnew.hour.Replace(":", ".") + "") + 9).ToString("00.00").Replace(".", ":");
                                        }
                                        else
                                        {
                                            setnew.end = (Convert.ToDecimal(setnew.hour.Replace(":", ".") + "") + 8).ToString("00.00").Replace(".", ":");
                                        }
                                        newlist.Add(setnew);
                                    }
                                }
                                foreach (var r in removes)
                                {
                                    lstFile.Remove(r);
                                }
                                lstFile.AddRange(newlist);

                                //var outlst = lstFile.Where(w => Convert.ToDateTime(w.start).ToString("dd/MM/yyyy") != Convert.ToDateTime(w.end).ToString("dd/MM/yyyy")).ToList();
                                //var inlst = lstFile.Where(w => Convert.ToDateTime(w.start).ToString("dd/MM/yyyy") == Convert.ToDateTime(w.end).ToString("dd/MM/yyyy")).ToList();
                                if (lstFile.Any())
                                {
                                    result.lstNewData = lstFile.OrderBy(o => o.name).ToList();
                                }
                            }
                            catch (Exception e)
                            {
                                result.Msg = e.Message + "";
                                result.Status = SystemFunction.process_Failed;
                            }
                        }
                    }

                    Session[sSess] = lstFile;
                    Session["setvalue"] = lstFile;
                }
                else
                {
                    Session[sSess] = lstFile;
                    result.Status = "Clear";
                }
            }
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult UploadFileSave(vCan_file ItemData)
        {
            _TimeSheet_Detail result = new _TimeSheet_Detail();
            var count_ = 0;
            try
            {
                string sSess = Request.Form["sSess"];
                var listforsave = Session[sSess];
                File_Upload_Can objFile = new File_Upload_Can();
                objFile = Session[sSess] as File_Upload_Can;
                var _getTtype = _TM_Time_TypeService.FindByName("Normal Hour");

                var set = (List<vTimeSheet_Detail>)Session["setvalue"];
                List<TimeSheet_Detail> lst_map_detail = new List<TimeSheet_Detail>();
                foreach (var xp in set.Where(w => w.trainee_create_user != "0").ToList())
                {
                    var hour_ = SystemFunction.GetNumberNullToZero(xp.hour.Replace(":", ".")).ToString("00.00").Replace(".", ":");


                    var _getTSform = _TimeSheet_FormService.FindByCandidateID(Convert.ToInt32(xp.trainee_create_user + "")).OrderByDescending(o=> o.TM_PR_Candidate_Mapping_Id).FirstOrDefault();
                    var title_ = "" + hour_ + "|Normal Hour|" + xp.Engagement_Code + (xp.remark != null ? "|" + xp.remark : "");
                    if (_getTSform != null)
                    {
                        //var _getData = _TimeSheet_FormService.Find(nId);
                        var objSave = new TimeSheet_Detail()
                        {
                            //Id = Convert.ToInt32(SystemFunction.GetIntNullToZero(xp.id)),
                            date_start = Convert.ToDateTime(xp.date_start + " " + xp.start),
                            date_end = Convert.ToDateTime(xp.date_end + " " + xp.end),
                            title = title_,
                            hours = hour_,
                            TM_Time_Type_Id = _getTtype.Id,
                            Engagement_Code = xp.Engagement_Code,
                            remark = xp.remark,
                            TimeSheet_Form = _getTSform,
                            submit_status = "S",
                            approve_status = "Y",
                            Approve_user = _getTSform.Approve_user,
                            trainee_create_user = Convert.ToInt32(xp.trainee_create_user + ""),
                            trainee_create_date = DateTime.Now,
                            trainee_update_user = Convert.ToInt32(xp.trainee_create_user + ""),
                            trainee_update_date = DateTime.Now,
                            active_status = "Y",
                            Source_Type = "R",


                        };

                        lst_map_detail.Add(objSave);

                        if (!string.IsNullOrEmpty(title_) && _getTtype.Id != 0)
                        {
                            var change = _TimeSheet_DetailService.CreateNewOrUpdate(objSave);
                            if (change <= 0)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Can't Save " + objSave.title;
                                return Json(new
                                {
                                    result
                                });
                            }
                            else
                            {

                            }
                        }
                    }
                }
                count_ = lst_map_detail.Count();
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;

            }
            if (true)
            {
                result.Status = SystemFunction.process_Success;
                result.Msg = "Success " + count_+" Items.";
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