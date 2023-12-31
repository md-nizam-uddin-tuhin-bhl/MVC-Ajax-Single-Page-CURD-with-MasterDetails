﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class VmSale
    {
        public long SaleId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public List<VmSaleDetail> SaleDetails { get; set; } = new List<VmSaleDetail>();
        public string[] ProductName { get; set; }
        public decimal?[] Price { get; set; }

        public class VmSaleDetail
        {
            public long SaleDetailId { get; set; }
            public long? SaleId { get; set; }
            public string ProductName { get; set; }
            public decimal? Price { get; set; }
        }
    }
}