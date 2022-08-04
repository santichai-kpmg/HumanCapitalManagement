using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for NominationReport
/// </summary>
public class NominationReport : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.Utils.ImageCollection imageCollection1;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLBEffectiveDate2;
    private XRLabel xrLabel5;
    private XRLabel xrLBCostCenter;
    private XRLabel xrLBPosition;
    private XRLabel xrLBFullName;
    private XRLabel xrLBEffective_Date;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLBForFooter;
    private XRLabel xrLBStaffID;
    private XRLabel xrLBName;
    private XRLabel xrLabel4;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel8;
    private XRPanel xrPanel1;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLine xrLine1;
    private XRLabel xrLabel1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public NominationReport()
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NominationReport));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBEffectiveDate2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBCostCenter = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBPosition = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBFullName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBEffective_Date = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLBForFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBStaffID = new DevExpress.XtraReports.UI.XRLabel();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrPanel1,
            this.xrLine1,
            this.xrLabel1,
            this.xrPictureBox1,
            this.xrPictureBox2,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLBEffectiveDate2,
            this.xrLabel5,
            this.xrLBName,
            this.xrLabel4,
            this.xrLBCostCenter,
            this.xrLBPosition,
            this.xrLBFullName,
            this.xrLBEffective_Date});
            this.Detail.HeightF = 1488.767F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 10F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 472.6667F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(761.9999F, 170.5F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = resources.GetString("xrLabel8.Text");
            // 
            // xrPanel1
            // 
            this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel1.BorderWidth = 1F;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel2});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 110F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(761.9999F, 335.8333F);
            this.xrPanel1.StylePriority.UseBorders = false;
            this.xrPanel1.StylePriority.UseBorderWidth = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10F, 66.00001F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(741.9999F, 251.5F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = resources.GetString("xrLabel3.Text");
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial Narrow", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10F, 10F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(675.7239F, 36.33334F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Shareholder / Partner / Director (Thailand / Myanmar / Laos)";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(191.5F, 48F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(311.6667F, 23F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial Narrow", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(191.5F, 23.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(311.6667F, 43F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Nomination";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = "C:\\Project\\HCMV2\\HumanCapitalManagement\\Image\\KPMG-RGB.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(147F, 61F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.BackColor = System.Drawing.Color.Thistle;
            this.xrPictureBox2.ImageUrl = "C:\\Project\\HCMV2\\HumanCapitalManagement\\Image\\since-by-WannaP.png";
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(70.92142F, 1338.589F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(165.9722F, 72.90637F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox2.StylePriority.UseBackColor = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(70.92142F, 1411.496F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(185.4167F, 67.27087F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "(Ms. Wanna Puttaprasat)\r\nManager\r\nHuman Resources\r\n";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(70.92142F, 1192.326F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(583.003F, 146.2639F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = resources.GetString("xrLabel6.Text");
            // 
            // xrLBEffectiveDate2
            // 
            this.xrLBEffectiveDate2.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBEffectiveDate2.LocationFloat = new DevExpress.Utils.PointFloat(357.7274F, 1146.312F);
            this.xrLBEffectiveDate2.Name = "xrLBEffectiveDate2";
            this.xrLBEffectiveDate2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBEffectiveDate2.SizeF = new System.Drawing.SizeF(219.7915F, 22.13196F);
            this.xrLBEffectiveDate2.StylePriority.UseFont = false;
            this.xrLBEffectiveDate2.Text = "xrLBEffectiveDate2";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(70.92163F, 1126.347F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(580.5555F, 42.09726F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "This is to inform you that based on your performance during the probation period," +
    " you have passed your probation with effective ";
            // 
            // xrLBName
            // 
            this.xrLBName.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBName.LocationFloat = new DevExpress.Utils.PointFloat(110.1331F, 1076.289F);
            this.xrLBName.Name = "xrLBName";
            this.xrLBName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBName.SizeF = new System.Drawing.SizeF(341.6666F, 23F);
            this.xrLBName.StylePriority.UseFont = false;
            this.xrLBName.Text = "xrLBName";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(70.92163F, 1076.289F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(39.21132F, 23.00003F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Dear";
            // 
            // xrLBCostCenter
            // 
            this.xrLBCostCenter.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBCostCenter.LocationFloat = new DevExpress.Utils.PointFloat(70.92163F, 1012.741F);
            this.xrLBCostCenter.Name = "xrLBCostCenter";
            this.xrLBCostCenter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBCostCenter.SizeF = new System.Drawing.SizeF(407.6389F, 23F);
            this.xrLBCostCenter.StylePriority.UseFont = false;
            this.xrLBCostCenter.Text = "xrLBCostCenter";
            // 
            // xrLBPosition
            // 
            this.xrLBPosition.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBPosition.LocationFloat = new DevExpress.Utils.PointFloat(70.92163F, 989.7409F);
            this.xrLBPosition.Name = "xrLBPosition";
            this.xrLBPosition.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBPosition.SizeF = new System.Drawing.SizeF(407.6389F, 23F);
            this.xrLBPosition.StylePriority.UseFont = false;
            this.xrLBPosition.Text = "xrLBPosition";
            // 
            // xrLBFullName
            // 
            this.xrLBFullName.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBFullName.LocationFloat = new DevExpress.Utils.PointFloat(70.92163F, 966.7408F);
            this.xrLBFullName.Name = "xrLBFullName";
            this.xrLBFullName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBFullName.SizeF = new System.Drawing.SizeF(407.6388F, 23F);
            this.xrLBFullName.StylePriority.UseFont = false;
            this.xrLBFullName.Text = "xrLBFullName";
            // 
            // xrLBEffective_Date
            // 
            this.xrLBEffective_Date.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBEffective_Date.LocationFloat = new DevExpress.Utils.PointFloat(70.92163F, 900.451F);
            this.xrLBEffective_Date.Name = "xrLBEffective_Date";
            this.xrLBEffective_Date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBEffective_Date.SizeF = new System.Drawing.SizeF(407.6389F, 23F);
            this.xrLBEffective_Date.StylePriority.UseFont = false;
            this.xrLBEffective_Date.Text = "xrLBEffective_Date";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 56.48148F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLBForFooter,
            this.xrLBStaffID});
            this.BottomMargin.HeightF = 94F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLBForFooter
            // 
            this.xrLBForFooter.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.5F);
            this.xrLBForFooter.LocationFloat = new DevExpress.Utils.PointFloat(145.3222F, 22.88448F);
            this.xrLBForFooter.Name = "xrLBForFooter";
            this.xrLBForFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBForFooter.SizeF = new System.Drawing.SizeF(432.589F, 61.11551F);
            this.xrLBForFooter.StylePriority.UseFont = false;
            this.xrLBForFooter.Text = "xrLBForFooter";
            // 
            // xrLBStaffID
            // 
            this.xrLBStaffID.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.xrLBStaffID.LocationFloat = new DevExpress.Utils.PointFloat(597.0087F, 10.00002F);
            this.xrLBStaffID.Name = "xrLBStaffID";
            this.xrLBStaffID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBStaffID.SizeF = new System.Drawing.SizeF(88.71521F, 23F);
            this.xrLBStaffID.StylePriority.UseFont = false;
            this.xrLBStaffID.StylePriority.UseTextAlignment = false;
            this.xrLBStaffID.Text = "xrLBStaffID";
            this.xrLBStaffID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            // 
            // NominationReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(46, 19, 56, 94);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "17.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.NominationReport_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void NominationReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }
}
