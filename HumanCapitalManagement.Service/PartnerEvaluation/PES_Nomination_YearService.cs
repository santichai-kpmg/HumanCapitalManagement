﻿using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_YearService : ServiceBase<PES_Nomination_Year>
    {
        public PES_Nomination_YearService(IRepository<PES_Nomination_Year> repo) : base(repo)
        {
            //
        }
        public PES_Nomination_Year Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public PES_Nomination_Year FindNowYear()
        {
            var dNow = DateTime.Now;
            return Query().FirstOrDefault(s => s.evaluation_year.Value.Year == dNow.Year);
        }
        public PES_Nomination_Year FindForSelect(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.active_status == "Y");
        }
        public IEnumerable<PES_Nomination_Year> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Year> GetCRank(string name, string status)//,bool isAdmin
        {
            var sQuery = Query();

            //if (!string.IsNullOrEmpty(name))
            //{
            //    sQuery = sQuery.Where(w => ((w.crank_name_en + "").Trim().ToLower()).Contains((name + "").Trim().ToLower() ?? ((w.crank_name_en + "").Trim().ToLower())));
            //}
            //if (!string.IsNullOrEmpty(status))
            //{
            //    sQuery = sQuery.Where(w => w.active_status == status);
            //}
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public bool CanSave(PES_Nomination_Year PES_Nomination_Year)
        {
            bool sCan = false;

            if (PES_Nomination_Year.Id == 0)
            {
                if (Query().Any())
                {
                    var sCheck = Query().FirstOrDefault(w => w.evaluation_year.Value.Year == PES_Nomination_Year.evaluation_year.Value.Year);
                    if (sCheck == null)
                    {
                        sCan = true;
                    }
                }
                else
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => w.evaluation_year.Value.Year == PES_Nomination_Year.evaluation_year.Value.Year && w.Id != PES_Nomination_Year.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        #region Save Edit Delect 
        public int CreateNew(ref PES_Nomination_Year s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(PES_Nomination_Year s)
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
