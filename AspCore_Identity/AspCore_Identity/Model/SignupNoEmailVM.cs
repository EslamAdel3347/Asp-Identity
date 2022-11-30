using System.ComponentModel.DataAnnotations;

namespace AspCore_Identity.Model
{
    public class SignupNoEmailVM
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Please Enter vaild password")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
