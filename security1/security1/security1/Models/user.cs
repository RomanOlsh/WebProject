using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace mySite.Models
{

    public class connecteduser
    {
        int userNum;
        string userRealName;
        string userType;
        public connecteduser(int n, string rn, string t)
        {
            userNum = n;
            userRealName = rn;
            userType = t;
        }
        public override string ToString()
        {
            return userRealName;
        }
        public int Num
        {
            get
            {
                return userNum;
            }
        }
        public bool  IsAdmin
        {
            get
            {
                return userType.Equals("Admin");
            }
        }
    }
    public class user
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
      
        const string cnctnstring = @"Server=.\SQLEXPRESS;Database=mySiteDb;Trusted_Connection=True;";
        
        public static int  checkcredintial(string usr,string pwd)
        {
            //connecteduser x = null;
            int rc = 0;
            /*SqlConnection cnctn = null;
            try
            {
                cnctn = new SqlConnection(cnctnstring);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnctn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "UserCredentianlCheck";
                cmd.Parameters.Add("@userLogin", System.Data.SqlDbType.NVarChar, 50);
                cmd.Parameters.Add("@userPassword", System.Data.SqlDbType.NVarChar, 10);
                cnctn.Open();
                cmd.Parameters["@userLogin"].Value = usr;
                cmd.Parameters["@userPassword"].Value = pwd;
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    x = new connecteduser(int.Parse(rdr[0].ToString()), rdr[1].ToString(), rdr[2].ToString());
                    rc = 1;
                }
                else
                {
                    x = null;
                    rc = 0; // incorect username or password or the combination!
                }
                rdr.Close();
            }
            catch //(Exception prblm)
            {
                x = null;
                rc = -1;
            }
            finally
            {
                cnctn.Close();
            }
            t = x;
            */
            rc = 1;
            return rc;
        }
        public static int addUser(string usr, string pwd, string rnm)
        {
            int rc = 0;
            SqlConnection cnctn = null;
            try
            {
                cnctn = new SqlConnection(cnctnstring);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnctn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "AddUser";
                cmd.Parameters.Add("@userLogin", System.Data.SqlDbType.NVarChar, 50);
                cmd.Parameters.Add("@userPassword", System.Data.SqlDbType.NVarChar, 10);
                cmd.Parameters.Add("@userRealName", System.Data.SqlDbType.NVarChar, 40);
                cnctn.Open();
                cmd.Parameters["@userLogin"].Value = usr;
                cmd.Parameters["@userPassword"].Value = pwd;
                cmd.Parameters["@userRealName"].Value = rnm;
                rc = cmd.ExecuteNonQuery();
              
            }
            catch// (Exception prblm)
            {
                rc = -1;

            }
            finally
            {
                cnctn.Close();
            }
        
            return rc;
        }
    }
}