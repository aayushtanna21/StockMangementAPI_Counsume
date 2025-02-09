using System.ComponentModel.DataAnnotations;

namespace StockMangementAPI_Counsume.Models
{
    public class BillDetailsModel
    {
        public int BillDetailID { get; set; } // Unique identifier for bill detail
        public int BillID { get; set; } // Link to Bills table
        public DateTime? BillDate { get; set; }
        public int ProductID { get; set; } // Link to Products table
        public string? ProductName { get; set; }
		[Required(ErrorMessage = "Quantity is required")]
		public int Quantity { get; set; } // Quantity sold
		[Required(ErrorMessage = "Unit Price is required")]
		public decimal UnitPrice { get; set; } // Price per unit (NotNull)
		[Required(ErrorMessage = "Sub Total is required")]
		public decimal SubTotal { get; set; } // Quantity × UnitPrice
        public DateTime? Created { get; set; } // Creation timestamp
        public DateTime? Modified { get; set; } // Last modification timestamp (nullable)
    }
}
