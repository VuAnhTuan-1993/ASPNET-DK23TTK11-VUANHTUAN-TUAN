using System;
using System.Collections.Generic;

namespace AppleStore_MVC.Models
{
    public class DashboardViewModel
    {
        // KPI Cards
        public decimal RevenueMonth { get; set; }
        public decimal RevenueYear { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }

        // Chart Data
        public List<string> RevenueMonths { get; set; } = new List<string>();
        public List<decimal> RevenueValues { get; set; } = new List<decimal>();

        public List<string> TopProductNames { get; set; } = new List<string>();
        public List<int> TopProductSales { get; set; } = new List<int>();

        // Alerts
        //public List<string> LowStockProducts { get; set; } = new List<string>();
        public Dictionary<string, int> LowStockProducts { get; set; } = new Dictionary<string, int>();
    }
}
