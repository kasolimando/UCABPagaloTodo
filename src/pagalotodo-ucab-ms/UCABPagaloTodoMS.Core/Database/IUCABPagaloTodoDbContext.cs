using Microsoft.EntityFrameworkCore;
using UCABPagaloTodoMS.Core.Entities;

namespace UCABPagaloTodoMS.Core.Database
{
    public interface IUCABPagaloTodoDbContext
    {
        DbSet<Administrador> Administrador { get; } 
        DbSet<DeudaEntity> Deuda { get; }
        DbSet<ConsumidorEntity> Consumidor { get; }
        DbSet<FormatoConEntity> Formato { get; }
        DbSet<PagoEntity> Pago { get; }
        DbSet<PrestadorEntity> Prestador { get; }
        DbSet<ServicioEntity> Servicio { get; }
        DbSet<FormatoServicioEntity> FormatoServicio { get;}
        DbContext DbContext
        {
            get;
        }

        IDbContextTransactionProxy BeginTransaction();

        void ChangeEntityState<TEntity>(TEntity entity, EntityState state);

        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);
    }
}
