using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVenta.Data.Entities.Northwind
{
    [Table("VwServedCustomer", Schema = "DWH")]
    public class VwServedCustomer
    {
        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public int TotalCustomersServed { get; set; }
    }
}