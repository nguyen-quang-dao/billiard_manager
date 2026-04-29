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
        private BillDAL billDAL = new BillDAL();
        private TableDAL tableDAL = new TableDAL();

        private const double PRICE_PER_HOUR = 50000;

        public BillDTO GetOpenBill(int tableId)
        {
            return billDAL.GetOpenBill(tableId);
        }

        public void StartTable(int tableId)
        {
            var bill = billDAL.GetOpenBill(tableId);
            if (bill != null) return;

            billDAL.CreateBill(tableId);
            tableDAL.UpdateStatus(tableId, 1);
        }

        public double CalculateTotal(DateTime startTime)
        {
            TimeSpan time = DateTime.Now - startTime;
            return Math.Round(time.TotalHours * PRICE_PER_HOUR, 0);
        }

        public double Pay(int tableId)
        {
            var bill = billDAL.GetOpenBill(tableId);
            if (bill == null) return 0;

            double total = CalculateTotal(bill.StartTime);

            billDAL.PayBill(bill.BillId, total);
            tableDAL.UpdateStatus(tableId, 0);

            return total;
        }
    }
}