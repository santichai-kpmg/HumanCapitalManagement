using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Feedbacks.MiniHeart_Main
{
    public class vMiniHeart_Question_obj
    {
        public int Id { get; set; }
        public int Seq { get; set; }
        public int Group { get; set; }
        public string Group_Text { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Use { get; set; }
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_User { get; set; }
        public string check { get; set; }
        public string remark { get; set; }
        public string icon { get; set; }
        public string proposition
        {
            get
            {
                string valueset = "<strong>";
                if (!String.IsNullOrEmpty(icon))
                {
                    valueset += "<span class='"+ icon + "'></span>";
                }

                valueset += " " + Topic + "</strong>";
                if (!String.IsNullOrEmpty(Content))
                {
                    valueset += " <br> " + Content;
                }
                return valueset;
            }
            set
            {
                this.proposition = value;
            }

        }

    }
    public class vMiniHeart_Question : CSearchMiniHeart
    {
        public List<vMiniHeart_Question> lstData { get; set; }
    }

    public class vMiniHeart_Question_Return : CResutlWebMethod
    {

        public vMiniHeart_Question_obj maindata { get; set; }
        public List<vMiniHeart_Question_obj> lstData { get; set; }
    }
}