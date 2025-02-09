using System.ComponentModel.DataAnnotations;

namespace StockMangementAPI_Counsume.Models
{
    public class CustomersModel
    {
        public int CustomerID { get; set; } // Unique identifier for customers
		[Required(ErrorMessage = "Customer Name is Mandatory!!")]
		[MaxLength(20)]
		public string CustomerName { get; set; } // Name of the customer
		[Required(ErrorMessage = "Phone Number is required")]
		[MinLength(10)]
		public string PhoneNumber { get; set; } // Contact number
		[Required(ErrorMessage = "Email is requried")]
		[EmailAddress]
		public string Email { get; set; } // Email address (NotNull)
		[Required(ErrorMessage = "Address is required")]
		[MaxLength(80)]
		public string Address { get; set; } // Customer address
        public int UserID { get; set; } // User who performed the transaction
        public string? UserName { get; set; }
        public DateTime? Created { get; set; } // Creation timestamp
        public DateTime? Modified { get; set; } // Last modification timestamp (nullable)
    }
    public class CustomersDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
