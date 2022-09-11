using System.Collections.Generic;

namespace LumTomofunCustomization.API_Entity
{
    public class TopestEntity
    {
        public class Stock
        {
            public int StockID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class StockRoot
        {
            public string code { get; set; }
            public string msg { get; set; }
            public List<Stock> data { get; set; }
        }

        public class Product
        {
            public int GoodsID { get; set; }
            public string SKU { get; set; }
            public string GoodsCode { get; set; }
            public string CnName { get; set; }
            public string EnName { get; set; }
            public double Price { get; set; }
            public double Weight { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public double High { get; set; }
            public double RealWeight { get; set; }
            public double RealLength { get; set; }
            public double RealWidth { get; set; }
            public double RealHigh { get; set; }
        }

        public class ProductRoot
        {
            public string code { get; set; }
            public string msg { get; set; }
            public List<Product> data { get; set; }
        }

        public class Inventory
        {
            public string SKU { get; set; }
            public string GoodsCode { get; set; }
            public int AvailableQty { get; set; }
            public int StockQty { get; set; }
            public int UsedQty { get; set; }
            public string StockUnitNo { get; set; }
            public int StockID { get; set; }
        }

        public class InventoryRoot
        {
            public string code { get; set; }
            public string msg { get; set; }
            public List<Inventory> data { get; set; }
        }
    }
}
