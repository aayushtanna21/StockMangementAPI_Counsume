namespace StockMangementAPI_Counsume.Models
{
    public class UserSalesModel
    {
        public int SalesID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal? AmountDue { get; set; }
        public DateTime? SaleDate { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
    }
}
