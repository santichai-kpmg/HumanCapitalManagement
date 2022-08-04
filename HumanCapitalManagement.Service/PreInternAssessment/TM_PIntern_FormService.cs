using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PreInternAssessment
{
    public class TM_PIntern_FormService : ServiceBase<TM_PIntern_Form>
    {
        public TM_PIntern_FormService(IRepository<TM_PIntern_Form> repo) : base(repo)
        {
            //
        }
        public TM_PIntern_Form Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_PIntern_Form GetActivePInternForm(int nID = 0)
        {
            if (nID != 0)
            {
                return Query().Where(s => s.Id == nID).OrderByDescending(o => o.action_date).FirstOrDefault();
            }
            else
            {
                return Query().Where(s => s.active_status == "Y").OrderByDescending(o => o.action_date).FirstOrDefault();
            }
           
        }
        public IEnumerable<TM_PIntern_Form> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_PIntern_Form> GetPIntern_FormForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_PIntern_Form> GetPIntern_Form(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref TM_PIntern_Form s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_PIntern_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TM_PIntern_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id != s.Id && w.active_status == "Y").ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "N";
                    ed.update_date = DateTime.Now;
                    ed.update_user = s.update_user;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion


        #region calculate score
        public string CalScore(List<int> answerList)
        {
            bool status = true;
            bool core1 = true;
            bool core2 = true;
            bool core3 = true;
            bool core4 = true;
            bool core5 = true;
            // check if 5 in answer
            status = checkFailed(answerList);
            // slice answes in cores
            List<int> core1_answer = answerList.Skip(0).Take(3).ToList();
            List<int> core2_answer = answerList.Skip(4).Take(3).ToList();
            List<int> core3_answer = answerList.Skip(7).Take(3).ToList();
            List<int> core4_answer = answerList.Skip(10).Take(3).ToList();
            List<int> core5_answer = answerList.Skip(13).Take(3).ToList();

            core1 = checkCorePass(core1_answer);
            core2 = checkCorePass(core2_answer);
            core3 = checkCorePass(core3_answer);
            core4 = checkCorePass(core4_answer);
            core5 = checkCorePass(core5_answer);

            

            if (status)
            {
                List<bool> conditions = new List<bool>();
                conditions.Add(core1);
                conditions.Add(core2);
                conditions.Add(core3);
                return checkConditionStatus(conditions);

            }
            return "2";
        }

        public string checkConditionStatus(List<bool> condition_list)
        {
            //pass = 1, pending = 3, not-pass=2
            int count = 0;
            foreach(bool cond in condition_list)
            {
                if (cond)
                {
                    count += 1;
                }
            }

            if(count == 3)
            {
                return "1";
            }
            else if(count == 2)
            {
                return "3";
            }
            else
            {
                return "2";
            }
        }
        public bool checkFailed(List<int> answerList)
        {
            if (answerList.Contains(5))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool checkCorePass(List<int> score_list)
        {
            int count = 0;
            foreach (var score in score_list)
            {
                if(score >= 1 && score <= 3)
                {
                    count += 1;
                }
            }
            // check status answer conditions
            if (count >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    
    }
}
