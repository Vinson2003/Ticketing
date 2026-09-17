using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Entities;

namespace TicketingSystem.Data.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MtPermission> MtPermissions { get; set; }

    public virtual DbSet<MtRole> MtRoles { get; set; }

    public virtual DbSet<MtRolepermission> MtRolepermissions { get; set; }

    public virtual DbSet<MtTicketCategory> MtTicketCategories { get; set; }

    public virtual DbSet<MtTicketPriority> MtTicketPriorities { get; set; }

    public virtual DbSet<MtTicketStatus> MtTicketStatuses { get; set; }

    public virtual DbSet<MtUser> MtUsers { get; set; }

    public virtual DbSet<TrTicket> TrTickets { get; set; }

    public virtual DbSet<TrTicketComment> TrTicketComments { get; set; }

    public virtual DbSet<TrTicketHistory> TrTicketHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MtPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_permi__3213E83FE956A622");

            entity.ToTable("mt_permission");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Display)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("display");
            entity.Property(e => e.Seq).HasColumnName("seq");
            entity.Property(e => e.SubSeq).HasColumnName("sub_seq");
        });

        modelBuilder.Entity<MtRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_role__3213E83FD2525858");

            entity.ToTable("mt_role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MtRolepermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_rolep__3213E83FC243C5D1");

            entity.ToTable("mt_rolepermission");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "UQ_mt_rolepermission").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.MtRolepermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mt_rolepermission_mt_permission");

            entity.HasOne(d => d.Role).WithMany(p => p.MtRolepermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mt_rolepermission_mt_role");
        });

        modelBuilder.Entity<MtTicketCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_ticke__3213E83F505C5796");

            entity.ToTable("mt_ticket_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MtTicketPriority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_ticke__3213E83F8D660BB3");

            entity.ToTable("mt_ticket_priority");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        });

        modelBuilder.Entity<MtTicketStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_ticke__3213E83F92F56E18");

            entity.ToTable("mt_ticket_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
        });

        modelBuilder.Entity<MtUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mt_user__3213E83F51EEE4E7");

            entity.ToTable("mt_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.MtUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_mt_user_mt_role");
        });

        modelBuilder.Entity<TrTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tr_ticke__3213E83F39AE7356");

            entity.ToTable("tr_ticket");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedTo).HasColumnName("assigned_to");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ClosedAt).HasColumnName("closed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.PriorityId).HasColumnName("priority_id");
            entity.Property(e => e.ResolvedAt).HasColumnName("resolved_at");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TicketNo)
                .HasMaxLength(50)
                .HasColumnName("ticket_no");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TrTicketAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_tr_ticket_assigned_to");

            entity.HasOne(d => d.Category).WithMany(p => p.TrTickets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_category");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TrTicketCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_created_by");

            entity.HasOne(d => d.Priority).WithMany(p => p.TrTickets)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_priority");

            entity.HasOne(d => d.Status).WithMany(p => p.TrTickets)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_status");
        });

        modelBuilder.Entity<TrTicketComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tr_ticke__3213E83FD61752B3");

            entity.ToTable("tr_ticket_comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TrTicketComments)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_comment_ticket");

            entity.HasOne(d => d.User).WithMany(p => p.TrTicketComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_comment_user");
        });

        modelBuilder.Entity<TrTicketHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tr_ticke__3213E83F6839A19F");

            entity.ToTable("tr_ticket_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.TicketId).HasColumnName("ticket_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TrTicketHistories)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_history_user");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TrTicketHistories)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tr_ticket_history_ticket");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
