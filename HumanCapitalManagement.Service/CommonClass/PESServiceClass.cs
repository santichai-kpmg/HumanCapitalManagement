using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.CommonClass
{
    public class PESServiceClass
    {
        public enum StepApprovePlan
        {
            Self = 1,
            gHead = 2,
            pHead = 3,
            Ceo = 4,

        }
        public enum StepApproveEvaluate
        {
            Self = 8,
            gHead = 5,
            pHead = 6,
            Ceo = 7,
        }
        public enum KPIsBase
        {
            Billing = 6,
            Chargeability = 5,
            Collections = 7,
            Contribution_margin = 2,
            Fee_Managed = 1,
            Lock_up = 4,
            Recovery_rate = 3,
        }
        public enum Eva_Status
        {
            Draft_Plan = 1,
            Waiting_for_Planning_Approval = 2,
            Waiting_for_Revised_Plan = 3,
            Planning_Completed = 4,
            Draft_Evaluate = 5,
            Waiting_for_Evaluation_Approval = 6,
            Waiting_for_Revised_Evaluate = 7,
            Evaluation_Completed = 8
        }
    }
}
