using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    public class DBConnect
    {
        public static string connStr = "Data Source=(local);Initial Catalog=BilliardManagement;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connStr);
        }
    }
}