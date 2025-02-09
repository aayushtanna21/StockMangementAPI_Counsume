using System.ComponentModel.DataAnnotations;
namespace StockMangementAPI_Counsume.Models
{
    public class SuppliersModel
    {
        public int SupplierID { get; set; } // Unique identifier for suppliers
		[Required(ErrorMessage = "Supplier Name is Mandatory!!")]
		[MaxLength(20)]
		public string SupplierName { get; set; } // Name of the supplier
		[Required(ErrorMessage = "Phone Number is required")]
		[MinLength(10)]
		public string PhoneNumber { get; set; } // Contact number
		[Required(ErrorMessage = "Email is requried")]
		[EmailAddress]
		public string Email { get; set; } // Email address (NotNull)
		[Required(ErrorMessage ="Address is required")]
		[MaxLength(80)]
        public string Address { get; set; } // Address of the supplier
        public int UserID { get; set; } // User who performed the transaction
		public string? UserName { get; set; }
		public DateTime? Created { get; set; } // Creation timestamp
        public DateTime? Modified { get; set; } // Last modification timestamp (nullable)
    }
    public class SuppliersDropDownModel
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
    }
}
