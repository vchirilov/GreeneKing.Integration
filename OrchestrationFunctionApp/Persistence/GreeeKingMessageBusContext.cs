using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrchestrationFunctionApp.Persistence.Entities;

namespace OrchestrationFunctionApp.Persistence;

public partial class GreeeKingMessageBusContext : DbContext
{
    public GreeeKingMessageBusContext()
    {
    }

    public GreeeKingMessageBusContext(DbContextOptions<GreeeKingMessageBusContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MsgFlatFile> MsgFlatFiles { get; set; }

    public virtual DbSet<MsgEmptyEvent> MsgEmptyEvents { get; set; }

    public virtual DbSet<MsgInlineJson> MsgInlineJsons { get; set; }

    public virtual DbSet<MsgJsonFile> MsgJsonFiles { get; set; }

    public virtual DbSet<MsgXmlFile> MsgXmlFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MsgFlatFile>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("PK__MsgCsvFi__3214EC07ACC1226E");

            entity.ToTable("MsgFlatFile", "msgqueue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipelineAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrchestrationAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MessageID");
            entity.Property(e => e.Processed).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MsgEmptyEvent>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("PK__MsgEvent__3214EC076E763819");

            entity.ToTable("MsgEmptyEvent", "msgqueue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipelineAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrchestrationAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MessageID");
            entity.Property(e => e.Processed).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MsgInlineJson>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("PK__MsgInlin__3214EC077F52A316");

            entity.ToTable("MsgInlineJson", "msgqueue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipelineAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrchestrationAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MessageID");
            entity.Property(e => e.Processed).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MsgJsonFile>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("PK__MsgJsonF__3214EC07C355C7E0");

            entity.ToTable("MsgJsonFile", "msgqueue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipelineAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrchestrationAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MessageID");
            entity.Property(e => e.Processed).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MsgXmlFile>(entity =>
        {
            //entity.HasKey(e => e.Id).HasName("PK__MsgXmlFi__3214EC073F9C79DE");

            entity.ToTable("MsgXmlFile", "msgqueue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PipelineAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrchestrationAction)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MessageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MessageID");
            entity.Property(e => e.Processed).HasDefaultValueSql("((0))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
