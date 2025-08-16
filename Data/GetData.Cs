namespace Data
{
    public class GetData
    {
        public static List<Product> ProductList { get; set; } = [];

        static GetData()
        {
            ProductList = new List<Product>
            {
                new Product { Id=1, Name = "Laptop", Category = "Electronics", Price = 1200.00m },
                new Product { Id=2, Name = "Desk Chair", Category = "Furniture", Price = 350.00m },
                new Product { Id=3, Name = "Smartphone", Category = "Electronics", Price = 800.00m },
                new Product { Id=4, Name = "Coffee Table", Category = "Furniture", Price = 200.00m },
                new Product { Id=5, Name = "Gaming Chair", Category = "Furniture", Price = 1500.00m },
                new Product { Id=6, Name = "Car", Category = "Motor", Price = 1500.00m },
                new Product { Id=7, Name = "Bike", Category = "Motor", Price = 1500.00m }
            };
        }

    }
}