namespace StockMangementAPI_Counsume.Models
{
    public class UserSalesReturnModel
    {
        public int SalesReturnID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime? SaleReturnDate { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
    }
}
