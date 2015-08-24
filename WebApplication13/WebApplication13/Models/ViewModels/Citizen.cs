using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;// for remote

namespace WebApplication13.Models.ViewModels
{
    public class Citizen
    {
        [DisplayName("ת.ז.")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [RegularExpression(@"\d{5,8}-\d", ErrorMessage = "{0} חייב לכלול לפחות5 ספרות מקף וספרת ביקורת")]
        public string tz { get; set; }

        [DisplayName("שם משפחה")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = " {0} תקיו רק בין {2} ל {1} תוים")]
        public string lname {get; set;}

        [DisplayName("שם פרטי")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(14, MinimumLength = 2, ErrorMessage = "{0} תקין רק בין {2} ל {1} תוים")]
        public string fname { get; set; }

        [DisplayName("תאריך לידה")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [RegularExpression(@"((0|1|2)\d|30|31)/(0\d|1(0|1|2))/(18|19|20)\d\d", ErrorMessage = "מבנה {0} שגוי")]
        [Remote("ChkDob","Home")]
        public string dob { get; set; }

        [DisplayName(" גובה במטרים ")]
        [Range(0.35, 2.6, ErrorMessage = "{0} תקין הוא רק בתחום {1} מטרים עד {2} מטרים")]
        public float? height{get; set;}

        [DisplayName(" מיקוד ")]
        [Range(1000000, ulong.MaxValue, ErrorMessage = " {0} בישראל הוא לפחות בן 7 ספרות ")]
        public ulong? ilzipcode { get; set; }

        [DisplayName("דואל")]
        [StringLength(254, ErrorMessage = "{0} ארוך מידי")]
        [EmailAddress(ErrorMessage = "לא הזנת {0} במבנה תקין")]
        public string email { get; set; }

        [DisplayName(" סלולרי ")]
        [RegularExpression(@"(050|052|054|058|053)-\d{6,7}", ErrorMessage = "{0}  חייב לכלול גם קידומת  ומקף ")]
        public string phone { get; set; }

        [DisplayName("מספר ילדים")]
        [Range(1, 30, ErrorMessage = "{0} תקין הוא רק בין {1} ל{2}")]
        [DataType(DataType.Password)]
        public byte? numchild { get; set; }

        [DisplayName("חי")]
        public bool? islive { get; set; }

        [DisplayName("סיסמא")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "{0} תקינה רק בין {2} ל {1} תוים")]
        [DataType(DataType.Password)]
        public string pwd { get; set; }

        [DisplayName("אימות סיסמא")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("pwd", ErrorMessage = "חייב להיות זהה לסיסמא")]
        public string repwd { get; set; }

        //https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.datatype%28v=vs.100%29.aspx רשימת סוגי DATATYPE
        public bool isValid(out List<dynamic> problemslist)
        {
            bool rc;
            problemslist = null;
            try
            {
                var validationContext = new ValidationContext(this);
                List<ValidationResult> validationResults = new List<ValidationResult>();

                var isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);

                // If there any exception return them in the return result
                if (!isValid)
                {
                    problemslist = new List<dynamic>();
                    foreach (ValidationResult err in validationResults)
                        foreach (string nm in err.MemberNames)// as same error could repeate in more then one member
                            problemslist.Add(new  {mmbr=nm, errmsg=err.ErrorMessage });// we linear the error list
                    rc=false;
                }
                else
                    rc=true;
            }
           
            catch   (Exception prblm)
            {
                problemslist = new List<dynamic>();
                problemslist.Add(new { mmbr = "", errmsg = prblm.Message });
                rc=false;
            }
            return rc;
        }
        public bool isValid()
        {
            List<dynamic> t;
            return isValid(out t);
        }

    }
}