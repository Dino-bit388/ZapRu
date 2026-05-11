using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZapRu.Migrations
{
    /// <inheritdoc />
    public partial class AbsolB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    CarModelID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    YearStart = table.Column<int>(type: "integer", nullable: true),
                    YearEnd = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.CarModelID);
                });

            migrationBuilder.CreateTable(
                name: "PartsCatalogs",
                columns: table => new
                {
                    PartCatID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsCatalogs", x => x.PartCatID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SellerID = table.Column<int>(type: "integer", nullable: false),
                    CarModelID = table.Column<int>(type: "integer", nullable: false),
                    VIN = table.Column<string>(type: "character varying(17)", maxLength: 17, nullable: true),
                    Mileage = table.Column<int>(type: "integer", nullable: true),
                    Condition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ManufactureYear = table.Column<int>(type: "integer", nullable: false)
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
                    PriceEstimateID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    CarModelID = table.Column<int>(type: "integer", nullable: false),
                    Mileage = table.Column<int>(type: "integer", nullable: true),
                    Condition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
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
                    PartID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarID = table.Column<int>(type: "integer", nullable: false),
                    PartCatID = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Condition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsSold = table.Column<bool>(type: "boolean", nullable: false),
                    SoldAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ViewsCount = table.Column<int>(type: "integer", nullable: false)
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
                    NotificationID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    PartID = table.Column<int>(type: "integer", nullable: true),
                    FromUserID = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResponseStatus = table.Column<bool>(type: "boolean", nullable: true)
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
                    PhotoID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    EntityID = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Uploaded_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CarID = table.Column<int>(type: "integer", nullable: true),
                    PartID = table.Column<int>(type: "integer", nullable: true)
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
                    TransactionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartID = table.Column<int>(type: "integer", nullable: false),
                    BuyerID = table.Column<int>(type: "integer", nullable: false),
                    SellerID = table.Column<int>(type: "integer", nullable: false),
                    AgreedPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    BidID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartID = table.Column<int>(type: "integer", nullable: false),
                    BuyerID = table.Column<int>(type: "integer", nullable: false),
                    SellerID = table.Column<int>(type: "integer", nullable: false),
                    OfferedPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TransactionID = table.Column<int>(type: "integer", nullable: true)
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
                    ContractID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionID = table.Column<int>(type: "integer", nullable: false),
                    ContractNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SignedBySeller = table.Column<bool>(type: "boolean", nullable: false),
                    SignedByBuyer = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
