﻿namespace ApiCatalog.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public DateTime BoughtDate { get; set; }
        public int Stock { get; set; }
    }
}