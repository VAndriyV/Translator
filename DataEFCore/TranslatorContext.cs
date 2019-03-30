using System;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace DataEFCore
{
    public partial class TranslatorContext : DbContext, IDbContext
    {
        private const string ConnectionStringName = "connection.connection_string";

        public TranslatorContext()
        {
        }

        public TranslatorContext(DbContextOptions<TranslatorContext> options)
            : base(options)
        {
        }

        public DbSet<AutomaticRule> AutomaticRules { get; set; }
        public DbSet<LexemeClass> LexemeClasses { get; set; }
        public DbSet<Lexeme> Lexemes { get; set; }
        public DbSet<OutputConstant> OutputConstants { get; set; }
        public DbSet<OutputIdn> OutputIdns { get; set; }
        public DbSet<OutputLexeme> OutputLexemes { get; set; }

        async Task IDbContext.SaveChangesAsync()
        {
            await SaveChangesAsync();
        }

        async Task IDbContext.TruncateTable(string tableName)
        {
             await Database.ExecuteSqlCommandAsync($"TRUNCATE TABLE {tableName}", tableName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-A698P0U\\SQLEXPRESS;Database=Translator;Integrated Security=True;MultipleActiveResultSets=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutomaticRule>(entity =>
            {
                entity.ToTable("AutomaticRules");
                entity.Property(e => e.Information)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Lexeme)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LexemeClass>(entity =>
            {
                entity.ToTable("LexemeClasses");
                
            });

            modelBuilder.Entity<Lexeme>(entity =>
            {
                entity.ToTable("Lexemes");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
            

            modelBuilder.Entity<OutputConstant>(entity =>
            {
                entity.ToTable("OutputConstants");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OutputIdn>(entity =>
            {
                entity.ToTable("OutputIDNs");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OutputLexeme>(entity =>
            {
                entity.ToTable("OutputLexemes");
                entity.Property(e => e.Idncode).HasColumnName("IDNCode");
                entity.Property(e => e.LexemeName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });
        }
    }
}
