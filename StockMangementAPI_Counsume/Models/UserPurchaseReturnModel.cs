namespace StockMangementAPI_Counsume.Models
{
    public class UserPurchaseReturnModel
    {
        public int PurchaseReturnID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public int SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public int Quantity { get; set; }
        public DateTime? PurchaseReturnDate { get; set; }
    }
}
