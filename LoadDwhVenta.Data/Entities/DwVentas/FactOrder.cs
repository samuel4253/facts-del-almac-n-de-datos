    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVenta.Data.Entities.DwVentas
{
    [Table("FactOrders")]
    public partial class FactOrder
    {
        [Key]
        public int OrderNumber { get; set; } // Clave primaria

        public int ProductKey { get; set; }  // Clave foránea de producto (de Vwventa.ProductId)
        public int EmployeeKey { get; set; } // Clave foránea de empleado (de Vwventa.EmployeeId)
        public int ShipperKey { get; set; }  // Clave foránea de shipper (de Vwventa.ShipperId)
        public int CustomerKey { get; set; } // Clave foránea de cliente (de Vwventa.CustomerId)

        public string? Country { get; set; } // País (de Vwventa.Country)

        public decimal TotalVentas { get; set; } // Total de ventas (de Vwventa.TotalVentas)
        public decimal? CantidadVentas { get; set; } // Cantidad vendida (de Vwventa.Cantidad)
    }
}
