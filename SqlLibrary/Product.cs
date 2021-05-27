using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpToSqlLibrary
{
    public class Product
    {
        public int Id { get; set; }
        public string PartNbr { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string PhotoPath { get; set; }
        public int VendorsId { get; set; }
        public Vendor Vendor { get; set; } 
    }
}
