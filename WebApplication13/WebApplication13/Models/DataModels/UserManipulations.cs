using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using WebApplication13.Models.ViewModels;

namespace WebApplication13.Models.DataModels
{
    public class UserManipulations
    {
        // Method executes 'Login' procedure:
        public static bool ifUserExists(string email, string password)
        {
            bool result = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "isUserExists";
            cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 254).Value = email;
            cmd.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 254).Value = email;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                    result = true;
            }
            catch
            {
                
            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return result;

        }

        // Method executes 'Edit User' procedure:
        public static bool editUser(User user)
        {
            bool result = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "editUser";
            cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 254).Value = user.email;
            cmd.Parameters.Add("@Userpicture", System.Data.SqlDbType.Binary, 10).Value = user.imageFileName;
            cmd.Parameters.Add("@Actualname", System.Data.SqlDbType.NVarChar, 60).Value = user.realname;
            cmd.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 60).Value = user.password;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                    result = true;
            }
            catch
            {

            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return result;

        }

        // Method executes 'Add User' procedure:
        public static bool addUser(User user)
        {
            bool result = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "addUser";
            cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 254).Value = user.email;
            cmd.Parameters.Add("@Userpicture", System.Data.SqlDbType.Binary, 10).Value = user.imageFileName;
            cmd.Parameters.Add("@Actualname", System.Data.SqlDbType.NVarChar, 60).Value = user.realname;
            cmd.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 60).Value = user.password;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                    result = true;
            }
            catch
            {

            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return result;

        }
    }
}