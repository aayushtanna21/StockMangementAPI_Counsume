using System.ComponentModel.DataAnnotations;
namespace StockMangementAPI_Counsume.Models
{
    public class StockTransactionsModel
    {
        public int StockTransactionID { get; set; } // Unique transaction ID
        public int ProductID { get; set; } // Link to Products table
        public string? ProductName { get; set; }
        [Required(ErrorMessage ="Stock Transaction Type is required")]
        
        public string StockTransactionType { get; set; } // Type (e.g., purchase, sale, return)
        [Required(ErrorMessage ="Quantity is required")]
        public int Quantity { get; set; } // Number of items added/removed
        public DateTime? StockTransactionDate { get; set; } // Date of the transaction
        public int UserID { get; set; } // User who performed the transaction
        public string? UserName { get; set; }
        public DateTime? Modified { get; set; } // Last modification timestamp (nullable)
    }
}
