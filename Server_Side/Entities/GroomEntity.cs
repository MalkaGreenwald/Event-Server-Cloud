using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entities
{
    public class GroomEntity
    {
        static MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
        {
            Server = "35.187.167.135",
            UserID = "root",
            Password = "takeapeek",
            Database = "Event",
            CertificateFile = HttpContext.Current.Server.MapPath("Keys/client.pfx"),
            SslCa = HttpContext.Current.Server.MapPath("Keys/server-ca.pem"),
            //SslMode = MySqlSslMode.VerifyCA,
            SslMode = MySqlSslMode.None
        };
        static string con = csb.ConnectionString;
        public int id { get; set; }
        public string url { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public bool Add()
        {
            try
            {
                var connection = new MySqlConnection(con);
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO groom(url, name, token) VALUES(@url, @name, @token)";
                cmd.Parameters.Add("@url", MySqlDbType.String).Value = url;
                cmd.Parameters.Add("@name", MySqlDbType.String).Value = name;
                cmd.Parameters.Add("@token", MySqlDbType.String).Value = token;
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static List<GroomEntity> Get()
        {
            List<GroomEntity> grooms = new List<GroomEntity>();
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * from groom";
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    GroomEntity gr = new GroomEntity();
                    gr.url = reader["url"].ToString();
                    gr.name = reader["name"].ToString();
                    gr.token = reader["token"].ToString();
                    grooms.Add(gr);
                }
            }
            return grooms;
        }
        public static bool RemoveAll()
        {
            try
            {
                var connection = new MySqlConnection(con);
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "TRUNCATE TABLE groom";
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}