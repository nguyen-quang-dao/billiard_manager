using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BillBLL
    {
        BillDAL dal = new BillDAL();
        TableDAL tableDAL = new TableDAL();

        double pricePerHour = 50000;

        public BillDTO GetOpenBill(int tableId)
        {
            return dal.GetOpenBill(tableId);
        }

        public void StartTable(int tableId)
        {
            dal.CreateBill(tableId);
            tableDAL.UpdateStatus(tableId, 1);
        }

        public double CalculateTotal(DateTime start)
        {
            TimeSpan time = DateTime.Now - start;
            return Math.Round(time.TotalHours * pricePerHour, 0);
        }

        public void Pay(int tableId)
        {
            var bill = dal.GetOpenBill(tableId);
            if (bill == null) return;

            double total = CalculateTotal(bill.StartTime);

            dal.PayBill(bill.BillId, total);
            tableDAL.UpdateStatus(tableId, 0);
        }
    }
}