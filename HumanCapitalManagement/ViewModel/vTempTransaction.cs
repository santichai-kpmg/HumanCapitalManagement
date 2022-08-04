using HumanCapitalManagement.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
using StaffPersonalAdvanceProgram.App_Start;
using StaffPersonalAdvanceProgram.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
*/



namespace HumanCapitalManagement.ViewModel
{
    public class vTempTransaction
    {
        internal List<vListTempTransaction> lstTemp;

        public string Status { get; internal set; }
        public string Msg { get; internal set; }

        public class vTempTransaction_Save
        {

            public string Id { get; set; }

            public string IdEncrypt { get; set; }

            public string temp_file_name { get; set; }

            public string temp_description { get; set; }

            public string active_status { get; set; }

            public string update_user { get; set; }

            public string sfile_oldname { get; set; }

            public string id_company { get; set; }

            public string id_company_code { get; set; }

            public string date_create { get; set; }

            public string advancetype { get; set; }

            public string Session { get; set; }

            public List<vTempTransaction> lstTemp { get; set; }
        }

        public class lstTempTransaction_Return : CResutlWebMethod
        {
            public List<vTempTransaction_Return> lstData { get; set; }
        }
        public class vTempTransaction_Return
        {
            public string Id { get; set; }

            public string temp_file_name { get; set; }

            public string temp_description { get; set; }

            public string active_status { get; set; }

            public string update_user { get; set; }

            public string update_date { get; set; }

            public string id_company { get; set; }

            public string id_company_code { get; set; }

            public string create_date { get; set; }

            public string create_user { get; set; }

            public string advancetype { get; set; }
            public string Edit { get; set; }
            public string View { get; set; }
            public string Submit { get; set; }
        }

        public class vTempTransaction_Submit
        {
            public string Id { get; set; }

            public string IdEncrypt { get; set; }

            public string temp_file_name { get; set; }

            public string temp_description { get; set; }

            public string active_status { get; set; }

            public string create_date { get; set; }

            public string create_user { get; set; }

            public string id_company { get; set; }

            public string id_company_code { get; set; }

            public string advancetype { get; set; }

            public List<vTempTransaction_Submit> lstTempTransaction_Submit { get; set; }

        }

        public class vListTempTransaction
        {

        }


    }
}