using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZapRu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    CarModelID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    YearStart = table.Column<int>(type: "INTEGER", nullable: true),
                    YearEnd = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.CarModelID);
                });

            migrationBuilder.CreateTable(
                name: "PartsCatalogs",
                columns: table => new
                {
                    PartCatID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsCatalogs", x => x.PartCatID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsBlocked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SellerID = table.Column<int>(type: "INTEGER", nullable: false),
                    CarModelID = table.Column<int>(type: "INTEGER", nullable: false),
                    VIN = table.Column<string>(type: "TEXT", maxLength: 17, nullable: true),
                    Mileage = table.Column<int>(type: "INTEGER", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ManufactureYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarID);
                    table.ForeignKey(
                        name: "FK_Cars_CarModels_CarModelID",
                        column: x => x.CarModelID,
                        principalTable: "CarModels",
                        principalColumn: "CarModelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cars_Users_SellerID",
                        column: x => x.SellerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceEstimates",
                columns: table => new
                {
                    PriceEstimateID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    CarModelID = table.Column<int>(type: "INTEGER", nullable: false),
                    Mileage = table.Column<int>(type: "INTEGER", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceEstimates", x => x.PriceEstimateID);
                    table.ForeignKey(
                        name: "FK_PriceEstimates_CarModels_CarModelID",
                        column: x => x.CarModelID,
                        principalTable: "CarModels",
                        principalColumn: "CarModelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceEstimates_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarID = table.Column<int>(type: "INTEGER", nullable: false),
                    PartCatID = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsSold = table.Column<bool>(type: "INTEGER", nullable: false),
                    SoldAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ViewsCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartID);
                    table.ForeignKey(
                        name: "FK_Parts_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "CarID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parts_PartsCatalogs_PartCatID",
                        column: x => x.PartCatID,
                        principalTable: "PartsCatalogs",
                        principalColumn: "PartCatID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    PartID = table.Column<int>(type: "INTEGER", nullable: true),
                    FromUserID = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResponseStatus = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notifications_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_FromUserID",
                        column: x => x.FromUserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityType = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    EntityID = table.Column<int>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Uploaded_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CarID = table.Column<int>(type: "INTEGER", nullable: true),
                    PartID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoID);
                    table.ForeignKey(
                        name: "FK_Photos_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "CarID");
                    table.ForeignKey(
                        name: "FK_Photos_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartID = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerID = table.Column<int>(type: "INTEGER", nullable: false),
                    SellerID = table.Column<int>(type: "INTEGER", nullable: false),
                    AgreedPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transactions_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_BuyerID",
                        column: x => x.BuyerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_SellerID",
                        column: x => x.SellerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    BidID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PartID = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerID = table.Column<int>(type: "INTEGER", nullable: false),
                    SellerID = table.Column<int>(type: "INTEGER", nullable: false),
                    OfferedPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.BidID);
                    table.ForeignKey(
                        name: "FK_Bids_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "PartID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bids_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID");
                    table.ForeignKey(
                        name: "FK_Bids_Users_BuyerID",
                        column: x => x.BuyerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bids_Users_SellerID",
                        column: x => x.SellerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionID = table.Column<int>(type: "INTEGER", nullable: false),
                    ContractNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    SignedBySeller = table.Column<bool>(type: "INTEGER", nullable: false),
                    SignedByBuyer = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractID);
                    table.ForeignKey(
                        name: "FK_Contracts_Transactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "Transactions",
                        principalColumn: "TransactionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BuyerID",
                table: "Bids",
                column: "BuyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_PartID",
                table: "Bids",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_SellerID",
                table: "Bids",
                column: "SellerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_TransactionID",
                table: "Bids",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarModelID",
                table: "Cars",
                column: "CarModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_SellerID",
                table: "Cars",
                column: "SellerID");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TransactionID",
                table: "Contracts",
                column: "TransactionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FromUserID",
                table: "Notifications",
                column: "FromUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PartID",
                table: "Notifications",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserID",
                table: "Notifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CarID",
                table: "Parts",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_PartCatID",
                table: "Parts",
                column: "PartCatID");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_CarID",
                table: "Photos",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PartID",
                table: "Photos",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_PriceEstimates_CarModelID",
                table: "PriceEstimates",
                column: "CarModelID");

            migrationBuilder.CreateIndex(
                name: "IX_PriceEstimates_UserID",
                table: "PriceEstimates",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BuyerID",
                table: "Transactions",
                column: "BuyerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PartID",
                table: "Transactions",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SellerID",
                table: "Transactions",
                column: "SellerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "PriceEstimates");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "PartsCatalogs");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
