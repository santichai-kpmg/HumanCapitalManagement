using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Trainee_Eva_AnswerService : ServiceBase<TM_Trainee_Eva_Answer>
    {
        public TM_Trainee_Eva_AnswerService(IRepository<TM_Trainee_Eva_Answer> repo) : base(repo)
        {
            //
        }
        public TM_Trainee_Eva_Answer findByTraineeEvaId (int nId)
        {
           var sQuery = Query().Where(w => w.TM_Trainee_Eva.Id == nId).FirstOrDefault();

            return sQuery;
        }
        public int UpdateAnswer(List<TM_Trainee_Eva_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Trainee_Eva.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Evaluation_Question.Id).ToArray().Contains(w.TM_Evaluation_Question.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Trainee_Eva != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_Evaluation_Question.Id == item.TM_Evaluation_Question.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_Evaluation_Question.Id == item.TM_Evaluation_Question.Id).ToList().ForEach(ed =>
                    {
                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.trainee_rating = item.trainee_rating;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
        public int UpdateAnswerApprove(List<TM_Trainee_Eva_Answer> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Trainee_Eva.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_Evaluation_Question.Id).ToArray().Contains(w.TM_Evaluation_Question.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.TM_Trainee_Eva != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_Evaluation_Question.Id == item.TM_Evaluation_Question.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_Evaluation_Question.Id == item.TM_Evaluation_Question.Id).ToList().ForEach(ed =>
                    {
                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.inchage_rating = item.inchage_rating;
                        ed.inchage_comment = item.inchage_comment;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
