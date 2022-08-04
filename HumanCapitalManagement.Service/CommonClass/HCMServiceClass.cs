using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.CommonClass
{
    public class HCMServiceClass
    {
        public enum StatusCandidate
        {
            Interview = 3,
            Accepted = 12,
            AddNew = 20,
            OfferAccepted = 12,
            Turndown = 15,
            NoShow = 16,
            OnBoard = 14,
            Shortlist_BU = 1,
            BU_Response = 2,
            Passed = 5,
            Passed_NotOffer = 6,
            Comparing = 8,
            Pending = 9,
            Send_Hiring = 10,
            Withdrawn = 19,
            Blacklist = 17,
            Offer_Rejected = 13,
            Reject_Before_Offer_Date = 22,
            Withdraw_the_Offer_KPMG = 24,
            Reject_Before_Sending_Hiring = 25,
     
        }

        public enum StatusPR
        {
            Save_Draft = 1,
            Awaiting_Approval = 2,
            Recruiting = 3,
            Completed = 4,
            Reject = 5,
            Cancel = 6,
        }
        public enum StatusTraineeEvaluation
        {
            Save_Draft = 1,
            Bu_Approve = 2,
            HR_Approve = 3,
            Complect = 4,
            Revise = 5,
            BuRevise = 6,
        }
        public enum UnitGroup
        {
            HR = 18,
            
        }
    }
}
