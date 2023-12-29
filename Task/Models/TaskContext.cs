using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Task1.Models;

public partial class TaskContext :  IdentityDbContext<IdentityUser>
{
    public TaskContext()
    {
    }

    public TaskContext(DbContextOptions<TaskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DossierStructure> DossierStructures { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DossierStructure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DossierS__3214EC07D027B2EC");

            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.SectionCode).HasMaxLength(50);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__DossierSt__Paren__276EDEB3");
        });
        base.OnModelCreating(modelBuilder);
    }
}
