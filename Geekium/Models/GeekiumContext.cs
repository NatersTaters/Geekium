using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountPurchase> AccountPurchases { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<ItemsForCart> ItemsForCarts { get; set; }
        public virtual DbSet<PriceTrend> PriceTrends { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-UO7UC42;Database=geekium_db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.PointBalance).HasColumnName("point_balance");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_password");
            });

            modelBuilder.Entity<AccountPurchase>(entity =>
            {
                entity.ToTable("accountPurchases");

                entity.Property(e => e.AccountPurchaseId).HasColumnName("account_purchase_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.PointsGained).HasColumnName("points_gained");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnType("date")
                    .HasColumnName("purchase_date");

                entity.Property(e => e.PurchasePrice).HasColumnName("purchase_price");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.SellerName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("seller_name");

                entity.Property(e => e.TrackingNumber).HasColumnName("tracking_number");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountPurchases)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountPurchases_fk_accounts");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.AccountPurchases)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountPurchases_fk_sellerAccounts");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.NumberOfProducts).HasColumnName("number_of_products");

                entity.Property(e => e.PointsGained).HasColumnName("points_gained");

                entity.Property(e => e.TotalPrice).HasColumnName("total_price");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cart_fk_accounts");
            });

            modelBuilder.Entity<ItemsForCart>(entity =>
            {
                entity.ToTable("itemsForCart");

                entity.Property(e => e.ItemsForCartId).HasColumnName("items_for_cart_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.SellListingId).HasColumnName("sell_listing_id");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.ItemsForCarts)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("itemsForCart_fk_cart");

                entity.HasOne(d => d.SellListing)
                    .WithMany(p => p.ItemsForCarts)
                    .HasForeignKey(d => d.SellListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("itemsForCart_fk_sellListings");
            });

            modelBuilder.Entity<PriceTrend>(entity =>
            {
                entity.ToTable("priceTrends");

                entity.Property(e => e.PriceTrendId).HasColumnName("price_trend_id");

                entity.Property(e => e.AveragePrice).HasColumnName("average_price");

                entity.Property(e => e.DateOfUpdate)
                    .HasColumnType("date")
                    .HasColumnName("date_of_update");

                entity.Property(e => e.HighestPrice).HasColumnName("highest_price");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("item_name");

                entity.Property(e => e.LowestPrice).HasColumnName("lowest_price");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("receipt");

                entity.Property(e => e.ReceiptId).HasColumnName("receipt_id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.CartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("receipt_fk_cart");
            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.ToTable("rewards");

                entity.Property(e => e.RewardId).HasColumnName("reward_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.DateReceived)
                    .HasColumnType("date")
                    .HasColumnName("date_received");

                entity.Property(e => e.PointCost).HasColumnName("point_cost");

                entity.Property(e => e.RewardCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("reward_code");

                entity.Property(e => e.RewardType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reward_type");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rewards_fk_accounts");
            });

            modelBuilder.Entity<SellListing>(entity =>
            {
                entity.ToTable("sellListings");

                entity.Property(e => e.SellListingId).HasColumnName("sell_listing_id");

                entity.Property(e => e.PriceTrendId).HasColumnName("price_trend_id");

                entity.Property(e => e.SellDate)
                    .HasColumnType("date")
                    .HasColumnName("sell_date");

                entity.Property(e => e.SellDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sell_description");

                entity.Property(e => e.SellItemType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sell_item_type");

                entity.Property(e => e.SellPrice).HasColumnName("sell_price");

                entity.Property(e => e.SellQuantity).HasColumnName("sell_quantity");

                entity.Property(e => e.SellTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sell_title");

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
                    .HasName("PK__sellerAc__780A0A9706F60101");

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
                entity.ToTable("sellerReviews");

                entity.Property(e => e.SellerReviewId).HasColumnName("seller_review_id");

                entity.Property(e => e.BuyerRating).HasColumnName("buyer_rating");

                entity.Property(e => e.ReviewDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("review_description");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.SellerReviews)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sellerReviews_fk_sellerAccounts");
            });

            modelBuilder.Entity<ServiceListing>(entity =>
            {
                entity.ToTable("serviceListings");

                entity.Property(e => e.ServiceListingId).HasColumnName("service_listing_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.ListingDate)
                    .HasColumnType("date")
                    .HasColumnName("listing_date");

                entity.Property(e => e.ServiceDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("service_description");

                entity.Property(e => e.ServiceTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("service_title");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.ServiceListings)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("serviceListings_fk_accounts");
            });

            modelBuilder.Entity<TradeListing>(entity =>
            {
                entity.ToTable("tradeListings");

                entity.Property(e => e.TradeListingId).HasColumnName("trade_listing_id");

                entity.Property(e => e.SellerId).HasColumnName("seller_id");

                entity.Property(e => e.TradeDate)
                    .HasColumnType("date")
                    .HasColumnName("trade_date");

                entity.Property(e => e.TradeDescription)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("trade_description");

                entity.Property(e => e.TradeFor)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("trade_for");

                entity.Property(e => e.TradeItemType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("trade_item_type");

                entity.Property(e => e.TradeQuantity).HasColumnName("trade_quantity");

                entity.Property(e => e.TradeTitle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("trade_title");

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
