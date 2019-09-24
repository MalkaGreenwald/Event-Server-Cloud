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
    public class ImageEntity
    {
        static MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
        {
            Server = "35.187.167.135",
            UserID = "root",
            Password = "takeapeek",
            Database = "Event",
            CertificateFile = HttpContext.Current.Server.MapPath("C:key/client.pfx"),
            SslCa = HttpContext.Current.Server.MapPath("C:key/server-ca.pem"),
            //SslMode = MySqlSslMode.VerifyCA,
            SslMode = MySqlSslMode.None
        };
        static string con = csb.ConnectionString;
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Nullable<bool> isBlur { get; set; }
        public Nullable<bool> isClosedEye { get; set; }
        public Nullable<bool> isDark { get; set; }
        public Nullable<bool> isCutFace { get; set; }
        public Nullable<bool> isGroom { get; set; }
        public Nullable<bool> isLight { get; set; }
        public Nullable<bool> isOutdoors { get; set; }
        public Nullable<bool> isIndoors { get; set; }
        public Nullable<bool> hasChildren { get; set; }
        public Nullable<int> numPerson { get; set; }
        public Nullable<bool> isInRecycleBin { get; set; }
        public bool Add()
        {
            var connection = new MySqlConnection(con);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Images(url, name, isBlur, isClosedEyes, isGroom, isOutdoors, isIndoors, hasChildren, numPeople, isInRecycleBin) VALUES(@url, @name, @isBlur, @isClosedEyes, @isGroom, @isOutdoors, @isIndoors, @hasChildren, @numPeople, @isInRecycleBin)";
            cmd.Parameters.Add("@url", MySqlDbType.String).Value = url;
            cmd.Parameters.Add("@name", MySqlDbType.String).Value = name;
            cmd.Parameters.Add("@isBlur", MySqlDbType.String).Value = isBlur != null ? isBlur.ToString() : "false";
            cmd.Parameters.Add("@isClosedEyes", MySqlDbType.String).Value = isClosedEye != null ? isClosedEye.ToString() : "false";
            cmd.Parameters.Add("@isGroom", MySqlDbType.String).Value = isGroom != null ? isGroom.ToString() : "false";
            cmd.Parameters.Add("@isOutdoors", MySqlDbType.String).Value = isOutdoors != null ? isOutdoors.ToString() : "false";
            cmd.Parameters.Add("@isIndoors", MySqlDbType.String).Value = isIndoors != null ? isIndoors.ToString() : "false";
            cmd.Parameters.Add("@hasChildren", MySqlDbType.String).Value = hasChildren != null ? hasChildren.ToString() : "false";
            cmd.Parameters.Add("@numPeople", MySqlDbType.String).Value = numPerson != null ? numPerson.ToString() : "false";
            cmd.Parameters.Add("@isInRecycleBin", MySqlDbType.String).Value = isInRecycleBin != null ? isInRecycleBin.ToString() : "false";
            cmd.ExecuteNonQuery();
            connection.Close();
            return true;
        }
        public static List<ImageEntity> Get()
        {
            List<ImageEntity> images = new List<ImageEntity>();
            using (var connection = new MySqlConnection(con))
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * from Images";
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    ImageEntity img = new ImageEntity();
                    img.url = reader["url"].ToString();
                    img.name = reader["name"].ToString();
                    img.isBlur = bool.Parse(reader["isBlur"].ToString());
                    img.isClosedEye = bool.Parse(reader["isClosedEyes"].ToString());
                    img.isGroom = bool.Parse(reader["isGroom"].ToString());
                    img.isIndoors = bool.Parse(reader["isIndoors"].ToString());
                    img.isOutdoors = bool.Parse(reader["isOutdoors"].ToString());
                    img.hasChildren = bool.Parse(reader["hasChildren"].ToString());
                    img.numPerson = int.Parse(reader["numPeople"].ToString());
                    img.isInRecycleBin = bool.Parse(reader["isInRecycleBin"].ToString());
                    images.Add(img);
                }
            }
            return images;
        }
        public static bool UpdateRecycleBin(string url, bool toWhat)
        {
            try
            {
                using (var connection = new MySqlConnection(con))
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE Images SET isInRecycleBin=@isInRecycleBin WHERE url=@url";
                    cmd.Parameters.Add("@url", MySqlDbType.String).Value = url;
                    cmd.Parameters.Add("@isInRecycleBin", MySqlDbType.String).Value = toWhat.ToString();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
        public static bool UpdateIsGroom(string url, bool toWhat)
        {
            try
            {
                using (var connection = new MySqlConnection(con))
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE Images SET isGroom=@toWhat WHERE url=@url";
                    cmd.Parameters.Add("@url", MySqlDbType.String).Value = url;
                    cmd.Parameters.Add("@toWhat", MySqlDbType.String).Value = toWhat;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static bool RemoveAll()
        {
            try
            {
                var connection = new MySqlConnection(con);
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "TRUNCATE TABLE Images";
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
