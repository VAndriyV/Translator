﻿// <auto-generated />
using System;
using DataEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataEFCore.Migrations
{
    [DbContext(typeof(TranslatorContext))]
    [Migration("20190207145150_Entities rename")]
    partial class Entitiesrename
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Entities.AutomaticRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Alpha");

                    b.Property<int?>("Beta");

                    b.Property<string>("Information")
                        .HasMaxLength(80)
                        .IsUnicode(false);

                    b.Property<string>("Lexeme")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<int?>("Stack");

                    b.HasKey("Id");

                    b.ToTable("AutomaticRules");
                });

            modelBuilder.Entity("Domain.Entities.Lexeme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Lexemes");
                });

            modelBuilder.Entity("Domain.Entities.LexemeClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Values");

                    b.HasKey("Id");

                    b.ToTable("LexemeClasses");
                });

            modelBuilder.Entity("Domain.Entities.OutputConstant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("OutputConstants");
                });

            modelBuilder.Entity("Domain.Entities.OutputIdn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("OutputIDNs");
                });

            modelBuilder.Entity("Domain.Entities.OutputLexeme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ConstantCode");

                    b.Property<int?>("Idncode")
                        .HasColumnName("IDNCode");

                    b.Property<int>("LexemeId");

                    b.Property<string>("LexemeName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false);

                    b.Property<int>("Row");

                    b.HasKey("Id");

                    b.ToTable("OutputLexemes");
                });
#pragma warning restore 612, 618
        }
    }
}
