using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_TIF_FormService : ServiceBase<TM_TIF_Form>
    {
        public TM_TIF_FormService(IRepository<TM_TIF_Form> repo) : base(repo)
        {
            //
        }
        public TM_TIF_Form Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_TIF_Form GetActiveTIFForm(int nID = 0)
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
        public IEnumerable<TM_TIF_Form> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_TIF_Form> GetTIF_FormForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_TIF_Form> GetTIF_Form(string name, string status)//,bool isAdmin
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
        #region tif eva

        public int easyQScore(List<int> Q)
        {
            int total_score = 0;
            var QScore = new Dictionary<int, int>();
            QScore.Add(4, 1);
            QScore.Add(5, 3);
            QScore.Add(6, 2);
            QScore.Add(7, 1);
            QScore.Add(8, 0);

            foreach (int i in Q)
            {
                total_score += QScore[i];
            }
            return total_score;
        }

        public int mediumQScore(List<int> Q)
        {
            int total_score = 0;
            var QScore = new Dictionary<int, int>();
            QScore.Add(4, 8);
            QScore.Add(5, 6);
            QScore.Add(6, 4);
            QScore.Add(7, 2);
            QScore.Add(8, 0);

            foreach (int i in Q)
            {
                total_score += QScore[i];
            }
            return total_score;
        }

        public int hardQScore(List<int> Q)
        {
            int total_score = 0;
            var QScore = new Dictionary<int, int>();
            QScore.Add(4, 12);
            QScore.Add(5, 9);
            QScore.Add(6, 6);
            QScore.Add(7, 3);
            QScore.Add(8, 0);

            foreach (int i in Q)
            {
                total_score += QScore[i];
            }
            return total_score;
        }
        public string auditQEva(List<int> answer_list, string status)
        {
            List<int> easy_q = answer_list.Skip(0).Take(2).ToList();
            List<int> medium_q = answer_list.Skip(2).Take(2).ToList();
            List<int> hard_q = answer_list.Skip(4).Take(1).ToList();

            int total = 0;
            total += easyQScore(medium_q);
            total += mediumQScore(medium_q);
            total += hardQScore(hard_q);

            if(total>=18 && total<=36)
            {
                return status;

            }else if (total>=10 && total<=17)
            {
                if(status == "i")
                {
                    return "n";
                }
                return status;
            }
            else
            {
                return "f";
            }
        }
        public bool checkFailed(List<int> answerList)
        {
            if (answerList.Contains(9))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string InternEva(List<int> answerList)
        {
            bool status = true;
            int core2 = 3;
            int core3 = 3;
            string stringStatus = "i";
            status = checkFailed(answerList);
            // slice answes in cores
            List<int> core1_answer = answerList.Skip(0).Take(3).ToList();
            List<int> core2_answer = answerList.Skip(3).Take(3).ToList();
            List<int> core3_answer = answerList.Skip(6).Take(3).ToList();
            List<int> core4_answer = answerList.Skip(9).Take(2).ToList();
            List<int> core5_answer = answerList.Skip(11).Take(2).ToList();
            List<int> core6_answer = answerList.Skip(13).Take(4).ToList();
            //5,6,7,8,9
            //check core1
            if (core1_answer.Contains(8))
            {
                status = false;
            }

            //check core2&3
            core2 = checkCore(core2_answer);
            core3 = checkCore(core3_answer);

            List<int> subCoreAnswerList = new List<int>();
            subCoreAnswerList.Add(core2);
            subCoreAnswerList.Add(core3);

            if (status)
            {
                stringStatus = checkSubCore(subCoreAnswerList);
                // check acknowledge and future core
                if (stringStatus == "i")
                {
                    //merge uncompetency answer list
                    core4_answer.AddRange(core5_answer);
                    core4_answer.AddRange(core6_answer);
                    bool isDownGrade = checkUncompentecy(core4_answer);
                    if (isDownGrade)
                    {
                        return "n";
                    }

                }
                return stringStatus;

            }
            return "f";
        }

        public bool checkUncompentecy(List<int> intList)
        {
            if (intList.Contains(8))
            {
                return true;
            }
            return false;
        }

        public string checkSubCore(List<int> answerList)
        {
            int counted2 = answerList.Where(i => i == 2).ToList().Count;
            int counted3 = answerList.Where(i => i == 3).ToList().Count;
            if (counted2 == 2)
            {
                return "l";
            }
            else if (counted2 == 1 && counted3 == 0)
            {
                return "n";
            }
            if (answerList.Contains(3))
            {
                return "f";
            }
            else return "i";
        }
        public int checkCore(List<int> answerList)
        // 1 = all pass, 2 = 4 in answer, 3 = failed
        {
            int count = 0;
            foreach (int i in answerList)
            {
                if (i >= 5 && i <= 7)
                {
                    count += 1;
                }
            }
            if (count == 3)
            {
                return 1;
            }
            else if (count == 2)
            {
                return 2;
            }
            else return 3;
        }
        #endregion
        #region Save Edit Delect 
        public int CreateNew(ref TM_TIF_Form s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_TIF_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TM_TIF_Form s)
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
    }
}
