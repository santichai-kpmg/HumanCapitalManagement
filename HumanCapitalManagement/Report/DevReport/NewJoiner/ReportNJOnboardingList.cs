using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for ReportNJOnboardingList
/// </summary>
public class ReportNJOnboardingList : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader2;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ReportNJOnboardingList()
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 77F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 15.625F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.Dpi = 100F;
            this.PageHeader.HeightF = 100F;
            this.PageHeader.Name = "PageHeader";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Dpi = 100F;
            this.GroupHeader1.Expanded = false;
            this.GroupHeader1.HeightF = 100F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Dpi = 100F;
            this.GroupHeader2.Expanded = false;
            this.GroupHeader2.HeightF = 100F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 100F;
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 10.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(705.21F, 50.42F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "On boarding list (Audit + Tax)";
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 100F;
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 10.00001F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(9160.798F, 35.41667F);
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell33,
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45,
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55,
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrTableCell58,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell61,
            this.xrTableCell62});
            this.xrTableRow1.Dpi = 100F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 100F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "No";
            this.xrTableCell1.Weight = 0.15362089034417992D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 100F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "Start Date";
            this.xrTableCell2.Weight = 0.32644190910962728D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 100F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Company";
            this.xrTableCell3.Weight = 0.33087329967100176D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 100F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "Division";
            this.xrTableCell4.Weight = 0.42393071806327948D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 100F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Group";
            this.xrTableCell5.Weight = 0.432793174787113D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 100F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "Position Title";
            this.xrTableCell6.Weight = 0.44608737916631158D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 100F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Rank For Hiring";
            this.xrTableCell7.Weight = 0.52585059201726736D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 100F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "Type Of Employeement";
            this.xrTableCell8.Weight = 0.73855374154749109D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 100F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Text = "(EN) Prefix";
            this.xrTableCell9.Weight = 0.38848059419250236D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 100F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "(EN)First Name";
            this.xrTableCell10.Weight = 0.707534775466793D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 100F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "(EN) Last Name";
            this.xrTableCell11.Weight = 0.68980892706991959D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 100F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "(EN) Nick Name";
            this.xrTableCell12.Weight = 0.68980892706991959D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 100F;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Text = "(TH) Name";
            this.xrTableCell13.Weight = 0.68980892706991959D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 100F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Gender";
            this.xrTableCell14.Weight = 0.4283618036957314D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 100F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "Date Of Birth";
            this.xrTableCell15.Weight = 0.57016369759403962D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 100F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Birthplace";
            this.xrTableCell16.Weight = 0.57016369759403962D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 100F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Text = "Country Of Birth";
            this.xrTableCell17.Weight = 0.66322107781807249D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 100F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Text = "Marital Status";
            this.xrTableCell18.Weight = 0.54357585772915606D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Dpi = 100F;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Text = "Nationality";
            this.xrTableCell19.Weight = 0.54357585772915606D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Dpi = 100F;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Text = "Identification Number";
            this.xrTableCell20.Weight = 0.70753442583347637D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Dpi = 100F;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Text = "(P) House No.";
            this.xrTableCell21.Weight = 0.52141968174730091D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 100F;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Text = "(P) Moo &   Soi";
            this.xrTableCell22.Weight = 0.52141968174730091D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 100F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Text = "(P) Road";
            this.xrTableCell23.Weight = 0.52141968174730091D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Dpi = 100F;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Text = "(P) Sub-District (แขวง/ตำบล)";
            this.xrTableCell24.Weight = 0.87592499014406322D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Dpi = 100F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Text = "(P) District (เขต/อำเภอ)";
            this.xrTableCell25.Weight = 0.68980826138585438D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Dpi = 100F;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Text = "(P) Province";
            this.xrTableCell26.Weight = 0.38848113637434167D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 100F;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Text = "(P) Postal Code";
            this.xrTableCell27.Weight = 0.51698751464409753D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Dpi = 100F;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Text = "(P) Country";
            this.xrTableCell28.Weight = 0.40620625087460532D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Dpi = 100F;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Text = "(P) Telephone Name";
            this.xrTableCell29.Weight = 0.64106493107209939D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Dpi = 100F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Text = "(P) Mobile";
            this.xrTableCell30.Weight = 0.37961846136061472D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Dpi = 100F;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Text = "(C) House No.";
            this.xrTableCell31.Weight = 0.47710848671343886D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Dpi = 100F;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Text = "(C) Moo & Soi";
            this.xrTableCell32.Weight = 0.52142185834207688D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Dpi = 100F;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Text = "(C)Road";
            this.xrTableCell33.Weight = 0.57902810074613176D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Dpi = 100F;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Text = "(C) Sub-District (แขวง/ตำบล)";
            this.xrTableCell34.Weight = 0.86263073905751475D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Dpi = 100F;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Text = "(C) District (เขต/อำเภอ)";
            this.xrTableCell35.Weight = 0.77843668553312817D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Dpi = 100F;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Text = "(C) Province";
            this.xrTableCell36.Weight = 0.77843668553312817D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Dpi = 100F;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Text = "(C) Postal Code";
            this.xrTableCell37.Weight = 0.77843668553312817D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Dpi = 100F;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Text = "(C) Country";
            this.xrTableCell38.Weight = 0.77843668553312817D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Dpi = 100F;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Text = "(C) Telephone Number";
            this.xrTableCell39.Weight = 0.77843668553312817D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Dpi = 100F;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.Text = "(C) Mobile";
            this.xrTableCell40.Weight = 0.66765480998548443D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Dpi = 100F;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Text = "Bank Account Name";
            this.xrTableCell41.Weight = 0.69867323859063679D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Dpi = 100F;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Text = "Bank Account Number";
            this.xrTableCell42.Weight = 0.71639775926052018D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Dpi = 100F;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Text = "Bank Account Branch Name";
            this.xrTableCell43.Weight = 1.039887344815577D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Dpi = 100F;
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.Text = "StudentID";
            this.xrTableCell44.Weight = 0.7385498764777D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Dpi = 100F;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.Text = "Social Security - TH";
            this.xrTableCell45.Weight = 0.7385498764777D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Dpi = 100F;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.Text = "Provident Fund - TH";
            this.xrTableCell46.Weight = 0.7385498764777D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Dpi = 100F;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.Text = "Death Contribution";
            this.xrTableCell47.Weight = 0.6366291487231851D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.Dpi = 100F;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.Text = "Country";
            this.xrTableCell48.Weight = 0.5967474014046803D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.Dpi = 100F;
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.Text = "(B) Current GPA Transcript";
            this.xrTableCell49.Weight = 0.86706052337941864D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Dpi = 100F;
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.Text = "Certificate";
            this.xrTableCell50.Weight = 0.70753353224271387D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.Dpi = 100F;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Text = "(B)Major study";
            this.xrTableCell51.Weight = 0.70753353224271387D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.Dpi = 100F;
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Text = "Recruitment Status";
            this.xrTableCell52.Weight = 0.70753353224271387D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Dpi = 100F;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Text = "English Test Name";
            this.xrTableCell53.Weight = 0.70753353224271387D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.Dpi = 100F;
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.Text = "English Test Scores";
            this.xrTableCell54.Weight = 0.70753353224271387D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Dpi = 100F;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Text = "English Test Date ";
            this.xrTableCell55.Weight = 0.70753353224271387D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.Dpi = 100F;
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.Text = "Sourcing Channel";
            this.xrTableCell56.Weight = 0.70753353224271387D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Dpi = 100F;
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.Text = "Trainee Number";
            this.xrTableCell57.Weight = 0.70753353224271387D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Dpi = 100F;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.Text = "Military services doc";
            this.xrTableCell58.Weight = 0.70753353224271387D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.Dpi = 100F;
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.Text = "IBMP";
            this.xrTableCell59.Weight = 0.48596966738225184D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.Dpi = 100F;
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.Text = "Technical Test 1 Name";
            this.xrTableCell60.Weight = 0.69423965343607541D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Dpi = 100F;
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Text = "Technical Test 1 Score";
            this.xrTableCell61.Weight = 0.80945221259648037D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Dpi = 100F;
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Text = "Technical Test Date";
            this.xrTableCell62.Weight = 0.80945221259648037D;
            // 
            // ReportNJOnboardingList
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupHeader2});
            this.Margins = new System.Drawing.Printing.Margins(26, 74, 77, 16);
            this.PageHeight = 1000;
            this.PageWidth = 15000;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
