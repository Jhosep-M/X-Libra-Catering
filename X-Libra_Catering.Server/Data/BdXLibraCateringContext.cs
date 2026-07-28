using Microsoft.EntityFrameworkCore;
using X_Libra_Catering.Server.Models;

namespace X_Libra_Catering.Server.Data
{
    public partial class BdXLibraCateringContext : DbContext
    {
        public BdXLibraCateringContext(DbContextOptions<BdXLibraCateringContext> options) : base(options) { }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Evento> Eventos { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Vehiculo> Vehiculos { get; set; }
        public virtual DbSet<PedidoCabecera> PedidoCabeceras { get; set; }
        public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("CLIENTES");
                entity.HasKey(e => e.Id).HasName("PK_CLIENTES");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nombre).HasColumnName("NOMBRE").HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Telefono).HasColumnName("TELEFONO").HasMaxLength(15).IsUnicode(false);
                entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Direccion).HasColumnName("DIRECCION").HasMaxLength(200).IsUnicode(false);
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.ToTable("EVENTOS");
                entity.HasKey(e => e.Id).HasName("PK_EVENTOS");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ClienteId).HasColumnName("CLIENTE_ID");
                entity.Property(e => e.NombreEvento).HasColumnName("NOMBRE_EVENTO").HasMaxLength(150).IsUnicode(false);
                entity.Property(e => e.TipoEvento).HasColumnName("TIPO_EVENTO").HasConversion<string>().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Estado).HasColumnName("ESTADO").HasConversion<string>().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.FechaEvento).HasColumnName("FECHA_EVENTO");
                entity.Property(e => e.Ubicacion).HasColumnName("UBICACION").HasMaxLength(200).IsUnicode(false);
                entity.Property(e => e.NumInvitados).HasColumnName("NUM_INVITADOS");
                entity.HasOne(e => e.Cliente).WithMany(c => c.Eventos).HasForeignKey(e => e.ClienteId).HasConstraintName("FK_EVENTOS_CLIENTES");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("MENUS");
                entity.HasKey(e => e.Id).HasName("PK_MENUS");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Nombre).HasColumnName("NOMBRE").HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Descripcion).HasColumnName("DESCRIPCION").HasMaxLength(500).IsUnicode(false);
                entity.Property(e => e.Categoria).HasColumnName("CATEGORIA").HasConversion<string>().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Precio).HasColumnName("PRECIO").HasColumnType("decimal(10,2)");
                entity.Property(e => e.RequiereRefrigeracion).HasColumnName("REQUIERE_REFRIGERACION");
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.ToTable("VEHICULOS");
                entity.HasKey(e => e.Id).HasName("PK_VEHICULOS");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Marca).HasColumnName("MARCA").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Modelo).HasColumnName("MODELO").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Placa).HasColumnName("PLACA").HasMaxLength(15).IsUnicode(false);
                entity.HasIndex(e => e.Placa).IsUnique().HasDatabaseName("UQ_VEHICULOS_PLACA");
                entity.Property(e => e.CapacidadKg).HasColumnName("CAPACIDAD_KG").HasColumnType("decimal(8,2)");
                entity.Property(e => e.TieneRefrigeracion).HasColumnName("TIENE_REFRIGERACION");
                entity.Property(e => e.Disponible).HasColumnName("DISPONIBLE");
            });

            modelBuilder.Entity<PedidoCabecera>(entity =>
            {
                entity.ToTable("PEDIDO_CABECERA");
                entity.HasKey(e => e.Id).HasName("PK_PEDIDO_CABECERA");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.EventoId).HasColumnName("EVENTO_ID");
                entity.Property(e => e.VehiculoId).HasColumnName("VEHICULO_ID");
                entity.Property(e => e.FechaPedido).HasColumnName("FECHA_PEDIDO");
                entity.Property(e => e.Estado).HasColumnName("ESTADO").HasConversion<string>().HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Total).HasColumnName("TOTAL").HasColumnType("decimal(12,2)");
                entity.HasOne(e => e.Evento).WithMany(e => e.Pedidos).HasForeignKey(e => e.EventoId).HasConstraintName("FK_PEDIDO_CABECERA_EVENTOS");
                entity.HasOne(e => e.Vehiculo).WithMany(v => v.Pedidos).HasForeignKey(e => e.VehiculoId).HasConstraintName("FK_PEDIDO_CABECERA_VEHICULOS");
            });

            modelBuilder.Entity<PedidoDetalle>(entity =>
            {
                entity.ToTable("PEDIDO_DETALLE");
                entity.HasKey(e => e.Id).HasName("PK_PEDIDO_DETALLE");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.PedidoId).HasColumnName("PEDIDO_ID");
                entity.Property(e => e.MenuId).HasColumnName("MENU_ID");
                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");
                entity.Property(e => e.PrecioUnitario).HasColumnName("PRECIO_UNITARIO").HasColumnType("decimal(10,2)");
                entity.Property(e => e.Subtotal).HasColumnName("SUBTOTAL").HasColumnType("decimal(12,2)");
                entity.HasOne(e => e.Pedido).WithMany(p => p.Detalles).HasForeignKey(e => e.PedidoId).HasConstraintName("FK_PEDIDO_DETALLE_PEDIDO_CABECERA");
                entity.HasOne(e => e.Menu).WithMany(m => m.PedidoDetalles).HasForeignKey(e => e.MenuId).HasConstraintName("FK_PEDIDO_DETALLE_MENUS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
