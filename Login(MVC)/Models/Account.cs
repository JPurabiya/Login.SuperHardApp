using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Login_MVC_.Models
{
    public class Account
    {
        [Required(ErrorMessage = "Username is required.")]
        [RegularExpression(".+\\@.+\\..+",ErrorMessage ="Please Enter valid Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }   

    }
}