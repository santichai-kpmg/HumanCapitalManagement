﻿using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Candidate_StatusRepository : RepositoryBase<TM_Candidate_Status>
    {
        public TM_Candidate_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
