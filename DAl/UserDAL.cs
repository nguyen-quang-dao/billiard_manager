using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class UserDAL
    {
        public UserDTO Login(string username, string password)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = "SELECT * FROM Users WHERE Username=@u AND Password=@p";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new UserDTO
                    {
                        UserId = (int)rd["UserId"],
                        Username = rd["Username"].ToString(),
                        FullName = rd["FullName"].ToString(),
                        Role = (int)rd["Role"]
                    };
                }
            }
            return null;
        }
    }
}
