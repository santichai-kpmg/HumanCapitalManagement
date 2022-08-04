using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.CommonVM
{
    public class CPTRSearchAll
    {
    }
    public class CSearchPTRMailBox
    {
        [DefaultValue("")]
        public string id { get; set; }

        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string division { get; set; }
    }
    public class CSearchPTRMasterData
    {

        [DefaultValue("")]
        public string id { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
    }
    public class CSearchPTREvaluation
    {

        [DefaultValue("")]
        public string id { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string fy_year { get; set; }
        [DefaultValue("")]
        public string status_id { get; set; }
        [DefaultValue("")]
        public string group_id { get; set; }
        [DefaultValue("")]
        public string session { get; set; }
    }
    public class CSearchNMNAdmin
    {
        [DefaultValue("")]
        public string id { get; set; }

        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string division { get; set; }
    }
    public class CSearchNMNForm
    {

        [DefaultValue("")]
        public string id { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string fy_year { get; set; }
        [DefaultValue("")]
        public string status_id { get; set; }
        [DefaultValue("")]
        public string group_id { get; set; }
        [DefaultValue("")]
        public string session { get; set; }
        [DefaultValue("")]
        public string signature_id { get; set; }
    }
}