﻿// <auto-generated />
using System;
using DotChat_Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DotChat_Repositories.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231122141527_v11")]
    partial class v11
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.24")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatConnectionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("connectedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("connectionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("disConnectedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("ChatConnectionLogs");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("channelCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("channelCloseUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("connectionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isChannelClosed")
                        .HasColumnType("bit");

                    b.Property<string>("userId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("chatGroups");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("chatgroupId")
                        .HasColumnType("int");

                    b.Property<bool>("isChannelMutedByMember")
                        .HasColumnType("bit");

                    b.Property<bool>("isMemberChannelAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("isMemberRemovedFromChannel")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("memberRemovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("memberUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("chatgroupId");

                    b.HasIndex("userId");

                    b.ToTable("chatGroupMembers");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMemberInbox", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("archiveDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("chatGroupMemberId")
                        .HasColumnType("int");

                    b.Property<int>("chatGroupMessagesId")
                        .HasColumnType("int");

                    b.Property<bool>("isArchived")
                        .HasColumnType("bit");

                    b.Property<bool>("isRead")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("readDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("chatGroupMessagesId")
                        .IsUnique();

                    b.ToTable("ChatGroupMemberInboxes");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMessages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("chatGroupMemberId")
                        .HasColumnType("int");

                    b.Property<bool>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("messageTimestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("chatGroupMemberId");

                    b.ToTable("chatGroupMessages");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("avatarImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("confirmedPasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("isAvatarImageSet")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatConnectionLog", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.User", "user")
                        .WithMany("chatConnectionLogs")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroup", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.User", "user")
                        .WithMany()
                        .HasForeignKey("userId");

                    b.Navigation("user");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMember", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.ChatGroup", "chatGroup")
                        .WithMany("chatGroupMembers")
                        .HasForeignKey("chatgroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotChat_Entities.DbSet.User", "user")
                        .WithMany()
                        .HasForeignKey("userId");

                    b.Navigation("chatGroup");

                    b.Navigation("user");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMemberInbox", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.ChatGroupMessages", "chatGroupMessages")
                        .WithOne("chatGroupMemberInboxe")
                        .HasForeignKey("DotChat_Entities.DbSet.ChatGroupMemberInbox", "chatGroupMessagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("chatGroupMessages");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMessages", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.ChatGroupMember", "chatGroupMember")
                        .WithMany("ChatGroupMessages")
                        .HasForeignKey("chatGroupMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("chatGroupMember");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DotChat_Entities.DbSet.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DotChat_Entities.DbSet.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroup", b =>
                {
                    b.Navigation("chatGroupMembers");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMember", b =>
                {
                    b.Navigation("ChatGroupMessages");
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.ChatGroupMessages", b =>
                {
                    b.Navigation("chatGroupMemberInboxe")
                        .IsRequired();
                });

            modelBuilder.Entity("DotChat_Entities.DbSet.User", b =>
                {
                    b.Navigation("chatConnectionLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
