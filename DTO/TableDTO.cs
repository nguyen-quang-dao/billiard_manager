using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TableDTO
    {
        public int TableId { get; set; }
        public string TableName { get; set; }
        public double PricePerHour { get; set; }
        public int Status { get; set; }
    }
}
