using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication13.Models.ViewModels
{
    public class Article
    {
        
        public int articleId { get; set; }

        public int userId { get; set; }

        [DisplayName("Article Name")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = " {0} תקיו רק בין {2} ל {1} תוים")]
        public string articleName{ get; set; }

        [DisplayName("Article Content")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = " {0} תקיו רק בין {2} ל {1} תוים")]
        public string articleContent { get; set; }

        [DisplayName("Date")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        public DateTime date { get; set; }
        
        [DisplayName("Is Public")]
        [Required(ErrorMessage = "{0} הוא שדה חובה")]
        public bool isPublic{ get; set; }
    }
}