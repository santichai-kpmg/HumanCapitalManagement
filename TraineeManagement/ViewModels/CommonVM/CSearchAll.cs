using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TraineeManagement.ViewModels.CommonVM
{
    public class CSearchAll
    {
    }
    public class C_USERS_RETURN
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_last_name { get; set; }
        public string unit_name { get; set; }
        public string user_position { get; set; }
        public string rank_id { get; set; }
        public string user_rank { get; set; }

    }
    public class CSearchEvaluation
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchTimeSheet
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
}