namespace Application;

public interface IDataBaseContext
{
    DbSet<User> Users { get; }
    DbSet<Organization> Organizations { get; }

    Task MigrateAsync(CancellationToken token = default);
    Task<int> SaveChangesAsync(CancellationToken token = default);
    Task TrySeedAsync(CancellationToken token = default);
}