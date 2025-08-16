using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace MyHelper.MyLinq
{

    public static class MyLinq
    {

        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static int MyCount<T>(this IEnumerable<T> source)
        {
            if (source is ICollection<T> collection)
            {
                return collection.Count();
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

        public static bool MyAny<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) return true;
            }

            return false;
        }

        public static T MyFirst<T>(this IEnumerable<T> source)
        {
            T? first = source.TryGetFirst(out bool found);

            if (!found)
            {
                throw new Exception("Not Found");
            }

            return first;
        }

        public static T MyFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            T? first = source.TryGetFirst(predicate, out bool found);

            if (!found)
            {
                throw new Exception("Not Found");
            }

            return first;
        }

        public static IEnumerable<T> MyDistinct<T>(this IEnumerable<T> source)
        {

            HashSet<T> hs = new HashSet<T>();

            foreach (var item in source)
            {
                if (!hs.Contains(item))
                {
                    yield return item;
                }
                hs.Add(item);
            }
        }

        public static IEnumerable<TResult> MySelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            foreach (var item in source)
            {
                IEnumerable<TResult> innerColection = selector(item);

                foreach (var innerItem in innerColection) {
                    yield return innerItem;
                }
            }
        }

        public static IEnumerable<T> MyTake<T>(this IEnumerable<T> source, int count)
        {
            int size = source.Count();
            if (count > size)
            {
                throw new Exception("Index out of bound.");
            }

            for (int i = 0; i < count; i++) {
                yield return source.ElementAt(i);
            }
        }

        public static IEnumerable<T> MySkip<T>(this IEnumerable<T> source, int count)
        {
            int size = source.Count();
            if (count > size)
            {
                throw new Exception("Index out of bound.");
            }

            for (int i = count; i < size; i++) {
                yield return source.ElementAt(i);
            }
        }

        public static IEnumerable<T> MyUnion<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            var seen = new HashSet<T>();

            foreach (var item in first)
            {
                if (seen.Add(item))
                    yield return item;
            }

            foreach (var item in second)
            {
                if (seen.Add(item))
                    yield return item;
            }
        }

        public static IEnumerable<T> MyOrderBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            var list = source.ToList();

            list.Sort((x, y) => Comparer<TKey>.Default.Compare(keySelector(x), keySelector(y)));

            return list;
        }

        public static TResult MyMin<T, TResult>(
            this IEnumerable<T> source,
            Func<T, TResult> selector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));

            using var e = source.GetEnumerator();
            if (!e.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");

            TResult value = selector(e.Current);

            var comparer = Comparer<TResult>.Default;

            while (e.MoveNext())
            {
                TResult x = selector(e.Current);
                if (comparer.Compare(x, value) < 0)
                {
                    value = x;
                }
            }

            return value;
        }

        public static T MyMin<T>(this IEnumerable<T> source) where T : IComparable<T>
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            using var e = source.GetEnumerator();
            if (!e.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");

            T value = e.Current;

            while (e.MoveNext())
            {
                if (e.Current.CompareTo(value) < 0)
                {
                    value = e.Current;
                }
            }

            return value;
        }

        public static IEnumerable<MyGroup<TKey, TElement>> MyGroupBy<TElement, TKey>(
            this IEnumerable<TElement> source,
            Func<TElement, TKey> keySelector)
        {
            var groups = new Dictionary<TKey, MyGroup<TKey, TElement>>();

            foreach (var item in source)
            {
                TKey key = keySelector(item);

                if (!groups.TryGetValue(key, out var group))
                {
                    group = new MyGroup<TKey, TElement>(key);
                    groups.Add(key, group);
                }
                group.Add(item);
            }

            foreach (var group in groups.Values)
            {
                yield return group;
            }
        }


        private static T? TryGetFirst<T>(this IEnumerable<T> source, out bool found)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            using (IEnumerator<T> e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    found = true;
                    return e.Current;
                }
            }

            found = false;
            return default;
        }

        private static T? TryGetFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate, out bool found)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            using IEnumerator<T> e = source.GetEnumerator();
            while (e.MoveNext())
            {
                if (predicate(e.Current))
                {
                    found = true;
                    return e.Current;
                }
            }

            found = false;
            return default;
        }

    }

    public class MyGroup<TKey, TElement> : IEnumerable<TElement>
    {

        private readonly List<TElement> _items = [];
        public TKey Key { get; }

        public MyGroup(TKey key)
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

}