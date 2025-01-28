namespace StockMangementAPI_Counsume.Models
{
    public class StockTransactionsModel
    {
        public int StockTransactionID { get; set; } // Unique transaction ID
        public int ProductID { get; set; } // Link to Products table
        public string StockTransactionType { get; set; } // Type (e.g., purchase, sale, return)
        public int Quantity { get; set; } // Number of items added/removed
        public DateTime StockTransactionDate { get; set; } // Date of the transaction
        public int UserID { get; set; } // User who performed the transaction
        public DateTime? Modified { get; set; } // Last modification timestamp (nullable)
    }
}
