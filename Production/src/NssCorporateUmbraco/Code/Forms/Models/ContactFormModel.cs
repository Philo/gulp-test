using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NssCorporateUmbraco.Code.Base.Constants;

namespace NssCorporateUmbraco.Code.Forms.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Your email address is not in a valid format")]
        public string Email { get; set; }

        [Display(Name = "Postal address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        public string Message { get; set; }

    }
}