﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HumanCapitalManagement.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class New_HRISEntities : DbContext
    {
        public New_HRISEntities()
            : base("name=New_HRISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<tbMaster_Company> tbMaster_Company { get; set; }
        public virtual DbSet<tbMaster_Unit> tbMaster_Unit { get; set; }
        public virtual DbSet<tbMaster_Pool> tbMaster_Pool { get; set; }
        public virtual DbSet<tbMaster_UnitGroup> tbMaster_UnitGroup { get; set; }
        public virtual DbSet<tbMaster_CompanyHead> tbMaster_CompanyHead { get; set; }
        public virtual DbSet<tbMaster_PoolHead> tbMaster_PoolHead { get; set; }
        public virtual DbSet<vw_Unit> vw_Unit { get; set; }
        public virtual DbSet<JobInfo> JobInfo { get; set; }
        public virtual DbSet<AllInfo_WS> AllInfo_WS { get; set; }
        public virtual DbSet<vw_StaffPhoto_FileName> vw_StaffPhoto_FileName { get; set; }
        public virtual DbSet<tbMaster_Rank> tbMaster_Rank { get; set; }
        public virtual DbSet<TB_OutSource> TB_OutSource { get; set; }
        public virtual DbSet<tbMaster_UnitGroupHead> tbMaster_UnitGroupHead { get; set; }
    }
}
