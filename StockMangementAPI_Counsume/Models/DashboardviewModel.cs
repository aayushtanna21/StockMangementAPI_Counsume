namespace StockMangementAPI_Counsume.Models
{
    public class DashboardviewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<TopProduct> TopProducts { get; set; } = new List<TopProduct>();
        public List<TopCustomer> TopCustomers { get; set; } = new List<TopCustomer>();
    }
    public class TopProduct
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
    }

    public class TopCustomer
    {
        public string CustomerName { get; set; }
        public int TotalPurchases { get; set; }
    }
}
