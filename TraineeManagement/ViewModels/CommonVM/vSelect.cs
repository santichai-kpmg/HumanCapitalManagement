using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TraineeManagement.ViewModels.CommonVM
{
    public class vSelect
    {
        public string id { get; set; }
        public string databind { get; set; }
        public string value { get; set; }
        [DefaultValue(false)]
        public bool disable { get; set; }
        public List<lstDataSelect> lstData { get; set; }

    }

    public class lstDataSelect
    {
        public string text { get; set; }
        public string detail { get; set; }
        public string value { get; set; }
        public int? point { get; set; }
    }


    public class vSelectBox
    {
        public string id { get; set; }
        public string[] avalue { get; set; }
        [DefaultValue(false)]
        public bool disable { get; set; }
        public List<lstDataSelectBox> lstData { get; set; }

    }
    public class vCandidateBox
    {
        public string id { get; set; }
        public string status_id { get; set; }
        public string[] avalue { get; set; }
        [DefaultValue(false)]
        public bool disable { get; set; }
        public List<lstDataSelectBox> lstData { get; set; }

    }
    public class lstDataSelectBox
    {
        public string text { get; set; }
        public string value { get; set; }
        public string Vgroup { get; set; }
        public string nFix { get; set; }
    }
}