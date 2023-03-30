using System.Data;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }


    public IDbConnection Connection => Database.GetDbConnection();

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public DbSet<Domain.Entities.NOnbir.Category> N11Category => Set<Domain.Entities.NOnbir.Category>();
    public DbSet<Domain.Entities.NOnbir.Attribute> N11Attribute => Set<Domain.Entities.NOnbir.Attribute>();

    public DbSet<Domain.Entities.NOnbir.AttributeValue> N11AttributeValue =>
        Set<Domain.Entities.NOnbir.AttributeValue>();

    public DbSet<Domain.Entities.Trendyol.Category> TrendyolCategory => Set<Domain.Entities.Trendyol.Category>();
    public DbSet<Domain.Entities.Trendyol.Attribute> TrendyolAttribute => Set<Domain.Entities.Trendyol.Attribute>();

    public DbSet<Domain.Entities.Trendyol.AttributeValue> TrendyolAttributeValue =>
        Set<Domain.Entities.Trendyol.AttributeValue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}