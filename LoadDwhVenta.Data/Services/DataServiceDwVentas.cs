using LoadDwhVenta.Data.Contexts.DwVentas;
using LoadDwhVenta.Data.Contexts.Nortwind;
using LoadDwhVenta.Data.Core;
using LoadDwhVenta.Data.Entities.DwVentas;
using LoadDwhVenta.Data.Entities.Northwind;
using LoadDWVentas.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoadDWVentas.Data.Services
{
    public class DataServiceDwVentas : IDataServiceDwVentas
    {
        private readonly NorthwindContext _northwindContext;
        private readonly DwhVentasContext _dwhVentasContext;

        public DataServiceDwVentas(NorthwindContext northwindContext,
                                   DwhVentasContext dwhVentasContext)
        {
            _northwindContext = northwindContext;
            _dwhVentasContext = dwhVentasContext;
        }

        public async Task<OperactionResult> LoadDHW()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                //await LoadCategory();
                //await LoadCustomer();
                //await LoadEmployee();
                //await LoadProduct();
                //await LoadShipper();
                //await loadClienteAtentidos();
                await LoadFactOrders();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el DWH Ventas. {ex.Message}";
            }

            return result;
        }

        private async Task<OperactionResult> LoadCategory()
        {
            OperactionResult operation = new OperactionResult();
            try
            {
                // Eliminar datos existentes
                await _dwhVentasContext.DimCategories.ExecuteDeleteAsync();

                // Cargar nuevas categorías
                var categories = await _northwindContext.Categories.Select(cat => new DimCategory()
                {
                    CategoryName = cat.CategoryName,
                    CategoryId = cat.CategoryId
                }).AsNoTracking().ToListAsync();

                await _dwhVentasContext.DimCategories.AddRangeAsync(categories);
                await _dwhVentasContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de categoría. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperactionResult> LoadCustomer()
        {
            OperactionResult operation = new OperactionResult();
            try
            {
                // Eliminar datos existentes
                await _dwhVentasContext.DimCustomers.ExecuteDeleteAsync();

                // Obtener y cargar clientes
                var customers = await _northwindContext.Customers.Select(cust => new DimCustomer()
                {
                    CustomerName = cust.ContactName,
                    CustomerId = cust.CustomerId
                }).AsNoTracking().ToListAsync();

                await _dwhVentasContext.DimCustomers.AddRangeAsync(customers);
                await _dwhVentasContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de cliente. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperactionResult> LoadEmployee()
        {
            OperactionResult operation = new OperactionResult();
            try
            {
                // Eliminar datos existentes
                await _dwhVentasContext.DimEmployees.ExecuteDeleteAsync();

                // Obtener y cargar empleados
                var employees = await _northwindContext.Employees.Select(emp => new DimEmployee()
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeName = string.Concat(emp.FirstName, " ", emp.LastName)
                }).AsNoTracking().ToListAsync();

                await _dwhVentasContext.DimEmployees.AddRangeAsync(employees);
                await _dwhVentasContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de empleado. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperactionResult> LoadProduct()
        {
            OperactionResult operation = new OperactionResult();
            try
            {
                // Eliminar datos existentes
                await _dwhVentasContext.DimProducts.ExecuteDeleteAsync();

                // Obtener y cargar productos
                var products = await _northwindContext.Products.Select(prod => new DimProduct()
                {
                    ProductName = prod.ProductName,
                    ProductId = prod.ProductId
                }).AsNoTracking().ToListAsync();

                await _dwhVentasContext.DimProducts.AddRangeAsync(products);
                await _dwhVentasContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de producto. {ex.Message}";
            }
            return operation;
        }

        private async Task<OperactionResult> LoadShipper()
        {
            OperactionResult operation = new OperactionResult();
            try
            {
                // Eliminar datos existentes
                await _dwhVentasContext.DimShippers.ExecuteDeleteAsync();

                // Obtener y cargar transportistas
                var shippers = await _northwindContext.Shippers.Select(ship => new DimShipper()
                {
                    ShipperId = ship.ShipperId,
                    ShipperName = ship.CompanyName
                }).AsNoTracking().ToListAsync();

                await _dwhVentasContext.DimShippers.AddRangeAsync(shippers);
                await _dwhVentasContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                operation.Success = false;
                operation.Message = $"Error cargando dimensión de transportista. {ex.Message}";
            }
            return operation;
        }
        private async Task<OperactionResult> LoadFactOrders()
        {
            OperactionResult result = new OperactionResult();

            try
            {

                var ventas = await _northwindContext.Vwventas.AsNoTracking().ToListAsync();
                


                int[] ordersId = await _dwhVentasContext.FactOrders.Select(cd => cd.OrderNumber).ToArrayAsync();

                if (ordersId.Any())
                {
                    await _dwhVentasContext.FactOrders.Where(cd => ordersId.Contains(cd.OrderNumber))
                                                  .AsNoTracking()
                                                  .ExecuteDeleteAsync();
                }

                foreach (var venta in ventas)
                {
                    var customer = await _dwhVentasContext.DimCustomers.SingleOrDefaultAsync(cust => cust.CustomerId == venta.CustomerId);
                    var employee = await _dwhVentasContext.DimEmployees.SingleOrDefaultAsync(emp => emp.EmployeeId == venta.EmployeeId);
                    var shippers = await _dwhVentasContext.DimShippers.SingleOrDefaultAsync(ship => ship.ShipperId == venta.ShipperId);
                    var product = await _dwhVentasContext.DimProducts.SingleOrDefaultAsync(pro => pro.ProductId == venta.ProductId);


                    FactOrder factOrder = new FactOrder()
                    {
                        CantidadVentas = venta.Cantidad.HasValue ? Math.Floor((decimal)venta.Cantidad.Value) : 0, // Conversión explícita a decimal
                        Country = venta.Country ?? string.Empty, // Evitar nulos para Country
                        CustomerKey = customer?.CustomerKey ?? 0, // Validar que el cliente exista
                        EmployeeKey = employee?.EmployeeKey ?? 0, // Validar que el empleado exista
                        ProductKey = product?.ProductKey ?? 0,   // Validar que el producto exista
                        ShipperKey = shippers?.ShipperKey ?? 0,   // Validar que el transportista exista
                        TotalVentas = venta.TotalVentas.HasValue ? (decimal)venta.TotalVentas.Value : 0m // Manejar nulos
                    };


                    await _dwhVentasContext.FactOrders.AddAsync(factOrder);

                    await _dwhVentasContext.SaveChangesAsync();
                }



                result.Success = true;
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de ventas {ex.Message} ";
            }

            return result;
        }
        private async Task<OperactionResult> loadClienteAtentidos()
        {
            OperactionResult result = new OperactionResult() { Success = true };

            try
            {
                var customerServeds = await _northwindContext.VwServedCustomers.AsNoTracking().ToListAsync();

                int[] customerIds = _dwhVentasContext.FactClientesAtendidos.Select(cli => cli.ClienteAtendidoId).ToArray();

                //Limpiamos la tabla de facts //

                if (customerIds.Any())
                {
                    await _dwhVentasContext.FactClientesAtendidos.Where(fact => customerIds.Contains(fact.ClienteAtendidoId))
                                                            .AsNoTracking()
                                                            .ExecuteDeleteAsync();
                }

                //Carga el fact de clientes atendidos. //
                foreach (var customer in customerServeds)
                {
                    var employee = await _dwhVentasContext.DimEmployees
                                                      .SingleOrDefaultAsync(emp => emp.EmployeeId ==
                                                                               customer.EmployeeId);


                    FactClientesAtendido factClienteAtendido = new FactClientesAtendido()
                    {
                        EmployeeKey = employee.EmployeeKey,
                        TotalClientes = customer.TotalCustomersServed
                    };


                    await _dwhVentasContext.FactClientesAtendidos.AddAsync(factClienteAtendido);

                    await _dwhVentasContext.SaveChangesAsync();
                }

                result.Success = true;

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de clientes atendidos {ex.Message} ";
            }
            return result;

        }
    }
}
