using System.ComponentModel.DataAnnotations;

namespace StockMangementAPI_Counsume.Models
{
	public class PaymentsModel
	{
        public int PaymentID { get; set; } // Unique payment ID
        public int BillID { get; set; } // Link to Bills table
        public DateTime? BillDate { get; set; }
		[Required(ErrorMessage = "Payment Mode is required")]
		public string PaymentMode { get; set; } // Mode of payment (e.g., Cash, Card)
		[Required(ErrorMessage = "Amount Paid is required")]
		public decimal AmountPaid { get; set; } // Amount paid
        public DateTime? PaymentDate { get; set; } // Date of payment
        public DateTime? Modified { get; set; } // Last modification timestamp (nullable)
    }
}
