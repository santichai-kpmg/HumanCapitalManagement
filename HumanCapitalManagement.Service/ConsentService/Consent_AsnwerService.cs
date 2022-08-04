﻿using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.ConsentService
{
    public class Consent_AsnwerService : ServiceBase<Consent_Asnwer>
    {
        public Consent_AsnwerService(IRepository<Consent_Asnwer> repo) : base(repo)
        {
            //
        }
        public Consent_Asnwer Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.Active_Status == "Y");
        }
        public IEnumerable<Consent_Asnwer> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.Active_Status == "Y");
            return sQuery.ToList();
        }
       
        #region Save Edit Delect 
        public int CreateNew(ref Consent_Asnwer s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }

        public int CreateNewOrUpdate(Consent_Asnwer s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData == null)
            {
                Add(s);

                sResult = SaveChanges();
            }
            else
            {
               //Add Code for update

                sResult = SaveChanges();


            }
            //sResult = SaveChanges();
            return sResult;
        }

        #endregion
    }
}
