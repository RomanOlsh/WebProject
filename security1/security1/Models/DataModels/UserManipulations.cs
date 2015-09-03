using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using mySite.Models.ViewModels;

namespace mySite.Models.DataModels
{
    public class UserManipulations
    {
        //Get User number:
        public static int getUserNumber(string email, string password)
        {
            int result = 0;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "ifUserExists";
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = email;
            cmd.Parameters.Add("@userPassowrd", System.Data.SqlDbType.NVarChar, 254).Value = password;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = (int)rdr["Usernum"];
                    
                }
                else
                {
                    result = 0; // incorect username or password or the combination!
                }
                rdr.Close();
            }
            catch //(Exception prblm)
            {
                result = -1;
            }
            finally
            {
                cnctn.Close();
            }
            return result;

        }


        // Method executes 'Login' procedure:
        public static int ifUserExists(string email, string password, out ConnectedUser connectedUser)
        {
            int result = 0;
            ConnectedUser tempUser = null;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "ifUserExists";
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = email;
            cmd.Parameters.Add("@userPassowrd", System.Data.SqlDbType.NVarChar, 254).Value = password;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    tempUser = new ConnectedUser((int)rdr["Usernum"], email);
                    result = 1;
                }
                else
                {
                    tempUser = null;
                    result = 0; // incorect username or password or the combination!
                }
                rdr.Close();
            }
            catch //(Exception prblm)
            {
                tempUser = null;
                result = -1;
            }
            finally
            {
                cnctn.Close();
            }
            connectedUser = tempUser;
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
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = user.email;
            cmd.Parameters.Add("@userUserpicture", System.Data.SqlDbType.Binary, 10).Value = user.imageFileName;
            cmd.Parameters.Add("@userActualname", System.Data.SqlDbType.NVarChar, 60).Value = user.realname;
            cmd.Parameters.Add("@userPassword", System.Data.SqlDbType.NVarChar, 60).Value = user.password;
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
        public static int addUser(User user)
        {
            int finalResult = 0;
            bool result = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "addUser";

            if (null != user.imageFileName)
            {
                cmd.Parameters.Add("@userUserpicture", System.Data.SqlDbType.VarChar, -1).Value = user.imageFileName;
            }
            else
            {
                cmd.Parameters.Add("@userUserpicture", System.Data.SqlDbType.VarChar, -1).Value = null;

            }


            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = user.email;
            cmd.Parameters.Add("@userActualname", System.Data.SqlDbType.NVarChar, 60).Value = user.realname;
            cmd.Parameters.Add("@userPassword", System.Data.SqlDbType.NVarChar, 60).Value = user.password;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                {
                    result = true;
                    finalResult = getUserNumber(user.email, user.password);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return finalResult;
        }
    }
}