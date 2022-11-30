using System.ComponentModel.DataAnnotations;

namespace AspCore_Identity.Model
{
    public class SignInVM
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Please Enter vaild password")]
        public string Password { get; set; }
        public bool RememberMe  { get; set; }
    }
}
