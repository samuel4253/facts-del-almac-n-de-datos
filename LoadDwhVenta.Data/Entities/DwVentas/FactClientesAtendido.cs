
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoadDwhVenta.Data.Entities.DwVentas
{
    [Table("FactClientesAtendidos")]

    public partial class FactClientesAtendido
    {
        [Key]
        public int ClienteAtendidoId { get; set; }

        public int EmployeeKey { get; set; }

        public int TotalClientes { get; set; }


    }
}