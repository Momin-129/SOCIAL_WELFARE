using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServicePlus.Models.Entities;

public partial class ServicePlusContext : DbContext
{
    public ServicePlusContext()
    {
    }

    public ServicePlusContext(DbContextOptions<ServicePlusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CitizenDetailsResultModel> CitizenDetailsResultModels { get; set; }
    public virtual DbSet<Address> Address { get; set; }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<CitizenDetail> CitizenDetails { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<HalqaPanchayat> HalqaPanchayats { get; set; }

    public virtual DbSet<Otpstore> Otpstores { get; set; }

    public virtual DbSet<Pincode> Pincodes { get; set; }

    public virtual DbSet<RequestPhase> RequestPhases { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceSpecific> ServiceSpecifics { get; set; }

    public virtual DbSet<Tehsil> Tehsils { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<Village> Villages { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CitizenDetailsResultModel>().HasNoKey();
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2AFB47E94E3D");

            entity.ToTable("Address");

            entity.Property(e => e.AddressDetails).IsUnicode(false);

            entity.HasOne(d => d.Block).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.BlockId)
                .HasConstraintName("FK__Address__BlockId__12C8C788");

            entity.HasOne(d => d.District).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Address__Distric__10E07F16");

            entity.HasOne(d => d.HalqaPanchayat).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.HalqaPanchayatId)
                .HasConstraintName("FK__Address__HalqaPa__13BCEBC1");

            entity.HasOne(d => d.Pincode).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.PincodeId)
                .HasConstraintName("FK__Address__Pincode__1699586C");

            entity.HasOne(d => d.Tehsil).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.TehsilId)
                .HasConstraintName("FK__Address__TehsilI__11D4A34F");

            entity.HasOne(d => d.Village).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.VillageId)
                .HasConstraintName("FK__Address__Village__14B10FFA");

            entity.HasOne(d => d.Ward).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK__Address__WardId__15A53433");
        });

        modelBuilder.Entity<BankDetail>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__BankDeta__65A475E7C6B0B05E");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
        });

        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__Block__65A475E7660AFC9E");

            entity.ToTable("Block");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.BlockName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");

            entity.HasOne(d => d.District).WithMany(p => p.Blocks)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Block__DistrictI__11158940");
        });

        modelBuilder.Entity<CitizenDetail>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__CitizenD__BDA103F53CCB6D8C");

            entity.ToTable("CitizenDetail");

            entity.Property(e => e.ApplicantImage).IsUnicode(false);
            entity.Property(e => e.ApplicantName).IsUnicode(false);
            entity.Property(e => e.ApplicationStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Incomplete");
            entity.Property(e => e.Category).IsUnicode(false);
            entity.Property(e => e.DateOfSubmission)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.FatherGuardian)
                .IsUnicode(false)
                .HasColumnName("Father_Guardian");
            entity.Property(e => e.FormSpecific).IsUnicode(false);
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.BankDetails).WithMany(p => p.CitizenDetails)
                .HasForeignKey(d => d.BankDetailsId)
                .HasConstraintName("FK_BankDetails");

            entity.HasOne(d => d.PermanentAddress).WithMany(p => p.CitizenDetailPermanentAddresses)
                .HasForeignKey(d => d.PermanentAddressId)
                .HasConstraintName("FK_PermanentAddress");

            entity.HasOne(d => d.PresentAddress).WithMany(p => p.CitizenDetailPresentAddresses)
                .HasForeignKey(d => d.PresentAddressId)
                .HasConstraintName("FK_PresentAddress");

            entity.HasOne(d => d.Service).WithMany(p => p.CitizenDetails)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_Service");

            entity.HasOne(d => d.User).WithMany(p => p.CitizenDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenDetail_Users");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__District__65A475E763220CCC");

            entity.ToTable("District");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.DistrictName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__Document__65A475E7E14698B3");

            entity.ToTable("Document");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.DateOfSubmission)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Citizen).WithMany(p => p.Documents)
                .HasForeignKey(d => d.CitizenId)
                .HasConstraintName("FK__Document__Citize__60924D76");
        });

        modelBuilder.Entity<HalqaPanchayat>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__HalqaPan__65A475E72BB70FB4");

            entity.ToTable("HalqaPanchayat");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.PanchayatName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Block).WithMany(p => p.HalqaPanchayats)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HalqaPanc__Block__2057CCD0");
        });

        modelBuilder.Entity<Otpstore>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OTPSTORE");

            entity.Property(e => e.Otp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("otp");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OTPSTORE__UserID__1229A90A");
        });

        modelBuilder.Entity<Pincode>(entity =>
        {
            entity.HasKey(e => e.PincodeId).HasName("PK__Pincode__0632E301E85CEC0C");

            entity.ToTable("Pincode");

            entity.Property(e => e.PincodeId).HasColumnName("pincode_id");
            entity.Property(e => e.PincodeNumber).HasColumnName("pincode_number");
        });

        modelBuilder.Entity<RequestPhase>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__RequestP__65A475E7119BF50E");

            entity.ToTable("RequestPhase");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.Phases)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).HasMaxLength(255);

            entity.HasOne(d => d.Citizen).WithMany(p => p.RequestPhases)
                .HasForeignKey(d => d.CitizenId)
                .HasConstraintName("FK__RequestPh__Citiz__53385258");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__3E0DB8AFD72D5D45");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("department_name");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("service_name");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("state");
        });

        modelBuilder.Entity<ServiceSpecific>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__ServiceS__65A475E78D290518");

            entity.ToTable("ServiceSpecific");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.OfficerDocuments).HasMaxLength(255);
            entity.Property(e => e.Phases).HasMaxLength(255);

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceSpecifics)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceSp__Servi__06ED0088");
        });

        modelBuilder.Entity<Tehsil>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__Tehsil__65A475E71AF8E6A4");

            entity.ToTable("Tehsil");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.DistrictId).HasColumnName("DistrictID");
            entity.Property(e => e.TehsilName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.District).WithMany(p => p.Tehsils)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK__Tehsil__District__1A9EF37A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_PK");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Valid).HasDefaultValue(false);
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D8F62E1CAEDA");

            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.UserTypeValue)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Village>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__Village__65A475E706CB511C");

            entity.ToTable("Village");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.VillageName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.HalqaPanchayat).WithMany(p => p.Villages)
                .HasForeignKey(d => d.HalqaPanchayatId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Village__HalqaPa__2334397B");

            entity.HasOne(d => d.Tehsil).WithMany(p => p.Villages)
                .HasForeignKey(d => d.TehsilId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Village__TehsilI__24285DB4");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PK__Ward__65A475E7E4152A16");

            entity.ToTable("Ward");

            entity.Property(e => e.Uuid).HasColumnName("UUID");
            entity.Property(e => e.WardName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Village).WithMany(p => p.Wards)
                .HasForeignKey(d => d.VillageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ward__VillageId__2704CA5F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
