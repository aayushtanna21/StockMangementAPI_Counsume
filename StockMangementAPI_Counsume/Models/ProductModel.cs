namespace StockMangementAPI_Counsume.Models
{
    public class ProductModel
    {
        public int ProductID { get; set; } // Unique identifier for products
        public string ProductName { get; set; } // Name of the product
        public int CategoryID { get; set; } // Link to Categories table
        public int StockQuantity { get; set; } // Current stock level
        public decimal Price { get; set; } // Selling price of the product
        public decimal CostPrice { get; set; } // Purchase price
        public int ReorderLevel { get; set; } // Minimum stock level for reorder
        public string Unit { get; set; } // Unit of measurement (e.g., kg)
        public int UserID { get; set; } // User who performed the transaction
        public DateTime? Created { get; set; } // Creation timestamp
        public DateTime? Modified { get; set; }
    }
}
