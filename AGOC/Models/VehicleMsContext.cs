using Microsoft.EntityFrameworkCore;

namespace AGOC.Models;

public partial class VehicleMsContext : DbContext
{
    public VehicleMsContext()
    {
    }

    public VehicleMsContext(DbContextOptions<VehicleMsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LookupDepartment> LookupDepartments { get; set; }

    public virtual DbSet<LookupVehicleStatus> LookupVehicleStatuses { get; set; }

    public virtual DbSet<LookupViolationType> LookupViolationTypes { get; set; }

    public virtual DbSet<Parking> Parkings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }

    public virtual DbSet<TrafficViolation> TrafficViolations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleCategoryLookup> VehicleCategoryLookups { get; set; }

    public virtual DbSet<VehicleHandover> VehicleHandovers { get; set; }

    public virtual DbSet<VehicleStatus> VehicleStatuses { get; set; }

    public virtual DbSet<VehiclesLookupDetaile> VehiclesLookupDetailes { get; set; }

    public virtual DbSet<VehiclesLookupMain> VehiclesLookupMains { get; set; }

    public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<MessageRecipient> MessageRecipients { get; set; }
    public virtual DbSet<MessageStatus> MessageStatuses { get; set; }
    public virtual DbSet<MessageLog> MessageLogs { get; set; }
    public virtual DbSet<UserPreference> UserPreferences { get; set; }
    public virtual DbSet<BatchProcessing> BatchProcessings { get; set; }
    public virtual DbSet<MessageAnalytics> MessageAnalytics { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("data source=10.12.100.29;initial catalog=VehicleMS;Integrated Security=false;User Id=SVC_VMS;Password=R@nD0m@7788;Connection Timeout=9000;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LookupDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LookupDe__3214EC07A608C4E9");

            entity.ToTable("LookupDepartment");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<LookupVehicleStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LookupVe__3214EC075DF57F74");

            entity.ToTable("LookupVehicleStatus");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<LookupViolationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LookupVi__3214EC07397569D4");

            entity.ToTable("LookupViolationType");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ViolationType).HasMaxLength(100);
        });

        modelBuilder.Entity<Parking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Parking__43E7B631857E0F3C");

            entity.ToTable("Parking");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName).HasMaxLength(300);
            entity.Property(e => e.LicensePlateNumber).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ParkingSpotNumber).HasMaxLength(50);
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Parkings)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK_Parking_Vehicle1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07223851E7");

            entity.HasIndex(e => e.RoleDescription, "UQ__Roles__A2DDC1C9E155212A").IsUnique();

            entity.Property(e => e.CreatedByName).HasMaxLength(500);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByName).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleDescription).HasMaxLength(500);
        });

        modelBuilder.Entity<Smslog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SMSLogs__3214EC0788020288");

            entity.ToTable("SMSLogs");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.SentOn).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<TrafficViolation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TrafficV__18B6DC285B07D776");

            entity.ToTable("TrafficViolation");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FineAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LicensePlateNumber).HasMaxLength(50);
            entity.Property(e => e.LookupViolationTypeId).HasColumnName("LookupViolationTypeID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
            entity.Property(e => e.ViolationType).HasMaxLength(100);

            entity.HasOne(d => d.LookupViolationType).WithMany(p => p.TrafficViolations)
                .HasForeignKey(d => d.LookupViolationTypeId)
                .HasConstraintName("FK_TrafficViolation_LookupViolationType");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.TrafficViolations)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK_TrafficViolation_Vehicle");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07960C214C");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4AA900AD0").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345755662F").IsUnique();

            entity.Property(e => e.CreatedByName).HasMaxLength(500);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.LastName).HasMaxLength(500);
            entity.Property(e => e.ModifiedByName).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(500);
            entity.Property(e => e.Username).HasMaxLength(500);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC07D4A37424");

            entity.Property(e => e.CreatedByName).HasMaxLength(500);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedByName).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRoles__RoleI__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoles__UserI__619B8048");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicle__476B54B2A49E8C25");

            entity.ToTable("Vehicle");

            entity.Property(e => e.Brand).HasMaxLength(200);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LicensePlateNumber).HasMaxLength(200);
            entity.Property(e => e.Model).HasMaxLength(200);
            entity.Property(e => e.ModifiedBy).HasMaxLength(200);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SerialNumber).HasMaxLength(200);
            entity.Property(e => e.StatusText).HasMaxLength(200);
            entity.Property(e => e.VehicleTypeText).HasMaxLength(200);
            entity.Property(e => e.Year).HasMaxLength(50);

            entity.HasOne(d => d.Status).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Vehicle_LookupVehicleStatus");
        });

        modelBuilder.Entity<VehicleCategoryLookup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VehicleC__3214EC0719630214");

            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.VehiclesLookupDetail).WithMany(p => p.VehicleCategoryLookups)
                .HasForeignKey(d => d.VehiclesLookupDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VehicleCategoryLookups_VehiclesLookupDetaile");
        });

        modelBuilder.Entity<VehicleHandover>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VehicleH__DB2A1F61F31C71A1");

            entity.ToTable("VehicleHandover");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeDepartment).HasMaxLength(300);
            entity.Property(e => e.EmployeeDepartmentId).HasColumnName("EmployeeDepartmentID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeName).HasMaxLength(300);
            entity.Property(e => e.EmployeeTitle).HasMaxLength(300);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.StatusText).HasMaxLength(100);
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.VehicleHandovers)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK__VehicleHa__Vehic__29572725");
        });

        modelBuilder.Entity<VehicleStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VehicleS__3214EC07E379DB4A");

            entity.ToTable("VehicleStatus");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LookupVehicleStatusId).HasColumnName("LookupVehicleStatusID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.VehicleId).HasColumnName("VehicleID");
        });

        modelBuilder.Entity<VehiclesLookupDetaile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicles__3214EC070B6A48D4");

            entity.ToTable("VehiclesLookupDetaile");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.VehiclesLookupMainDescription).HasMaxLength(50);

            entity.HasOne(d => d.VehiclesLookupMain).WithMany(p => p.VehiclesLookupDetailes)
                .HasForeignKey(d => d.VehiclesLookupMainId)
                .HasConstraintName("FK_VehiclesLookupDetaile_VehiclesLookupMain");
        });

        modelBuilder.Entity<VehiclesLookupMain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vehicles__3214EC0787836D8F");

            entity.ToTable("VehiclesLookupMain");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
