using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication13.Models.ViewModels
{
    public class User
    {
        [DisplayName("Real Name")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = " {0} תקיו רק בין {2} ל {1} תוים")]
        public string realname { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "{0} תקינה רק בין {2} ל {1} תוים")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [DisplayName("E-Mail")]
        [StringLength(254, ErrorMessage = "{0} ארוך מידי")]
        [EmailAddress(ErrorMessage = "לא הזנת {0} במבנה תקין")]
        public string email { get; set; }

        [DisplayName("Captcha Validation")]
        [Required(ErrorMessage = "מילת בתמונה היא שדה חובה")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "{0} חייב להיות באורך  {2} תוים")]
        public string Capthca { get; set; }

        public string imageFileName{ get; set; }

        public int userId { get; set; }
    }
}