using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data;
using System.Globalization;

namespace HumanCapitalManagement.Models.OldTable
{
    [Table("TBmst_FYPlan")]
    public class FYPlan
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Plan ID")]
        public long Id { get; set; }

        public string Pool { get; set; }

        public string LOB { get; set; }

        public string FY { get; set; }

        public string ResigneeForcast { get; set; }

        public int EstimatePreviousFY { get; set; }

        public int NewHire { get; set; }

        public int Replace { get; set; }

        //Estimate Previous FY by rank
        public int epPTR { get; set; }
        public int epDIR { get; set; }
        public int epAD1 { get; set; }
        public int epAD2 { get; set; }
        public int epMGR1 { get; set; }
        public int epMGR2 { get; set; }
        public int epMGR3 { get; set; }
        public int epMGR4 { get; set; }
        public int epMGR5 { get; set; }
        public int epAM1 { get; set; }
        public int epAM2 { get; set; }
        public int epSR1 { get; set; }
        public int epSR2 { get; set; }
        public int epAA1 { get; set; }
        public int epAA2 { get; set; }
        public int epPARA { get; set; }
        public int epEA3 { get; set; }
        public int epEA2 { get; set; }
        public int epEA1 { get; set; }

        public int epAD()
        {
            return epAD2 + epAD1;
        }
        public int epMGR()
        {
            return epMGR5 + epMGR4 + epMGR3 + epMGR2 + epMGR1;
        }
        public int epAM()
        {
            return epAM2 + epAM1;
        }
        public int epSR()
        {
            return epSR2 + epSR1;
        }
        public int epAA()
        {
            return epAA2 + epAA1;
        }
        public int epEA()
        {
            return epEA3 + epEA2 + epEA1;
        }

        //New Hire by rank
        public int nhPTR { get; set; }
        public int nhDIR { get; set; }
        public int nhAD1 { get; set; }
        public int nhAD2 { get; set; }
        public int nhMGR1 { get; set; }
        public int nhMGR2 { get; set; }
        public int nhMGR3 { get; set; }
        public int nhMGR4 { get; set; }
        public int nhMGR5 { get; set; }
        public int nhAM1 { get; set; }
        public int nhAM2 { get; set; }
        public int nhSR1 { get; set; }
        public int nhSR2 { get; set; }
        public int nhAA1 { get; set; }
        public int nhAA2 { get; set; }
        public int nhPARA { get; set; }
        public int nhEA1 { get; set; }
        public int nhEA2 { get; set; }
        public int nhEA3 { get; set; }

        public int nhAD()
        {
            return nhAD2 + nhAD1;
        }
        public int nhMGR()
        {
            return nhMGR5 + nhMGR4 + nhMGR3 + nhMGR2 + nhMGR1;
        }
        public int nhAM()
        {
            return nhAM2 + nhAM1;
        }
        public int nhSR()
        {
            return nhSR2 + nhSR1;
        }
        public int nhAA()
        {
            return nhAA2 + nhAA1;
        }
        public int nhEA()
        {
            return nhEA3 + nhEA2 + nhEA1;
        }

        //Replacement by rank
        public int rpPTR { get; set; }
        public int rpDIR { get; set; }
        public int rpAD1 { get; set; }
        public int rpAD2 { get; set; }
        public int rpMGR1 { get; set; }
        public int rpMGR2 { get; set; }
        public int rpMGR3 { get; set; }
        public int rpMGR4 { get; set; }
        public int rpMGR5 { get; set; }
        public int rpAM1 { get; set; }
        public int rpAM2 { get; set; }
        public int rpSR1 { get; set; }
        public int rpSR2 { get; set; }
        public int rpAA1 { get; set; }
        public int rpAA2 { get; set; }
        public int rpPARA { get; set; }
        public int rpEA1 { get; set; }
        public int rpEA2 { get; set; }
        public int rpEA3 { get; set; }

        public int rpAD()
        {
            return rpAD2 + rpAD1;
        }
        public int rpMGR()
        {
            return rpMGR5 + rpMGR4 + rpMGR3 + rpMGR2 + rpMGR1;
        }
        public int rpAM()
        {
            return rpAM2 + rpAM1;
        }
        public int rpSR()
        {
            return rpSR2 + rpSR1;
        }
        public int rpAA()
        {
            return rpAA2 + rpAA1;
        }
        public int rpEA()
        {
            return rpEA3 + rpEA2 + rpEA1;
        }


        public int TotalHC()
        {
            return EstimatePreviousFY + Replace + NewHire;
        }

        public int TotalRequest()
        {
            return Replace + NewHire;
        }

        public int TotalPlan()
        {
            return EstimatePreviousFY + NewHire;
        }

        public int TotalOperatingPlan()
        {
            return epPTR + epDIR + epAD2 + epAD1 + epMGR5 + epMGR4 + epMGR3 + epMGR2 + epMGR1 + epAM2 + epAM1 + epSR2 + epSR1 + epAA2 + epAA1 + epPARA + epEA3 + epEA2 + epEA1;
        }

        public int TotalNewHire()
        {
            return nhPTR + nhDIR + nhAD2 + nhAD1 + nhMGR5 + nhMGR4 + nhMGR3 + nhMGR2 + nhMGR1 + nhAM2 + nhAM1 + nhSR2 + nhSR1 + nhAA2 + nhAA1 + nhPARA + nhEA3 + nhEA2 + nhEA1;
        }

        public int TotalReplaceRequest()
        {
            return rpPTR + rpDIR + rpAD2 + rpAD1 + rpMGR5 + rpMGR4 + rpMGR3 + rpMGR2 + rpMGR1 + rpAM2 + rpAM1 + rpSR2 + rpSR1 + rpAA2 + rpAA1 + rpPARA + rpEA3 + rpEA2 + rpEA1;
        }
    }
}