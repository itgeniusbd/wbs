using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Models;

namespace WBS.Web;

public partial class WbsNgoContext : DbContext
{
    public WbsNgoContext()
    {
    }

    public WbsNgoContext(DbContextOptions<WbsNgoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.AccountBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AccountName).HasMaxLength(200);
            entity.Property(e => e.AccountNameBn).HasMaxLength(200);
            entity.Property(e => e.AccountNumber).HasMaxLength(100);
            entity.Property(e => e.AccountType).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.BranchName).HasMaxLength(200);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Default_Status).HasColumnName("Default_Status");
            entity.Property(e => e.Deleted_Expense)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Deleted_Expense");
            entity.Property(e => e.Deleted_Income)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Deleted_Income");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DescriptionBn).HasMaxLength(500);
            entity.Property(e => e.Total_Expense)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_Expense");
            entity.Property(e => e.Total_IN)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_IN");
            entity.Property(e => e.Total_Income)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_Income");
            entity.Property(e => e.Total_OUT)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Total_OUT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
