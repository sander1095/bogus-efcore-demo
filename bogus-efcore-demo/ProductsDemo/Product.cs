namespace ProductsDemo
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset CreationDate { get; set; }
        public ICollection<ProductProductCategory> ProductProductCategories { get; set; } = new List<ProductProductCategory>();
        
        // STEP: Enable this during the demo and add a migration
        //public string Description { get; set; } = null!; 
    }

    public class ProductProductCategory
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public Product Product { get; set; } = null!;
        public ProductCategory Category { get; set; } = null!;
    }

    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
