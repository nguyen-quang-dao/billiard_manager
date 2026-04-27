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

                string sql = "SELECT TableId, TableName, Status FROM Tables";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new TableDTO
                    {
                        TableId = (int)rd["TableId"],
                        TableName = rd["TableName"].ToString(),
                        Status = (int)rd["Status"]
                    });
                }
            }

            return list;
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
    }
}