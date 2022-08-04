using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for exProbation_Letter
/// </summary>
public class exProbation_Letter : DevExpress.XtraReports.UI.XtraReport
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
    private XRLabel xrLBCompanyEng;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLBFloor;
    private XRLabel xrLabel8;
    private XRLabel xrLabel12;
    private XRLabel xrLBFloorTH;
    private XRLabel xrLabel3;
    private XRLabel xrLabel9;
    private XRLabel xrLabel10;
    private XRLabel xrLabel11;
    private XRLabel xrLabel1;
    private XRLabel xrLBCompanyTh;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public exProbation_Letter()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(exProbation_Letter));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBFloor = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBFloorTH = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLBCompanyEng = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLBCompanyTh = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrLabel3,
            this.xrLBFloor,
            this.xrLabel8,
            this.xrLabel12,
            this.xrLabel1,
            this.xrLBFloorTH,
            this.xrLabel9,
            this.xrLabel10,
            this.xrLabel11,
            this.xrLBCompanyEng,
            this.xrPictureBox1,
            this.xrLBCompanyTh,
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
            this.Detail.HeightF = 797.1003F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.CanGrow = false;
            this.xrLabel3.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 47F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(183.5069F, 16F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "เขตสาทร กรุงเทพฯ 10120";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.TextTrimming = System.Drawing.StringTrimming.Word;
            this.xrLabel3.WordWrap = false;
            // 
            // xrLBFloor
            // 
            this.xrLBFloor.CanGrow = false;
            this.xrLBFloor.Font = new System.Drawing.Font("Univers for KPMG Light", 9F);
            this.xrLBFloor.LocationFloat = new DevExpress.Utils.PointFloat(173.81F, 25F);
            this.xrLBFloor.Name = "xrLBFloor";
            this.xrLBFloor.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBFloor.SizeF = new System.Drawing.SizeF(202.9198F, 17.4F);
            this.xrLBFloor.StylePriority.UseFont = false;
            this.xrLBFloor.StylePriority.UseTextAlignment = false;
            this.xrLBFloor.Text = "FR";
            this.xrLBFloor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLBFloor.WordWrap = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Univers for KPMG Light", 6F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(187.11F, 25F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(12.5F, 13F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "th";
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Univers for KPMG Light", 9F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(173.81F, 13F);
            this.xrLabel12.Multiline = true;
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(202.9198F, 114.1636F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "\r\n        \r\n1 South Sathorn Road, Yannawa\r\nSathorn, Bangkok 10120, Thailand\r\nTel " +
    "  +66 2677 2000\r\nFax  +66 2677 2222\r\nWebsite  home.kpmg/th";
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 30F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(183.5069F, 18F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "1 ถนนสาทรใต้ แขวงยานนาวา";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel1.TextTrimming = System.Drawing.StringTrimming.Word;
            this.xrLabel1.WordWrap = false;
            // 
            // xrLBFloorTH
            // 
            this.xrLBFloorTH.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLBFloorTH.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 25F);
            this.xrLBFloorTH.Name = "xrLBFloorTH";
            this.xrLBFloorTH.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLBFloorTH.ProcessDuplicatesMode = DevExpress.XtraReports.UI.ProcessDuplicatesMode.Merge;
            this.xrLBFloorTH.SizeF = new System.Drawing.SizeF(183.5069F, 18F);
            this.xrLBFloorTH.StylePriority.UseFont = false;
            this.xrLBFloorTH.StylePriority.UsePadding = false;
            this.xrLBFloorTH.Text = "FR";
            // 
            // xrLabel9
            // 
            this.xrLabel9.CanGrow = false;
            this.xrLabel9.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 58F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(183.5069F, 18F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UsePadding = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "โทร       +66 2677 2000";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel9.TextTrimming = System.Drawing.StringTrimming.Word;
            this.xrLabel9.WordWrap = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.CanGrow = false;
            this.xrLabel10.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 71F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(183.5069F, 18F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UsePadding = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "แฟกซ์   +66 2677 2222";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel10.TextTrimming = System.Drawing.StringTrimming.Word;
            this.xrLabel10.WordWrap = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 87F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(183.5068F, 20.38095F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "เว็บไซต์  home.kpmg/th";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel11.TextTrimming = System.Drawing.StringTrimming.Word;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLBCompanyEng
            // 
            this.xrLBCompanyEng.AutoWidth = true;
            this.xrLBCompanyEng.BorderWidth = 0F;
            this.xrLBCompanyEng.Font = new System.Drawing.Font("Univers for KPMG Light", 9F);
            this.xrLBCompanyEng.LocationFloat = new DevExpress.Utils.PointFloat(173.81F, 10F);
            this.xrLBCompanyEng.Name = "xrLBCompanyEng";
            this.xrLBCompanyEng.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLBCompanyEng.SizeF = new System.Drawing.SizeF(258.1598F, 18F);
            this.xrLBCompanyEng.StylePriority.UseBorderWidth = false;
            this.xrLBCompanyEng.StylePriority.UseFont = false;
            this.xrLBCompanyEng.StylePriority.UsePadding = false;
            this.xrLBCompanyEng.StylePriority.UseTextAlignment = false;
            this.xrLBCompanyEng.Text = "xrLBCompanyEng";
            this.xrLBCompanyEng.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = "C:\\Project\\HCMV2\\HumanCapitalManagement\\Image\\KPMG-RGB.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(147F, 61F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLBCompanyTh
            // 
            this.xrLBCompanyTh.AutoWidth = true;
            this.xrLBCompanyTh.BorderWidth = 0F;
            this.xrLBCompanyTh.Font = new System.Drawing.Font("CordiaUPC", 13.5F);
            this.xrLBCompanyTh.LocationFloat = new DevExpress.Utils.PointFloat(480.81F, 8.367439F);
            this.xrLBCompanyTh.Name = "xrLBCompanyTh";
            this.xrLBCompanyTh.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrLBCompanyTh.SizeF = new System.Drawing.SizeF(263.1875F, 18F);
            this.xrLBCompanyTh.StylePriority.UseBorderWidth = false;
            this.xrLBCompanyTh.StylePriority.UseFont = false;
            this.xrLBCompanyTh.StylePriority.UsePadding = false;
            this.xrLBCompanyTh.StylePriority.UseTextAlignment = false;
            this.xrLBCompanyTh.Text = "xrLBCompanyTh";
            this.xrLBCompanyTh.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.BackColor = System.Drawing.Color.Thistle;
            this.xrPictureBox2.ImageUrl = "C:\\Project\\HCMV2\\HumanCapitalManagement\\Image\\since-by-WannaP.png";
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(70.0881F, 654.2931F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(165.9722F, 72.90637F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox2.StylePriority.UseBackColor = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(70.0881F, 727.1996F);
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
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(70.0881F, 508.0292F);
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
            this.xrLBEffectiveDate2.LocationFloat = new DevExpress.Utils.PointFloat(356.894F, 462.0157F);
            this.xrLBEffectiveDate2.Name = "xrLBEffectiveDate2";
            this.xrLBEffectiveDate2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBEffectiveDate2.SizeF = new System.Drawing.SizeF(219.7915F, 22.13196F);
            this.xrLBEffectiveDate2.StylePriority.UseFont = false;
            this.xrLBEffectiveDate2.Text = "xrLBEffectiveDate2";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(70.08826F, 442.0504F);
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
            this.xrLBName.LocationFloat = new DevExpress.Utils.PointFloat(109.2996F, 391.9926F);
            this.xrLBName.Name = "xrLBName";
            this.xrLBName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBName.SizeF = new System.Drawing.SizeF(341.6666F, 23F);
            this.xrLBName.StylePriority.UseFont = false;
            this.xrLBName.Text = "xrLBName";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(70.08826F, 391.9926F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(39.21132F, 23.00003F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Dear";
            // 
            // xrLBCostCenter
            // 
            this.xrLBCostCenter.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBCostCenter.LocationFloat = new DevExpress.Utils.PointFloat(70.08826F, 328.4445F);
            this.xrLBCostCenter.Name = "xrLBCostCenter";
            this.xrLBCostCenter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBCostCenter.SizeF = new System.Drawing.SizeF(407.6389F, 23F);
            this.xrLBCostCenter.StylePriority.UseFont = false;
            this.xrLBCostCenter.Text = "xrLBCostCenter";
            // 
            // xrLBPosition
            // 
            this.xrLBPosition.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBPosition.LocationFloat = new DevExpress.Utils.PointFloat(70.08826F, 305.4445F);
            this.xrLBPosition.Name = "xrLBPosition";
            this.xrLBPosition.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBPosition.SizeF = new System.Drawing.SizeF(407.6389F, 23F);
            this.xrLBPosition.StylePriority.UseFont = false;
            this.xrLBPosition.Text = "xrLBPosition";
            // 
            // xrLBFullName
            // 
            this.xrLBFullName.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBFullName.LocationFloat = new DevExpress.Utils.PointFloat(70.08826F, 282.4445F);
            this.xrLBFullName.Name = "xrLBFullName";
            this.xrLBFullName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLBFullName.SizeF = new System.Drawing.SizeF(407.6388F, 23F);
            this.xrLBFullName.StylePriority.UseFont = false;
            this.xrLBFullName.Text = "xrLBFullName";
            // 
            // xrLBEffective_Date
            // 
            this.xrLBEffective_Date.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLBEffective_Date.LocationFloat = new DevExpress.Utils.PointFloat(70.08826F, 216.1547F);
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
            this.xrLBForFooter.Font = new System.Drawing.Font("Univers for KPMG Light", 5.5F);
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
            // exProbation_Letter
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
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
