﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Milieusysteem.Data;

#nullable disable

namespace Milieusysteem.Migrations
{
    [DbContext(typeof(MilieuSysteemDb))]
    [Migration("20231106130818_AddMilieusysteemToDatabase")]
    partial class AddMilieusysteemToDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Milieusysteem.Models.Choice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Advice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AmountOfVotes")
                        .HasColumnType("int");

                    b.Property<int?>("ChoiceId")
                        .HasColumnType("int");

                    b.Property<string>("ChoiceTekst")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClimateSurveyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChoiceId");

                    b.HasIndex("ClimateSurveyId");

                    b.ToTable("choices");
                });

            modelBuilder.Entity("Milieusysteem.Models.ChoiceAmount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChoiceCount")
                        .HasColumnType("int");

                    b.Property<int>("ChoiceId")
                        .HasColumnType("int");

                    b.Property<int>("SurveyCounterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ChoiceAmounts");
                });

            modelBuilder.Entity("Milieusysteem.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("Milieusysteem.Models.ClimateSurvey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<int?>("SurveyCountId")
                        .HasColumnType("int");

                    b.Property<string>("SurveyQuestion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("TeacherId");

                    b.ToTable("climateSurveys");
                });

            modelBuilder.Entity("Milieusysteem.Models.SurveyCounter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("ClimateSurveyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("surveyCounters");
                });

            modelBuilder.Entity("Milieusysteem.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Milieusysteem.Models.Choice", b =>
                {
                    b.HasOne("Milieusysteem.Models.Choice", null)
                        .WithMany("Choices")
                        .HasForeignKey("ChoiceId");

                    b.HasOne("Milieusysteem.Models.ClimateSurvey", null)
                        .WithMany("SurveyChoices")
                        .HasForeignKey("ClimateSurveyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Milieusysteem.Models.Class", b =>
                {
                    b.HasOne("Milieusysteem.Models.Teacher", null)
                        .WithMany("Classes")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Milieusysteem.Models.ClimateSurvey", b =>
                {
                    b.HasOne("Milieusysteem.Models.Class", null)
                        .WithMany("FinishedSurveys")
                        .HasForeignKey("ClassId");

                    b.HasOne("Milieusysteem.Models.Teacher", null)
                        .WithMany("ClimateSurveys")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Milieusysteem.Models.Choice", b =>
                {
                    b.Navigation("Choices");
                });

            modelBuilder.Entity("Milieusysteem.Models.Class", b =>
                {
                    b.Navigation("FinishedSurveys");
                });

            modelBuilder.Entity("Milieusysteem.Models.ClimateSurvey", b =>
                {
                    b.Navigation("SurveyChoices");
                });

            modelBuilder.Entity("Milieusysteem.Models.Teacher", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("ClimateSurveys");
                });
#pragma warning restore 612, 618
        }
    }
}
