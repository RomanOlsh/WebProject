using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using WebApplication13.Models.ViewModels;

namespace WebApplication13.Models.DataModels
{
    public class ArticlesManipulations
    {
        // Method executes 'Update Article' procedure:
        public static bool updateArticle(Article article)
        {
            bool result = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "editArticle";
            cmd.Parameters.Add("@Articletitle", System.Data.SqlDbType.NVarChar, 128).Value = article.articleName;
            cmd.Parameters.Add("@Cngdt", System.Data.SqlDbType.DateTime).Value = article.date;
            cmd.Parameters.Add("@Articlecntnt", System.Data.SqlDbType.NVarChar, -1).Value = article.articleContent;
            cmd.Parameters.Add("@Articlepublic", System.Data.SqlDbType.Bit, 1).Value = article.isPublic;
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

        //Method executes 'Delete Article' proceedure:
        public static bool deleteArticle(Article article)
        {
            bool result = false;
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "deleteArticle";
            cmd.Parameters.Add("@Articleid", System.Data.SqlDbType.BigInt).Value = article.articleId;
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

        //Method executes 'Get All Articles' procedure:
        private static List<Article> getAllArticles(string email)
        {
            List<Article> rslt = new List<Article>();
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "getAllArticles";
            cmd.Parameters.Add("@usersEmail", System.Data.SqlDbType.BigInt).Value = email;
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
                    rslt.Add(new Article
                    {
                        articleId = (int)rdr["Articleid"],
                        userId = (int)rdr["Ownerid"],
                        articleName = rdr["Articletitle"].ToString(),
                        articleContent = rdr["Articlecntnt"].ToString(),
                        date = (DateTime)rdr["Cngdt"],
                        isPublic = (bool)rdr["Articlepublic"]
                    });
                    i++;
                }
                if (i == 0)
                    rslt = null;
            }
            catch// (Exception prblm)
            {
                rslt = null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
                if (cnctn != null)
                    cnctn.Close();
            }
            return rslt;
        }

        //Method executes 'Get User ID' procedure:
        private static int getUserID(string email)
        {
            SqlConnection cnctn = null;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "getUserID";
            cmd.Parameters.Add("@userEmail", System.Data.SqlDbType.BigInt).Value = email;
            int result = 0;
            try
            {
                cnctn = new SqlConnection(Models.DataModels.DataGlobals.cnctnstrng);
                cmd.Connection = cnctn;
                cnctn.Open();
                result = (int)cmd.ExecuteScalar();
            }
            catch// (Exception prblm)
            {
            }
            finally
            {
                if (cnctn != null)
                    cnctn.Close();
            }
            return result;
        }

        //Method filters all got articles to 2 sub-lists 'My' and 'All Public':
        public static Dictionary<String, List<Article>> getSortedSetOfArticles(string email)
        {
            int userID = getUserID(email);
            Dictionary<String, List<Article>> sortedArticles = new Dictionary<String, List<Article>>();
            List<Article> myArticles = new List<Article>(),
                allPublicArticles = new List<Article>(),
                allArticles = new List<Article>();

            allArticles = getAllArticles(email);
            foreach (Article article in allArticles)
            {
                //My article:
                if (article.userId == userID)
                {
                    myArticles.Add(article);
                }
                else
                {
                    if (article.isPublic) allPublicArticles.Add(article);
                }

            }

            sortedArticles.Add("myArticles", myArticles);
            sortedArticles.Add("allPublicArticles", allPublicArticles);

            return sortedArticles;
        }
    }
}