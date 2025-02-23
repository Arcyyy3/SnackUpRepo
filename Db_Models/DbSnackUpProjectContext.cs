using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SnackUpAPI.Db_Models;

public partial class DbSnackUpProjectContext : DbContext
{
    public DbSnackUpProjectContext()
    {
    }

    public DbSnackUpProjectContext(DbContextOptions<DbSnackUpProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allergen> Allergens { get; set; }

    public virtual DbSet<BundleItem> BundleItems { get; set; }

    public virtual DbSet<BundleProduct> BundleProducts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }

    public virtual DbSet<ClassDeliveryCode> ClassDeliveryCodes { get; set; }

    public virtual DbSet<ClassDeliveryLog> ClassDeliveryLogs { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryHistory> InventoryHistories { get; set; }

    public virtual DbSet<LoggerClient> LoggerClients { get; set; }

    public virtual DbSet<LoggerServer> LoggerServers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderTracking> OrderTrackings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<ProducerUser> ProducerUsers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductAllergen> ProductAllergens { get; set; }

    public virtual DbSet<ProductPromotion> ProductPromotions { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

    public virtual DbSet<ShoppingSession> ShoppingSessions { get; set; }

    public virtual DbSet<SupportRequest> SupportRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.249.188,1433;Database=DB_SnackUpProject;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.HasKey(e => e.AllergenId).HasName("PK__Allergen__158B937FAD6E2C38");

            entity.HasIndex(e => e.AllergenName, "UQ__Allergen__7D9886198A25F3F1").IsUnique();

            entity.Property(e => e.AllergenId).HasColumnName("AllergenID");
            entity.Property(e => e.AllergenName).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Modified).HasColumnType("datetime");
        });

        modelBuilder.Entity<BundleItem>(entity =>
        {
            entity.HasKey(e => e.BundleItemId).HasName("PK__BundleIt__FE10C843078FAA1A");

            entity.Property(e => e.BundleItemId).HasColumnName("BundleItemID");
            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Bundle).WithMany(p => p.BundleItems)
                .HasForeignKey(d => d.BundleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BundleItems_Bundles");

            entity.HasOne(d => d.Product).WithMany(p => p.BundleItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_BundleItems_Products");
        });

        modelBuilder.Entity<BundleProduct>(entity =>
        {
            entity.HasKey(e => e.BundleId).HasName("PK__BundlePr__42003BB13762A5C5");

            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.BundleName).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.Moment).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B2ABA1A874C");

            entity.Property(e => e.CartItemId).HasColumnName("CartItemID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.Recreation).HasMaxLength(50);
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__CartItems__Produ__58147813");

            entity.HasOne(d => d.Session).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SessionId)
                .HasConstraintName("FK__CartItems__Sessi__572053DA");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B6FB12405");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0B220B4E9").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Modified).HasColumnType("datetime");
        });

        modelBuilder.Entity<CategoryProduct>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryP__Categ__413112BB");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CategoryP__Produ__403CEE82");
        });

        modelBuilder.Entity<ClassDeliveryCode>(entity =>
        {
            entity.HasKey(e => e.ClassDeliveryCodeId).HasName("PK__ClassDel__F6467E3B5E81BD62");

            entity.Property(e => e.ClassDeliveryCodeId).HasColumnName("ClassDeliveryCodeID");
            entity.Property(e => e.Code1)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Code2)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.RetrievedCode1).HasDefaultValue(false);
            entity.Property(e => e.RetrievedCode2).HasDefaultValue(false);
            entity.Property(e => e.SchoolClassId).HasColumnName("SchoolClassID");

            entity.HasOne(d => d.SchoolClass).WithMany(p => p.ClassDeliveryCodes)
                .HasForeignKey(d => d.SchoolClassId)
                .HasConstraintName("FK_ClassDeliveryCodes_SchoolClasses");
        });

        modelBuilder.Entity<ClassDeliveryLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__ClassDel__5E5499A862D84719");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.ClassDeliveryCodeId).HasColumnName("ClassDeliveryCodeID");
            entity.Property(e => e.CodeType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ClassDeliveryCode).WithMany(p => p.ClassDeliveryLogs)
                .HasForeignKey(d => d.ClassDeliveryCodeId)
                .HasConstraintName("FK__ClassDeli__Class__619DE24D");

            entity.HasOne(d => d.User).WithMany(p => p.ClassDeliveryLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ClassDeli__UserI__62920686");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Inventor__B40CC6EDC20F5102");

            entity.ToTable("Inventory");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.QuantityReserved).HasDefaultValue(0);
            entity.Property(e => e.ReorderLevel).HasDefaultValue(10);

            entity.HasOne(d => d.Product).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.ProductId)
                .HasConstraintName("FK_Inventory_Products");
        });

        modelBuilder.Entity<InventoryHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Inventor__4D7B4ADDEC2990A8");

            entity.ToTable("InventoryHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.ChangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ChangeReason).HasMaxLength(255);
            entity.Property(e => e.ChangedBy).HasMaxLength(100);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryHistories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Inventory__Produ__4CA2C567");
        });

        modelBuilder.Entity<LoggerClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoggerCl__3214EC27806A946B");

            entity.ToTable("LoggerClient");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LogMessage).HasMaxLength(4000);
            entity.Property(e => e.LogSource).HasMaxLength(255);
            entity.Property(e => e.StackTrace).HasMaxLength(4000);
        });

        modelBuilder.Entity<LoggerServer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoggerSe__3214EC279255B5B4");

            entity.ToTable("LoggerServer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LogMessage).HasMaxLength(4000);
            entity.Property(e => e.LogSource).HasMaxLength(255);
            entity.Property(e => e.StackTrace).HasMaxLength(4000);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF76828EA6");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.SchoolClassId).HasColumnName("SchoolClassID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.SchoolClass).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SchoolClassId)
                .HasConstraintName("FK_Orders_SchoolClasses");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__OrderDet__135C314D0DEEC33C");

            entity.Property(e => e.DetailId).HasColumnName("DetailID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductCode).HasMaxLength(10);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Recreation).HasMaxLength(10);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<OrderTracking>(entity =>
        {
            entity.HasKey(e => e.OrderTrackingId).HasName("PK__OrderTra__F4A0A6EEC28B191F");

            entity.Property(e => e.OrderTrackingId).HasColumnName("OrderTrackingID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderTrackings)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderTrackings_Orders");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A5838DF597A");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Payments_Orders");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.HasKey(e => e.ProducerId).HasName("PK__Producer__133696B2554B0DEF");

            entity.Property(e => e.ProducerId).HasColumnName("ProducerID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.PhotoLinkProduttore)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProducerName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProducerUser>(entity =>
        {
            entity.HasKey(e => new { e.ProducerId, e.UserId }).HasName("PK__Producer__C24E1A78115E6EA1");

            entity.Property(e => e.ProducerId).HasColumnName("ProducerID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");

            entity.HasOne(d => d.Producer).WithMany(p => p.ProducerUsers)
                .HasForeignKey(d => d.ProducerId)
                .HasConstraintName("FK__ProducerU__Produ__6EF7DD6B");

            entity.HasOne(d => d.User).WithMany(p => p.ProducerUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProducerU__UserI__6FEC01A4");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED393C79A8");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BundleId).HasColumnName("BundleID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Details).HasColumnType("text");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.PhotoLinkProdotto)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProducerId).HasColumnName("ProducerID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Raccomandation).HasColumnType("text");

            entity.HasOne(d => d.Producer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProducerId)
                .HasConstraintName("FK_Products_Producers");
        });

        modelBuilder.Entity<ProductAllergen>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.AllergenId }).HasName("PK__ProductA__55547FDA621F00F8");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.AllergenId).HasColumnName("AllergenID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");

            entity.HasOne(d => d.Allergen).WithMany(p => p.ProductAllergens)
                .HasForeignKey(d => d.AllergenId)
                .HasConstraintName("FK__ProductAl__Aller__6B274C87");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductAllergens)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductAl__Produ__6A33284E");
        });

        modelBuilder.Entity<ProductPromotion>(entity =>
        {
            entity.HasKey(e => e.ProductPromotionId).HasName("PK__ProductP__9E52FB237E97139A");

            entity.Property(e => e.ProductPromotionId).HasColumnName("ProductPromotionID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPromotions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductPromotions_Products");

            entity.HasOne(d => d.Promotion).WithMany(p => p.ProductPromotions)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_ProductPromotions_Promotions");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42F2F21A83407");

            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.PromotionName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07033D407E");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefreshTokens_Users");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PK__Schools__3DA4677BE52D4612");

            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ProducerId).HasColumnName("ProducerID");
            entity.Property(e => e.SchoolName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SchoolClass>(entity =>
        {
            entity.HasKey(e => e.SchoolClassId).HasName("PK__SchoolCl__24FB0D5010BE604D");

            entity.Property(e => e.SchoolClassId).HasColumnName("SchoolClassID");
            entity.Property(e => e.ClassSection)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

            entity.HasOne(d => d.School).WithMany(p => p.SchoolClasses)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_SchoolClasses_Schools");
        });

        modelBuilder.Entity<ShoppingSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Shopping__C9F4927053552673");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ShoppingS__UserI__525B9EBD");
        });

        modelBuilder.Entity<SupportRequest>(entity =>
        {
            entity.HasKey(e => e.SupportRequestId).HasName("PK__SupportR__CEDE143C8DA07365");

            entity.Property(e => e.SupportRequestId).HasColumnName("SupportRequestID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Order).WithMany(p => p.SupportRequests)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_SupportRequests_Orders");

            entity.HasOne(d => d.User).WithMany(p => p.SupportRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_SupportRequests_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC4846A1AE");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B7DE6454").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SchoolClassId).HasColumnName("SchoolClassID");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SchoolClass).WithMany(p => p.Users)
                .HasForeignKey(d => d.SchoolClassId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Users_SchoolClasses");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Wallets__84D4F92E0D6028AE");

            entity.Property(e => e.WalletId).HasColumnName("WalletID");
            entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Wallets_Users");
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__WalletTr__55433A4B399609EF");

            entity.HasIndex(e => e.WalletId, "IDX_WalletTransactions_WalletID");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_WalletTransactions_Wallets");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
