namespace P1.Lamdas
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
    
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal OrderValue { get; set; }
    }

    public class LinqChallenge
    {
        public LinqChallenge()
        {
            List<Product> products = new()
            {
                new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1200.00m },
                new Product { Id = 2, Name = "Desk Chair", Category = "Furniture", Price = 350.00m },
                new Product { Id = 3, Name = "Smartphone", Category = "Electronics", Price = 800.00m },
                new Product { Id = 4, Name = "Coffee Table", Category = "Furniture", Price = 200.00m },
                new Product { Id = 5, Name = "Headphones", Category = "Electronics", Price = 150.00m },
                new Product { Id = 6, Name = "Backpack", Category = "Accessories", Price = 75.00m },
                new Product { Id = 7, Name = "Gaming Monitor", Category = "Electronics", Price = 1500.00m },
                new Product { Id = 8, Name = "T-shirt", Category = "Apparel", Price = 25.00m }
            };

            var filterProducts = products
             .Where(product => product.Category == "Electronics")
             .ToList();

            // filterProducts.ForEach(p =>
            // {
            //     Console.WriteLine(p.Id);
            // });

            filterProducts = filterProducts.Where(p => p.Price > 200).ToList();
            // filterProducts.ForEach(p =>
            // {
            //     Console.WriteLine(p.Name);
            // });

            filterProducts.Sort((a, b) => b.Price.CompareTo(a.Price));
            // filterProducts.ForEach(p =>
            // {
            //     Console.WriteLine(p.Price);
            // });

            var objects = filterProducts
                .Select(p => new { p.Name, p.Price })
                .ToList();


            // in short
            var ElectronicsProducts = products
                .Where(p => p.Category == "Electronics" && p.Price > 200)
                .OrderByDescending(p => p.Price)
                .Select(p => new { p.Name, p.Price });

            // foreach (var p in ElectronicsProducts)
            // {
            //     Console.WriteLine($"{p.Name} - ${p.Price}");
            // }

            List<Customer> customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Alice", Country = "USA" },
                new Customer { Id = 2, Name = "Bob", Country = "Canada" },
                new Customer { Id = 3, Name = "Charlie", Country = "USA" },
                new Customer { Id = 4, Name = "David", Country = "UK" },
                new Customer { Id = 5, Name = "Eve", Country = "USA" }
            };

            List<Order> orders = new List<Order>
            {
                new Order { Id = 101, CustomerId = 1, OrderValue = 250.00m },
                new Order { Id = 102, CustomerId = 3, OrderValue = 50.00m },
                new Order { Id = 103, CustomerId = 1, OrderValue = 750.00m },
                new Order { Id = 104, CustomerId = 2, OrderValue = 120.00m },
                new Order { Id = 105, CustomerId = 5, OrderValue = 300.00m },
                new Order { Id = 106, CustomerId = 3, OrderValue = 100.00m },
                new Order { Id = 107, CustomerId = 1, OrderValue = 500.00m },
                new Order { Id = 108, CustomerId = 4, OrderValue = 900.00m }
            };

            var customerOrderTotals = customers
                .Where(c => c.Country == "USA")
                .Join(orders, c => c.Id, o => o.CustomerId, (customer, order) => new { Customer = customer, Order = order })
                .GroupBy(item => item.Customer)
                .Select(group => new
                {
                    CustomerName = group.Key.Name,
                    TotalOrderValue = group.Sum(item => item.Order.OrderValue)
                })
                .OrderByDescending(x => x.TotalOrderValue);


            var groupByData = customers
                .GroupBy(item => item.Country)
                .Select(item => new { item.Key, V = item.Count(item => item.Id == 1) })
                .ToList();

            var ordersByCustomer = customers
                .Join(orders, c => c.Id, o => o.CustomerId, (c, o) => new { Id = c.Id, CustomerName = c.Name, OrderValue = o.OrderValue })
                .GroupBy(item => item.Id)
                .Select(group => new { Id = group.Key, Name = group.First().CustomerName, Total = group.Sum(item => item.OrderValue) })
                .OrderBy(item => item.Total)
                .ToList();
            

            var expensiveProducts = products
                .Where(p => p.Category == "Electronics")        // Filter by category
                .Except(products.Where(p => p.Name == "Smartphone")) // Exclude one item
                .OrderByDescending(p => p.Price)                // Order by price
                .Take(2)                                        // Take only the top 2
                .Select(p => p.Name);                           // Get just the names

            foreach (var name in expensiveProducts)
            {
                Console.WriteLine(name);
            }

            // foreach (var d in ordersByCustomer)
            // {
            //     Console.WriteLine(d.Id + " " + d.Name+ " "+d.Total);
            // }

            // Console.WriteLine("Data:");
            // foreach (var customerTotal in customerOrderTotals)
            // {
            //     Console.WriteLine($"{customerTotal.CustomerName} - ${customerTotal.TotalOrderValue}");
            // }
        }
    }

}