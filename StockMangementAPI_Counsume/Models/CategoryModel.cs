using System.ComponentModel.DataAnnotations;

namespace StockMangementAPI_Counsume.Models
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
		[Required(ErrorMessage = "Category Name is Mandatory!!")]
		[MaxLength(20)]
		public string CategoryName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
    public class CategoryDropDownModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
