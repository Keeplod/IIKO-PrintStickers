namespace PrintStickers.Models
{
    public class ModelIikoOrder
    {
        public string[] CustomerIds { get; set; }
        public Items[] Items { get; set; }
        public int Number { get; set; }
    }

    public class Items
    {
        public Product Product { get; set; }
        public Guest Guest { get; set; }
        public double Amount { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
    }

    public class Guest
    {
        public string Name { get; set; }
    }
}
