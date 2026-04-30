using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DTO;

namespace DAL
{
    public class TableDAL
    {
        public List<TableDTO> GetTables()
        {
            List<TableDTO> list = new List<TableDTO>();

            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();
                string sql = "SELECT * FROM Tables";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new TableDTO
                    {
                        TableId = (int)rd["TableId"],
                        TableName = rd["TableName"].ToString(),
                        Status = (int)rd["Status"],
                        PricePerHour = rd["PricePerHour"] == DBNull.Value ? 0 : Convert.ToDouble(rd["PricePerHour"])
                    });
                }
            }

            return list;
        }
        public void Insert(string name, double price)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO Tables(TableName, PricePerHour, Status) VALUES(@n, @p, 0)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@p", price);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(int id, string name, double price)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();
                string sql = "UPDATE Tables SET TableName=@n, PricePerHour=@p WHERE TableId=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@p", price);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM Tables WHERE TableId=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStatus(int tableId, int status)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = "UPDATE Tables SET Status=@status WHERE TableId=@id";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", tableId);

                cmd.ExecuteNonQuery();
            }
        }
        public void UpdatePrice(int tableId, double price)
        {
            using (SqlConnection con = DBConnect.GetConnection())
            {
                con.Open();

                string sql = "UPDATE Tables SET PricePerHour=@p WHERE TableId=@id";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@p", price);
                cmd.Parameters.AddWithValue("@id", tableId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}