﻿// <auto-generated />
using System;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infra.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("AppCore.Domain.Entities.TestExecution.ResultSnapshot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExecutionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<TimeSpan>("TestDuration")
                        .HasColumnType("interval");

                    b.Property<Guid?>("TestSuiteId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TestSuiteId");

                    b.ToTable("ResultSnapshots");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestExecution.TestSuite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("TagExpression")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TestSuites");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Feature", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Scenario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("FeatureId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Scenarios");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("FeatureTag", b =>
                {
                    b.Property<Guid>("FeatureListId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsListId")
                        .HasColumnType("uuid");

                    b.HasKey("FeatureListId", "TagsListId");

                    b.HasIndex("TagsListId");

                    b.ToTable("FeatureTag");
                });

            modelBuilder.Entity("ScenarioTag", b =>
                {
                    b.Property<Guid>("ScenarioListId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagsListId")
                        .HasColumnType("uuid");

                    b.HasKey("ScenarioListId", "TagsListId");

                    b.HasIndex("TagsListId");

                    b.ToTable("ScenarioTag");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestExecution.ResultSnapshot", b =>
                {
                    b.HasOne("AppCore.Domain.Entities.TestExecution.TestSuite", null)
                        .WithMany("ResultList")
                        .HasForeignKey("TestSuiteId");

                    b.OwnsMany("AppCore.Domain.Entities.TestExecution.ScenarioResult", "ScenarioResultList", b1 =>
                        {
                            b1.Property<Guid>("OwnerId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("CreatedBy")
                                .HasColumnType("text");

                            b1.Property<DateTime>("ExecutionDate")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<DateTime?>("LastModified")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("LastModifiedBy")
                                .HasColumnType("text");

                            b1.Property<Guid>("ProjectId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ScenarioId")
                                .HasColumnType("uuid");

                            b1.Property<string>("StatusReason")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<TimeSpan>("TestDuration")
                                .HasColumnType("interval");

                            b1.Property<int>("TestStatus")
                                .HasColumnType("integer");

                            b1.HasKey("OwnerId", "Id");

                            b1.ToTable("ScenarioResult");

                            b1.WithOwner()
                                .HasForeignKey("OwnerId");
                        });

                    b.Navigation("ScenarioResultList");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Feature", b =>
                {
                    b.HasOne("AppCore.Domain.Entities.TestRepository.Project", "Project")
                        .WithMany("FeatureList")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Scenario", b =>
                {
                    b.HasOne("AppCore.Domain.Entities.TestRepository.Feature", "Feature")
                        .WithMany("Scenarios")
                        .HasForeignKey("FeatureId");

                    b.HasOne("AppCore.Domain.Entities.TestRepository.Project", "Project")
                        .WithMany("ScenarioList")
                        .HasForeignKey("ProjectId");

                    b.OwnsMany("AppCore.Domain.Entities.TestRepository.Step", "StepsList", b1 =>
                        {
                            b1.Property<Guid>("OwnerId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .UseIdentityByDefaultColumn();

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Order")
                                .HasColumnType("integer");

                            b1.Property<Guid?>("ScenarioId")
                                .HasColumnType("uuid");

                            b1.HasKey("OwnerId", "Id");

                            b1.HasIndex("ScenarioId");

                            b1.ToTable("Step");

                            b1.WithOwner()
                                .HasForeignKey("OwnerId");

                            b1.HasOne("AppCore.Domain.Entities.TestRepository.Scenario", "Scenario")
                                .WithMany()
                                .HasForeignKey("ScenarioId");

                            b1.Navigation("Scenario");
                        });

                    b.Navigation("Feature");

                    b.Navigation("Project");

                    b.Navigation("StepsList");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Tag", b =>
                {
                    b.HasOne("AppCore.Domain.Entities.TestRepository.Project", "Project")
                        .WithMany("Tags")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("FeatureTag", b =>
                {
                    b.HasOne("AppCore.Domain.Entities.TestRepository.Feature", null)
                        .WithMany()
                        .HasForeignKey("FeatureListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Domain.Entities.TestRepository.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScenarioTag", b =>
                {
                    b.HasOne("AppCore.Domain.Entities.TestRepository.Scenario", null)
                        .WithMany()
                        .HasForeignKey("ScenarioListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AppCore.Domain.Entities.TestRepository.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestExecution.TestSuite", b =>
                {
                    b.Navigation("ResultList");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Feature", b =>
                {
                    b.Navigation("Scenarios");
                });

            modelBuilder.Entity("AppCore.Domain.Entities.TestRepository.Project", b =>
                {
                    b.Navigation("FeatureList");

                    b.Navigation("ScenarioList");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
