using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore_Identity.Model
{
    public class SignupVM
    {
        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please Enter User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Please Enter vaild password")]
        public string Password { get; set; }
    }
}
