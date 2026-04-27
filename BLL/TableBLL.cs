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

        public List<TableDTO> GetTables()
        {
            return dal.GetTables();
        }
    }
}
