

using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVenta.Data.Entities.Northwind
{
    [Table("VWVentas", Schema = "DWH")]
    public class Vwventa
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int ShipperId { get; set; }
        public string? Shipper { get; set; }
        public string? Country { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? TotalVentas { get; set; }
        public int? Cantidad { get; set; }
    }

}
