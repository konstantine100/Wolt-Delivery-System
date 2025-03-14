﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wolt.Data;

#nullable disable

namespace Wolt.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BasketFood", b =>
                {
                    b.Property<int>("basketsId")
                        .HasColumnType("int");

                    b.Property<int>("foodsId")
                        .HasColumnType("int");

                    b.HasKey("basketsId", "foodsId");

                    b.HasIndex("foodsId");

                    b.ToTable("BasketFood");
                });

            modelBuilder.Entity("BasketProduct", b =>
                {
                    b.Property<int>("basketsId")
                        .HasColumnType("int");

                    b.Property<int>("productsId")
                        .HasColumnType("int");

                    b.HasKey("basketsId", "productsId");

                    b.HasIndex("productsId");

                    b.ToTable("BasketProduct");
                });

            modelBuilder.Entity("FoodOrderItem", b =>
                {
                    b.Property<int>("foodsId")
                        .HasColumnType("int");

                    b.Property<int>("ordersId")
                        .HasColumnType("int");

                    b.HasKey("foodsId", "ordersId");

                    b.HasIndex("ordersId");

                    b.ToTable("FoodOrderItem");
                });

            modelBuilder.Entity("OrderItemProduct", b =>
                {
                    b.Property<int>("ordersId")
                        .HasColumnType("int");

                    b.Property<int>("productsId")
                        .HasColumnType("int");

                    b.HasKey("ordersId", "productsId");

                    b.HasIndex("productsId");

                    b.ToTable("OrderItemProduct");
                });

            modelBuilder.Entity("Wolt.Models.Basket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("Wolt.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Wolt.Models.CustomerDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LoyaltyPoints")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("isVip")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("CustomerDetails");
                });

            modelBuilder.Entity("Wolt.Models.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MenuCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MenuCategoryId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("Wolt.Models.FoodCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FoodChainId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FoodChainId")
                        .IsUnique();

                    b.ToTable("FoodCategories");
                });

            modelBuilder.Entity("Wolt.Models.FoodChain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("OrderFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan>("OrderTime")
                        .HasColumnType("time");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<decimal>("ParticipantNumber")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ParticipantScore")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("FoodChains");
                });

            modelBuilder.Entity("Wolt.Models.Ingridients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AdditionalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Calories")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("FoodId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.ToTable("Ingridients");
                });

            modelBuilder.Entity("Wolt.Models.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FoodCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FoodCategoryId")
                        .IsUnique();

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("Wolt.Models.MenuCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("MenuCategories");
                });

            modelBuilder.Entity("Wolt.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("OrderTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Wolt.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Wolt.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("Wolt.Models.ProdCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProdactionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdactionId");

                    b.ToTable("ProdCategories");
                });

            modelBuilder.Entity("Wolt.Models.Prodaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StoreCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreCategoryId")
                        .IsUnique();

                    b.ToTable("Prodactions");
                });

            modelBuilder.Entity("Wolt.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProdCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Wolt.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FoodCategoryId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("FridayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("FridayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("MondayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("MondayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SaturdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SaturdayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SundayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SundayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("ThursdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("ThursdayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TuesdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TuesdayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("WednesdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("WednesdayOpenTime")
                        .HasColumnType("time");

                    b.Property<bool>("isFridayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isFridayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isMondayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isMondayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isSaturdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isSaturdayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isSundayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isSundayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isThursdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isThursdayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isTuesdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isTuesdayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isWednesdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isWednesdayOpen")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("FoodCategoryId")
                        .IsUnique();

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Wolt.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("OrderFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<TimeSpan>("OrderTime")
                        .HasColumnType("time");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<decimal>("ParticipantNumber")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ParticipantScore")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Rating")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Wolt.Models.StoreCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId")
                        .IsUnique();

                    b.ToTable("StoreCategories");
                });

            modelBuilder.Entity("Wolt.Models.StoreSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("FridayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("FridayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("MondayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("MondayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SaturdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SaturdayOpenTime")
                        .HasColumnType("time");

                    b.Property<int>("StoreCategoryId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("SundayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("SundayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("ThursdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("ThursdayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TuesdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TuesdayOpenTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("WednesdayCloseTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("WednesdayOpenTime")
                        .HasColumnType("time");

                    b.Property<bool>("isFridayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isFridayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isMondayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isMondayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isSaturdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isSaturdayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isSundayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isSundayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isThursdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isThursdayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isTuesdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isTuesdayOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("isWednesdayAllDay")
                        .HasColumnType("bit");

                    b.Property<bool>("isWednesdayOpen")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("StoreCategoryId")
                        .IsUnique();

                    b.ToTable("StoreSchedules");
                });

            modelBuilder.Entity("BasketFood", b =>
                {
                    b.HasOne("Wolt.Models.Basket", null)
                        .WithMany()
                        .HasForeignKey("basketsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wolt.Models.Food", null)
                        .WithMany()
                        .HasForeignKey("foodsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BasketProduct", b =>
                {
                    b.HasOne("Wolt.Models.Basket", null)
                        .WithMany()
                        .HasForeignKey("basketsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wolt.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("productsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoodOrderItem", b =>
                {
                    b.HasOne("Wolt.Models.Food", null)
                        .WithMany()
                        .HasForeignKey("foodsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wolt.Models.OrderItem", null)
                        .WithMany()
                        .HasForeignKey("ordersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrderItemProduct", b =>
                {
                    b.HasOne("Wolt.Models.OrderItem", null)
                        .WithMany()
                        .HasForeignKey("ordersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wolt.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("productsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wolt.Models.Basket", b =>
                {
                    b.HasOne("Wolt.Models.Customer", "Customer")
                        .WithOne("Basket")
                        .HasForeignKey("Wolt.Models.Basket", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Wolt.Models.CustomerDetails", b =>
                {
                    b.HasOne("Wolt.Models.Customer", "Customer")
                        .WithOne("CustomerDetails")
                        .HasForeignKey("Wolt.Models.CustomerDetails", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Wolt.Models.Food", b =>
                {
                    b.HasOne("Wolt.Models.MenuCategory", "MenuCategory")
                        .WithMany("Foods")
                        .HasForeignKey("MenuCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuCategory");
                });

            modelBuilder.Entity("Wolt.Models.FoodCategory", b =>
                {
                    b.HasOne("Wolt.Models.FoodChain", "FoodChain")
                        .WithOne("Category")
                        .HasForeignKey("Wolt.Models.FoodCategory", "FoodChainId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodChain");
                });

            modelBuilder.Entity("Wolt.Models.FoodChain", b =>
                {
                    b.HasOne("Wolt.Models.Owner", "Owner")
                        .WithMany("FoodChains")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Wolt.Models.Ingridients", b =>
                {
                    b.HasOne("Wolt.Models.Food", "Food")
                        .WithMany("Ingridients")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");
                });

            modelBuilder.Entity("Wolt.Models.Menu", b =>
                {
                    b.HasOne("Wolt.Models.FoodCategory", "FoodCategory")
                        .WithOne("Menu")
                        .HasForeignKey("Wolt.Models.Menu", "FoodCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodCategory");
                });

            modelBuilder.Entity("Wolt.Models.MenuCategory", b =>
                {
                    b.HasOne("Wolt.Models.Menu", "Menu")
                        .WithMany("Categories")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("Wolt.Models.Order", b =>
                {
                    b.HasOne("Wolt.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Wolt.Models.OrderItem", b =>
                {
                    b.HasOne("Wolt.Models.Order", "Order")
                        .WithOne("orderItem")
                        .HasForeignKey("Wolt.Models.OrderItem", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Wolt.Models.ProdCategory", b =>
                {
                    b.HasOne("Wolt.Models.Prodaction", "Prodaction")
                        .WithMany("Categories")
                        .HasForeignKey("ProdactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Prodaction");
                });

            modelBuilder.Entity("Wolt.Models.Prodaction", b =>
                {
                    b.HasOne("Wolt.Models.StoreCategory", "StoreCategory")
                        .WithOne("Prodaction")
                        .HasForeignKey("Wolt.Models.Prodaction", "StoreCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StoreCategory");
                });

            modelBuilder.Entity("Wolt.Models.Product", b =>
                {
                    b.HasOne("Wolt.Models.ProdCategory", "ProdCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProdCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProdCategory");
                });

            modelBuilder.Entity("Wolt.Models.Schedule", b =>
                {
                    b.HasOne("Wolt.Models.FoodCategory", "FoodCategory")
                        .WithOne("Schedule")
                        .HasForeignKey("Wolt.Models.Schedule", "FoodCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodCategory");
                });

            modelBuilder.Entity("Wolt.Models.Store", b =>
                {
                    b.HasOne("Wolt.Models.Owner", "Owner")
                        .WithMany("Stores")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Wolt.Models.StoreCategory", b =>
                {
                    b.HasOne("Wolt.Models.Store", "Store")
                        .WithOne("Category")
                        .HasForeignKey("Wolt.Models.StoreCategory", "StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Wolt.Models.StoreSchedule", b =>
                {
                    b.HasOne("Wolt.Models.StoreCategory", "StoreCategory")
                        .WithOne("Schedule")
                        .HasForeignKey("Wolt.Models.StoreSchedule", "StoreCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StoreCategory");
                });

            modelBuilder.Entity("Wolt.Models.Customer", b =>
                {
                    b.Navigation("Basket")
                        .IsRequired();

                    b.Navigation("CustomerDetails")
                        .IsRequired();

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Wolt.Models.Food", b =>
                {
                    b.Navigation("Ingridients");
                });

            modelBuilder.Entity("Wolt.Models.FoodCategory", b =>
                {
                    b.Navigation("Menu")
                        .IsRequired();

                    b.Navigation("Schedule")
                        .IsRequired();
                });

            modelBuilder.Entity("Wolt.Models.FoodChain", b =>
                {
                    b.Navigation("Category")
                        .IsRequired();
                });

            modelBuilder.Entity("Wolt.Models.Menu", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Wolt.Models.MenuCategory", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("Wolt.Models.Order", b =>
                {
                    b.Navigation("orderItem")
                        .IsRequired();
                });

            modelBuilder.Entity("Wolt.Models.Owner", b =>
                {
                    b.Navigation("FoodChains");

                    b.Navigation("Stores");
                });

            modelBuilder.Entity("Wolt.Models.ProdCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Wolt.Models.Prodaction", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("Wolt.Models.Store", b =>
                {
                    b.Navigation("Category")
                        .IsRequired();
                });

            modelBuilder.Entity("Wolt.Models.StoreCategory", b =>
                {
                    b.Navigation("Prodaction")
                        .IsRequired();

                    b.Navigation("Schedule")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
