namespace Infrastructure.DataBase;

public class DataBaseContext : DbContext, IDataBaseContext
{
    public const string ConnectionString = "POSTGRESQL";

    public DataBaseContext() {}
    public DataBaseContext(DbContextOptions options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        ConfigureEntity(model.Entity<User>());
        ConfigureEntity(model.Entity<Organization>());
    }

    private static void ConfigureEntity(EntityTypeBuilder<Organization> entity)
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        entity.HasIndex(x => x.Id).IsUnique();
        entity.HasIndex(x => x.Name);
    }

    private static void ConfigureEntity(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(x => x.Id);
        
        entity.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(100);

        entity.Property(x => x.Patronymic)
            .HasMaxLength(100);

        entity.Property(x => x.Mail)
            .IsRequired()
            .HasMaxLength(320);

        entity.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        entity.HasIndex(x => x.Name);
        entity.HasIndex(x => x.Surname);
        entity.HasIndex(x => x.Mail).IsUnique();
        entity.HasIndex(x => x.PhoneNumber).IsUnique();
        
        entity.HasIndex(x => x.Id).IsUnique();
    }

    public Task MigrateAsync(CancellationToken token = default) =>
        Database.MigrateAsync(token);

    public async Task TrySeedAsync(CancellationToken token = default)
    {
        if (Users.Any())
            return;

        User user = new()
        {
            Name = "Bob",
            Surname = "Martin",
            Patronymic = "Cecil",
            PhoneNumber = "12535475123",
            Mail = "bobmartin@gmail.com",
        };
        await Users.AddAsync(user, token);
        await SaveChangesAsync(token);
        
        Organization organization = new()
        {
            Name = "Google"
        };
        await Organizations.AddAsync(organization, token);
        await SaveChangesAsync(token);
    }
}