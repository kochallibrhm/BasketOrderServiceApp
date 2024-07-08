namespace BasketOrderServiceApp.OrderService.Data;

public class BasketOrderServiceAppContext : DbContext {
    public BasketOrderServiceAppContext(DbContextOptions<BasketOrderServiceAppContext> options) : base(options) {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<Order>()
            .Property(o => o.Id)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<Order>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .IsRequired(); 
    }
}
