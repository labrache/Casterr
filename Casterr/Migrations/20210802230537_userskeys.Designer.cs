﻿// <auto-generated />
using System;
using Casterr.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Casterr.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210802230537_userskeys")]
    partial class userskeys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("Casterr.Data.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("mailKey")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<bool>("mailSubscribe")
                        .HasColumnType("INTEGER");

                    b.Property<string>("uKey")
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Casterr.Data.Episode", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Plot")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("absoluteEpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("episode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("episodeJson")
                        .HasColumnType("TEXT");

                    b.Property<string>("fileName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("premiered")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("publishDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("season")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("showid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("thumbFileName")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("showid");

                    b.ToTable("Episode");
                });

            modelBuilder.Entity("Casterr.Data.Movie", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Plot")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("dirName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("director")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<int>("duration")
                        .HasMaxLength(16)
                        .HasColumnType("INTEGER");

                    b.Property<string>("fanartFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("fileName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("genre")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("keep")
                        .HasColumnType("INTEGER");

                    b.Property<string>("movieJson")
                        .HasColumnType("TEXT");

                    b.Property<string>("movieSet")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("originalTitle")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("posterFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("premiered")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("publishDate")
                        .HasColumnType("TEXT");

                    b.Property<float>("rating")
                        .HasColumnType("REAL");

                    b.Property<string>("status")
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("studio")
                        .HasColumnType("TEXT");

                    b.Property<int>("year")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Casterr.Data.Seasons", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Plot")
                        .HasColumnType("TEXT");

                    b.Property<int>("episodeCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("episodeFileCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<float>("percentOfEpisodes")
                        .HasColumnType("REAL");

                    b.Property<string>("poster")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("seasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("showid")
                        .HasColumnType("INTEGER");

                    b.Property<int>("totalEpisodeCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("showid");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Casterr.Data.Tvshow", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("OriginalTitle")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Plot")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("bannerFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("dirName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("fanartFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("genre")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("imdbId")
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("network")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("posterFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("premiered")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("publishDate")
                        .HasColumnType("TEXT");

                    b.Property<float>("rating")
                        .HasColumnType("REAL");

                    b.Property<string>("status")
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("thumbStorage")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("tmdbBackdropFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("tmdbId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("tmdbPosterFile")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("tvdbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("TVShows");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Casterr.Data.Episode", b =>
                {
                    b.HasOne("Casterr.Data.Tvshow", "show")
                        .WithMany("episodes")
                        .HasForeignKey("showid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("show");
                });

            modelBuilder.Entity("Casterr.Data.Seasons", b =>
                {
                    b.HasOne("Casterr.Data.Tvshow", "show")
                        .WithMany("seasons")
                        .HasForeignKey("showid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("show");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Casterr.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Casterr.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Casterr.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Casterr.Data.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Casterr.Data.Tvshow", b =>
                {
                    b.Navigation("episodes");

                    b.Navigation("seasons");
                });
#pragma warning restore 612, 618
        }
    }
}
