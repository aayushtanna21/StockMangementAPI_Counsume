namespace StockMangementAPI_Counsume.Models
{
    public class CustomersModel
    {
        public int CustomerID { get; set; } // Unique identifier for customers
        public string CustomerName { get; set; } // Name of the customer
        public string PhoneNumber { get; set; } // Contact number
        public string Email { get; set; } // Email address (NotNull)
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
