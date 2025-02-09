namespace StockMangementAPI_Counsume.Models
{
    public class UserPurchaseModel
    {
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public int SupplierID { get; set; }
        public string? SupplierName { get; set; }
        public int Quantity { get; set; }
        public int CostPrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}
