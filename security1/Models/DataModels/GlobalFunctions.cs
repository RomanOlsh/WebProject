using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mySite.Models.DataModels
{
    public class GlobalFunctions
    {

        /**
         * Method returns connection string. 
         **/
        public static string getConnectionString()
        {
            return @"Server=.\SQLEXPRESS;DataBase=projectdb;Trusted_Connection=True";
        }

        /**
       * Method generates salt byte array.
       **/
        private static byte[] generateSalt()
        {
            /* Static salt not depended in time/length */
            string saltEncodedString = "DariaErmakova";
            byte[] salt = Encoding.Unicode.GetBytes(saltEncodedString);
            return salt;
        }

        /**
         * Method returns encripted password.
         **/
        public static byte[] getEncriptedPassword(string givenPassword)
        {
            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(givenPassword, generateSalt(), 1001); // you should save per user the salt base that is the 
            byte[] saltResult = bytes.GetBytes(givenPassword.Length * 2 + Encoding.Unicode.GetString(generateSalt()).Length); //8 is minimum password length(*2 as utf)
            return saltResult;
        }

        /**
         * Method copares password from DB with the given one password.
         **/
        public static bool checkPassword(byte[] existingPassword, string givenPassword)
        {

            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(givenPassword, generateSalt(), 1001);
            byte[] tempResult = bytes.GetBytes(givenPassword.Length * 2 + Encoding.Unicode.GetString(generateSalt()).Length);
            bool result = false;
            if (tempResult.Length == existingPassword.Length)
            {
                int rslt = 0;
                for (int i = 0; i < tempResult.Length; i++) // this to prevent time deduction attack
                    rslt |= tempResult[i] ^ existingPassword[i]; // bit  and , or
                if (rslt == 0)
                    result = true;
            }
            Console.WriteLine(result);
            return result;
        }

    }
}