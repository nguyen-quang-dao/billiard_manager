using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BillDTO
    {
        public int BillId { get; set; }
        public int TableId { get; set; }
        public DateTime StartTime { get; set; }
        public int Status { get; set; }
    }
}
