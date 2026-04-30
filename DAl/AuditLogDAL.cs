using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    public class AuditLogDAL
    {
        public void Insert(int userId, string action, string desc)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = @"INSERT INTO AuditLogs(UserId, Action, Description)
                               VALUES(@u, @a, @d)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@u", userId);
                cmd.Parameters.AddWithValue("@a", action);
                cmd.Parameters.AddWithValue("@d", desc);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
