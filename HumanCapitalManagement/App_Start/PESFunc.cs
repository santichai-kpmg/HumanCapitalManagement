using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using HumanCapitalManagement.ViewModel.NMNVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.App_Start
{
    public class PESFunc
    {
        public static string GetSignatureName(List<PES_Nomination_Signatures> lstData)
        {
            string sReturn = "";
            List<AllInfo_WS> lstReturn = new List<AllInfo_WS>();
            if (lstData != null && lstData.Any())
            {
                New_HRISEntities dbHr = new New_HRISEntities();
                string[] aUpdateUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                // string[] aNameNStep = new string[] { };


                // List<string> aNameNStep = new List<string>();
                var _User = dbHr.AllInfo_WS.Where(w => aUpdateUser.Contains(w.EmpNo)).ToList();

                var aNameNStep = (from item in lstData.OrderBy(o => o.TM_PES_NMN_SignatureStep.seq)
                                  from lstEmpReq in _User.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  select new SignatureReturn
                                  {
                                      sValue = item.Req_Approve_user + " : " + lstEmpReq.EmpFullName + " (" + (item.TM_PES_NMN_SignatureStep != null ? item.TM_PES_NMN_SignatureStep.Step_name_en + "" : "") + ")"
                                  }).ToList();

                sReturn = string.Join("<br/>", aNameNStep.Select(s => s.sValue));

            }

            return sReturn;
        }
        public static string GetApprovalPESName(List<PTR_Evaluation_Approve> lstData)
        {
            string sReturn = "";
            List<AllInfo_WS> lstReturn = new List<AllInfo_WS>();
            if (lstData != null && lstData.Any())
            {
                New_HRISEntities dbHr = new New_HRISEntities();
                string[] aUpdateUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                // string[] aNameNStep = new string[] { };


                // List<string> aNameNStep = new List<string>();
                var _User = dbHr.AllInfo_WS.Where(w => aUpdateUser.Contains(w.EmpNo)).ToList();

                var aNameNStep = (from item in lstData.OrderBy(o => o.TM_PTR_Eva_ApproveStep.seq)
                                  from lstEmpReq in _User.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  select new SignatureReturn
                                  {
                                      sValue = item.Req_Approve_user + " : " + lstEmpReq.EmpFullName + " (" + (item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.Step_name_en + "" : "") + ")"
                                  }).ToList();

                sReturn = string.Join("<br/>", aNameNStep.Select(s => s.sValue));

            }

            return sReturn;
        }
        public static string GetRatingYear(List<PES_Final_Rating_Year> lstData, string EmpNo)
        {
            string sReturn = "";
            if (lstData != null && lstData.Any())
            {



                var aNameNStep = (from item in lstData.OrderBy(o => o.evaluation_year)
                                  select new SignatureReturn
                                  {
                                      sValue = item.evaluation_year.Value.DateTimebyCulture("yyyy") + " : " + (item.PES_Final_Rating != null ? item.PES_Final_Rating.Where(w => w.active_status == "Y" && w.user_no == EmpNo).Select(s => s.Final_TM_Annual_Rating.rating_name_en).FirstOrDefault() + "" : "")
                                  }).ToList();

                sReturn = string.Join("<br/>", aNameNStep.Select(s => s.sValue));

            }

            return sReturn;
        }

        public static List<vNominationForm_lst_approve> GetNominationApproveList(List<PES_Nomination_Signatures> lstData, PES_Nomination PES_Nomination)
        {
            List<vNominationForm_lst_approve> lstReturn = new List<vNominationForm_lst_approve>();
            if (PES_Nomination != null && lstData != null && lstData.Any())
            {
                New_HRISEntities dbHr = new New_HRISEntities();
                if (PES_Nomination.TM_PES_NMN_Status_Id == (int)PESClass.Nomination_Status.Form_Completed)
                {
                    //string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                    //var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                    //lstReturn = (from lst in lstData.Where(a => a.active_status == "Y")
                    //             from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                    //             select new vNominationForm_lst_approve
                    //             {
                    //                 id = lst.TM_PES_NMN_SignatureStep != null ? lst.TM_PES_NMN_SignatureStep.Id : 0,
                    //                 step_name = lst.TM_PES_NMN_SignatureStep.Step_name_en,
                    //                 app_name = lstEmpReq.EmpFullName,
                    //                 nStep = lst.TM_PES_NMN_SignatureStep.seq.HasValue ? lst.TM_PES_NMN_SignatureStep.seq.Value : 0,
                    //                 approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture("yyyyMMddHHMMss") : "",
                    //                 short_step_name = lst.TM_PES_NMN_SignatureStep.Step_short_name_en,
                    //                 agree_status = lst.Approve_status + "" == "Y" ? ((!string.IsNullOrEmpty(lst.Agree_Status) ? (lst.Agree_Status + "" == "Y" ? "Agree" : "Disagree") : "") + "<br/>" + (lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "")) : "",
                    //                 description = HCMFunc.DataDecryptPES(lst.responses),
                    //                 annual_rating = lst.Approve_status + "" == "Y" ? (lst.TM_Annual_Rating != null ? lst.TM_Annual_Rating.rating_name_en : "") : "",
                    //             }).ToList();

                    #region Final view

                    //check viewer mode
                    List<int> nFullPermission = new List<int>();
                    nFullPermission.Add((int)PESClass.SignaturesStep.Ceo);
                    nFullPermission.Add((int)PESClass.SignaturesStep.Deputy_Ceo);

                    if (lstData.Any(a => nFullPermission.Contains(a.TM_PES_NMN_SignatureStep.Id) && a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo) || CGlobal.UserIsAdminPES())
                    {
                        string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();

                        lstReturn = (from lst in lstData.Where(a => a.active_status == "Y")
                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vNominationForm_lst_approve
                                     {
                                         step_name = lst.TM_PES_NMN_SignatureStep.Step_name_en,
                                         app_name = lstEmpReq.EmpFullName,
                                         nStep = lst.TM_PES_NMN_SignatureStep.seq.HasValue ? lst.TM_PES_NMN_SignatureStep.seq.Value : 0,
                                         approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture("yyyyMMddHHMMss") : "",
                                         short_step_name = lst.TM_PES_NMN_SignatureStep.Step_short_name_en,
                                         agree_status = lst.Approve_status + "" == "Y" ? ((!string.IsNullOrEmpty(lst.Agree_Status) ? lst.TM_PES_NMN_SignatureStep.Id == 5 && lst.PES_Nomination.PES_Nomination_Year.evaluation_year.Value.Year >= 2020 ? (lst.Agree_Status + "" == "Y" ? "Acknowledge" : "Disagree") : (lst.Agree_Status + "" == "Y" ? "Agree" : "Disagree") : "") + "<br/>" + (lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "")) : "",
                                         description = HCMFunc.DataDecryptPES(lst.responses),
                                         annual_rating = lst.Approve_status + "" == "Y" ? (lst.TM_Annual_Rating != null ? lst.TM_Annual_Rating.rating_name_en : "") : "",
                                     }).ToList();
                    }
                    else
                    {
                        string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                        lstReturn = (from lst in lstData.Where(a => a.active_status == "Y" && (a.TM_PES_NMN_SignatureStep.Id != (int)PESClass.SignaturesStep.Nominating || a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo))
                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vNominationForm_lst_approve
                                     {
                                         step_name = lst.TM_PES_NMN_SignatureStep.Step_name_en,
                                         app_name = lstEmpReq.EmpFullName,
                                         nStep = lst.TM_PES_NMN_SignatureStep.seq.HasValue ? lst.TM_PES_NMN_SignatureStep.seq.Value : 0,
                                         id = lst.TM_PES_NMN_SignatureStep != null ? lst.TM_PES_NMN_SignatureStep.Id : 0,
                                         approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture("yyyyMMddHHMMss") : "",
                                         short_step_name = lst.TM_PES_NMN_SignatureStep.Step_short_name_en,
                                         agree_status = lst.Approve_status + "" == "Y" ? ((!string.IsNullOrEmpty(lst.Agree_Status) ? lst.TM_PES_NMN_SignatureStep.Id == 5 && lst.PES_Nomination.PES_Nomination_Year.evaluation_year.Value.Year >= 2020 ? (lst.Agree_Status + "" == "Y" ? "Acknowledge" : "Disagree") : (lst.Agree_Status + "" == "Y" ? "Agree" : "Disagree") : "") + "<br/>" + (lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "")) : "",
                                         description = HCMFunc.DataDecryptPES(lst.responses),
                                         annual_rating = lst.Approve_status + "" == "Y" ? (lst.TM_Annual_Rating != null ? lst.TM_Annual_Rating.rating_name_en : "") : "",

                                     }).ToList();


                        var nViewMode = lstData.Where(w => w.active_status == "Y" && w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.TM_PES_NMN_SignatureStep_Id).Select(s => s.TM_PES_NMN_SignatureStep_Id).FirstOrDefault();
                        var nSeqStep = lstData.Where(w => w.active_status == "Y" && w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.TM_PES_NMN_SignatureStep.seq).Select(s => s.TM_PES_NMN_SignatureStep.seq).FirstOrDefault();
                        if (nViewMode > 0)
                        {
                            if (nViewMode.Value == (int)PESClass.SignaturesStep.Self || nViewMode.Value == (int)PESClass.SignaturesStep.Sponsoring_Partner)
                            {
                                lstReturn.Where(w => w.id == (int)PESClass.SignaturesStep.Nominating).ToList().ForEach(ed => { ed.description = ""; });
                            }
                            else if (nViewMode.Value == (int)PESClass.SignaturesStep.Group_Head || nViewMode.Value == (int)PESClass.SignaturesStep.HOP || nViewMode.Value == (int)PESClass.SignaturesStep.Risk_Management)
                            {
                                lstReturn.Where(w => w.id == (int)PESClass.SignaturesStep.Nominating).ToList().ForEach(ed => { ed.description = ""; });
                            }
                            else if (nViewMode.Value == (int)PESClass.SignaturesStep.Nominating)
                            {

                            }
                        }

                    }
                    var _getNCCEO = lstData.Where(w => w.active_status == "Y" && w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Nominating && w.Req_Approve_user == PESClass.CEO).FirstOrDefault();
                    if (_getNCCEO != null)
                    {
                        lstReturn.Add(new vNominationForm_lst_approve
                        {
                            step_name = _getNCCEO.TM_PES_NMN_SignatureStep.Step_name_en,
                            app_name = "Average comment",
                            nStep = _getNCCEO.TM_PES_NMN_SignatureStep.seq.HasValue ? _getNCCEO.TM_PES_NMN_SignatureStep.seq.Value : 0,
                            id = _getNCCEO.TM_PES_NMN_SignatureStep != null ? _getNCCEO.TM_PES_NMN_SignatureStep.Id : 0,
                            approve_date = _getNCCEO.Approve_date.HasValue ? _getNCCEO.Approve_date.Value.DateTimebyCulture("yyyyMMddHHMMss") : "",
                            short_step_name = _getNCCEO.TM_PES_NMN_SignatureStep.Step_short_name_en,
                            agree_status = _getNCCEO.Approve_status + "" == "Y" ? ((!string.IsNullOrEmpty(_getNCCEO.Agree_Status) ? _getNCCEO.TM_PES_NMN_SignatureStep.Id == 5 ? (_getNCCEO.Agree_Status + "" == "Y" ? "Acknowledge" : "Disagree") : (_getNCCEO.Agree_Status + "" == "Y" ? "Agree" : "Disagree") : "") + "<br/>" + (_getNCCEO.Approve_date.HasValue ? _getNCCEO.Approve_date.Value.DateTimebyCulture() : "")) : "",
                            description = HCMFunc.DataDecryptPES(_getNCCEO.responses),
                            annual_rating = _getNCCEO.Approve_status + "" == "Y" ? (_getNCCEO.TM_Annual_Rating != null ? _getNCCEO.TM_Annual_Rating.rating_name_en : "") : "",
                        });
                    }
                    #endregion
                }
                else
                {
                    //check viewer mode
                    List<int> nFullPermission = new List<int>();
                    nFullPermission.Add((int)PESClass.SignaturesStep.Ceo);
                    nFullPermission.Add((int)PESClass.SignaturesStep.Deputy_Ceo);
                    nFullPermission.Add((int)PESClass.SignaturesStep.COO);
                    //nFullPermission.Add((int)PESClass.SignaturesStep.Risk_Management);
                    //nFullPermission.Add((int)PESClass.SignaturesStep.Nominating);

                    if (lstData.Any(a => nFullPermission.Contains(a.TM_PES_NMN_SignatureStep.Id) && a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo) || CGlobal.UserIsAdminPES())
                    {
                        string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                        lstReturn = (from lst in lstData.Where(a => a.active_status == "Y")
                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vNominationForm_lst_approve
                                     {
                                         step_name = lst.TM_PES_NMN_SignatureStep.Step_name_en,
                                         app_name = lstEmpReq.EmpFullName,
                                         nStep = lst.TM_PES_NMN_SignatureStep.seq.HasValue ? lst.TM_PES_NMN_SignatureStep.seq.Value : 0,
                                         approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture("yyyyMMddHHMMss") : "",
                                         short_step_name = lst.TM_PES_NMN_SignatureStep.Step_short_name_en,
                                         agree_status = lst.Approve_status + "" == "Y" ? ((!string.IsNullOrEmpty(lst.Agree_Status) ? lst.TM_PES_NMN_SignatureStep.Id == 5 && lst.PES_Nomination.PES_Nomination_Year.evaluation_year.Value.Year >= 2020 ? (lst.Agree_Status + "" == "Y" ? "Acknowledge" : "Disagree") : (lst.Agree_Status + "" == "Y" ? "Agree" : "Disagree") : "") + "<br/>" + (lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "")) : "",
                                         description = HCMFunc.DataDecryptPES(lst.responses),
                                         annual_rating = lst.Approve_status + "" == "Y" ? (lst.TM_Annual_Rating != null ? lst.TM_Annual_Rating.rating_name_en : "") : "",
                                     }).ToList();
                    }
                    else
                    {
                        string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                        lstReturn = (from lst in lstData.Where(a => a.active_status == "Y" && (a.TM_PES_NMN_SignatureStep.Id != (int)PESClass.SignaturesStep.Nominating || a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo))
                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vNominationForm_lst_approve
                                     {
                                         step_name = lst.TM_PES_NMN_SignatureStep.Step_name_en,
                                         app_name = lstEmpReq.EmpFullName,
                                         nStep = lst.TM_PES_NMN_SignatureStep.seq.HasValue ? lst.TM_PES_NMN_SignatureStep.seq.Value : 0,
                                         id = lst.TM_PES_NMN_SignatureStep != null ? lst.TM_PES_NMN_SignatureStep.Id : 0,
                                         approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture("yyyyMMddHHMMss") : "",
                                         short_step_name = lst.TM_PES_NMN_SignatureStep.Step_short_name_en,
                                         agree_status = lst.Approve_status + "" == "Y" ? ((!string.IsNullOrEmpty(lst.Agree_Status) ? lst.TM_PES_NMN_SignatureStep.Id == 5 && lst.PES_Nomination.PES_Nomination_Year.evaluation_year.Value.Year >= 2020 ? (lst.Agree_Status + "" == "Y" ? "Acknowledge" : "Disagree") : (lst.Agree_Status + "" == "Y" ? "Agree" : "Disagree") : "") + "<br/>" + (lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "")) : "",
                                         description = HCMFunc.DataDecryptPES(lst.responses),
                                         annual_rating = lst.Approve_status + "" == "Y" ? (lst.TM_Annual_Rating != null ? lst.TM_Annual_Rating.rating_name_en : "") : "",

                                     }).ToList();


                        var nViewMode = lstData.Where(w => w.active_status == "Y" && w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.TM_PES_NMN_SignatureStep_Id).Select(s => s.TM_PES_NMN_SignatureStep_Id).FirstOrDefault();
                        var nSeqStep = lstData.Where(w => w.active_status == "Y" && w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo).OrderByDescending(o => o.TM_PES_NMN_SignatureStep.seq).Select(s => s.TM_PES_NMN_SignatureStep.seq).FirstOrDefault();
                        if (nViewMode > 0)
                        {
                            if (nViewMode.Value == (int)PESClass.SignaturesStep.Self || nViewMode.Value == (int)PESClass.SignaturesStep.Sponsoring_Partner)
                            {
                                lstReturn.Where(w => w.id != (int)PESClass.SignaturesStep.Self && w.id != (int)PESClass.SignaturesStep.Sponsoring_Partner && w.id != (int)PESClass.SignaturesStep.Risk_Management).ToList().ForEach(ed =>
                                {
                                    ed.description = "";
                                    ed.annual_rating = "";
                                });
                            }
                            else if (nViewMode.Value == (int)PESClass.SignaturesStep.Group_Head || nViewMode.Value == (int)PESClass.SignaturesStep.HOP || nViewMode.Value == (int)PESClass.SignaturesStep.Risk_Management)
                            {
                                lstReturn.Where(w => w.nStep > nSeqStep.Value).ToList().ForEach(ed =>
                                {
                                    ed.description = "";
                                    ed.annual_rating = "";
                                });
                            }
                            else if (nViewMode.Value == (int)PESClass.SignaturesStep.Nominating)
                            {
                                lstReturn.Where(w => w.id != nViewMode.Value).ToList().ForEach(ed =>
                                {
                                    ed.description = "";
                                    ed.annual_rating = "";
                                });
                            }
                        }

                    }






                }
            }

            return lstReturn.ToList();
        }

        public static string charge_Map(string rate, string rank, int pool, int unitgroup)
        {
            string sReturn = "";
            //itas
            if (unitgroup == 14100063)
            {
                //ptr 12
                if (rank =="12")
                {
                    sReturn = "45-60%";
                }
                //di 20 
                else if (rank == "20")
                {
                    sReturn = "50-70%";
                }
                //ad 32
                else if (rank == "32")
                {
                    sReturn = "60-70%";
                }
            }
            //audit
            else if (pool == 1)
            {
                //ptr 12
                if (rank == "12")
                {
                    sReturn = "45-60%";
                }
                //di 20 
                else if (rank == "20")
                {
                    sReturn = "50-70%";
                }
                //ad 32
                else if (rank == "32")
                {
                    sReturn = "60-70%";
                }
            }
            //none
            else 
            {
                //ptr 12
                if (rank == "12")
                {
                    sReturn = "45-60%";
                }
                //di 20 
                else if (rank == "20")
                {
                    sReturn = "50-70%";
                }
                //ad 32
                else if (rank == "32")
                {
                    sReturn = "60-70%";
                }
            }
            return sReturn;
        }
    }
    public class SignatureReturn
    {
        public string sValue { get; set; }
    }

}