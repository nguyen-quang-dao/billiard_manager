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
                               WHERE TableId = @id AND EndTime IS NULL
                               ORDER BY StartTime DESC";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", tableId);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    return new BillDTO
                    {
                        BillId = (int)rd["BillId"],
                        TableId = (int)rd["TableId"],
                        StartTime = (DateTime)rd["StartTime"],
                        EndTime = rd["EndTime"] == DBNull.Value ? null : (DateTime?)rd["EndTime"],
                        TotalAmount = rd["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDouble(rd["TotalAmount"]),
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
                               SET EndTime = GETDATE(),
                                   TotalAmount = @total,
                                   Status = 1
                               WHERE BillId = @billId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@billId", billId);
                cmd.Parameters.AddWithValue("@total", total);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
