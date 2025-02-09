using System.ComponentModel.DataAnnotations;

namespace StockMangementAPI_Counsume.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; } // Unique identifier for products
		[Required(ErrorMessage = "Product Name is Mandatory!!")]
		[MaxLength(20)]
		public string ProductName { get; set; } // Name of the product
		public string? ProductImage { get; set; }
		public int CategoryID { get; set; } // Link to Categories table
        public string? CategoryName { get; set; }
		[Required(ErrorMessage = "Stock Quantity is required")]
		public int StockQuantity { get; set; } // Current stock level
		[Required(ErrorMessage = "Price is required")]
		public decimal Price { get; set; } // Selling price of the product
		[Required(ErrorMessage = "Cost Price is required")]
		public decimal CostPrice { get; set; } // Purchase price
		[Required(ErrorMessage = "Reorder Level is required")]
		public int ReorderLevel { get; set; } // Minimum stock level for reorder
		[Required(ErrorMessage = "Unit is required")]
		public string Unit { get; set; } // Unit of measurement (e.g., kg)
        public int CustomerID { get; set; } // User who performed the transaction
        public string? CustomerName { get; set; }
        public DateTime? Created { get; set; } // Creation timestamp
        public DateTime? Modified { get; set; }
		public IFormFile? ImageFile { get; set; }
	}
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
