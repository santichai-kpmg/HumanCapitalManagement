﻿using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.Probation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Probation_Group_QuestionRepository : RepositoryBase<TM_Probation_Group_Question>
    {
        public TM_Probation_Group_QuestionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
