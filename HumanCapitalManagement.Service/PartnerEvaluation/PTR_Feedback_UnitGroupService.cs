using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Feedback_UnitGroupService : ServiceBase<PTR_Feedback_UnitGroup>
    {
        public PTR_Feedback_UnitGroupService(IRepository<PTR_Feedback_UnitGroup> repo) : base(repo)
        {
            //
        }
        public PTR_Feedback_UnitGroup Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public bool CanSave(PTR_Feedback_UnitGroup PTR_Feedback_UnitGroup)
        {
            bool sCan = false;


            var sCheck = Query().FirstOrDefault(w => w.PTR_Evaluation.Id == PTR_Feedback_UnitGroup.PTR_Evaluation.Id
                && w.unitcode == PTR_Feedback_UnitGroup.unitcode
                && w.active_status == "Y");
            if (sCheck == null)
            {
                sCan = true;
            }

            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(PTR_Feedback_UnitGroup s)
        {
            var sResult = 0;
            if (s.Id == 0)
            {
                Add(s);
            }
            else
            {
                var _getData = Query().Where(w => w.PTR_Evaluation.Id == s.PTR_Evaluation.Id && w.unitcode == s.unitcode).FirstOrDefault();
                if (_getData != null)
                {
                    _getData.active_status = s.active_status;
                    _getData.update_date = s.update_date;
                    _getData.update_user = s.update_user;
                    _getData.appreciations = s.appreciations;
                    _getData.active_status = s.active_status;
                    _getData.PTR_Evaluation = s.PTR_Evaluation;
                    _getData.recommendations = s.recommendations;
                    _getData.TM_Feedback_Rating = s.TM_Feedback_Rating;

                }
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int Update(PTR_Feedback_UnitGroup s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
