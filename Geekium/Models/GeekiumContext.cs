using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Geekium.Models
{
    public partial class GeekiumContext : DbContext
    {
        public GeekiumContext()
        {
        }

        public GeekiumContext(DbContextOptions<GeekiumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountPurchase> AccountPurchases { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<ItemsForCart> ItemsForCart { get; set; }
        public virtual DbSet<PriceTrend> PriceTrends { get; set; }
        public virtual DbSet<Receipt> Receipt { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<SellListing> SellListings { get; set; }
        public virtual DbSet<SellerAccount> SellerAccounts { get; set; }
        public virtual DbSet<SellerReview> SellerReviews { get; set; }
        public virtual DbSet<ServiceListing> ServiceListings { get; set; }
        public virtual DbSet<TradeListing> TradeListings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress19;Database=geekium_db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountPurchase>(entity =>
            {
                entity.HasKey(e => e.AccountPurchaseId)
                    .HasName("PK__accountP__40A9D28D9A169BA3");

                entity.ToTable("accountPurchases");

                entity.Property(e => e.AccountPurchaseId).HasColumnName("account_purchase_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.PointsGained).HasColumnName("points_gained");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnName("purchase_date")
                    .HasColumnType("date");

                entity.Property(e => e.PurchasePrice).HasColumnName("purchase_price");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.SellerName)
                    .IsRequired()
                    .HasColumnName("seller_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrackingNumber).HasColumnName("tracking_number");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountPurchases)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountPurchases_fk_accounts");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.AccountPurchases)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountPurchases_fk_cart");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.AccountPurchases)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountPurchases_fk_sellerAccounts");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__accounts__46A222CD3C4F137E");

                entity.ToTable("accounts");

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__accounts__7C9273C482CBFE49")
                    .IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaswordHash)
                    .IsRequired()
                    .HasColumnName("pasword_hash")
                    .IsUnicode(false);

                entity.Property(e => e.PointBalance).HasColumnName("point_balance");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("user_password")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.NumberOfProducts).HasColumnName("number_of_products");

                entity.Property(e => e.PointsGained).HasColumnName("points_gained");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price");

                entity.Property(e => e.TransactionComplete)
                    .HasColumnName("transactionComplete")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cart_fk_accounts");
            });

            modelBuilder.Entity<ItemsForCart>(entity =>
            {
                entity.ToTable("itemsForCart");

                entity.Property(e => e.ItemsForCartId).HasColumnName("items_for_cart_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.SellListingId).HasColumnName("sell_listing_id");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.ItemsForCart)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("itemsForCart_fk_cart");

                entity.HasOne(d => d.SellListing)
                    .WithMany(p => p.ItemsForCart)
                    .HasForeignKey(d => d.SellListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("itemsForCart_fk_sellListings");
            });

            modelBuilder.Entity<PriceTrend>(entity =>
            {
                entity.HasKey(e => e.PriceTrendId)
                    .HasName("PK__priceTre__A1354F7027A935D4");

                entity.ToTable("priceTrends");

                entity.Property(e => e.PriceTrendId).HasColumnName("price_trend_id");

                entity.Property(e => e.AveragePrice).HasColumnName("average_price");

                entity.Property(e => e.DateOfUpdate)
                    .HasColumnName("date_of_update")
                    .HasColumnType("date");

                entity.Property(e => e.HighestPrice).HasColumnName("highest_price");

                entity.Property(e => e.ItemName)
                    .HasColumnName("item_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LowestPrice).HasColumnName("lowest_price");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("receipt");

                entity.Property(e => e.ReceiptId).HasColumnName("receipt_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Receipt)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("receipt_fk_cart");
            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.HasKey(e => e.RewardId)
                    .HasName("PK__rewards__3DD599BCB472D69A");

                entity.ToTable("rewards");

                entity.Property(e => e.RewardId).HasColumnName("reward_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.DateReceived)
                    .HasColumnName("date_received")
                    .HasColumnType("date");

                entity.Property(e => e.PointCost).HasColumnName("point_cost");

                entity.Property(e => e.RewardCode)
                    .IsRequired()
                    .HasColumnName("reward_code")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RewardType)
                    .IsRequired()
                    .HasColumnName("reward_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rewards_fk_accounts");
            });

            modelBuilder.Entity<SellListing>(entity =>
            {
                entity.HasKey(e => e.SellListingId)
                    .HasName("PK__sellList__BC5EA263E112CA88");

                entity.ToTable("sellListings");

                entity.Property(e => e.SellListingId).HasColumnName("sell_listing_id");

                entity.Property(e => e.PriceTrendId).HasColumnName("price_trend_id");

                entity.Property(e => e.PriceTrendKeywords)
                    .HasColumnName("price_trend_keywords")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SellDate)
                    .HasColumnName("sell_date")
                    .HasColumnType("date");

                entity.Property(e => e.SellDescription)
                    .IsRequired()
                    .HasColumnName("sell_description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SellImage)
                    .HasColumnName("sell_image")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SellItemType)
                    .IsRequired()
                    .HasColumnName("sell_item_type")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SellPrice).HasColumnName("sell_price");

                entity.Property(e => e.SellQuantity).HasColumnName("sell_quantity");

                entity.Property(e => e.SellTitle)
                    .IsRequired()
                    .HasColumnName("sell_title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.HasOne(d => d.PriceTrend)
                    .WithMany(p => p.SellListings)
                    .HasForeignKey(d => d.PriceTrendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sellListings_fk_priceTrends");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellListings)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sellListings_fk_sellerAccounts");
            });

            modelBuilder.Entity<SellerAccount>(entity =>
            {
                entity.HasKey(e => e.SellerId)
                    .HasName("PK__sellerAc__780A0A97A3EC8D99");

                entity.ToTable("sellerAccounts");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.AverageRating).HasColumnName("average_rating");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.SellerAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sellerAccounts_fk_accounts");
            });

            modelBuilder.Entity<SellerReview>(entity =>
            {
                entity.HasKey(e => e.SellerReviewId)
                    .HasName("PK__sellerRe__E62B70AF3CC066A9");

                entity.ToTable("sellerReviews");

                entity.Property(e => e.SellerReviewId).HasColumnName("seller_review_id");

                entity.Property(e => e.BuyerRating).HasColumnName("buyer_rating");

                entity.Property(e => e.ReviewDescription)
                    .HasColumnName("review_description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellerReviews)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sellerReviews_fk_sellerAccounts");
            });

            modelBuilder.Entity<ServiceListing>(entity =>
            {
                entity.HasKey(e => e.ServiceListingId)
                    .HasName("PK__serviceL__6F79AD45AB047C2D");

                entity.ToTable("serviceListings");

                entity.Property(e => e.ServiceListingId).HasColumnName("service_listing_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ListingDate)
                    .HasColumnName("listing_date")
                    .HasColumnType("date");

                entity.Property(e => e.ServiceDescription)
                    .IsRequired()
                    .HasColumnName("service_description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceImage)
                    .HasColumnName("service_image")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceTitle)
                    .IsRequired()
                    .HasColumnName("service_title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ServiceListings)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("serviceListings_fk_accounts");
            });

            modelBuilder.Entity<TradeListing>(entity =>
            {
                entity.HasKey(e => e.TradeListingId)
                    .HasName("PK__tradeLis__609C8CAEE860F261");

                entity.ToTable("tradeListings");

                entity.Property(e => e.TradeListingId).HasColumnName("trade_listing_id");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.TradeDate)
                    .HasColumnName("trade_date")
                    .HasColumnType("date");

                entity.Property(e => e.TradeDescription)
                    .IsRequired()
                    .HasColumnName("trade_description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TradeFor)
                    .IsRequired()
                    .HasColumnName("trade_for")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TradeImage)
                    .HasColumnName("trade_image")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TradeItemType)
                    .IsRequired()
                    .HasColumnName("trade_item_type")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TradeLocation)
                    .HasColumnName("trade_location")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TradeQuantity).HasColumnName("trade_quantity");

                entity.Property(e => e.TradeTitle)
                    .IsRequired()
                    .HasColumnName("trade_title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.TradeListings)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tradeListings_fk_sellerAccounts");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
