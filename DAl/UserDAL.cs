using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    public class UserDAL
    {
        public bool CheckLogin(string user, string pass)
        {
            SqlConnection con = DBConnect.GetConnection();
            con.Open();

            string sql = "SELECT COUNT(*) FROM Users WHERE Username=@u AND Password=@p";
            SqlCommand cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@u", user);
            cmd.Parameters.AddWithValue("@p", pass);

            int result = (int)cmd.ExecuteScalar();
            con.Close();

            return result > 0;
        }
    }
}
