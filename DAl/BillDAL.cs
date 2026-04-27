using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class BillDAL
    {
        public BillDTO GetOpenBill(int tableId)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = @"SELECT TOP 1 *
                                FROM Bills
                                WHERE TableId = @tableId AND EndTime IS NULL
                                ORDER BY StartTime DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@tableId", tableId);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new BillDTO
                    {
                        BillId = (int)rd["BillId"],
                        TableId = (int)rd["TableId"],
                        StartTime = (DateTime)rd["StartTime"],
                        Status = (int)rd["Status"]
                    };
                }
            }

            return null;
        }

        public void CreateBill(int tableId)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = @"INSERT INTO Bills(TableId, StartTime, EndTime, Status)
               VALUES(@tableId, GETDATE(), NULL, 0)";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@tableId", tableId);
                cmd.ExecuteNonQuery();
            }
        }

        public void PayBill(int billId, double total)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = @"UPDATE Bills
               SET EndTime = GETDATE(), Status = 1, TotalAmount = @total
               WHERE BillId = @billId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@billId", billId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
