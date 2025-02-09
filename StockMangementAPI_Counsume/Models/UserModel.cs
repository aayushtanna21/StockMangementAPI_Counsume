using System.ComponentModel.DataAnnotations;
namespace StockMangementAPI_Counsume.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        [Required(ErrorMessage ="User Name is Mandatory!!")]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is Requried")]
        [Range(4,16,ErrorMessage ="Password Should be between 4 to 16")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Email is requried")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Phone Number is required")]
        [MinLength(10)]
        public string PhoneNumber { get; set; }
        public DateTime? Created { get; set;}
        public DateTime? Modified { get; set;}
    }
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
