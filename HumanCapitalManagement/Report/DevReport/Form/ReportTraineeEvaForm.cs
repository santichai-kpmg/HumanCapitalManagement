using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace HumanCapitalManagement.Report.DevReport.Form
{
    public partial class ReportTraineeEvaForm : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportTraineeEvaForm()
        {
            InitializeComponent();
        }

        private void xrPicCheck_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void xrCellDevelop_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrCellLeaned_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
