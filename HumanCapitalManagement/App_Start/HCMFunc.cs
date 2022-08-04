using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using static HumanCapitalManagement.Controllers.Common.LoginController;

namespace HumanCapitalManagement.App_Start
{
    public class HCMFunc
    {
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "HCM";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();

                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "HCM";
            try
            {
                byte[] cipherBytes = Convert.FromBase64String((cipherText.Replace(" ", "+")));
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {
                return "";

            }

        }
        public static string EncryptPES(string clearText)
        {
            string EncryptionKey = "trpkk";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string DecryptPES(string cipherText)
        {
            string EncryptionKey = "trpkk";
            try
            {
                byte[] cipherBytes = Convert.FromBase64String((cipherText.Replace(" ", "+")));
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {
                return "";

            }

        }
        public static string DataEncryptPES(string clearText)
        {
            string EncryptionKey = "w";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string DataDecryptPES(string cipherText)
        {
            string EncryptionKey = "w";
            try
            {
                if (!String.IsNullOrEmpty(cipherText))
                {
                    byte[] cipherBytes = Convert.FromBase64String((cipherText.Replace(" ", "+")));
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            cipherText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
                return cipherText;
            }
            catch
            {
                return "";

            }

        }
        public static List<Menu> CreatListMenu(List<Menu> Menu, string menu_id)
        {
            List<Menu> lstReturn = new List<Menu>();

            foreach (var lsM in Menu.Where(w => w.MENU_PARENT == menu_id).OrderBy(o => o.MENU_SEQ).ToList())
            {
                lstReturn.Add(lsM);
                if (lsM.MENU_SUB + "" == "Y")
                {
                    lstReturn = lstReturn.Concat(CreatListMenu(Menu, lsM.MENU_ID)).ToList();
                }
            }
            return lstReturn;
        }

        public static List<vSelectBoxGroup> Get_GroupForSelect()
        {
            StoreDb db = new StoreDb();
            List<vSelectBoxGroup> lstReturn = new List<vSelectBoxGroup>();
            lstReturn = db.TM_Divisions.Select(s => new vSelectBoxGroup
            {
                id = s.division_code + "",
                name = s.division_name_en,
                sgroup = s.TM_Pool.Pool_name_en

            }).ToList();
            return lstReturn;
        }
        public static List<AllInfo_WS> GetUpdateUser(string[] aUpdateUser)
        {
            List<AllInfo_WS> lstReturn = new List<AllInfo_WS>();
            if (aUpdateUser.Length > 0)
            {
                New_HRISEntities dbHr = new New_HRISEntities();
                List<EmpNoUpdate> _getEmpNO = (from item in dbHr.AllInfo_WS.Where(w => aUpdateUser.Contains(w.UserID))
                                               group new { item } by new { item.UserID } into grp
                                               select new EmpNoUpdate
                                               {
                                                   empNo = grp.OrderByDescending(o => o.item.Status).Select(s => s.item.EmpNo).FirstOrDefault()
                                               }).ToList();
                string[] aEmpno = _getEmpNO.Select(s => s.empNo).ToArray();
                lstReturn = dbHr.AllInfo_WS.Where(w => aEmpno.Contains(w.EmpNo)).ToList();
                lstReturn.ForEach(ed => { ed.UserID = (ed.UserID + "").Trim().ToLower(); });
            }

            return lstReturn;
        }
        public class EmpNoUpdate
        {
            public string empNo { get; set; }
        }
        public static string CreateApproveTable(PersonnelRequest obj)
        {
            string sHtml = "";
            if (obj != null)
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                List<string> lstUserApp = new List<string>();
                lstUserApp.Add(obj.request_user);
                lstUserApp.Add(obj.Req_BUApprove_user);
                lstUserApp.Add(obj.Req_HeadApprove_user);
                lstUserApp.Add(obj.Req_CeoApprove_user);
                if (!string.IsNullOrEmpty(obj.BUApprove_user))
                {
                    lstUserApp.Add(obj.BUApprove_user);
                }
                if (!string.IsNullOrEmpty(obj.HeadApprove_user))
                {
                    lstUserApp.Add(obj.HeadApprove_user);
                }
                if (!string.IsNullOrEmpty(obj.CeoApprove_user))
                {
                    lstUserApp.Add(obj.CeoApprove_user);
                }
                var _getApproveStep = db.TM_Step_Approve.Where(w => w.active_status == "Y").ToList();
                if (_getApproveStep.Any())
                {
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    List<vPersonnelAp_obj> lstApprove = new List<vPersonnelAp_obj>();
                    lstApprove = _getApproveStep.Select(s => new vPersonnelAp_obj
                    {
                        nStep = s.step,
                        step_name = (s.step_name_en + "").Replace("(", "<br/>("),

                    }).ToList();
                    if (_getEmp.Any())
                    {
                        var _getUserReq = _getApproveStep.Where(w => w.step == 1).FirstOrDefault();
                        if (_getUserReq != null)
                        {
                            vPersonnelAp_obj objApp = new vPersonnelAp_obj();
                        }
                        //Requested by
                        lstApprove.Where(w => w.nStep == 1).ToList().ForEach(ed =>
                         {
                             ed.app_name = _getEmp.Where(w => w.EmpNo == obj.request_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                             ed.approve_date = obj.request_date.HasValue ? obj.request_date.Value.DateTimebyCulture() : "";
                             ed.description = obj.remark + "";
                         });
                        //Requested by Group Head
                        lstApprove.Where(w => w.nStep == 2).ToList().ForEach(ed =>
                         {
                             if (obj.BUApprove_date.HasValue)
                             {
                                 ed.app_name = _getEmp.Where(w => w.EmpNo == obj.Req_BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                 +
                                 ""
                                 +
                                 ((!string.IsNullOrEmpty(obj.BUApprove_user) && obj.BUApprove_user != obj.Req_BUApprove_user) ?
                                 "<br/>(by "
                                 + _getEmp.Where(w => w.EmpNo == obj.BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                 + ")"
                                 : "");
                                 ed.approve_date = obj.BUApprove_date.Value.DateTimebyCulture();
                             }
                             else
                             {
                                 ed.app_name = _getEmp.Where(w => w.EmpNo == obj.Req_BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                 ed.approve_date = "";
                             }
                             ed.description = obj.BUApprove_remark + "";
                         });
                        //Approve by Functional HOP
                        lstApprove.Where(w => w.nStep == 3).ToList().ForEach(ed =>
                        {
                            if (obj.HeadApprove_date.HasValue)
                            {

                                ed.app_name = _getEmp.Where(w => w.EmpNo == obj.Req_HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                +
                                ""
                                +
                                ((!string.IsNullOrEmpty(obj.HeadApprove_user) && obj.HeadApprove_user != obj.Req_HeadApprove_user) ?
                                "<br/>(by "
                                + _getEmp.Where(w => w.EmpNo == obj.HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                + ")"
                                : "");
                                ed.approve_date = obj.HeadApprove_date.Value.DateTimebyCulture();
                            }
                            else
                            {
                                ed.app_name = _getEmp.Where(w => w.EmpNo == obj.Req_HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                ed.approve_date = "";
                            }
                            ed.description = obj.HeadApprove_remark + "";
                        });
                        //ceo approve
                        if (obj.need_ceo_approve + "" == "Y")
                        {
                            lstApprove.Where(w => w.nStep == 4).ToList().ForEach(ed =>
                            {
                                if (obj.CeoApprove_date.HasValue)
                                {

                                    ed.app_name = _getEmp.Where(w => w.EmpNo == obj.Req_CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                    +
                                    ""
                                    +
                                    ((!string.IsNullOrEmpty(obj.CeoApprove_user) && obj.CeoApprove_user != obj.Req_CeoApprove_user) ?
                                    "<br/>(by "
                                    + _getEmp.Where(w => w.EmpNo == obj.CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                    + ")"
                                    : "");
                                    ed.approve_date = obj.CeoApprove_date.Value.DateTimebyCulture();
                                }
                                else
                                {
                                    ed.app_name = _getEmp.Where(w => w.EmpNo == obj.Req_CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                    ed.approve_date = "";
                                }
                                ed.description = obj.CeoApprove_remark + "";
                            });
                        }
                        else
                        {
                            lstApprove.RemoveAll(w => w.nStep == 4);
                        }
                    }

                    #region table detail
                    sHtml = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width:400px"">
	                    <thead>
		                    <tr>
			
			                    <th scope=""col"" style=""background-color:#001a41; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Summary</span></span></span></th>
			                    <th scope=""col"" style=""background-color:#001a41; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Details</span></span></span></th>
		
		                    </tr>
	                    </thead>
	                    <tbody>";
                    string sHtmlTrRef = @"<tr>
			<td scope=""col"" style=""background:#538c00; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Ref. No.</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.RefNo + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrRef;
                    string sHtmlTrGroup = @"<tr>
			<td scope=""col"" style=""background:#fef266; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Group</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_Divisions.division_name_en + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrGroup;

                    string sEmploy = obj.TM_Employment_Request.TM_Employment_Type != null ? obj.TM_Employment_Request.TM_Employment_Type.employee_type_name_en : "";
                    string sReq = obj.TM_Employment_Request.TM_Request_Type != null ? obj.TM_Employment_Request.TM_Request_Type.request_type_name_en : "";
                    string sHtmlTrEmploy = @"<tr>
			<td scope=""col"" style=""background:#538c00; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Type of Employement</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEmploy + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrEmploy;
                    string sHtmlTrReq = @"<tr>
			<td scope=""col"" style=""background:#538c00; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Type of Request</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sReq + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrReq;

                    string sPosition = obj.TM_Position != null ? obj.TM_Position.position_name_en : "";
                    string sRank = obj.TM_Pool_Rank != null ? obj.TM_Pool_Rank.Pool_rank_name_en : "";

                    string sHtmlTrPosition = @"<tr>
			<td scope=""col"" style=""background:#fef266; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Position</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sPosition + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrPosition;

                    string sHtmlTrRank = @"<tr>
			<td scope=""col"" style=""background:#fef266; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Rank</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sRank + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrRank;

                    string sHtmlTrHC = @"<tr>
			<td scope=""col"" style=""background:#fef266; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;No. of HC</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.no_of_headcount + @"</span></span></td>
		
		                            </tr>";
                    sHtml += sHtmlTrHC;


                    sHtml += @"</tbody></table>";
                    #endregion

                    if (lstApprove.Any())
                    {
                        //sHtml += "<br/>";
                        sHtml += @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width:800px"">
	<thead>
		<tr>
			<th scope=""col"" style=""background-color:#001a41; border-color:#888888; width:25%"">&nbsp;</th>
			<th scope=""col"" style=""background-color:#001a41; border-color:#888888; width:25%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Approver</span></span></span></th>
			<th scope=""col"" style=""background-color:#001a41; border-color:#888888; width:15%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Approval Date</span></span></span></th>
			<th scope=""col"" style=""background-color:#001a41; border-color:#888888; width:35%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Remark</span></span></span></th>
		</tr>
	</thead>
	<tbody>";
                        foreach (var Item in lstApprove.OrderBy(o => o.nStep))
                        {
                            string sHtmlTr = @"<tr>
			<td scope=""col"" style=""background:#fef266; border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;{0}</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;{1}</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:center; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">{2}</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;{3}</span></span></td>
		                            </tr>";
                            string sFinal = string.Format(sHtmlTr, Item.step_name, Item.app_name, Item.approve_date, Item.description);
                            sHtml += sFinal;
                        }
                        sHtml += @"</tbody></table>";
                    }
                }

            }

            return sHtml;
        }
        #region Pre Intern
        public static string CreatPreInternFormTable(TM_Candidate_PIntern obj)
        {
            string sHtml = "";
            if (obj != null)
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                List<string> lstUserApp = new List<string>();
                if (obj.TM_Candidate_PIntern_Approv != null && obj.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                {
                    lstUserApp = obj.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s => s.Req_Approve_user).ToList();
                }


                sHtml = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width:400px"">
	                    <thead>
		                    <tr>
			
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Summary</span></span></span></th>
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Details</span></span></span></th>
		
		                    </tr>
	                    </thead>
	                    <tbody>";
                string sHtmlTrRef = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Ref. No.</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRef;
                string sHtmlTrGroup = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Group</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrGroup;
                string sHtmlTrCandidate = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Candidate Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px; color: rgb(0,32,96);"">&nbsp;" + obj.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + obj.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrCandidate;
                string sRecRank = obj.Recommended_Rank != null ? obj.Recommended_Rank.Pool_rank_name_en : "";
                string sHtmlTrRecRank = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Recommended Rank<br/>(After Interview)</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sRecRank + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRecRank;
                if (lstUserApp.Any())
                {
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    int nCount = 1;
                    foreach (var item in lstUserApp)
                    {
                        string sEvaluatiorName = _getEmp.Where(w => w.EmpNo == item).Select(s => s.EmpFullName).FirstOrDefault();
                        if (nCount == 1)
                        {
                            string sHtmlTr1st = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;1st Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>

                                     </ tr > ";
                            sHtml += sHtmlTr1st;
                        }
                        else if (nCount == 2)
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;2nd Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        else
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Additional Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        nCount++;
                    }

                }
                string sResult = obj.TM_PIntern_Status != null ? obj.TM_PIntern_Status.PIntern_status_name_en : "";
                string sHtmlTrResult = @"<tr>
			<td scope=""col"" style=""background:rgb(146,208,80); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Interview Result</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sResult + @"</span></span></td>
                       </tr>";
                sHtml += sHtmlTrResult;


                sHtml += @"</tbody></table>";

            }

            return sHtml;
        }
        public static string CreatPreIntern_Mass_FormTable(TM_Candidate_PIntern_Mass obj)
        {
            string sHtml = "";
            if (obj != null)
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                List<string> lstUserApp = new List<string>();
                if (obj.TM_Candidate_PIntern_Mass_Approv != null && obj.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                {
                    lstUserApp = obj.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s => s.Req_Approve_user).ToList();
                }


                sHtml = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width:400px"">
	                    <thead>
		                    <tr>
			
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Summary</span></span></span></th>
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Details</span></span></span></th>
		
		                    </tr>
	                    </thead>
	                    <tbody>";
                string sHtmlTrRef = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Ref. No.</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRef;
                string sHtmlTrGroup = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Group</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrGroup;
                string sHtmlTrCandidate = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Candidate Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px; color: rgb(0,32,96);"">&nbsp;" + obj.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + obj.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrCandidate;
                string sRecRank = obj.Recommended_Rank != null ? obj.Recommended_Rank.Pool_rank_name_en : "";
                string sHtmlTrRecRank = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Recommended Rank<br/>(After Interview)</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sRecRank + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRecRank;
                if (lstUserApp.Any())
                {
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    int nCount = 1;
                    foreach (var item in lstUserApp)
                    {
                        string sEvaluatiorName = _getEmp.Where(w => w.EmpNo == item).Select(s => s.EmpFullName).FirstOrDefault();
                        if (nCount == 1)
                        {
                            string sHtmlTr1st = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;1st Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>

                                     </ tr > ";
                            sHtml += sHtmlTr1st;
                        }
                        else if (nCount == 2)
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;2nd Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        else
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Additional Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        nCount++;
                    }

                }
                string sResult = obj.TM_PIntern_Status != null ? obj.TM_PIntern_Status.PIntern_status_name_en : "";
                string sHtmlTrResult = @"<tr>
			<td scope=""col"" style=""background:rgb(146,208,80); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Interview Result</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sResult + @"</span></span></td>
                       </tr>";
                sHtml += sHtmlTrResult;


                sHtml += @"</tbody></table>";

            }

            return sHtml;
        }
        #endregion

        public static string CreatTIFFormTable(TM_Candidate_TIF obj)
        {
            string sHtml = "";
            if (obj != null)
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                List<string> lstUserApp = new List<string>();
                if (obj.TM_Candidate_TIF_Approv != null && obj.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y"))
                {
                    lstUserApp = obj.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s => s.Req_Approve_user).ToList();
                }


                sHtml = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width:400px"">
	                    <thead>
		                    <tr>
			
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Summary</span></span></span></th>
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Details</span></span></span></th>
		
		                    </tr>
	                    </thead>
	                    <tbody>";
                string sHtmlTrRef = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Ref. No.</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRef;
                string sHtmlTrGroup = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Group</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrGroup;
                string sHtmlTrCandidate = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Candidate Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px; color: rgb(0,32,96);"">&nbsp;" + obj.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + obj.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrCandidate;
                string sRecRank = obj.Recommended_Rank != null ? obj.Recommended_Rank.Pool_rank_name_en : "";
                string sHtmlTrRecRank = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Recommended Rank<br/>(After Interview)</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sRecRank + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRecRank;
                if (lstUserApp.Any())
                {
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    int nCount = 1;
                    foreach (var item in lstUserApp)
                    {
                        string sEvaluatiorName = _getEmp.Where(w => w.EmpNo == item).Select(s => s.EmpFullName).FirstOrDefault();
                        if (nCount == 1)
                        {
                            string sHtmlTr1st = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;1st Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>

                                     </ tr > ";
                            sHtml += sHtmlTr1st;
                        }
                        else if (nCount == 2)
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;2nd Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        else
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Additional Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        nCount++;
                    }

                }
                string sResult = obj.TM_TIF_Status != null ? obj.TM_TIF_Status.tif_status_name_en : "";
                string sHtmlTrResult = @"<tr>
			<td scope=""col"" style=""background:rgb(146,208,80); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Interview Result</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sResult + @"</span></span></td>
                       </tr>";
                sHtml += sHtmlTrResult;


                sHtml += @"</tbody></table>";

            }

            return sHtml;
        }
        public static string CreatMassTIFFormTable(TM_Candidate_MassTIF obj)
        {
            string sHtml = "";
            if (obj != null)
            {
                StoreDb db = new StoreDb();
                New_HRISEntities dbHr = new New_HRISEntities();
                List<string> lstUserApp = new List<string>();
                if (obj.TM_Candidate_MassTIF_Approv != null && obj.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                {
                    lstUserApp = obj.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s => s.Req_Approve_user).ToList();
                }


                sHtml = @"<table border=""1"" cellpadding=""0"" cellspacing=""0"" style=""width:400px"">
	                    <thead>
		                    <tr>
			
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Summary</span></span></span></th>
			                    <th scope=""col"" style=""background-color:#6699ff; border-color:#888888; width:50%""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px""><span style=""color:#ffffff"">Details</span></span></span></th>
		
		                    </tr>
	                    </thead>
	                    <tbody>";
                string sHtmlTrRef = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Ref. No.</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRef;
                string sHtmlTrGroup = @"<tr>
			<td scope=""col"" style=""background:rgb(255,204,255); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Group</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + obj.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrGroup;
                string sHtmlTrCandidate = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Candidate Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px; color: rgb(0,32,96);"">&nbsp;" + obj.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + obj.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrCandidate;
                string sRecRank = obj.Recommended_Rank != null ? obj.Recommended_Rank.Pool_rank_name_en : "";
                string sHtmlTrRecRank = @"<tr>
			<td scope=""col"" style=""background:rgb(226,239,218); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Recommended Rank<br/>(After Interview)</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sRecRank + @"</span></span></td>
		
		                            </tr>";
                sHtml += sHtmlTrRecRank;
                if (lstUserApp.Any())
                {
                    string[] aUser = lstUserApp.ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                    int nCount = 1;
                    foreach (var item in lstUserApp)
                    {
                        string sEvaluatiorName = _getEmp.Where(w => w.EmpNo == item).Select(s => s.EmpFullName).FirstOrDefault();
                        if (nCount == 1)
                        {
                            string sHtmlTr1st = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;1st Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>

                                     </ tr > ";
                            sHtml += sHtmlTr1st;
                        }
                        else if (nCount == 2)
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;2nd Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        else
                        {
                            string sHtmlTr2nd = @"<tr>
			<td scope=""col"" style=""background:rgb(255,255,153); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Additional Evaluator's Name</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sEvaluatiorName + @"</span></span></td>
		
		                            </tr>";
                            sHtml += sHtmlTr2nd;
                        }
                        nCount++;
                    }

                }

                string sResult = obj.TM_MassTIF_Status != null ? obj.TM_MassTIF_Status.masstif_status_name_en : "";
                string sHtmlTrResult = @"<tr>
			<td scope=""col"" style=""background:rgb(146,208,80); border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;Interview Result</span></span></td>
			<td scope=""col"" style=""border-color:#888888; text-align:left; vertical-align:middle""><span style=""font-family:Arial,Helvetica,sans-serif""><span style=""font-size:12px"">&nbsp;" + sResult + @"</span></span></td>
                       </tr>";
                sHtml += sHtmlTrResult;

                sHtml += @" </tbody></table>";

            }

            return sHtml;
        }
        public static bool SendMail(vObjectMail_Send objMail, ref string msg)
        {
            wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
            bool bSuss = true;
            try
            {
                string UseMail = System.Web.Configuration.WebConfigurationManager.AppSettings["UseMail"];
                string demoMailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailTo"];
                string demoMailCc = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailCC"];
                bSuss = wsMail.SendMail(
                          objMail.mail_from
                          , objMail.title_mail_from
                          , UseMail != "1" ? demoMailTo + "" : objMail.mail_to
                          , UseMail != "1" ? demoMailCc + "" : objMail.mail_cc
                          , ""
                          , objMail.mail_subject
                          , objMail.mail_content);
            }
            catch (Exception e)
            {
                msg = e.Message;
                bSuss = false;

            }
            return bSuss;
        }
        public static bool SendMailPES(vObjectMail_Send objMail, ref string msg)
        {
            wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
            bool bSuss = true;
            try
            {
                string UseMail = System.Web.Configuration.WebConfigurationManager.AppSettings["UseMailPES"];
                string demoMailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailTo"];
                string demoMailCc = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailCC"];
                wsMail.SendMailAsync(
                 objMail.mail_from
                 , objMail.title_mail_from
                 , UseMail != "1" ? demoMailTo + "" : objMail.mail_to
                // , UseMail != "1" ? demoMailCc + ",sithakarn@kpmg.co.th,darunee@kpmg.co.th" : objMail.mail_cc
                    , UseMail != "1" ? demoMailCc + "" : objMail.mail_cc
                  //, UseMail != "1" ? "" : ""
                  , UseMail != "1" ? "" : "sithakarn@kpmg.co.th"
                 , objMail.mail_subject + ""
                 , objMail.mail_content);
            }
            catch (Exception e)
            {
                msg = e.Message;
                bSuss = false;

            }
            return bSuss;
        }
        public static bool SendMailEva(vObjectMail_Send objMail, ref string msg)
        {
            wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
            bool bSuss = true;
            try
            {
                string UseMail = System.Web.Configuration.WebConfigurationManager.AppSettings["UseMailTrainee"];
                string demoMailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailTo"];
                string demoMailCc = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailCC"];
                wsMail.SendMailAsync(
                           objMail.mail_from
                           , objMail.title_mail_from
                           , UseMail != "1" ? demoMailTo + "" : objMail.mail_to
                           , UseMail != "1" ? demoMailCc + "" : objMail.mail_cc
                           , UseMail != "1" ? "" : ""
                           , objMail.mail_subject
                           , objMail.mail_content);
            }
            catch (Exception e)
            {
                msg = e.Message;
                bSuss = false;

            }
            return bSuss;
        }
        public static bool SendTIFForm(vObjectMail_Send objMail, ref string msg)
        {
            wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
            bool bSuss = true;
            try
            {
                string UseMail = System.Web.Configuration.WebConfigurationManager.AppSettings["UseMailTIFForm"];
                string demoMailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailTo"];
                string demoMailCc = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailCC"];
                wsMail.SendMailAsync(
                          objMail.mail_from
                          , objMail.title_mail_from
                          , UseMail != "1" ? demoMailTo + "" : objMail.mail_to
                          , UseMail != "1" ? demoMailCc + "" : objMail.mail_cc
                          , UseMail != "1" ? "" : ""
                          , objMail.mail_subject
                          , objMail.mail_content);
            }
            catch (Exception e)
            {
                msg = e.Message;
                bSuss = false;

            }
            return bSuss;
        }
        public static string GetUrl(string sType, string sID, string Url, string emp_no)
        {

            string sUrl = "";
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(sType) && !string.IsNullOrEmpty(sID))
            {
                if (sType == "submit")
                {
                    obj.id = sType + Encrypt(sID);
                    obj.type = sType;
                    obj.emp_no = emp_no;
                    try
                    {

                        string qryStr = JsonConvert.SerializeObject(obj,
                                        Formatting.Indented,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore,
                                            DefaultValueHandling = DefaultValueHandling.Ignore,
                                        });
                        string qryStrEncrypt = Encrypt(qryStr);

                        sUrl = Url + "?qryStr=" + qryStrEncrypt;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }
                else
                {
                    obj.id = sType + ("RC" + sID + "M");
                    obj.type = sType;
                    obj.emp_no = emp_no;
                    try
                    {

                        string qryStr = JsonConvert.SerializeObject(obj,
                                        Formatting.Indented,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore,
                                            DefaultValueHandling = DefaultValueHandling.Ignore,
                                        });
                        string qryStrEncrypt = Encrypt(qryStr);
                        sUrl = Url + "?qryStr=" + qryStrEncrypt;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }

            }
            return sUrl;
        }
        public static string GetUrlTrainee(string sType, string sID, string Url, string emp_no)
        {

            string sUrl = "";
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(sType) && !string.IsNullOrEmpty(sID))
            {
                obj.id = sType + Encrypt(sID);
                obj.type = sType;
                obj.emp_no = emp_no;
                try
                {

                    string qryStr = JsonConvert.SerializeObject(obj,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                    });
                    string qryStrEncrypt = Encrypt(qryStr);

                    sUrl = Url + "?qryStr=" + qryStrEncrypt;
                }
                catch (Exception)
                {
                    return "";
                }

            }
            return sUrl;
        }
        public static string GetUrlPTR(string sType, string sID, string Url, string emp_no)
        {
            string sUrl = "";
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(sType) && !string.IsNullOrEmpty(sID))
            {
                obj.id = sType + EncryptPES(sID);
                obj.type = sType;
                obj.emp_no = emp_no;
                try
                {
                    string qryStr = JsonConvert.SerializeObject(obj,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                    });
                    string qryStrEncrypt = EncryptPES(qryStr);

                    sUrl = Url + "?qryStr=" + qryStrEncrypt;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return sUrl;
        }
        public static string GetUrlTracking(string sType, string sID, string Url, string emp_no)
        {

            string sUrl = "";
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(sType) && !string.IsNullOrEmpty(sID))
            {
                obj.id = sType + ("RC" + sID + "M");
                obj.type = sType;
                obj.emp_no = emp_no;
                try
                {

                    string qryStr = JsonConvert.SerializeObject(obj,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                    });
                    string qryStrEncrypt = Encrypt(qryStr);
                    sUrl = Url + "?qryStr=" + qryStrEncrypt;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return sUrl;
        }

        public static string GetUrlFeedback(string sType, string sID, string Url, string emp_no, string emp_to)
        {

            string sUrl = "";
            vObjectMail_Feedback obj = new vObjectMail_Feedback();
            if (!string.IsNullOrEmpty(sType) && !string.IsNullOrEmpty(sID))
            {
                if (sType == "submit")
                {
                    obj.id = sType + Encrypt(sID);
                    obj.type = sType;
                    obj.emp_no = emp_no;
                    obj.emp_to = emp_to;
                    try
                    {

                        string qryStr = JsonConvert.SerializeObject(obj,
                                        Formatting.Indented,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore,
                                            DefaultValueHandling = DefaultValueHandling.Ignore,
                                        });
                        string qryStrEncrypt = Encrypt(qryStr);

                        sUrl = Url + "?qryStr=" + qryStrEncrypt;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }
                else
                {
                    obj.id = sType + ("RC" + sID + "M");
                    obj.type = sType;
                    obj.emp_no = emp_no;
                    obj.emp_to = emp_to;
                    try
                    {

                        string qryStr = JsonConvert.SerializeObject(obj,
                                        Formatting.Indented,
                                        new JsonSerializerSettings
                                        {
                                            NullValueHandling = NullValueHandling.Ignore,
                                            MissingMemberHandling = MissingMemberHandling.Ignore,
                                            DefaultValueHandling = DefaultValueHandling.Ignore,
                                        });
                        string qryStrEncrypt = Encrypt(qryStr);
                        sUrl = Url + "?qryStr=" + qryStrEncrypt;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }

            }
            return sUrl;
        }

        public static vObjectMail DecryptUrl(string qryString)
        {
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(qryString))
            {
                try
                {
                    var qryDecrypt = Decrypt(qryString);
                    obj = (vObjectMail)JsonConvert.DeserializeObject(qryDecrypt, (typeof(vObjectMail)));
                    obj.id = (obj.id + "").Replace(obj.type, "");

                }
                catch (Exception)
                {


                }
            }
            return obj;
        }
        public static vObjectMail_Feedback DecrypFeedback(string qryString)
        {
            vObjectMail_Feedback obj = new vObjectMail_Feedback();
            if (!string.IsNullOrEmpty(qryString))
            {
                try
                {
                    var qryDecrypt = Decrypt(qryString);
                    obj = (vObjectMail_Feedback)JsonConvert.DeserializeObject(qryDecrypt, (typeof(vObjectMail_Feedback)));
                    obj.id = (obj.id + "").Replace(obj.type, "");

                }
                catch (Exception)
                {


                }
            }
            return obj;
        }
        public static vObjectMail DecryptUrlPES(string qryString)
        {
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(qryString))
            {
                try
                {
                    var qryDecrypt = DecryptPES(qryString);
                    obj = (vObjectMail)JsonConvert.DeserializeObject(qryDecrypt, (typeof(vObjectMail)));
                    obj.id = (obj.id + "").Replace(obj.type, "");

                }
                catch (Exception)
                {


                }
            }
            return obj;
        }
        public static vObjectLogin_return funcLogin(string emp_no)
        {
            var getlogin = WebConfigurationManager.AppSettings["IsLogin"];
            vObjectLogin_return bReturn = new vObjectLogin_return();
            string winloginname = HttpContext.Current.User.Identity.Name;
            if (getlogin == "0")
                winloginname = "th\\"+WebConfigurationManager.AppSettings["IsUserID"];

            string EmpUserID = (winloginname.Substring(winloginname.IndexOf("\\") + 1) + "").ToLower();

            bReturn.sSucc = false;
            try
            {
                wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
                if (!string.IsNullOrEmpty(emp_no))
                {

                    DataTable staff = wsHRis.getEmployeeInfoByEmpNo(emp_no);

                    if (getlogin == "0")
                        staff = wsHRis.getEmployeeInfoByEmpNo(WebConfigurationManager.AppSettings["IsUserNo"]);

                    if (staff != null)
                    {
                        if (staff.Rows.Count == 1)
                        {
                            New_HRISEntities dbHr = new New_HRISEntities();
                            var userID = staff.Columns.Contains("UserID") ? (staff.Rows[0].Field<string>("UserID") + "").ToLower() : "";
                            var UserNo = staff.Columns.Contains("EmpNo") ? staff.Rows[0].Field<string>("EmpNo") + "" : "";
                            if (UserNo != "" && userID != "" && UserNo == emp_no && userID == EmpUserID)
                            {
                                User newUser = new User(userID);
                                //string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
                                newUser.UserId = staff.Columns.Contains("UserID") ? (staff.Rows[0].Field<string>("UserID") + "").ToLower() : "";
                                newUser.EmployeeNo = staff.Columns.Contains("EmpNo") ? staff.Rows[0].Field<string>("EmpNo") + "" : "";
                                newUser.FirstName = staff.Columns.Contains("EmpFirstName") ? staff.Rows[0].Field<string>("EmpFirstName") + "" : "";
                                newUser.LastName = staff.Columns.Contains("EmpSurname") ? staff.Rows[0].Field<string>("EmpSurname") + "" : "";
                                newUser.FullName = staff.Columns.Contains("EmpName") ? staff.Rows[0].Field<string>("EmpName") + "" : "";
                                newUser.EMail = staff.Columns.Contains("Email") ? staff.Rows[0].Field<string>("Email") + "" : "";
                                newUser.OfficePhone = staff.Columns.Contains("OfficePhone") ? staff.Rows[0].Field<string>("OfficePhone") + "" : "";
                                newUser.Company = staff.Columns.Contains("CompanyCode") ? staff.Rows[0].Field<string>("CompanyCode") + "" : "";
                                newUser.PI = staff.Columns.Contains("PI") ? staff.Rows[0].Field<string>("PI") + "" : "";
                                newUser.Pool = staff.Columns.Contains("Pool") ? staff.Rows[0].Field<string>("Pool") + "" : "";
                                newUser.Division = staff.Columns.Contains("Division") ? staff.Rows[0].Field<string>("Division") + "" : "";
                                newUser.UnitGroup = staff.Columns.Contains("UnitGroup") ? staff.Rows[0].Field<string>("UnitGroup") + "" : "";
                                newUser.Rank = staff.Columns.Contains("RankCode") ? staff.Rows[0].Field<string>("RankCode") + "" : "";
                                newUser.RankID = staff.Columns.Contains("RankID") ? staff.Rows[0].Field<string>("RankID") + "" : "";
                                newUser.lstDivision = new List<lstDivision>();
                                var CheckDivi = dbHr.vw_Unit.Where(w => w.UnitID + "" == newUser.Division + "").FirstOrDefault();
                                if (CheckDivi != null)
                                {
                                    newUser.lstDivision.Add(new lstDivision
                                    {

                                        sID = CheckDivi.UnitGroupID + "",// dbHr.vw_Unit.Where(w => w.UnitID == newUser.Division).Select(s => s.UnitGroupID + "").FirstOrDefault(),
                                        sName = newUser.UnitGroup,
                                        from_role = "Y",
                                        sCompany_code = CheckDivi.CountryCode + "" == "TH" ? "4100" : CheckDivi.CompanyCode,
                                    });
                                }
                                var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
                                var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
                                var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == newUser.EmployeeNo && w.RankID == 0).ToList();
                                if (CheckCEO.Any())
                                {
                                    newUser.lstDivision.AddRange(dbHr.vw_Unit.AsEnumerable().Where(w => (CheckCEO.Select(s => s.CompanyID + "").ToArray()).Contains(w.CompanyID + "")).Select(s => new lstDivision
                                    {
                                        sID = s.UnitGroupID + "",
                                        sName = s.UnitName,
                                        from_role = "Y",
                                        sCompany_code = s.CountryCode + "" == "TH" ? "4100" : s.CompanyCode,
                                    }).ToList());
                                }
                                if (CheckPool.Any())
                                {
                                    newUser.lstDivision.AddRange(dbHr.vw_Unit.AsEnumerable().Where(w => (CheckPool.Select(s => s.PoolID + "").ToArray()).Contains(w.PoolID + "")).Select(s => new lstDivision
                                    {
                                        sID = s.UnitGroupID + "",
                                        sName = s.UnitName,
                                        from_role = "Y",
                                        sCompany_code = s.CountryCode + "" == "TH" ? "4100" : s.CompanyCode,
                                    }).ToList());
                                }
                                if (CheckGroupH.Any())
                                {
                                    newUser.lstDivision.AddRange(dbHr.vw_Unit.AsEnumerable().Where(w => (CheckGroupH.Select(s => s.UnitGroupID + "").ToArray()).Contains(w.UnitGroupID + "")).Select(s => new lstDivision
                                    {
                                        sID = s.UnitGroupID + "",
                                        sName = s.UnitName,
                                        from_role = "Y",
                                        sCompany_code = s.CountryCode + "" == "TH" ? "4100" : s.CompanyCode,
                                    }).ToList());
                                }


                                var GetPic = dbHr.vw_StaffPhoto_FileName.Where(w => w.Employeeno == newUser.EmployeeNo).FirstOrDefault();
                                try
                                {
                                    //string sPicturePath = @"\\thbkkfsr05\data3$\PMPS\HR\Photo\Phoomchai_All\Phoomchai_Active\";
                                    string sPicturePath = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];// "/thbkkfsr05/data3$/PMPS/HR/Photo/Phoomchai_All/Phoomchai_Active/";
                                    var absolutePath = HttpContext.Current.Server.MapPath("~/Image/noimgaaaa.png");
                                    if (GetPic != null)
                                    {
                                        sPicturePath = (sPicturePath + GetPic.PhotoName + ".jpg").Replace("/", "\\");
                                        //if (File.Exists(sPicturePath))
                                        //{
                                        //    // newUser.Picture = String.Format("data:image/jpg;base64,{0}", File.ReadAllBytes(sPicturePath));
                                        //    newUser.aPicture = File.ReadAllBytes(sPicturePath);
                                        //}
                                        //else if (File.Exists(absolutePath))
                                        //{
                                        //    //newUser.Picture = String.Format("data:image/png;base64,{0}", File.ReadAllBytes("~/Image/noimgaaaa.png"));
                                        //    newUser.aPicture = File.ReadAllBytes(absolutePath);
                                        //    //newUser.aPicture = File.ReadAllBytes("~/Image/noimgaaaa.png");
                                        //}
                                        //else
                                        //{
                                        //    newUser.aPicture = null;
                                        //}
                                        newUser.Picture = sPicturePath;
                                    }
                                }
                                catch (Exception)
                                {
                                    newUser.Picture = null;
                                }
                                HttpContext.Current.Session["UserInfo"] = newUser;
                                bReturn.sSucc = false;

                            }
                            else
                            {
                                bReturn.sSucc = true;
                            }

                        }
                        else
                        {
                            bReturn.sSucc = true;
                        }
                    }
                    else
                    {
                        bReturn.sSucc = true;
                    }
                }

            }
            catch (Exception ex)
            {
                bReturn.sSucc = true;
                // throw;
                return bReturn;
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Session["UserInfo"] as string))
            {
                bReturn.sSucc = true;
            }
            return bReturn;
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static class EnumUtil
        {
            public static IEnumerable<T> GetValues<T>()
            {
                return Enum.GetValues(typeof(T)).Cast<T>();
            }
        }

    }
    public static class SHA
    {
        private const int SALT_SIZE = 8;
        private const int NUM_ITERATIONS = 1000;
        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        public static string GenerateSHA256String(string inputString)
        {
            byte[] buf = new byte[SALT_SIZE];
            rng.GetBytes(buf);
            string salt = Convert.ToBase64String(buf);

            Rfc2898DeriveBytes deriver2898 = new Rfc2898DeriveBytes(inputString.Trim(), buf, NUM_ITERATIONS);
            string hash = Convert.ToBase64String(deriver2898.GetBytes(16));
            return salt + ':' + hash;
        }

        public static bool IsPasswordValid(string password, string saltHash)
        {
            string[] parts = saltHash.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)

                return false;
            byte[] buf = Convert.FromBase64String(parts[0]);
            Rfc2898DeriveBytes deriver2898 = new Rfc2898DeriveBytes(password.Trim(), buf, NUM_ITERATIONS);
            string computedHash = Convert.ToBase64String(deriver2898.GetBytes(16));
            return parts[1].Equals(computedHash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static int fwGetWorkingDays(this DateTime firstDay, DateTime lastDay, List<DateTime> bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
            {
                return 1;
                throw new ArgumentException("Incorrect last day " + lastDay);
            }
            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
    }
    public class ClearMapping
    {

        public static bool UpdateTM_SubGroup(int item)
        {
            StoreDb db = new StoreDb();
            var GetPersonnelRequest = db.PersonnelRequest.FirstOrDefault(x => x.Id == item);
            if (GetPersonnelRequest != null)
            {
                GetPersonnelRequest.TM_SubGroup = null;
                //value assign to other entity
                db.Entry(GetPersonnelRequest).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
    public static class EnumerableEx
    {
        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
        public static string TexttoReport(this string str)
        {
            string sReturn = "";
            if (!string.IsNullOrEmpty(str))
            {
                List<string> newList = str.Split(' ').ToList();

                foreach (var item in newList)
                {
                    if (item.Length > 40)
                    {
                        sReturn += (String.Join(" ", (item.SplitBy(40)).ToArray())) + " ";
                    }
                    else
                    {
                        sReturn += item + " ";
                    }
                }
            }
            return sReturn;
        }
        public static string TexttoReportnewline(this string str, int nCut = 45)
        {
            string sReturn = "";
            if (!string.IsNullOrEmpty(str))
            {
                List<string> newList = str.Split(' ').ToList();

                foreach (var item in newList)
                {
                    if (item.Length > nCut)
                    {
                        sReturn += (String.Join(Environment.NewLine + " ", (item.SplitBy(nCut)).ToArray())) + " ";
                    }
                    else
                    {
                        sReturn += item + " ";
                    }
                }
            }
            sReturn += Environment.NewLine + "  ";
            return sReturn;
        }
    }


}