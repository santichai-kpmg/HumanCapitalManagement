using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraineeManagement.App_Start
{
    public class TraineeClass
    {
        public enum StatusCandidate
        {
            Interview = 3,
            Accepted = 12,
            AddNew = 20,
            OfferAccepted = 12,
            Turndown = 15,
            NoShow = 16,
        }
        public enum StatusTraineeEvaluation
        {
            Save_Draft = 1,
            Bu_Approve = 2,
            HR_Approve = 3,
            Complect = 4,
            Revise = 5,
        }
    }
}