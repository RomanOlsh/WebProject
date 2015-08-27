using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using WebApplication13.Models.ViewModels;

namespace WebApplication13.Models.DataModels
{
    public class Citizens
    {
        // Method executes SELECT procedure:
      /*  public static Citizen getCtznbytz(string tz)
        {
            Citizen result = null;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "getCitizenByTz";
            cmd.Parameters.Add("@tz", System.Data.SqlDbType.NVarChar, 10).Value = tz;
            SqlDataReader rdr = null;
            int i = 0;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    result = new Citizen { tz = rdr["ctznTz"].ToString(), lname = rdr["ctznLname"].ToString(), fname = rdr["ctznFname"].ToString(), dob = rdr["ctznDob"].ToString(), height = (float)rdr["ctznHgt"], email = rdr["ctznEml"].ToString(), phone = rdr["ctznPhone"].ToString(), pwd = rdr["ctznPwd"].ToString(), numchild = (byte)rdr["ctznNumchilds"] };
                    i++;
                }
                if (i != 1) //if we didn't found any or more then 1
                    result = null;
            }
            catch// (Exception prblm)
            {
                result = null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (cnctn != null)
                    cnctn.Close();
            }
            return result;

        }

        // Method executes UPDATE procedure:
        public static bool updCtznbytz(Citizen citezen)
        {
            if (!citezen.isValid())
                return false;
            bool rslt = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "updateCitizen";
            cmd.Parameters.Add("@tz", System.Data.SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@lname", System.Data.SqlDbType.NVarChar, 16);
            cmd.Parameters.Add("@fname", System.Data.SqlDbType.NVarChar, 14);
            cmd.Parameters.Add("@dob", System.Data.SqlDbType.DateTime);
            cmd.Parameters.Add("@height", System.Data.SqlDbType.Real);
            cmd.Parameters.Add("@eml", System.Data.SqlDbType.NVarChar, 254);
            cmd.Parameters.Add("@phone", System.Data.SqlDbType.NVarChar, 11);

            cmd.Parameters.Add("@numchilds", System.Data.SqlDbType.TinyInt);
            cmd.Parameters.Add("@livestat", System.Data.SqlDbType.Bit);
            cmd.Parameters.Add("@pwd", System.Data.SqlDbType.NVarChar, 12);
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                cmd.Parameters["@tz"].Value = citezen.tz;
                cmd.Parameters["@lname"].Value = citezen.lname;
                cmd.Parameters["@fname"].Value = citezen.fname;
                cmd.Parameters["@dob"].Value = citezen.dob;
                cmd.Parameters["@height"].Value = citezen.height;
                cmd.Parameters["@eml"].Value = citezen.email;
                cmd.Parameters["@phone"].Value = citezen.phone;
                cmd.Parameters["@numchilds"].Value = citezen.numchild;
                cmd.Parameters["@livestat"].Value = citezen.islive;
                cmd.Parameters["@pwd"].Value = citezen.pwd;
                int rc = cmd.ExecuteNonQuery();
                if (rc > 0)
                    rslt = true;
            }
            catch// (Exception prblm)
            {
                rslt = false;
            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return rslt;
        }

        public static bool dltbytz(string tz)
        {
            bool rslt=false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
           
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "dltCtznbytz";
            cmd.Parameters.Add("@tz2dlt", System.Data.SqlDbType.NVarChar, 10);
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                cmd.Parameters["@tz2dlt"].Value = tz;
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                    rslt = true;
            }
            catch// (Exception prblm)
            {
                rslt = false;
            }
            finally
            {
                if (cnctn!=null)
                    cnctn.Close();
            }
            return rslt;
        }
       
        public static List<Citizen> getallofthem()
        {
            List<Citizen> rslt=new List<Citizen>();
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
           
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "getallCitizens";
            SqlDataReader rdr = null;
            int i = 0;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                rdr=cmd.ExecuteReader();
                while (rdr.Read())
                {
                    rslt.Add(new Citizen { tz = rdr["ctznTz"].ToString(), fname = rdr["ctznFname"].ToString(), lname = rdr["ctznLname"].ToString(), email = rdr["ctznEml"].ToString(), phone = rdr["ctznPhone"].ToString() });
                    i++;
                }
                if (i==0)
                    rslt=null;
            }
            catch// (Exception prblm)
            {
                rslt = null;
            }
            finally
            {
                if (rdr!=null)
                    rdr.Close();
                if (cnctn!=null)
                    cnctn.Close();
            }
            return rslt;
        }
        
        public static bool addCitizen(Citizen nwone)
        {
            if (!nwone.isValid())
                return false;
            bool rslt=false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "addCitizen";
            cmd.Parameters.Add("@tz", System.Data.SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@lname", System.Data.SqlDbType.NVarChar, 16);
            cmd.Parameters.Add("@fname", System.Data.SqlDbType.NVarChar, 14);
            cmd.Parameters.Add("@dob", System.Data.SqlDbType.DateTime);
            cmd.Parameters.Add("@height", System.Data.SqlDbType.Real);
            cmd.Parameters.Add("@eml", System.Data.SqlDbType.NVarChar, 254);
            cmd.Parameters.Add("@phone", System.Data.SqlDbType.NVarChar, 11);
          
            cmd.Parameters.Add("@numchilds", System.Data.SqlDbType.TinyInt);
            cmd.Parameters.Add("@livestat", System.Data.SqlDbType.Bit);
            cmd.Parameters.Add("@pwd", System.Data.SqlDbType.NVarChar, 12);
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                cmd.Parameters["@tz"].Value=nwone.tz;
                cmd.Parameters["@lname"].Value = nwone.lname;
                cmd.Parameters["@fname"].Value = nwone.fname;
                cmd.Parameters["@dob"].Value = nwone.dob;
                cmd.Parameters["@height"].Value = nwone.height;
                cmd.Parameters["@eml"].Value = nwone.email;
                cmd.Parameters["@phone"].Value = nwone.phone;
                cmd.Parameters["@numchilds"].Value = nwone.numchild;
                cmd.Parameters["@livestat"].Value = nwone.islive;
                cmd.Parameters["@pwd"].Value = nwone.pwd;
                int rc = cmd.ExecuteNonQuery();
                if (rc == 1)
                    rslt = true;
            }
            catch// (Exception prblm)
            {
                rslt = false;
            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return rslt;
        }

        public static bool getCitizenByTz(string tz)
        {
            bool rslt = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();
           
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "getCitizenByTz";
            cmd.Parameters.Add("@tz", System.Data.SqlDbType.NVarChar, 10);
            
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                cmd.Parameters["@tz"].Value = tz;
                
                int rc = (int)cmd.ExecuteScalar();
                if (rc == 1)
                    rslt = true;
            }
            catch //(Exception prblm)
            {
                rslt = false;
            }
            finally
            {
                if (cnctn!=null)
                    cnctn.Close();
            }
            return rslt;
        }*/
    }
}