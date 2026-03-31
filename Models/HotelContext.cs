using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HotelReymer.Models;

public partial class HotelContext : DbContext
{
    public HotelContext()
    {
    }

    public HotelContext(DbContextOptions<HotelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingRoom> BookingRooms { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<HousekeepingTask> HousekeepingTasks { get; set; }

    public virtual DbSet<MarketingCampaign> MarketingCampaigns { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomCategory> RoomCategories { get; set; }

    public virtual DbSet<Stay> Stays { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Server=DESKTOP-U9VKBRO\\SQLEXPRESS;Database=Hotel_Reymer;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("AUDIT_LOG");

            entity.Property(e => e.LogId).HasColumnName("LOG_ID");
            entity.Property(e => e.Action)
                .HasMaxLength(200)
                .HasColumnName("ACTION");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("DATE");
            entity.Property(e => e.Entity)
                .HasMaxLength(100)
                .HasColumnName("ENTITY");
            entity.Property(e => e.EntityId)
                .HasMaxLength(50)
                .HasColumnName("ENTITY_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AUDIT_USER");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("BOOKING");

            entity.Property(e => e.BookingId).HasColumnName("BOOKING_ID");
            entity.Property(e => e.CheckinDatePlan).HasColumnName("CHECKIN_DATE_PLAN");
            entity.Property(e => e.CheckoutDatePlan).HasColumnName("CHECKOUT_DATE_PLAN");
            entity.Property(e => e.ClientId).HasColumnName("CLIENT_ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .HasColumnName("COMMENT");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.Client).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKING_CLIENT");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKING_USER");
        });

        modelBuilder.Entity<BookingRoom>(entity =>
        {
            entity.ToTable("BOOKING_ROOM");

            entity.Property(e => e.BookingRoomId).HasColumnName("BOOKING_ROOM_ID");
            entity.Property(e => e.BookingId).HasColumnName("BOOKING_ID");
            entity.Property(e => e.Capacity)
                .HasDefaultValue(1)
                .HasColumnName("CAPACITY");
            entity.Property(e => e.PricePerDay)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("PRICE_PER_DAY");
            entity.Property(e => e.RoomId).HasColumnName("ROOM_ID");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingRooms)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_BR_BOOKING");

            entity.HasOne(d => d.Room).WithMany(p => p.BookingRooms)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BR_ROOM");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("CLIENT");

            entity.Property(e => e.ClientId).HasColumnName("CLIENT_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("NAME");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(20)
                .HasColumnName("PASSPORT_NUMBER");
            entity.Property(e => e.PassportSeries)
                .HasMaxLength(10)
                .HasColumnName("PASSPORT_SERIES");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("PHONE");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("STATUS");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.ToTable("DISCOUNT");

            entity.Property(e => e.DiscountId).HasColumnName("DISCOUNT_ID");
            entity.Property(e => e.ClientId).HasColumnName("CLIENT_ID");
            entity.Property(e => e.DiscountValue)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("DISCOUNT_VALUE");
            entity.Property(e => e.EndDate).HasColumnName("END_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
            entity.Property(e => e.ReasonText)
                .HasMaxLength(300)
                .HasColumnName("REASON_TEXT");
            entity.Property(e => e.StartDate).HasColumnName("START_DATE");

            entity.HasOne(d => d.Client).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_DISCOUNT_CLIENT");
        });

        modelBuilder.Entity<HousekeepingTask>(entity =>
        {
            entity.HasKey(e => e.HousekeepingId).HasName("PK_HOUSEKEEPING");

            entity.ToTable("HOUSEKEEPING_TASK");

            entity.Property(e => e.HousekeepingId).HasColumnName("HOUSEKEEPING_ID");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .HasColumnName("COMMENT");
            entity.Property(e => e.CompletedAt).HasColumnName("COMPLETED_AT");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.DueAt).HasColumnName("DUE_AT");
            entity.Property(e => e.PriorityNo)
                .HasDefaultValue(5)
                .HasColumnName("PRIORITY_NO");
            entity.Property(e => e.RoomId).HasColumnName("ROOM_ID");
            entity.Property(e => e.TaskStatus)
                .HasMaxLength(20)
                .HasColumnName("TASK_STATUS");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.Room).WithMany(p => p.HousekeepingTasks)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HOUSE_ROOM");

            entity.HasOne(d => d.User).WithMany(p => p.HousekeepingTasks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_HOUSE_USER");
        });

        modelBuilder.Entity<MarketingCampaign>(entity =>
        {
            entity.HasKey(e => e.CampaignId);

            entity.ToTable("MARKETING_CAMPAIGN");

            entity.Property(e => e.CampaignId).HasColumnName("CAMPAIGN_ID");
            entity.Property(e => e.AdjustmentValue)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("ADJUSTMENT_VALUE");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.EndDate).HasColumnName("END_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("NAME");
            entity.Property(e => e.RoomCategoryId).HasColumnName("ROOM_CATEGORY_ID");
            entity.Property(e => e.StartDate).HasColumnName("START_DATE");

            entity.HasOne(d => d.RoomCategory).WithMany(p => p.MarketingCampaigns)
                .HasForeignKey(d => d.RoomCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CAMPAIGN_CATEGORY");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("PAYMENT");

            entity.Property(e => e.PaymentId).HasColumnName("PAYMENT_ID");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.ExternalId)
                .HasMaxLength(100)
                .HasColumnName("EXTERNAL_ID");
            entity.Property(e => e.PaymentAt)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("PAYMENT_AT");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(20)
                .HasColumnName("PAYMENT_METHOD");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasColumnName("PAYMENT_STATUS");
            entity.Property(e => e.StayId).HasColumnName("STAY_ID");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.Stay).WithMany(p => p.Payments)
                .HasForeignKey(d => d.StayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAYMENT_STAY");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PAYMENT_USER");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("ROOM");

            entity.Property(e => e.RoomId).HasColumnName("ROOM_ID");
            entity.Property(e => e.CategoryId).HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Floor).HasColumnName("FLOOR");
            entity.Property(e => e.Note)
                .HasMaxLength(500)
                .HasColumnName("NOTE");
            entity.Property(e => e.RoomNumber)
                .HasMaxLength(20)
                .HasColumnName("ROOM_NUMBER");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("STATUS");

            entity.HasOne(d => d.Category).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROOM_CATEGORY");
        });

