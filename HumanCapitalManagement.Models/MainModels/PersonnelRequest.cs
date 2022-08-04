using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class PersonnelRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual TM_Divisions TM_Divisions { get; set; }

        public int? TM_SubGroup_Id { get; set; }
        [ForeignKey("TM_SubGroup_Id")]
        public virtual TM_SubGroup TM_SubGroup { get; set; }
        public virtual TM_Employment_Request TM_Employment_Request { get; set; }
        public virtual TM_Pool_Rank TM_Pool_Rank { get; set; }
        public int? TM_Position_Id { get; set; }
        [ForeignKey("TM_Position_Id")]
        public virtual TM_Position TM_Position { get; set; }
        [StringLength(50)]
        public string RefNo { get; set; }

        [StringLength(50)]
        public string user_replaced { get; set; }

        [StringLength(10)]
        public string replaced_status { get; set; }


        public int no_of_headcount { get; set; }
        public DateTime? target_period { get; set; }
        public DateTime? target_period_to { get; set; }


        [MaxLengthAttribute()]
        public string job_descriptions { get; set; }
        [MaxLengthAttribute()]
        public string qualification_experience { get; set; }
        [MaxLengthAttribute()]
        public string remark { get; set; }
        //requset
        public DateTime? request_date { get; set; }
        [StringLength(50)]
        public string request_user { get; set; }

        //group head
        [StringLength(50)]
        public string Req_BUApprove_user { get; set; }
        public DateTime? BUApprove_date { get; set; }
        [StringLength(50)]
        public string BUApprove_user { get; set; }
        [StringLength(10)]
        public string BUApprove_status { get; set; }
        [StringLength(500)]
        public string BUApprove_remark { get; set; }
        //Pool head
        [StringLength(50)]
        public string Req_HeadApprove_user { get; set; }
        public DateTime? HeadApprove_date { get; set; }
        [StringLength(50)]
        public string HeadApprove_user { get; set; }
        [StringLength(10)]
        public string HeadApprove_status { get; set; }
        [StringLength(500)]
        public string HeadApprove_remark { get; set; }
        //Ceo Head
        [StringLength(50)]
        public string Req_CeoApprove_user { get; set; }
        public DateTime? CeoApprove_date { get; set; }
        [StringLength(50)]
        public string CeoApprove_user { get; set; }
        [StringLength(10)]
        public string CeoApprove_status { get; set; }
        [StringLength(500)]
        public string CeoApprove_remark { get; set; }
        [StringLength(10)]
        public string need_ceo_approve { get; set; }


        [StringLength(1000)]
        public string reject_reason { get; set; }
        [StringLength(50)]
        public string reject_user { get; set; }

        [StringLength(1000)]
        public string cancel_reason { get; set; }
        [StringLength(50)]
        public string cancel_user { get; set; }
        [StringLength(10)]
        public string type_of_TIFForm { get; set; }
        [MaxLengthAttribute()]
        public string hr_remark { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual TM_PR_Status TM_PR_Status { get; set; }
        public virtual ICollection<E_Mail_History> E_Mail_History { get; set; }
        public virtual ICollection<TM_PR_Candidate_Mapping> TM_PR_Candidate_Mapping { get; set; }
        public int? no_of_eva { get; set; }

        public bool ClearSub()
        {
            TM_SubGroup = null;// Nullable<Option<TM_SubGroup>>;//null;
            //_getData.TM_SubGroup = Nullable<T>;
            return true;
        }

        public DateTime? Approved_Date()
        {
            DateTime? sReturn = null;
            if (need_ceo_approve + "" == "Y")
            {
                sReturn = CeoApprove_date;
            }
            else
            {
                sReturn = HeadApprove_date;
            }
            return sReturn;
        }

    }

}
