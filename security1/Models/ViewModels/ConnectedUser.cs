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
        public int userNumber;

        public string userEmail;

        public string realName;

        public string sessionStartedTime { get; set; }

        public byte[] userPicture { get; set; }
       
        public ConnectedUser(int userNumber, string userEmail, string realName)
        {
            this.userNumber = userNumber;
            this.userEmail = userEmail;
            this.realName = realName;
        }
        
        public override string ToString()
        {
            return this.realName;
        }
       
        public int getUserNum
        {
            get
            {
                return userNumber;
            }
        }

        public string getUserEmail
        {
            get
            {
                return userEmail;
            }
        }
       
    }
}