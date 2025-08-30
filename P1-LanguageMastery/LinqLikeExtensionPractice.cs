using MyHelper.MyLinq;
using Data;

namespace P1.Linq.Pr
{
    public class LinqLikeExtensionPractice
    {
        public LinqLikeExtensionPractice()
        {

            List<int> list = [1, 2, 2, 3, 4, 4, 5];
            List<List<int>> nestedList = [[1, 2], [3, 4, 5], [7, 8, 9, 10]];
            List<string> stringList = ["Shaon", "Hridoy", "Mia", "Abul Kalam", "Rock Star"];

            Console.WriteLine(list.MyCount());
            Console.WriteLine(list.MyCount(x => x % 2 == 0));

            List<Product> productList = GetData.ProductList;

            Console.WriteLine(productList.MyCount(p => p.Category == "Furniture"));
            Console.WriteLine(productList.MyAny(p => p.Id == 1));

            Console.WriteLine(list.MyFirst());
            Console.WriteLine(productList.MyFirst().Name);
            Console.WriteLine(productList.MyFirst(p => p.Category == "Furniture").Name);

            var distinctList = list.MyDistinct().ToList();

            List<int> singleList = nestedList.MySelectMany(n => n).ToList();

            var products = productList.MyTake(7).ToList();
            products = productList.MySkip(5).ToList();
            products = productList.MyOrderBy(p => p.Price).ToList();

            list = list.MyUnion(singleList).ToList();

            Console.WriteLine("Object Min: " + productList.MyMin(p => p.Name));
            Console.WriteLine("Normal List Min: " + stringList.MyMin());

            List<MyGroup<string, Product>> groupsData = productList.MyGroupBy(p => p.Category).ToList();
        }
    }
}
