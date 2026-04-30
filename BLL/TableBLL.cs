using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class TableBLL
    {
        TableDAL dal = new TableDAL();
        private AuditLogDAL logDAL = new AuditLogDAL();
        public List<TableDTO> GetTables()
        {
            return dal.GetTables();
        }

        public void AddTable(string name, double price, UserDTO user)
        {
            if (user.Role != 1) throw new Exception("Không có quyền");
            dal.Insert(name, price);
        }

        public void UpdateTable(int id, string name, double price, UserDTO user)
        {
            if (user.Role != 1) throw new Exception("Không có quyền");
            dal.Update(id, name, price);
        }

        public void DeleteTable(int id, UserDTO user)
        {
            if (user.Role != 1) throw new Exception("Không có quyền");
            dal.Delete(id);
        }
    }
}
