using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wolt.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baskets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoyaltyPoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isVip = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDetails_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodChains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParticipantScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParticipantNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodChains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodChains_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParticipantScore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParticipantNumber = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodChainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodCategories_FoodChains_FoodChainId",
                        column: x => x.FoodChainId,
                        principalTable: "FoodChains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCategories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_FoodCategories_FoodCategoryId",
                        column: x => x.FoodCategoryId,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isMondayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isTuesdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isWednesdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isThursdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isFridayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isSaturdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isSundayOpen = table.Column<bool>(type: "bit", nullable: false),
                    MondayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TuesdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WednesdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ThursdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FridayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SaturdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SundayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MondayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TuesdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WednesdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ThursdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FridayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SaturdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SundayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    isMondayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isTuesdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isWednesdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isThursdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isFridayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isSaturdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isSundayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    FoodCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_FoodCategories_FoodCategoryId",
                        column: x => x.FoodCategoryId,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prodactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prodactions_StoreCategories_StoreCategoryId",
                        column: x => x.StoreCategoryId,
                        principalTable: "StoreCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isMondayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isTuesdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isWednesdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isThursdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isFridayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isSaturdayOpen = table.Column<bool>(type: "bit", nullable: false),
                    isSundayOpen = table.Column<bool>(type: "bit", nullable: false),
                    MondayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TuesdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WednesdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ThursdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FridayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SaturdayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SundayOpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MondayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TuesdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WednesdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ThursdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    FridayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SaturdayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    SundayCloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    isMondayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isTuesdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isWednesdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isThursdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isFridayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isSaturdayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    isSundayAllDay = table.Column<bool>(type: "bit", nullable: false),
                    StoreCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreSchedules_StoreCategories_StoreCategoryId",
                        column: x => x.StoreCategoryId,
                        principalTable: "StoreCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategories_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionId = table.Column<int>(type: "int", nullable: false),
                    ProdactionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdCategories_Prodactions_ProdactionId",
                        column: x => x.ProdactionId,
                        principalTable: "Prodactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MenuCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foods_MenuCategories_MenuCategoryId",
                        column: x => x.MenuCategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ProdCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProdCategories_ProdCategoryId",
                        column: x => x.ProdCategoryId,
                        principalTable: "ProdCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasketFood",
                columns: table => new
                {
                    basketsId = table.Column<int>(type: "int", nullable: false),
                    foodsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketFood", x => new { x.basketsId, x.foodsId });
                    table.ForeignKey(
                        name: "FK_BasketFood_Baskets_basketsId",
                        column: x => x.basketsId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketFood_Foods_foodsId",
                        column: x => x.foodsId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodOrderItem",
                columns: table => new
                {
                    foodsId = table.Column<int>(type: "int", nullable: false),
                    ordersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrderItem", x => new { x.foodsId, x.ordersId });
                    table.ForeignKey(
                        name: "FK_FoodOrderItem_Foods_foodsId",
                        column: x => x.foodsId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOrderItem_OrderItems_ordersId",
                        column: x => x.ordersId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingridients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calories = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingridients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingridients_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasketProduct",
                columns: table => new
                {
                    basketsId = table.Column<int>(type: "int", nullable: false),
                    productsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketProduct", x => new { x.basketsId, x.productsId });
                    table.ForeignKey(
                        name: "FK_BasketProduct_Baskets_basketsId",
                        column: x => x.basketsId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketProduct_Products_productsId",
                        column: x => x.productsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemProduct",
                columns: table => new
                {
                    ordersId = table.Column<int>(type: "int", nullable: false),
                    productsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemProduct", x => new { x.ordersId, x.productsId });
                    table.ForeignKey(
                        name: "FK_OrderItemProduct_OrderItems_ordersId",
                        column: x => x.ordersId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemProduct_Products_productsId",
                        column: x => x.productsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketFood_foodsId",
                table: "BasketFood",
                column: "foodsId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProduct_productsId",
                table: "BasketProduct",
                column: "productsId");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_CustomerId",
                table: "Baskets",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDetails_CustomerId",
                table: "CustomerDetails",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodCategories_FoodChainId",
                table: "FoodCategories",
                column: "FoodChainId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodChains_OwnerId",
                table: "FoodChains",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrderItem_ordersId",
                table: "FoodOrderItem",
                column: "ordersId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_MenuCategoryId",
                table: "Foods",
                column: "MenuCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingridients_FoodId",
                table: "Ingridients",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_MenuId",
                table: "MenuCategories",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_FoodCategoryId",
                table: "Menus",
                column: "FoodCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemProduct_productsId",
                table: "OrderItemProduct",
                column: "productsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Prodactions_StoreCategoryId",
                table: "Prodactions",
                column: "StoreCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProdCategories_ProdactionId",
                table: "ProdCategories",
                column: "ProdactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProdCategoryId",
                table: "Products",
                column: "ProdCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_FoodCategoryId",
                table: "Schedules",
                column: "FoodCategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategories_StoreId",
                table: "StoreCategories",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_OwnerId",
                table: "Stores",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSchedules_StoreCategoryId",
                table: "StoreSchedules",
                column: "StoreCategoryId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketFood");

            migrationBuilder.DropTable(
                name: "BasketProduct");

            migrationBuilder.DropTable(
                name: "CustomerDetails");

            migrationBuilder.DropTable(
                name: "FoodOrderItem");

            migrationBuilder.DropTable(
                name: "Ingridients");

            migrationBuilder.DropTable(
                name: "OrderItemProduct");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "StoreSchedules");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProdCategories");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Prodactions");

            migrationBuilder.DropTable(
                name: "FoodCategories");

            migrationBuilder.DropTable(
                name: "StoreCategories");

            migrationBuilder.DropTable(
                name: "FoodChains");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
