using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using mySite.Models.ViewModels;
using System.IO;

namespace mySite.Models.DataModels
{
    public class UserManipulations
    {
        //Get User number:
        public static int getUserNumber(string email)
        {
            int result = 0;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "ifUserExists";
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = email;
            try
            {
                cnctn = new SqlConnection(GlobalFunctions.getConnectionString());
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
            try
            {
                cnctn = new SqlConnection(GlobalFunctions.getConnectionString());
                cmd.Connection = cnctn;
                cnctn.Open();
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    //Check if passed password is right:
                    if (GlobalFunctions.checkPassword((byte[])rdr["Passowrd"], password)) 
                    {
                        tempUser = new ConnectedUser((int)rdr["Usernum"], email, rdr["Actualname"].ToString());

                        try
                        {
                            tempUser.userPicture = (byte[])rdr["Userpicture"];
                        }
                        catch { }
                        result = 1;
                    }
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
        public static int editUser(User user, out ConnectedUser connectedUser)
        {
            byte[] data = null;
            int finalResult = 0;
            ConnectedUser tempUser = null;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "editUser";

            if (null != user.imageFileName)
            {
                MemoryStream target = new MemoryStream();
                user.imageFileName.InputStream.CopyTo(target);
                data = target.ToArray();
                cmd.Parameters.Add("@userUserpicture", System.Data.SqlDbType.VarBinary, -1).Value = data;
            }

            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = user.email;
            cmd.Parameters.Add("@userActualname", System.Data.SqlDbType.NVarChar, 60).Value = user.realname;
            cmd.Parameters.Add("@userPassword", System.Data.SqlDbType.VarBinary, 60).Value = GlobalFunctions.getEncriptedPassword(user.password);
            try
            {
                cnctn = new SqlConnection(GlobalFunctions.getConnectionString());
                cmd.Connection = cnctn;
                cnctn.Open();
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                {
                    finalResult = getUserNumber(user.email);
                    tempUser = new ConnectedUser(finalResult, user.email, user.realname);
                    if (null != data)
                    {
                        tempUser.userPicture = data;
                    }
                }
                else
                { tempUser = null; }
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
            connectedUser = tempUser;
            return finalResult;

        }

        // Method executes 'Add User' procedure:
        public static int addUser(User user, out ConnectedUser connectedUser)
        {
            byte[] data = null;
            int finalResult = 0;
            ConnectedUser tempUser = null;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "addUser";

            if (null != user.imageFileName)
            {
                MemoryStream target = new MemoryStream();
                user.imageFileName.InputStream.CopyTo(target);
                data = target.ToArray();
                cmd.Parameters.Add("@userUserpicture", System.Data.SqlDbType.VarBinary, -1).Value = data;
            }

            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.NVarChar, 254).Value = user.email;
            cmd.Parameters.Add("@userActualname", System.Data.SqlDbType.NVarChar, 60).Value = user.realname;
            cmd.Parameters.Add("@userPassword", System.Data.SqlDbType.VarBinary, 60).Value = GlobalFunctions.getEncriptedPassword(user.password);
            try
            {
                cnctn = new SqlConnection(GlobalFunctions.getConnectionString());
                cmd.Connection = cnctn;
                cnctn.Open();
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                {
                    finalResult = getUserNumber(user.email);
                    tempUser = new ConnectedUser(finalResult, user.email, user.realname);
                    if (null != data)
                    {
                        tempUser.userPicture = data;
                    }
                }
                else
                { tempUser = null; }
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
            connectedUser = tempUser;
            return finalResult;
        }
    }
}