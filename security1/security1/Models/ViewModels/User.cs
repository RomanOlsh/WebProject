using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.ComponentModel;

namespace mySite.Models
{
    public class ConnectedUser
    {
        int userNumber;
       
        string userEmail;
       
        public ConnectedUser(int userNumber, string userEmail)
        {
            this.userNumber = userNumber;
            this.userEmail = userEmail;
        }
        
        public override string ToString()
        {
            return userEmail;
        }
       
        public int getUserNum
        {
            get
            {
                return userNumber;
            }
        }
       
    }
  
    public class User
    {
       // [Required]
       // [Display(Name="Login id")]
        public string userLogin { get; set; }
      
        // [Required]
       // [StringLength(14,ErrorMessage="maximum password length is 14")]
       // [Display(Name = "Passowrd")]
        public string userPassword { get; set; }
       
       // [Display(Name = "Actual Name")]
        public string userRealName { get; set; }

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
        [StringLength(6, ErrorMessage = "{0} חייב להיות באורך  {2} תוים")]
        public string Capthca { get; set; }

        public string imageFileName { get; set; }

        public int userId { get; set; }

    }
}