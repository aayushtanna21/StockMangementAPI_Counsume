using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StockMangementAPI_Counsume.Models;
using System.Data;

namespace StockMangementAPI_Counsume.Controllers.User
{
	public class UserHomeController : Controller
	{
        private readonly string _connectionstring;
        public UserHomeController(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("StockMangmentDB");
        }

        //Index Action for Dashboard
        public IActionResult Index()
        {
            DashboardviewModel model = new DashboardviewModel();

            using (SqlConnection conn = new SqlConnection(_connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetDashboardMetrics", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader reader = cmd.ExecuteReader();

                // Read Summary Data
                if (reader.Read())
                {
                    model.TotalProducts = reader.GetInt32(0);
                    model.TotalCustomers = reader.GetInt32(1);
                    model.TotalSales = reader.GetInt32(2);
                    model.TotalRevenue = reader.GetDecimal(3);
                }

                // Read Top Products
                reader.NextResult();
                while (reader.Read())
                {
                    model.TopProducts.Add(new TopProduct
                    {
                        ProductName = reader.GetString(0),
                        TotalSold = reader.GetInt32(1)
                    });
                }

                // Read Top Customers
                reader.NextResult();
                while (reader.Read())
                {
                    model.TopCustomers.Add(new TopCustomer
                    {
                        CustomerName = reader.GetString(0),
                        TotalPurchases = reader.GetInt32(1)
                    });
                }
            }

            return View(model);
        }
    }
}
