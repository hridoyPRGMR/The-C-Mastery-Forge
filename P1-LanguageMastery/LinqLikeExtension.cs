using System.Collections;
using P1.Lamdas;

namespace P1.LInq
{
    public static class MyExtension
    {
        public static void PrintAll<T>(this IEnumerable<T> source) {
            foreach (var item in source)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class MyGrouping<TKey, TElement> : IEnumerable<TElement>
    {
        private readonly List<TElement> _items = new List<TElement>();
        public TKey Key { get; }

        public MyGrouping(TKey key)
        {
            Key = key;
        }

        public void Add(TElement item)
        {
            _items.Add(item);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public static class MyLinq
    {
        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        public static IEnumerable<TResult> MySelect<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<T> MyOrder<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.OrderBy(keySelector);
        }

        public static int MyCount<T>(this IEnumerable<T> source)
        {
            if (source is ICollection<T> collection)
            {
                return collection.Count;
            }

            int count = 0;
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    count++;
                }
            }
            return count;
        }

        public static int MyCount<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var filteredItems = source.MyWhere(predicate);

            return filteredItems.MyCount();
        }

        public static IEnumerable<MyGrouping<TKey, TElement>> MyGroupBy<TElement, TKey>(
            this IEnumerable<TElement> source,
            Func<TElement, TKey> keySelector
        )
        {
            var groups = new Dictionary<TKey, MyGrouping<TKey, TElement>>();

            foreach (var item in source)
            {
                TKey key = keySelector(item);

                if (!groups.TryGetValue(key, out var group))
                {
                    group = new MyGrouping<TKey, TElement>(key);
                    groups.Add(key, group);
                }

                group.Add(item);
            }

            foreach (var group in groups.Values)
            {
                yield return group;
            }
        }

        public static IEnumerable<TResult> MySelectMany<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {

             // Outer loop: iterates through the source collection (e.g., List<List<int>>)
            foreach (var item in source)
            {
                // Get the inner collection by applying the selector
                IEnumerable<TResult> innerCollection = selector(item);

                // Inner loop: iterates through the items in the inner collection
                foreach (var innerItem in innerCollection)
                {
                    // Yield return each item one by one
                    yield return innerItem;
                }
            }
        }
    
    }

   

    public class LInqLikeExtension
    {
        public LInqLikeExtension()
        {
            var nums = new List<int> { 1, 2, 7, 3, 4, 5 };
            var evens = nums.MyWhere(n => n % 2 == 0);
            var squared = nums.MyWhere(n => n % 2 != 0).MySelect(n => n * n).MyOrder(n => -n);

            // squared.PrintAll();
            // Console.WriteLine(squared.MyCount(n => n % 2 != 0));

            List<Product> products = new List<Product>
            {
                new Product { Id=1, Name = "Laptop", Category = "Electronics", Price = 1200.00m },
                new Product { Id=2, Name = "Desk Chair", Category = "Furniture", Price = 350.00m },
                new Product { Id=3, Name = "Smartphone", Category = "Electronics", Price = 800.00m },
                new Product { Id=4, Name = "Coffee Table", Category = "Furniture", Price = 200.00m },
                new Product { Id=6, Name = "Gaming Chair", Category = "Furniture", Price = 1500.00m },
                new Product { Id=7, Name = "Car", Category = "Motor", Price = 1500.00m },
                new Product { Id=8, Name = "Bike", Category = "Motor", Price = 1500.00m }
            };

            var productGroups = products.MyGroupBy(p => p.Category);

            // foreach (var group in productGroups)
            // {
            //     Console.WriteLine($"--- Category: {group.Key} ({group.Count()} products) ---");
            //     foreach (var product in group)
            //     {
            //         Console.WriteLine($"  - {product.Name} (${product.Price})");
            //     }
            // }

            int totalProducts = products.MyCount();
            int priceMOreThanThreeHundred = products.MyCount(p => p.Price > 300);

            // Console.WriteLine($"Total Products: {totalProducts}, Price More Than Three Hundred: {priceMOreThanThreeHundred}");

            List<List<int>> lists = [[1, 2, 3], [4, 5, 6], [7, 8], [9, 10]];
            List<int> list = lists.MySelectMany(l => l).ToList();

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

        }
    }
}
