using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Storefront.UI.MVC.Models
{
    public class ContactViewModel
    {
        //All we are going to provide the class is the properties class member.
        [Required(ErrorMessage = "* Please provide a Name *")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Please provide a Email *")]
        [DataType(DataType.EmailAddress)]
        //If you want to ensure your validation or customize the validation you can still use a regular expression (regexlib.com) Ex:
        //   [RegularExpression("regex", ErrorMessage = "* Provide a valid email *")]
        public string Email { get; set; }


        public string Subject { get; set; }

        [Required(ErrorMessage = "* Please provide a Message *")]
        [UIHint("MultilineText")]//changes the input to a text area
        public string Message { get; set; }

    }//end class
}//end NS