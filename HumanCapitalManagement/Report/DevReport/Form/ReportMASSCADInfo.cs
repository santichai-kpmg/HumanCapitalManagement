using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for ReportMASSCADInfo
/// </summary>
public class ReportMASSCADInfo : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    private XRLabel xrLabel4;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRLabel xrCandidateName;
    private XRLabel xrGroup;
    private XRLabel xrPosition;
    private XRLabel xrRecRank;
    private XRLabel xrResult;
    private XRLabel xrCoreScore;
    private XRLabel xrAuditingScore;
    private XRLabel xrTotalScore;
    private XRLabel xrLabel8;
    private XRPanel xrPanel1;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel1;
    private XRLabel xrLabel3;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrlblPrintbt;
    private XRTable xrTable6;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell50;
    private XRTableCell xrFirstEvaName;
    private XRTableCell xrTableCell52;
    private XRTableCell xrSecEvaName;
    private XRTableRow xrTableRow25;
    private XRTableCell xrTableCell53;
    private XRTableCell xrFirstEvaRank;
    private XRTableCell xrTableCell55;
    private XRTableCell xrSecEvaRank;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell1;
    private XRTableCell xrFirstEvaGroup;
    private XRTableCell xrTableCell3;
    private XRTableCell xrSecEvaGroup;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell58;
    private XRTableCell xrFirstEvaDate;
    private XRTableCell xrTableCell60;
    private XRTableCell xrSecEvaDate;
    private XRTableCell xrTableCell10;
    private XRTable xrTable4;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private ReportFooterBand ReportFooter;
    private XRTable xrTable2;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell9;
    private XRTableCell xrComment;
    private XRPanel xrPanel2;
    private XRPictureBox xrPicCheck;
    private XRLabel xrLabel6;
    private HumanCapitalManagement.Report.DataSet.FormDataset.dsReportMASSTIFForm dsReportMASSTIFForm1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ReportMASSCADInfo()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportMASSCADInfo));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrlblPrintbt = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCandidateName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrGroup = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPosition = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRecRank = new DevExpress.XtraReports.UI.XRLabel();
            this.xrResult = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCoreScore = new DevExpress.XtraReports.UI.XRLabel();
            this.xrAuditingScore = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalScore = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrFirstEvaName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSecEvaName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrFirstEvaRank = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSecEvaRank = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrFirstEvaGroup = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSecEvaGroup = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrFirstEvaDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSecEvaDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrComment = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPicCheck = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.dsReportMASSTIFForm1 = new HumanCapitalManagement.Report.DataSet.FormDataset.dsReportMASSTIFForm();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsReportMASSTIFForm1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(780.0002F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell10});
            this.xrTableRow1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.StylePriority.UseBorders = false;
            this.xrTableRow1.StylePriority.UseFont = false;
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BorderWidth = 0.3F;
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "dsAdInfo.seq")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 2, 0, 100F);
            this.xrTableCell2.StylePriority.UseBorderWidth = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell2.Weight = 0.16329435578330476D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BorderWidth = 0.3F;
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "dsAdInfo.question")});
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 2, 0, 100F);
            this.xrTableCell4.StylePriority.UseBorderWidth = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.TextTrimming = System.Drawing.StringTrimming.None;
            this.xrTableCell4.Weight = 0.74355725243725335D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BorderWidth = 0.3F;
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "dsAdInfo.multi_answer")});
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 2, 0, 100F);
            this.xrTableCell5.StylePriority.UseBorderWidth = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.TextTrimming = System.Drawing.StringTrimming.None;
            this.xrTableCell5.Weight = 1.1219774552758062D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BorderWidth = 0.3F;
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "dsAdInfo.other_answer")});
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 2, 0, 100F);
            this.xrTableCell10.StylePriority.UseBorderWidth = false;
            this.xrTableCell10.StylePriority.UsePadding = false;
            this.xrTableCell10.TextTrimming = System.Drawing.StringTrimming.None;
            this.xrTableCell10.Weight = 0.97888405714568794D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 24F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrlblPrintbt});
            this.BottomMargin.HeightF = 32F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.Format = "Page {0} of {1}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(663.4168F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(114.5833F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrlblPrintbt
            // 
            this.xrlblPrintbt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrlblPrintbt.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrlblPrintbt.Name = "xrlblPrintbt";
            this.xrlblPrintbt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlblPrintbt.SizeF = new System.Drawing.SizeF(551.0416F, 23F);
            this.xrlblPrintbt.StylePriority.UseFont = false;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrPanel1,
            this.xrLabel1,
            this.xrLabel3,
            this.xrCandidateName,
            this.xrGroup,
            this.xrPosition,
            this.xrRecRank,
            this.xrResult,
            this.xrCoreScore,
            this.xrAuditingScore,
            this.xrTotalScore,
            this.xrLabel8,
            this.xrLabel4});
            this.PageHeader.HeightF = 282.2916F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable4
            // 
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 248.9583F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrTable4.SizeF = new System.Drawing.SizeF(780.0001F, 33.33333F);
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.xrTableRow5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableRow5.BorderWidth = 0.3F;
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18});
            this.xrTableRow5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.StylePriority.UseBackColor = false;
            this.xrTableRow5.StylePriority.UseBorders = false;
            this.xrTableRow5.StylePriority.UseBorderWidth = false;
            this.xrTableRow5.StylePriority.UseFont = false;
            this.xrTableRow5.StylePriority.UseTextAlignment = false;
            this.xrTableRow5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow5.Weight = 1.3333332824707032D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(69)))), ((int)(((byte)(161)))));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseForeColor = false;
            this.xrTableCell15.Text = "No.";
            this.xrTableCell15.Weight = 0.13334098706958669D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(69)))), ((int)(((byte)(161)))));
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseForeColor = false;
            this.xrTableCell16.Text = "Question";
            this.xrTableCell16.Weight = 0.6071671872602864D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(69)))), ((int)(((byte)(161)))));
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseForeColor = false;
            this.xrTableCell17.Text = "Answer";
            this.xrTableCell17.Weight = 0.91617410910554575D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(69)))), ((int)(((byte)(161)))));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseForeColor = false;
            this.xrTableCell18.Text = "Other Answer";
            this.xrTableCell18.Weight = 0.79932716098772971D;
            // 
            // xrPanel1
            // 
            this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
            this.xrPanel1.BorderWidth = 0.3F;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.xrPanel1.KeepTogether = false;
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(389F, 80F);
            this.xrPanel1.StylePriority.UseBorders = false;
            this.xrPanel1.StylePriority.UseBorderWidth = false;
            this.xrPanel1.StylePriority.UseFont = false;
            this.xrPanel1.StylePriority.UseTextAlignment = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(16.08505F, 9.999998F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(107.29F, 37F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            this.xrPictureBox1.StylePriority.UsePadding = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrLabel1.BorderWidth = 0.3F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(389.0001F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(391.0002F, 25F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseBorderWidth = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "TECHNICAL INTERVIEW FORM";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrLabel3.BorderWidth = 0.3F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(388.9999F, 25F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(391.0004F, 55F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseBorderWidth = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "This form is to be used for rating all individual interviewed and is part of the " +
    "official record. Please rate applicant according to the following definitions.";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCandidateName
            // 
            this.xrCandidateName.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrCandidateName.BorderWidth = 0.3F;
            this.xrCandidateName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrCandidateName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 79.99998F);
            this.xrCandidateName.Name = "xrCandidateName";
            this.xrCandidateName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrCandidateName.SizeF = new System.Drawing.SizeF(780.0003F, 25F);
            this.xrCandidateName.StylePriority.UseBorders = false;
            this.xrCandidateName.StylePriority.UseBorderWidth = false;
            this.xrCandidateName.StylePriority.UseFont = false;
            this.xrCandidateName.StylePriority.UsePadding = false;
            this.xrCandidateName.StylePriority.UseTextAlignment = false;
            this.xrCandidateName.Text = "Candidate Name:";
            this.xrCandidateName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrGroup
            // 
            this.xrGroup.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrGroup.BorderWidth = 0.3F;
            this.xrGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrGroup.LocationFloat = new DevExpress.Utils.PointFloat(0F, 105F);
            this.xrGroup.Name = "xrGroup";
            this.xrGroup.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrGroup.SizeF = new System.Drawing.SizeF(388.9999F, 25F);
            this.xrGroup.StylePriority.UseBorders = false;
            this.xrGroup.StylePriority.UseBorderWidth = false;
            this.xrGroup.StylePriority.UseFont = false;
            this.xrGroup.StylePriority.UsePadding = false;
            this.xrGroup.StylePriority.UseTextAlignment = false;
            this.xrGroup.Text = "Group:";
            this.xrGroup.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPosition
            // 
            this.xrPosition.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrPosition.BorderWidth = 0.3F;
            this.xrPosition.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPosition.LocationFloat = new DevExpress.Utils.PointFloat(0F, 130F);
            this.xrPosition.Name = "xrPosition";
            this.xrPosition.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPosition.SizeF = new System.Drawing.SizeF(388.9999F, 25F);
            this.xrPosition.StylePriority.UseBorders = false;
            this.xrPosition.StylePriority.UseBorderWidth = false;
            this.xrPosition.StylePriority.UseFont = false;
            this.xrPosition.StylePriority.UsePadding = false;
            this.xrPosition.StylePriority.UseTextAlignment = false;
            this.xrPosition.Text = "Position:";
            this.xrPosition.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrRecRank
            // 
            this.xrRecRank.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrRecRank.BorderWidth = 0.3F;
            this.xrRecRank.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRecRank.LocationFloat = new DevExpress.Utils.PointFloat(0F, 155F);
            this.xrRecRank.Name = "xrRecRank";
            this.xrRecRank.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrRecRank.SizeF = new System.Drawing.SizeF(388.9999F, 25F);
            this.xrRecRank.StylePriority.UseBorders = false;
            this.xrRecRank.StylePriority.UseBorderWidth = false;
            this.xrRecRank.StylePriority.UseFont = false;
            this.xrRecRank.StylePriority.UsePadding = false;
            this.xrRecRank.StylePriority.UseTextAlignment = false;
            this.xrRecRank.Text = "Recommended Rank (After interview):";
            this.xrRecRank.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrResult
            // 
            this.xrResult.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrResult.BorderWidth = 0.3F;
            this.xrResult.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrResult.LocationFloat = new DevExpress.Utils.PointFloat(0F, 180F);
            this.xrResult.Name = "xrResult";
            this.xrResult.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrResult.SizeF = new System.Drawing.SizeF(388.9999F, 25F);
            this.xrResult.StylePriority.UseBorders = false;
            this.xrResult.StylePriority.UseBorderWidth = false;
            this.xrResult.StylePriority.UseFont = false;
            this.xrResult.StylePriority.UsePadding = false;
            this.xrResult.StylePriority.UseTextAlignment = false;
            this.xrResult.Text = "Interview Result:";
            this.xrResult.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCoreScore
            // 
            this.xrCoreScore.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrCoreScore.BorderWidth = 0.3F;
            this.xrCoreScore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCoreScore.LocationFloat = new DevExpress.Utils.PointFloat(388.9998F, 105F);
            this.xrCoreScore.Name = "xrCoreScore";
            this.xrCoreScore.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrCoreScore.SizeF = new System.Drawing.SizeF(391.0005F, 25F);
            this.xrCoreScore.StylePriority.UseBorders = false;
            this.xrCoreScore.StylePriority.UseBorderWidth = false;
            this.xrCoreScore.StylePriority.UseFont = false;
            this.xrCoreScore.StylePriority.UsePadding = false;
            this.xrCoreScore.StylePriority.UseTextAlignment = false;
            this.xrCoreScore.Text = "Core Competency score:";
            this.xrCoreScore.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrAuditingScore
            // 
            this.xrAuditingScore.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrAuditingScore.BorderWidth = 0.3F;
            this.xrAuditingScore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrAuditingScore.LocationFloat = new DevExpress.Utils.PointFloat(388.9998F, 130F);
            this.xrAuditingScore.Name = "xrAuditingScore";
            this.xrAuditingScore.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrAuditingScore.SizeF = new System.Drawing.SizeF(391.0005F, 25F);
            this.xrAuditingScore.StylePriority.UseBorders = false;
            this.xrAuditingScore.StylePriority.UseBorderWidth = false;
            this.xrAuditingScore.StylePriority.UseFont = false;
            this.xrAuditingScore.StylePriority.UsePadding = false;
            this.xrAuditingScore.StylePriority.UseTextAlignment = false;
            this.xrAuditingScore.Text = "Auditing Question score:";
            this.xrAuditingScore.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTotalScore
            // 
            this.xrTotalScore.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTotalScore.BorderWidth = 0.3F;
            this.xrTotalScore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTotalScore.LocationFloat = new DevExpress.Utils.PointFloat(388.9999F, 155F);
            this.xrTotalScore.Name = "xrTotalScore";
            this.xrTotalScore.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTotalScore.SizeF = new System.Drawing.SizeF(391.0004F, 25F);
            this.xrTotalScore.StylePriority.UseBorders = false;
            this.xrTotalScore.StylePriority.UseBorderWidth = false;
            this.xrTotalScore.StylePriority.UseFont = false;
            this.xrTotalScore.StylePriority.UsePadding = false;
            this.xrTotalScore.StylePriority.UseTextAlignment = false;
            this.xrTotalScore.Text = "Total score:";
            this.xrTotalScore.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel8.BorderWidth = 0.3F;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(388.9998F, 180F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(391.0005F, 25F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseBorderWidth = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel4.BorderWidth = 0.3F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 217.7083F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(780.0001F, 31.25F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseBorderWidth = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Additional Information";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.PageFooter.HeightF = 100F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrTable6
            // 
            this.xrTable6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(2.000173F, 0F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow24,
            this.xrTableRow25,
            this.xrTableRow3,
            this.xrTableRow26});
            this.xrTable6.SizeF = new System.Drawing.SizeF(778F, 100F);
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UsePadding = false;
            this.xrTable6.StylePriority.UseTextAlignment = false;
            this.xrTable6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell50,
            this.xrFirstEvaName,
            this.xrTableCell52,
            this.xrSecEvaName});
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Weight = 1D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.StylePriority.UseTextAlignment = false;
            this.xrTableCell50.Text = "First Evaluator:";
            this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell50.Weight = 0.52870594014851791D;
            // 
            // xrFirstEvaName
            // 
            this.xrFirstEvaName.Name = "xrFirstEvaName";
            this.xrFirstEvaName.Weight = 1.4712940598514821D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.StylePriority.UseTextAlignment = false;
            this.xrTableCell52.Text = "Second Evalautor :";
            this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell52.Weight = 0.58226268150506111D;
            // 
            // xrSecEvaName
            // 
            this.xrSecEvaName.Name = "xrSecEvaName";
            this.xrSecEvaName.Weight = 1.4177373184949389D;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell53,
            this.xrFirstEvaRank,
            this.xrTableCell55,
            this.xrSecEvaRank});
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Weight = 1D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.StylePriority.UseTextAlignment = false;
            this.xrTableCell53.Text = "Rank:";
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell53.Weight = 0.52870605782555424D;
            // 
            // xrFirstEvaRank
            // 
            this.xrFirstEvaRank.Name = "xrFirstEvaRank";
            this.xrFirstEvaRank.Weight = 1.4712939421744458D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.StylePriority.UseTextAlignment = false;
            this.xrTableCell55.Text = "Rank:";
            this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell55.Weight = 0.58226268150506111D;
            // 
            // xrSecEvaRank
            // 
            this.xrSecEvaRank.Name = "xrSecEvaRank";
            this.xrSecEvaRank.Weight = 1.4177373184949389D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrFirstEvaGroup,
            this.xrTableCell3,
            this.xrSecEvaGroup});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Group:";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell1.Weight = 0.52870605782555424D;
            // 
            // xrFirstEvaGroup
            // 
            this.xrFirstEvaGroup.Name = "xrFirstEvaGroup";
            this.xrFirstEvaGroup.Weight = 1.4712939421744458D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Group:";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.Weight = 0.58226268150506111D;
            // 
            // xrSecEvaGroup
            // 
            this.xrSecEvaGroup.Name = "xrSecEvaGroup";
            this.xrSecEvaGroup.Weight = 1.4177373184949389D;
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell58,
            this.xrFirstEvaDate,
            this.xrTableCell60,
            this.xrSecEvaDate});
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Weight = 1D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.StylePriority.UseTextAlignment = false;
            this.xrTableCell58.Text = "Date:";
            this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell58.Weight = 0.52870594014851791D;
            // 
            // xrFirstEvaDate
            // 
            this.xrFirstEvaDate.Name = "xrFirstEvaDate";
            this.xrFirstEvaDate.Weight = 1.4712940598514821D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.StylePriority.UseTextAlignment = false;
            this.xrTableCell60.Text = "Date:";
            this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell60.Weight = 0.58226268150506111D;
            // 
            // xrSecEvaDate
            // 
            this.xrSecEvaDate.Name = "xrSecEvaDate";
            this.xrSecEvaDate.Weight = 1.4177373184949389D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrPanel2,
            this.xrLabel6});
            this.ReportFooter.HeightF = 73.95834F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.BorderWidth = 0.3F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0.0003814697F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable2.SizeF = new System.Drawing.SizeF(779.9999F, 25F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseBorderWidth = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.BorderColor = System.Drawing.Color.Black;
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrComment});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.StylePriority.UseBorderColor = false;
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Additional Comments :";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell9.Weight = 0.60456681608353191D;
            // 
            // xrComment
            // 
            this.xrComment.BorderColor = System.Drawing.Color.Black;
            this.xrComment.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.xrComment.Multiline = true;
            this.xrComment.Name = "xrComment";
            this.xrComment.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrComment.StylePriority.UseBorderColor = false;
            this.xrComment.StylePriority.UseFont = false;
            this.xrComment.StylePriority.UsePadding = false;
            this.xrComment.StylePriority.UseTextAlignment = false;
            this.xrComment.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrComment.TextTrimming = System.Drawing.StringTrimming.None;
            this.xrComment.Weight = 1.4005748086554506D;
            // 
            // xrPanel2
            // 
            this.xrPanel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPicCheck});
            this.xrPanel2.KeepTogether = false;
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0.0003814697F, 25F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(87.45829F, 48.95834F);
            this.xrPanel2.StylePriority.UseBackColor = false;
            this.xrPanel2.StylePriority.UseBorders = false;
            this.xrPanel2.StylePriority.UseTextAlignment = false;
            // 
            // xrPicCheck
            // 
            this.xrPicCheck.Image = ((System.Drawing.Image)(resources.GetObject("xrPicCheck.Image")));
            this.xrPicCheck.LocationFloat = new DevExpress.Utils.PointFloat(50.29157F, 0F);
            this.xrPicCheck.Name = "xrPicCheck";
            this.xrPicCheck.SizeF = new System.Drawing.SizeF(26.20837F, 23F);
            this.xrPicCheck.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.xrLabel6.ForeColor = System.Drawing.Color.MidnightBlue;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(87.45867F, 25F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(692.5416F, 48.95834F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = resources.GetString("xrLabel6.Text");
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // dsReportMASSTIFForm1
            // 
            this.dsReportMASSTIFForm1.DataSetName = "dsReportMASSTIFForm";
            this.dsReportMASSTIFForm1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ReportMASSCADInfo
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.dsReportMASSTIFForm1});
            this.DataMember = "dsAdInfo";
            this.DataSource = this.dsReportMASSTIFForm1;
            this.Margins = new System.Drawing.Printing.Margins(25, 22, 24, 32);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsReportMASSTIFForm1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
