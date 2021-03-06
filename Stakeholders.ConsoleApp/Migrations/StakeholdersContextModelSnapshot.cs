﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Regira.Stakeholders.Core.Entities;
using Regira.Stakeholders.Library.Data;

namespace Regira.Stakeholders.ConsoleApp.Migrations
{
    [DbContext(typeof(StakeholdersContext))]
    partial class StakeholdersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.ContactRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id")
                        .HasName("pk_contact_roles");

                    b.ToTable("contact_roles");
                });

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.Stakeholder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Kbo")
                        .HasColumnName("kbo")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Note")
                        .HasColumnName("note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Phone")
                        .HasColumnName("phone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("StakeholderType")
                        .HasColumnName("stakeholder_type")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("pk_stakeholders");

                    b.ToTable("stakeholders");

                    b.HasDiscriminator<int>("StakeholderType");
                });

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.StakeholderContact", b =>
                {
                    b.Property<int>("RoleGiverId")
                        .HasColumnName("role_giver_id")
                        .HasColumnType("int");

                    b.Property<int>("RoleBearerId")
                        .HasColumnName("role_bearer_id")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnName("role_id")
                        .HasColumnType("int");

                    b.HasKey("RoleGiverId", "RoleBearerId", "RoleId")
                        .HasName("pk_stakeholder_contacts");

                    b.HasIndex("RoleBearerId")
                        .HasName("ix_stakeholder_contacts_role_bearer_id");

                    b.HasIndex("RoleId")
                        .HasName("ix_stakeholder_contacts_role_id");

                    b.ToTable("stakeholder_contacts");
                });

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.Organization", b =>
                {
                    b.HasBaseType("Regira.Stakeholders.Core.Entities.Stakeholder");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasIndex("Kbo")
                        .HasName("ix_stakeholders_kbo");

                    b.ToTable("stakeholders1");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.Person", b =>
                {
                    b.HasBaseType("Regira.Stakeholders.Core.Entities.Stakeholder");

                    b.Property<string>("FamilyName")
                        .HasColumnName("family_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("GivenName")
                        .HasColumnName("given_name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Salutation")
                        .HasColumnName("salutation")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasIndex("Kbo")
                        .HasName("ix_stakeholders_kbo1");

                    b.ToTable("stakeholders2");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.Stakeholder", b =>
                {
                    b.OwnsMany("Regira.Stakeholders.Core.Entities.AccountNumber", "BankAccounts", b1 =>
                        {
                            b1.Property<int>("StakeholderId")
                                .HasColumnName("stakeholder_id")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnName("id")
                                .HasColumnType("int");

                            b1.Property<int>("AccountType")
                                .HasColumnName("account_type")
                                .HasColumnType("int");

                            b1.Property<string>("Number")
                                .HasColumnName("account_number")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.HasKey("StakeholderId", "Id")
                                .HasName("pk_account_number");

                            b1.ToTable("AccountNumber");

                            b1.WithOwner()
                                .HasForeignKey("StakeholderId")
                                .HasConstraintName("fk_account_number_stakeholders_stakeholder_id");
                        });

                    b.OwnsOne("Regira.Stakeholders.Core.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<int>("StakeholderId")
                                .HasColumnName("id")
                                .HasColumnType("int");

                            b1.Property<string>("Box")
                                .HasColumnName("address_box")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("City")
                                .HasColumnName("address_city")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("CountryCode")
                                .HasColumnName("address_country_code")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("Number")
                                .HasColumnName("address_number")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("PostBox")
                                .HasColumnName("address_post_box")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("PostalCode")
                                .HasColumnName("address_postal_code")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.Property<string>("Street")
                                .HasColumnName("address_street")
                                .HasColumnType("longtext CHARACTER SET utf8mb4");

                            b1.HasKey("StakeholderId")
                                .HasName("pk_stakeholders");

                            b1.ToTable("stakeholders");

                            b1.WithOwner()
                                .HasForeignKey("StakeholderId")
                                .HasConstraintName("fk_address_stakeholders_stakeholder_id");
                        });
                });

            modelBuilder.Entity("Regira.Stakeholders.Core.Entities.StakeholderContact", b =>
                {
                    b.HasOne("Regira.Stakeholders.Core.Entities.Stakeholder", "RoleBearer")
                        .WithMany("OwnerContacts")
                        .HasForeignKey("RoleBearerId")
                        .HasConstraintName("fk_stakeholder_contacts_stakeholders_stakeholder_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Regira.Stakeholders.Core.Entities.Stakeholder", "RoleGiver")
                        .WithMany("OwnedContacts")
                        .HasForeignKey("RoleGiverId")
                        .HasConstraintName("fk_stakeholder_contacts_stakeholders_role_giver_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Regira.Stakeholders.Core.Entities.ContactRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_stakeholder_contacts_contact_roles_role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