        modelBuilder.Entity<RoomCategory>(entity =>
        {
            entity.ToTable("ROOM_CATEGORY");

            entity.Property(e => e.RoomCategoryId).HasColumnName("ROOM_CATEGORY_ID");
            entity.Property(e => e.BasePricePerDay)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("BASE_PRICE_PER_DAY");
            entity.Property(e => e.Capacity).HasColumnName("CAPACITY");
            entity.Property(e => e.ComfortLevel)
                .HasMaxLength(30)
                .HasColumnName("COMFORT_LEVEL");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Stay>(entity =>
        {
            entity.ToTable("STAY");

            entity.Property(e => e.StayId).HasColumnName("STAY_ID");
            entity.Property(e => e.AmountBeforeDiscount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("AMOUNT_BEFORE_DISCOUNT");
            entity.Property(e => e.BookingRoomId).HasColumnName("BOOKING_ROOM_ID");
            entity.Property(e => e.CheckinAt).HasColumnName("CHECKIN_AT");
            entity.Property(e => e.CheckoutAt).HasColumnName("CHECKOUT_AT");
            entity.Property(e => e.CheckoutDatePlan).HasColumnName("CHECKOUT_DATE_PLAN");
            entity.Property(e => e.DiscountAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("DISCOUNT_AMOUNT");
            entity.Property(e => e.StayStatus)
                .HasMaxLength(20)
                .HasColumnName("STAY_STATUS");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("TOTAL_AMOUNT");

            entity.HasOne(d => d.BookingRoom).WithMany(p => p.Stays)
                .HasForeignKey(d => d.BookingRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STAY_BR");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("USER_ACCOUNT");

            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("FULL_NAME");
            entity.Property(e => e.Hash)
                .HasMaxLength(64)
                .HasColumnName("HASH");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("LOGIN");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .HasColumnName("PHONE");
            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserAccounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
