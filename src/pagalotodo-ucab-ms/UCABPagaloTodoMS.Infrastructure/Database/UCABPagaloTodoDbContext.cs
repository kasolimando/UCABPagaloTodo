using System.Linq.Expressions;
using System.Reflection;
using UCABPagaloTodoMS.Core.Database;
using UCABPagaloTodoMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace UCABPagaloTodoMS.Infrastructure.Database;

[ExcludeFromCodeCoverage]
public class UCABPagaloTodoDbContext : DbContext, IUCABPagaloTodoDbContext
{
    public UCABPagaloTodoDbContext(DbContextOptions<UCABPagaloTodoDbContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<ValoresEntity> Valores { get; set; } = null!;

    public virtual DbSet<Administrador> Administrador { get; set; } = null!;
    public virtual DbSet<DeudaEntity> Deuda { get; set; } = null!;
    public virtual DbSet<ConsumidorEntity> Consumidor { get; set; } = null!;
    public virtual DbSet<FormatoConEntity> Formato { get; set; } = null!;
    public virtual DbSet<PagoEntity> Pago { get; set; } = null!;
    public virtual DbSet<PrestadorEntity> Prestador { get; set; } = null!;
    public virtual DbSet<ServicioEntity> Servicio { get; set; } = null!;
    public virtual DbSet<FormatoServicioEntity> FormatoServicio { get; set; } = null!;

    public DbContext DbContext
    {
        get
        {
            return this;
        }
    }

    public IDbContextTransactionProxy BeginTransaction()
    {
        return new DbContextTransactionProxy(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<DeudaEntity>().HasKey(c => new { c.Username, c.servicioId });
    }

    virtual public void SetPropertyIsModifiedToFalse<TEntity, TProperty>(TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression) where TEntity : class
    {
        Entry(entity).Property(propertyExpression).IsModified = false;
    }

    virtual public void ChangeEntityState<TEntity>(TEntity entity, EntityState state)
    {
        if (entity != null)
        {
            Entry(entity).State = state;
        }
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(string user, CancellationToken cancellationToken = default)
    {
        var state = new List<EntityState> { EntityState.Added, EntityState.Modified };

        var entries = ChangeTracker.Entries().Where(e =>
            e.Entity is BaseEntity && state.Any(s => e.State == s)
        );

        var dt = DateTime.UtcNow;

        return await base.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SaveEfContextChanges(CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(cancellationToken) >= 0;
    }

    public async Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default)
    {
        return await SaveChangesAsync(user, cancellationToken) >= 0;
    }
}
