using Microsoft.EntityFrameworkCore;

namespace AGOC.Models;

public partial class AGOCContext : DbContext
{
    public AGOCContext()
    {
    }

    public AGOCContext(DbContextOptions<AGOCContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LookupDepartment> LookupDepartments { get; set; }
    public virtual DbSet<EmployeeInfo> EmployeeInfo { get; set; }


    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }


    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

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

        modelBuilder.Entity<EmployeeInfo>(entity =>
        {
            entity.HasKey(e => e.EmployeeID); // Set primary key
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.EmployeeNameEng).HasMaxLength(100);
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.DepartmentNameEn).HasMaxLength(100);
            entity.Property(e => e.SectionName).HasMaxLength(100);
            entity.Property(e => e.SectionNameEn).HasMaxLength(100);
            entity.Property(e => e.JobTitle).HasMaxLength(100);
            entity.Property(e => e.JobTitleEn).HasMaxLength(100);
            entity.Property(e => e.PhoneExtension).HasMaxLength(10);
            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.EmploymentStatusID).IsRequired();
            // Configure other properties as needed
        });
        modelBuilder.Entity<MessageRecipient>(entity =>
        {
            entity.HasKey(e => e.RecipientID);

            entity.HasOne(e => e.Status)
                  .WithMany()
                  .HasForeignKey(e => e.StatusID)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_MessageRecipients_MessageStatuses_StatusID");

            entity.HasOne(e => e.Message)
                  .WithMany(m => m.Recipients)
                  .HasForeignKey(e => e.MessageID);
        });



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
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
